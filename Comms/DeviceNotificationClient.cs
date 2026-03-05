using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using FOMSSubmarine;

namespace Comms
{
	// Since the Class Library is using System.windows.forms we need to set the following
	// attribute to prevent VS2010 getting confused and attempting to open class library
	// .cs files in the form designer.
	[System.ComponentModel.DesignerCategory("")]
	
	class DeviceNotificationClient : Control
	{
		#region System APIs

		internal const int DBT_DEVICEARRIVAL = 0x8000;
		internal const int DBT_DEVICEREMOVECOMPLETE = 0x8004;
		internal const int DBT_DEVTYP_DEVICEINTERFACE = 5;
		internal const int DBT_DEVTYP_HANDLE = 6;
		internal const int DEVICE_NOTIFY_ALL_INTERFACE_CLASSES = 4;
		internal const int DEVICE_NOTIFY_SERVICE_HANDLE = 1;
		internal const int DEVICE_NOTIFY_WINDOW_HANDLE = 0;
		internal const int WM_DEVICECHANGE = 0x219;

		[StructLayout(LayoutKind.Sequential)]
		internal class DEV_BROADCAST_DEVICEINTERFACE
		{
			internal int dbcc_size;
			internal int dbcc_devicetype;
			internal int dbcc_reserved;
			internal Guid dbcc_classguid;
			internal short dbcc_name;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		internal class DEV_BROADCAST_DEVICEINTERFACE_1
		{
			internal int dbcc_size;
			internal int dbcc_devicetype;
			internal int dbcc_reserved;
			[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 16)]
			internal byte[] dbcc_classguid;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 255)]
			internal char[] dbcc_name;
		}

		[StructLayout(LayoutKind.Sequential)]
		internal class DEV_BROADCAST_HDR
		{
			internal int dbch_size;
			internal int dbch_devicetype;
			internal int dbch_reserved;
		}

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern IntPtr RegisterDeviceNotification(IntPtr hRecipient, IntPtr NotificationFilter, int Flags);

		[DllImport("user32.dll", SetLastError = true)]
		internal static extern bool UnregisterDeviceNotification(IntPtr Handle);

		[DllImport("hid.dll")]
		static extern void HidD_GetHidGuid(ref Guid Guid);

		#endregion System APIs

		public delegate void DeviceChangeEventHandler(object sender, CommHID.DeviceNotification notification, string devicePath = "");
		public event DeviceChangeEventHandler DeviceChanged;

		public IntPtr Register()
		{
			Debug.WriteLine("usbGenericHidCommunication:registerForDeviceNotifications() -> Method called");

			// A DEV_BROADCAST_DEVICEINTERFACE header holds information about the request.
			DEV_BROADCAST_DEVICEINTERFACE devBroadcastDeviceInterface = new DEV_BROADCAST_DEVICEINTERFACE();
			IntPtr devBroadcastDeviceInterfaceBuffer = IntPtr.Zero;
			IntPtr handle = IntPtr.Zero;

			// Get the required GUID
			Guid systemHidGuid = new Guid();
			HidD_GetHidGuid(ref systemHidGuid);

			try
			{
				// Set the parameters in the DEV_BROADCAST_DEVICEINTERFACE structure.
				int size = Marshal.SizeOf(devBroadcastDeviceInterface);
				devBroadcastDeviceInterface.dbcc_size = size;
				devBroadcastDeviceInterface.dbcc_devicetype = DBT_DEVTYP_DEVICEINTERFACE;
				devBroadcastDeviceInterface.dbcc_reserved = 0;
				devBroadcastDeviceInterface.dbcc_classguid = systemHidGuid;

				devBroadcastDeviceInterfaceBuffer = Marshal.AllocHGlobal(size);
				Marshal.StructureToPtr(devBroadcastDeviceInterface, devBroadcastDeviceInterfaceBuffer, true);

				// Register for notifications and store the returned handle
				handle = RegisterDeviceNotification(Handle, devBroadcastDeviceInterfaceBuffer, DEVICE_NOTIFY_WINDOW_HANDLE);
				Marshal.PtrToStructure(devBroadcastDeviceInterfaceBuffer, devBroadcastDeviceInterface);

				if (handle != IntPtr.Zero)
				{
					Debug.WriteLine("usbGenericHidCommunication:registerForDeviceNotifications() -> Notification registration succeded");
				}
				else
				{
					Debug.WriteLine("usbGenericHidCommunication:registerForDeviceNotifications() -> Notification registration failed");
				}
			}
			catch (Exception ex)
			{
				DebuggingHelper.Trace(ex);
			}
			finally
			{
				if (devBroadcastDeviceInterfaceBuffer != IntPtr.Zero)
					Marshal.FreeHGlobal(devBroadcastDeviceInterfaceBuffer);
			}

			return handle;
		}

		protected override void WndProc(ref Message m)
		{
			if (m.Msg != WM_DEVICECHANGE)
			{
				base.WndProc(ref m);
				return;
			}

			Debug.WriteLine("DeviceNotificationMessages() -> Device notification received");

			try
			{
				switch (m.WParam.ToInt32())
				{
					case DBT_DEVICEARRIVAL:
						Debug.WriteLine("DeviceNotificationMessages() -> New device attached");
						DeviceChanged?.Invoke(this, CommHID.DeviceNotification.Attached);
						break;

					case DBT_DEVICEREMOVECOMPLETE:
						Debug.WriteLine("NotificationMessages() -> A device has been removed");
						DEV_BROADCAST_DEVICEINTERFACE_1 devBroadcastDeviceInterface = new DEV_BROADCAST_DEVICEINTERFACE_1();
						DEV_BROADCAST_HDR devBroadcastHeader = new DEV_BROADCAST_HDR();

						Marshal.PtrToStructure(m.LParam, devBroadcastHeader);

						// Is the notification event concerning a device interface?
						if (devBroadcastHeader.dbch_devicetype == DBT_DEVTYP_DEVICEINTERFACE)
						{
							// Get the device path name of the affected device
							int size = Convert.ToInt32((devBroadcastHeader.dbch_size - 32) / 2);
							devBroadcastDeviceInterface.dbcc_name = new char[size + 1];
							Marshal.PtrToStructure(m.LParam, devBroadcastDeviceInterface);
							string deviceNameString = new string(devBroadcastDeviceInterface.dbcc_name, 0, size);
							DeviceChanged?.Invoke(this, CommHID.DeviceNotification.Detached);
						}
						break;

					default:
						Debug.WriteLine("DeviceNotificationMessages() -> Unknown notification message");
						break;
				}
			}
			catch (Exception ex)
			{
				DebuggingHelper.Trace(ex);
			}
		}
	}
}
