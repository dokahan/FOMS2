//
// Contains portions of the WFF GenericHID Communication Library which is (c)2011 Simon Inns
//

using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;

using Microsoft.Win32.SafeHandles;

namespace Comms
{
	class CommHID : ICommDevice
	{
		#region System APIs

		//[DllImport("kernel32.dll", SetLastError = true)]
		//internal static extern Boolean WriteFile(
		//	SafeFileHandle hFile,
		//	Byte[] lpBuffer,
		//	Int32 nNumberOfBytesToWrite,
		//	ref Int32 lpNumberOfBytesWritten,
		//	IntPtr lpOverlapped
		//	);

		[DllImport("kernel32.dll", SetLastError = true)]
		static extern bool WriteFile(SafeFileHandle hFile, [Out] byte[] lpBuffer, int nNumberOfBytesToWrite, ref int lpNumberOfBytesWritten, IntPtr lpOverlapped);

		[DllImport("kernel32.dll")]
		static extern bool ReadFile(SafeFileHandle hFile, IntPtr lpBuffer, int nNumberOfBytesToRead, ref int lpNumberOfBytesRead, IntPtr lpOverlapped);

		[DllImport("kernel32.dll", SetLastError = true)]
		static extern bool CloseHandle(IntPtr hObject);

		[DllImport("kernel32.dll")]
		static extern uint GetLastError();

		[DllImport("setupapi.dll", SetLastError = true)]
		static extern IntPtr SetupDiGetClassDevs(ref Guid ClassGuid, IntPtr Enumerator, IntPtr hwndParent, int Flags);

		[DllImport("setupapi.dll", SetLastError = true)]
		static extern bool SetupDiEnumDeviceInterfaces(IntPtr hDevInfo, IntPtr devInfo, ref Guid interfaceClassGuid, int memberIndex, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData);

		[DllImport("setupapi.dll", SetLastError = true)]
		static extern bool SetupDiDestroyDeviceInfoList(IntPtr DeviceInfoSet);

        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern Boolean SetupDiGetDeviceInterfaceDetail(
            IntPtr DeviceInfoSet,
            ref SP_DEVICE_INTERFACE_DATA DeviceInterfaceData,
            IntPtr DeviceInterfaceDetailData,
            Int32 DeviceInterfaceDetailDataSize,
            ref Int32 RequiredSize,
            IntPtr DeviceInfoData);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern SafeFileHandle CreateFile(
			string lpFileName,
			uint dwDesiredAccess,
			uint dwShareMode,
			IntPtr lpSecurityAttributes,
			uint dwCreationDisposition,
			uint dwFlagsAndAttributes,
			Int32 hTemplateFile
			);

		//[DllImport(@"kernel32.dll", SetLastError = true)]
		//static extern SafeFileHandle CreateFile(string fileName, uint fileAccess, uint fileShare, FileMapProtection securityAttributes, uint creationDisposition, uint flags, IntPtr overlapped);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern IntPtr CreateEvent(
			IntPtr SecurityAttributes,
			bool bManualReset,
			bool bInitialState,
			string lpName
			);

		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern int WaitForSingleObject(IntPtr hHandle, int dwMilliseconds);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern Boolean GetOverlappedResult(
			SafeFileHandle hFile,
			IntPtr lpOverlapped,
			ref Int32 lpNumberOfBytesTransferred,
			Boolean bWait
			);

		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern Int32 CancelIo(SafeFileHandle hFile);

		[DllImport("hid.dll")]
		static extern void HidD_GetHidGuid(ref Guid Guid);

		[DllImport("hid.dll", SetLastError = true)]
		static extern bool HidD_GetPreparsedData(SafeFileHandle HidDeviceObject, ref IntPtr PreparsedData);

		[DllImport("hid.dll", SetLastError = true)]
		static extern bool HidD_GetAttributes(SafeFileHandle HidDeviceObject, ref HIDD_ATTRIBUTES Attributes);

		//[DllImport("hid.dll", SetLastError = true)]
		//static extern uint HidP_GetCaps(IntPtr PreparsedData, ref HIDP_CAPS Capabilities);
		[DllImport("hid.dll", SetLastError = true)]
		internal static extern int HidP_GetCaps(IntPtr PreparsedData, ref HIDP_CAPS Capabilities);

		[DllImport("hid.dll", SetLastError = true)]
		static extern int HidP_GetButtonCaps(HIDP_REPORT_TYPE ReportType, [In, Out] HIDP_BUTTON_CAPS[] ButtonCaps, ref ushort ButtonCapsLength, IntPtr PreparsedData);

		[DllImport("hid.dll", SetLastError = true)]
		static extern int HidP_GetValueCaps(HIDP_REPORT_TYPE ReportType, [In, Out] HIDP_VALUE_CAPS[] ValueCaps, ref ushort ValueCapsLength, IntPtr PreparsedData);

		[DllImport("hid.dll", SetLastError = true)]
		static extern int HidP_MaxUsageListLength(HIDP_REPORT_TYPE ReportType, ushort UsagePage, IntPtr PreparsedData);

		[DllImport("hid.dll", SetLastError = true)]
		static extern int HidP_SetUsages(HIDP_REPORT_TYPE ReportType, ushort UsagePage, short LinkCollection, short Usages, ref int UsageLength, IntPtr PreparsedData, IntPtr Report, int ReportLength);

		[DllImport("hid.dll", SetLastError = true)]
		static extern int HidP_SetUsageValue(HIDP_REPORT_TYPE ReportType, ushort UsagePage, short LinkCollection, ushort Usage, ulong UsageValue, IntPtr PreparsedData, IntPtr Report, int ReportLength);

		//[DllImport("hid.dll", SetLastError = true)]
		//static extern bool HidD_FreePreparsedData(ref IntPtr PreparsedData);
		[DllImport("hid.dll", SetLastError = true)]
		internal static extern Boolean HidD_FreePreparsedData(IntPtr PreparsedData);

		#endregion System APIs

		#region HID Difinitions

		internal const int FILE_FLAG_OVERLAPPED = 0x40000000;
		internal const int WAIT_TIMEOUT = 0x102;
		internal const int WAIT_OBJECT_0 = 0;

		const int DIGCF_DEFAULT = 0x00000001;
		const int DIGCF_PRESENT = 0x00000002;
		const int DIGCF_ALLCLASSES = 0x00000004;
		const int DIGCF_PROFILE = 0x00000008;
		const int DIGCF_DEVICEINTERFACE = 0x00000010;

		const uint GENERIC_READ = 0x80000000;
		const uint GENERIC_WRITE = 0x40000000;
		const uint GENERIC_EXECUTE = 0x20000000;
		const uint GENERIC_ALL = 0x10000000;

		const uint FILE_SHARE_READ = 0x00000001;
		const uint FILE_SHARE_WRITE = 0x00000002;
		const uint FILE_SHARE_DELETE = 0x00000004;

		const uint CREATE_NEW = 1;
		const uint CREATE_ALWAYS = 2;
		const uint OPEN_EXISTING = 3;
		const uint OPEN_ALWAYS = 4;
		const uint TRUNCATE_EXISTING = 5;

		const int HIDP_STATUS_SUCCESS = 1114112;
		const int DEVICE_PATH = 260;
		const int INVALID_HANDLE_VALUE = -1;

		enum FileMapProtection : uint
		{
			PageReadonly = 0x02,
			PageReadWrite = 0x04,
			PageWriteCopy = 0x08,
			PageExecuteRead = 0x20,
			PageExecuteReadWrite = 0x40,
			SectionCommit = 0x8000000,
			SectionImage = 0x1000000,
			SectionNoCache = 0x10000000,
			SectionReserve = 0x4000000,
		}

		enum HIDP_REPORT_TYPE : ushort
		{
			HidP_Input = 0x00,
			HidP_Output = 0x01,
			HidP_Feature = 0x02,
		}

		[StructLayout(LayoutKind.Sequential)]
		struct LIST_ENTRY
		{
			public IntPtr Flink;
			public IntPtr Blink;
		}

		[StructLayout(LayoutKind.Sequential)]
		struct DEVICE_LIST_NODE
		{
			public LIST_ENTRY Hdr;
			public IntPtr NotificationHandle;
			public HID_DEVICE HidDeviceInfo;
			public bool DeviceOpened;
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct SP_DEVICE_INTERFACE_DATA
		{
			public Int32 cbSize;
			public Guid interfaceClassGuid;
			public Int32 flags;
			private UIntPtr reserved;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
		struct SP_DEVICE_INTERFACE_DETAIL_DATA
		{
			public int cbSize;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = DEVICE_PATH)]
			public string DevicePath;
		}

		[StructLayout(LayoutKind.Sequential)]
		struct SP_DEVINFO_DATA
		{
			public int cbSize;
			public Guid classGuid;
			public UInt32 devInst;
			public IntPtr reserved;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct HIDP_CAPS
		{
			[MarshalAs(UnmanagedType.U2)]
			public UInt16 Usage;
			[MarshalAs(UnmanagedType.U2)]
			public UInt16 UsagePage;
			[MarshalAs(UnmanagedType.U2)]
			public UInt16 InputReportByteLength;
			[MarshalAs(UnmanagedType.U2)]
			public UInt16 OutputReportByteLength;
			[MarshalAs(UnmanagedType.U2)]
			public UInt16 FeatureReportByteLength;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
			public UInt16[] Reserved;
			[MarshalAs(UnmanagedType.U2)]
			public UInt16 NumberLinkCollectionNodes;
			[MarshalAs(UnmanagedType.U2)]
			public UInt16 NumberInputButtonCaps;
			[MarshalAs(UnmanagedType.U2)]
			public UInt16 NumberInputValueCaps;
			[MarshalAs(UnmanagedType.U2)]
			public UInt16 NumberInputDataIndices;
			[MarshalAs(UnmanagedType.U2)]
			public UInt16 NumberOutputButtonCaps;
			[MarshalAs(UnmanagedType.U2)]
			public UInt16 NumberOutputValueCaps;
			[MarshalAs(UnmanagedType.U2)]
			public UInt16 NumberOutputDataIndices;
			[MarshalAs(UnmanagedType.U2)]
			public UInt16 NumberFeatureButtonCaps;
			[MarshalAs(UnmanagedType.U2)]
			public UInt16 NumberFeatureValueCaps;
			[MarshalAs(UnmanagedType.U2)]
			public UInt16 NumberFeatureDataIndices;
		};

		[StructLayout(LayoutKind.Sequential)]
		struct HIDD_ATTRIBUTES
		{
			public Int32 Size;
			public UInt16 VendorID;
			public UInt16 ProductID;
			public Int16 VersionNumber;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct ButtonData
		{
			public Int32 UsageMin;
			public Int32 UsageMax;
			public Int32 MaxUsageLength;
			public Int16 Usages;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct ValueData
		{
			public ushort Usage;
			public ushort Reserved;

			public ulong Value;
			public long ScaledValue;
		}

		[StructLayout(LayoutKind.Explicit)]
		struct HID_DATA
		{
			[FieldOffset(0)]
			public bool IsButtonData;
			[FieldOffset(1)]
			public byte Reserved;
			[FieldOffset(2)]
			public ushort UsagePage;
			[FieldOffset(4)]
			public Int32 Status;
			[FieldOffset(8)]
			public Int32 ReportID;
			[FieldOffset(16)]
			public bool IsDataSet;

			[FieldOffset(17)]
			public ButtonData ButtonData;
			[FieldOffset(17)]
			public ValueData ValueData;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct HIDP_Range
		{
			public ushort UsageMin, UsageMax;
			public ushort StringMin, StringMax;
			public ushort DesignatorMin, DesignatorMax;
			public ushort DataIndexMin, DataIndexMax;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct HIDP_NotRange
		{
			public ushort Usage, Reserved1;
			public ushort StringIndex, Reserved2;
			public ushort DesignatorIndex, Reserved3;
			public ushort DataIndex, Reserved4;
		}

		[StructLayout(LayoutKind.Explicit)]
		struct HIDP_BUTTON_CAPS
		{
			[FieldOffset(0)]
			public ushort UsagePage;
			[FieldOffset(2)]
			public byte ReportID;
			[FieldOffset(3), MarshalAs(UnmanagedType.U1)]
			public bool IsAlias;
			[FieldOffset(4)]
			public short BitField;
			[FieldOffset(6)]
			public short LinkCollection;
			[FieldOffset(8)]
			public short LinkUsage;
			[FieldOffset(10)]
			public short LinkUsagePage;
			[FieldOffset(12), MarshalAs(UnmanagedType.U1)]
			public bool IsRange;
			[FieldOffset(13), MarshalAs(UnmanagedType.U1)]
			public bool IsStringRange;
			[FieldOffset(14), MarshalAs(UnmanagedType.U1)]
			public bool IsDesignatorRange;
			[FieldOffset(15), MarshalAs(UnmanagedType.U1)]
			public bool IsAbsolute;
			[FieldOffset(16), MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
			public int[] Reserved;
			[FieldOffset(56)]
			public HIDP_Range Range;
			[FieldOffset(56)]
			public HIDP_NotRange NotRange;
		}

		[StructLayout(LayoutKind.Explicit)]
		struct HIDP_VALUE_CAPS
		{
			[FieldOffset(0)]
			public ushort UsagePage;
			[FieldOffset(2)]
			public byte ReportID;
			[FieldOffset(3), MarshalAs(UnmanagedType.U1)]
			public bool IsAlias;
			[FieldOffset(4)]
			public ushort BitField;
			[FieldOffset(6)]
			public ushort LinkCollection;
			[FieldOffset(8)]
			public ushort LinkUsage;
			[FieldOffset(10)]
			public ushort LinkUsagePage;
			[FieldOffset(12), MarshalAs(UnmanagedType.U1)]
			public bool IsRange;
			[FieldOffset(13), MarshalAs(UnmanagedType.U1)]
			public bool IsStringRange;
			[FieldOffset(14), MarshalAs(UnmanagedType.U1)]
			public bool IsDesignatorRange;
			[FieldOffset(15), MarshalAs(UnmanagedType.U1)]
			public bool IsAbsolute;
			[FieldOffset(16), MarshalAs(UnmanagedType.U1)]
			public bool HasNull;
			[FieldOffset(17)]
			public byte Reserved;
			[FieldOffset(18)]
			public short BitSize;
			[FieldOffset(20)]
			public short ReportCount;
			[FieldOffset(22)]
			public ushort Reserved2a;
			[FieldOffset(24)]
			public ushort Reserved2b;
			[FieldOffset(26)]
			public ushort Reserved2c;
			[FieldOffset(28)]
			public ushort Reserved2d;
			[FieldOffset(30)]
			public ushort Reserved2e;
			[FieldOffset(32)]
			public int UnitsExp;
			[FieldOffset(36)]
			public int Units;
			[FieldOffset(40)]
			public int LogicalMin;
			[FieldOffset(44)]
			public int LogicalMax;
			[FieldOffset(48)]
			public int PhysicalMin;
			[FieldOffset(52)]
			public int PhysicalMax;
			[FieldOffset(56)]
			public HIDP_Range Range;
			[FieldOffset(56)]
			public HIDP_NotRange NotRange;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
		struct HID_DEVICE
		{
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = DEVICE_PATH)]
			public string DevicePath;
			public IntPtr HidDevice;
			public bool OpenedForRead;
			public bool OpenedForWrite;
			public bool OpenedOverlapped;
			public bool OpenedExclusive;

			public IntPtr Ppd;
			public HIDP_CAPS Caps;
			public HIDD_ATTRIBUTES Attributes;

			public IntPtr[] InputReportBuffer;
			public HID_DATA[] InputData;
			public Int32 InputDataLength;
			public HIDP_BUTTON_CAPS[] InputButtonCaps;
			public HIDP_VALUE_CAPS[] InputValueCaps;

			public IntPtr[] OutputReportBuffer;
			public HID_DATA[] OutputData;
			public Int32 OutputDataLength;
			public HIDP_BUTTON_CAPS[] OutputButtonCaps;
			public HIDP_VALUE_CAPS[] OutputValueCaps;

			public IntPtr[] FeatureReportBuffer;
			public HID_DATA[] FeatureData;
			public Int32 FeatureDataLength;
			public HIDP_BUTTON_CAPS[] FeatureButtonCaps;
			public HIDP_VALUE_CAPS[] FeatureValueCaps;
		}

		#endregion

		public event ConnectedEventHandler Connected;
		public event ConnectionLostEventHandler ConnectionLost;
		public event DataReceivedEventHandler DataReceived;

		private struct DeviceInfo
		{
			public ushort targetVid;                // Our target device's VID
			public ushort targetPid;                // Our target device's PID
			public bool deviceAttached;             // Device attachment state flag
			public HIDD_ATTRIBUTES attributes;      // HID Attributes
			public HIDP_CAPS capabilities;          // HID Capabilities
			public SafeFileHandle readHandle;       // Read handle from the device
			public SafeFileHandle writeHandle;      // Write handle to the device
			public SafeFileHandle hidHandle;        // Handle used for communicating via hid.dll
			public string devicePathName;           // The device's path name
			public IntPtr deviceNotificationHandle; // The device's notification handle
		}

		private DeviceInfo deviceInformation;

		public enum DeviceNotification
		{
			Attached,
			Detached,
		}

		private readonly DeviceNotificationClient deviceNotificationClient = new DeviceNotificationClient();

		public CommHID(ushort vid, ushort pid)
		{
			Debug.WriteLine("WFF_GenericHID_Communication_Library:WFF_GenericHID_Communication_Library() -> Class constructor called");

			// Set the deviceAttached flag to false
			deviceInformation.deviceAttached = false;

			// Store the target device's VID and PID in the device attributes
			deviceInformation.targetVid = vid;
			deviceInformation.targetPid = pid;

			deviceNotificationClient.DeviceChanged += DeviceChanged;
			deviceInformation.deviceNotificationHandle = deviceNotificationClient.Register();
		}

		private void DeviceChanged(object sender, DeviceNotification notification, string devicePath)
		{
			if (notification == DeviceNotification.Attached)
			{
				if (!isDeviceAttached)
				{
					FindDevice();
					//onUsbEvent(EventArgs.Empty); // Generate an event
				}
			}
			else if (notification == DeviceNotification.Detached)
			{
				var cultureInfo = new System.Globalization.CultureInfo("en-US");
				if (devicePath.ToLower(cultureInfo) == deviceInformation.devicePathName.ToLower(cultureInfo))
				{
					Debug.WriteLine("The target USB device has been removed - detaching...");
					DetachDevice();
					//onUsbEvent(EventArgs.Empty); // Generate an event
				}
			}
		}

		private void DetachDevice()
		{
			Debug.WriteLine("WFF_GenericHID_Communication_Library:detachUsbDevice() -> Method called");

			// Is a device currently attached?
			if (isDeviceAttached)
			{
				Debug.WriteLine("WFF_GenericHID_Communication_Library:detachUsbDevice() -> Detaching device and closing file handles");
				// Set the device status to detached;
				isDeviceAttached = false;

				// Close the readHandle, writeHandle and hidHandle
				if (!deviceInformation.hidHandle.IsInvalid) deviceInformation.hidHandle.Close();
				if (!deviceInformation.readHandle.IsInvalid) deviceInformation.readHandle.Close();
				if (!deviceInformation.writeHandle.IsInvalid) deviceInformation.writeHandle.Close();

				// Throw an event
				//onUsbEvent(EventArgs.Empty);
				ConnectionLost?.Invoke(this);
			}
			else Debug.WriteLine("WFF_GenericHID_Communication_Library:detachUsbDevice() -> No device attached");
		}

		/// <summary>
		/// Is device attached?
		/// </summary>
		/// <remarks>This method is used to set (private) or test (public) the device attachment status</remarks>
		public bool isDeviceAttached
		{
			get
			{
				return deviceInformation.deviceAttached;
			}
			private set
			{
				deviceInformation.deviceAttached = value;
			}
		}

		public int isOutputReportLength
		{
			get
			{
				return (int)deviceInformation.capabilities.OutputReportByteLength;
			}
		}

		public void Initialize()
		{
			FindDevice();
		}

		public void Dispose()
		{
			Debug.WriteLine("CommHID Dispose() called");

			deviceNotificationClient.DeviceChanged -= DeviceChanged;
			DetachDevice();
		}

		public void SendData(byte[] data, int offset, int count)
		{
			byte[] outputBuffer = new Byte[isOutputReportLength];

			Array.Copy(data, 1, outputBuffer, 0, count - 1);
			if ((data[0] & 0x80) == 0)
			{
				writeSingleReportToDevice(outputBuffer);
			}
			data[0] &= 0x7f;
			if (data[0] == 0x01)
			{
				byte[] inputBuffer = new byte[deviceInformation.capabilities.InputReportByteLength];
				inputBuffer[0] = data[1];
				if (readSingleReportFromDevice(ref inputBuffer))
				{
					DataReceived?.Invoke(this, inputBuffer);
				}
			}



			//byte[] outputBuffer = new Byte[isOutputReportLength - 1];

			//Array.Copy(data, 1, outputBuffer, 0, count - 1);
			//writeSingleReportToDevice(outputBuffer);
			//if (data[0] == 0x01)
			//{
			//	byte[] inputBuffer = new byte[deviceInformation.capabilities.InputReportByteLength - 1];
			//	if (readSingleReportFromDevice(ref inputBuffer))
			//	{
			//		DataReceived?.Invoke(this, inputBuffer);
			//	}
			//}
		}

		#region outputToDeviceRegion

		/// <summary>
		/// writeRawReportToDevice - Writes a report to the device using interrupt transfer.
		/// Note: this method performs no checking on the buffer.  The first byte must 
		/// always be zero (or the write will fail!) and the second byte should be the
		/// command number for the USB device firmware.
		/// </summary>
		private bool writeRawReportToDevice(Byte[] outputReportBuffer)
		{
			bool success = false;
			int numberOfBytesWritten = 0;

			// Since the Windows API requires us to have a leading 0 in all file output buffers
			// we must create a 65 byte array, set the first byte to 0 and then copy in the 64
			// bytes of real data in order for the read/write operation to function correctly
			Byte[] adjustedOutputBuffer = new Byte[outputReportBuffer.Length];
			//adjustedOutputBuffer[0] = 0x01;                         // Report ID
			outputReportBuffer.CopyTo(adjustedOutputBuffer, 0);

			// Make sure a device is attached
			if (!isDeviceAttached)
			{
				Debug.WriteLine("writeRawReportToDevice(): -> No device attached!");
				return success;
			}

			try
			{
				// Set an output report via interrupt to the device
				success = WriteFile(
					deviceInformation.writeHandle,
					adjustedOutputBuffer,
					adjustedOutputBuffer.Length,
					ref numberOfBytesWritten,
					IntPtr.Zero);

				if (success) Debug.WriteLine("writeRawReportToDevice(): -> Write report succeeded");
				else
				{
					Debug.WriteLine("writeRawReportToDevice(): -> Write report failed!");
				}

				return success;
			}
			catch (Exception)
			{
				// An error - send out some debug and return failure
				Debug.WriteLine("writeRawReportToDevice(): -> EXCEPTION: When attempting to send an output report");
				return false;
			}
		}

		/// <summary>
		/// writeSingleReportToDevice - Writes a single report packet to the USB device.
		/// The size of the passed outputReportBuffer must be correct for the device, so
		/// this method checks the passed buffer's size against the output report size
		/// discovered by the device enumeration.
		/// </summary>
		/// <param name="outputReportBuffer"></param>
		/// <returns></returns>
		protected bool writeSingleReportToDevice(Byte[] outputReportBuffer)
		{
			bool success;

			// The size of our outputReportBuffer must be at least the same size as the output report (which is length+1).
			if (outputReportBuffer.Length != (int)deviceInformation.capabilities.OutputReportByteLength)
			{
				// outputReportBuffer is not the right length!
				Debug.WriteLine(
					"writeSingleReportToDevice(): -> ERROR: The referenced outputReportBuffer size is incorrect for the output report size!");
				return false;
			}

			// The writeRawReportToDevice method will write the passed buffer or return false
			success = writeRawReportToDevice(outputReportBuffer);

			return success;
		}

		/// <summary>
		/// writeMultipleReportsToDevice - Attempts to write multiple reports to the device in 
		/// a single write.  This action can be block the form execution if you write too much data.
		/// If you need more data to the device and want to avoid any blocking you will have to make
		/// multiple commands to the device and deal with the multiple requests and responses in your
		/// application.
		/// </summary>
		/// <param name="outputReportBuffer"></param>
		/// <param name="numberOfReports"></param>
		/// <returns></returns>
		protected bool writeMultipleReportsToDevice(Byte[] outputReportBuffer, int numberOfReports)
		{
			bool success = false;

			// Range check the number of reports
			if (numberOfReports == 0)
			{
				Debug.WriteLine(
					"writeMultipleReportsToDevice(): -> ERROR: You cannot write 0 reports!");
				return false;
			}

			if (numberOfReports > 128)
			{
				Debug.WriteLine(
					"writeMultipleReportsToDevice(): -> ERROR: Reference application testing does not verify the code for more than 128 reports");
				return false;
			}

			// The size of our outputReportBuffer must be at least the same size as the output report multiplied by the number of reports to be written.
			if (outputReportBuffer.Length != (((int)deviceInformation.capabilities.OutputReportByteLength - 1) * numberOfReports))
			{
				// outputReportBuffer is not the right length!
				Debug.WriteLine(
					"writeMultipleReportsToDevice(): -> ERROR: The referenced outputReportBuffer size is incorrect for the number of output reports requested!");
				return false;
			}

			// The windows API returns a write failure if we try to send more than deviceInformation.capabilities.outputReportByteLength bytes
			// in a single write to the device (would be nice if the HID API handled this, but it doesn't).  Therefore we have to split a multi-
			// report send into 64 byte chunks and send one at a time...

			Int64 reportNumber;
			Byte[] tempOutputBuffer = new Byte[deviceInformation.capabilities.OutputReportByteLength - 1];

			for (reportNumber = 0; reportNumber < numberOfReports; reportNumber++)
			{
				// Copy the next chunk of 64 bytes into the temporary output buffer
				Int64 startByte = 0;
				Int64 pointer = 0;

				for (startByte = reportNumber * 64; startByte < (reportNumber * 64) + 64; startByte++)
				{
					tempOutputBuffer[pointer] = outputReportBuffer[startByte];
					pointer++;
				}

				// The writeRawReportToDevice method will write the passed buffer or return false
				success = writeRawReportToDevice(tempOutputBuffer);

				if (success == false)
				{
					Debug.WriteLine(
					"writeMultipleReportsToDevice(): -> ERROR: Sending failed for report {0} of {0}!",
					reportNumber, numberOfReports);

					// Give up
					return false;
				}
			}

			return success;
		}
		#endregion

		#region inputFromDeviceRegion
		/// <summary>
		/// readRawReportFromDevice - Reads a raw report from the device with timeout handling
		/// Note: This method performs no checking on the buffer.  The first byte returned is
		/// usually zero, the second byte is the command that the USB firmware is replying to.
		/// The other 63 bytes are (possibly) data.
		/// 
		/// The maximum report size will be determind by the length of the inputReportBuffer.
		/// </summary>
		private bool readRawReportFromDevice(ref Byte[] inputReportBuffer, ref int numberOfBytesRead)
		{
			IntPtr eventObject = IntPtr.Zero;
			NativeOverlapped hidOverlapped = new NativeOverlapped();
			IntPtr nonManagedBuffer = IntPtr.Zero;
			IntPtr nonManagedOverlapped = IntPtr.Zero;
			Int32 result = 0;
			bool success;

			// Since the Windows API requires us to have a leading 0 in all file intput buffers
			// we must create a 65 byte array, set the first byte to 0 and then copy in the 64
			// bytes of real data in order for the read/write operation to function correctly
			Byte[] adjustedInputBuffer = new Byte[inputReportBuffer.Length];
			//adjustedInputBuffer[0] = 0;
			adjustedInputBuffer[0] = inputReportBuffer[0];

			// Make sure a device is attached
			if (!isDeviceAttached)
			{
				Debug.WriteLine("readRawReportFromDevice(): -> No device attached!");
				return false;
			}

			try
			{
				// Prepare an event object for the overlapped ReadFile
				eventObject = CreateEvent(IntPtr.Zero, false, false, "");

				hidOverlapped.OffsetLow = 0;
				hidOverlapped.OffsetHigh = 0;
				hidOverlapped.EventHandle = eventObject;

				// Allocate memory for the unmanaged input buffer and overlap structure.
				nonManagedBuffer = Marshal.AllocHGlobal(adjustedInputBuffer.Length);
				nonManagedOverlapped = Marshal.AllocHGlobal(Marshal.SizeOf(hidOverlapped));
				Marshal.StructureToPtr(hidOverlapped, nonManagedOverlapped, false);


				// Read the input report buffer
				//                Debug.WriteLine("readRawReportFromDevice(): -> Attempting to ReadFile");
				success = ReadFile(
					deviceInformation.readHandle,
					nonManagedBuffer,
					adjustedInputBuffer.Length,
					ref numberOfBytesRead,
					nonManagedOverlapped);

				Marshal.Copy(nonManagedBuffer, adjustedInputBuffer, 0, numberOfBytesRead);
				//                Debug.WriteLine("readRawReportFromDevice(): " + adjustedInputBuffer[0] + "  " + adjustedInputBuffer[1] + "  " + adjustedInputBuffer[2]);

				if (!success)
				{
					// We are now waiting for the FileRead to complete
					//                    Debug.WriteLine("readRawReportFromDevice(): -> ReadFile started, waiting for completion...");

					// Wait a maximum of 3 seconds for the FileRead to complete
					result = WaitForSingleObject(eventObject, 1000);

					switch (result)
					{
						// Has the ReadFile completed successfully?
						case (System.Int32)WAIT_OBJECT_0:
							success = true;

							// Get the number of bytes transferred
							GetOverlappedResult(deviceInformation.readHandle, nonManagedOverlapped, ref numberOfBytesRead, false);

							Debug.WriteLine(string.Format("readRawReportFromDevice(): -> ReadFile successful (overlapped) {0} bytes read", numberOfBytesRead));
							break;

						// Did the FileRead operation timeout?
						case WAIT_TIMEOUT:
							success = false;

							Debug.WriteLine("readRawReportFromDevice(): -> ReadFile timedout! USB device detached");

							// Cancel the ReadFile operation
							CancelIo(deviceInformation.readHandle);
							//if (!deviceInformation.hidHandle.IsInvalid) deviceInformation.hidHandle.Close();
							//if (!deviceInformation.readHandle.IsInvalid) deviceInformation.readHandle.Close();
							//if (!deviceInformation.writeHandle.IsInvalid) deviceInformation.writeHandle.Close();

							// Detach the USB device to try to get us back in a known state
							//DetachDevice();

							break;

						// Some other unspecified error has occurred?
						default:
							success = false;

							// Cancel the ReadFile operation

							// Detach the USB device to try to get us back in a known state
							//DetachDevice();

							Debug.WriteLine("readRawReportFromDevice(): -> ReadFile unspecified error! USB device detached");
							break;
					}
				}
				if (success)
				{
					// Report receieved correctly, copy the unmanaged input buffer over to the managed array buffer
					Marshal.Copy(nonManagedBuffer, adjustedInputBuffer, 0, numberOfBytesRead);

					// Make sure we didn't get a successful read with 0 bytes back
					if (numberOfBytesRead > 0)
					{
						// Now we need to loose the leading 0 byte and transfer the buffer over to the real input buffer
						// (I couldn't find a nicer way of doing this with a managed array of bytes, but if you know of 
						// one, let me know ;)
						Int64 byteCounter;
						for (byteCounter = 0; byteCounter < adjustedInputBuffer.Length; byteCounter++)
							inputReportBuffer[byteCounter] = adjustedInputBuffer[byteCounter];

						// Adjust the number of bytes read (since it's returned by reference)
						numberOfBytesRead -= 1;
					}

					Debug.WriteLine(string.Format("readRawReportFromDevice(): -> ReadFile successful returning {0} bytes", numberOfBytesRead));
				}
			}
			catch (Exception)
			{
				// An error - send out some debug and return failure
				Debug.WriteLine("readRawReportFromDevice(): -> EXCEPTION: When attempting to receive an input report");
				return false;
			}

			// Release non-managed objects before returning
			Marshal.FreeHGlobal(nonManagedBuffer);
			Marshal.FreeHGlobal(nonManagedOverlapped);

			// Close the file handle to release the object
			CloseHandle(eventObject);

			return success;
		}

		/// <summary>
		/// readSingleReportFromDevice - Reads a single report packet from the USB device.
		/// The size of the passed inputReportBuffer must be correct for the device, so
		/// this method checks the passed buffer's size against the input report size
		/// discovered by the device enumeration.
		/// </summary>
		/// <param name="inputReportBuffer"></param>
		/// <returns></returns>
		protected bool readSingleReportFromDevice(ref Byte[] inputReportBuffer)
		{
			bool success;
			int numberOfBytesRead = 0;

			//            Debug.WriteLine("readSingleReportFromDevice(): " + inputReportBuffer.Length + "   " + (int)deviceInformation.capabilities.inputReportByteLength);

			// The size of our inputReportBuffer must be at least the same size as the input report.
			if (inputReportBuffer.Length != (deviceInformation.capabilities.InputReportByteLength))
			{
				// inputReportBuffer is not the right length!
				Debug.WriteLine(
					"readSingleReportFromDevice(): -> ERROR: The referenced inputReportBuffer size is incorrect for the input report size!");
				return false;
			}

			// The readRawReportFromDevice method will fill the passed readBuffer or return false
			success = readRawReportFromDevice(ref inputReportBuffer, ref numberOfBytesRead);

			return success;
		}

		/// <summary>
		/// readMultipleReportsFromDevice - Attempts to retrieve multiple reports from the device in 
		/// a single read.  This action can be block the form execution if you request too much data.
		/// If you need more data from the device and want to avoid any blocking you will have to make
		/// multiple commands to the device and deal with the multiple requests and responses in your
		/// application.
		/// </summary>
		/// <param name="inputReportBuffer"></param>
		/// <param name="numberOfReports"></param>
		/// <returns></returns>
		protected bool readMultipleReportsFromDevice(ref Byte[] inputReportBuffer, int numberOfReports)
		{
			bool success = false;
			int numberOfBytesRead = 0;
			long pointerToBuffer = 0;

			// Note: The Windows HID API always returns with 65 bytes (i.e. a leading 0 and 64 real bytes) no
			// matter how much data we request from the ReadFile, so this method is effectively the same as
			// calling readRawReportFromDevice many times with a 64 byte buffer.  However it provides a nice
			// abstraction so it is kept.

			// Define a temporary buffer for assembling partial data reads into the completed inputReportBuffer
			Byte[] temporaryBuffer = new Byte[deviceInformation.capabilities.InputReportByteLength - 1];

			// Range check the number of reports
			if (numberOfReports == 0)
			{
				Debug.WriteLine(
					"readMultipleReportsFromDevice(): -> ERROR: You cannot request 0 reports!");
				return false;
			}

			if (numberOfReports > 128)
			{
				Debug.WriteLine(
					"readMultipleReportsFromDevice(): -> ERROR: Reference application testing does not verify the code for more than 128 reports");
				return false;
			}

			// The size of our inputReportBuffer must be at least the same size as the input report multiplied by the number of reports requested.
			if (inputReportBuffer.Length != (((int)deviceInformation.capabilities.InputReportByteLength - 1) * numberOfReports))
			{
				// inputReportBuffer is not the right length!
				Debug.WriteLine(
					"readMultipleReportsFromDevice(): -> ERROR: The referenced inputReportBuffer size is incorrect for the number of input reports requested!");
				return false;
			}

			// The readRawReportFromDevice method will fill the passed read buffer or return false
			while (pointerToBuffer != (((int)deviceInformation.capabilities.InputReportByteLength - 1) * numberOfReports))
			{
				Debug.WriteLine(
					"readMultipleReportsFromDevice(): -> Reading from device...");
				success = readRawReportFromDevice(ref temporaryBuffer, ref numberOfBytesRead);

				// Was the read successful?
				if (!success)
				{
					return false;
				}

				// Copy the received data into the referenced input buffer
				Array.Copy(temporaryBuffer, 0, inputReportBuffer, pointerToBuffer, (long)numberOfBytesRead);
				pointerToBuffer += (long)numberOfBytesRead;
			}


			Debug.WriteLine(
				"readMultipleReportsFromDevice(): -> Got {0} bytes from device", pointerToBuffer);
			return success;
		}

		#endregion

		/// <summary>
		/// Find all devices with the HID GUID attached to the system
		/// </summary>
		/// <remarks>This method searches for all devices that have the correct HID GUID and
		/// returns an array of matching device paths</remarks>
		private bool FindHIDDevices(ref String[] listOfDevicePathNames, ref int numberOfDevicesFound)
		{
			Debug.WriteLine("findHidDevices() -> Method called");
			// Detach the device if it's currently attached
			if (isDeviceAttached) DetachDevice();

			// Initialise the internal variables required for performing the search
			Int32 bufferSize = 0;
			IntPtr detailDataBuffer = IntPtr.Zero;
			Boolean deviceFound;
			IntPtr deviceInfoSet = new System.IntPtr();
			Boolean lastDevice = false;
			Int32 listIndex = 0;
			SP_DEVICE_INTERFACE_DATA deviceInterfaceData = new SP_DEVICE_INTERFACE_DATA();
			Boolean success;

			// Get the required GUID
			System.Guid systemHidGuid = new Guid();
			HidD_GetHidGuid(ref systemHidGuid);
			Debug.WriteLine(string.Format("findHidDevices() -> Fetched GUID for HID devices ({0})", systemHidGuid.ToString()));

			try
			{
				// Here we populate a list of plugged-in devices matching our class GUID (DIGCF_PRESENT specifies that the list
				// should only contain devices which are plugged in)
				Debug.WriteLine("findHidDevices() -> Using SetupDiGetClassDevs to get all devices with the correct GUID");
				deviceInfoSet = SetupDiGetClassDevs(ref systemHidGuid, IntPtr.Zero, IntPtr.Zero, DIGCF_PRESENT | DIGCF_DEVICEINTERFACE);

				// Reset the deviceFound flag and the memberIndex counter
				deviceFound = false;
				listIndex = 0;

				deviceInterfaceData.cbSize = Marshal.SizeOf(deviceInterfaceData);

				// Look through the retrieved list of class GUIDs looking for a match on our interface GUID
				do
				{
					//Debug.WriteLine("findHidDevices() -> Enumerating devices");
					success = SetupDiEnumDeviceInterfaces
						(deviceInfoSet,
						IntPtr.Zero,
						ref systemHidGuid,
						listIndex,
						ref deviceInterfaceData);

					if (!success)
					{
						//Debug.WriteLine("findHidDevices() -> No more devices left - giving up");
						lastDevice = true;
					}
					else
					{
						// The target device has been found, now we need to retrieve the device path so we can open
						// the read and write handles required for USB communication

						// First call is just to get the required buffer size for the real request
						success = SetupDiGetDeviceInterfaceDetail
							(deviceInfoSet,
							ref deviceInterfaceData,
							IntPtr.Zero,
							0,
							ref bufferSize,
							IntPtr.Zero);

						// Allocate some memory for the buffer
						detailDataBuffer = Marshal.AllocHGlobal(bufferSize);
						Marshal.WriteInt32(detailDataBuffer, (IntPtr.Size == 4) ? (4 + Marshal.SystemDefaultCharSize) : 8);

						// Second call gets the detailed data buffer
						//Debug.WriteLine("findHidDevices() -> Getting details of the device");
						success = SetupDiGetDeviceInterfaceDetail
							(deviceInfoSet,
							ref deviceInterfaceData,
							detailDataBuffer,
							bufferSize,
							ref bufferSize,
							IntPtr.Zero);

						// Skip over cbsize (4 bytes) to get the address of the devicePathName.
						IntPtr pDevicePathName = new IntPtr(detailDataBuffer.ToInt32() + 4);

						// Get the String containing the devicePathName.
						listOfDevicePathNames[listIndex] = Marshal.PtrToStringAuto(pDevicePathName);

						//Debug.WriteLine(string.Format("findHidDevices() -> Found matching device (memberIndex {0})", memberIndex));
						deviceFound = true;
					}
					listIndex = listIndex + 1;
				}
				while (!((lastDevice == true)));
			}
			catch (Exception)
			{
				// Something went badly wrong... output some debug and return false to indicated device discovery failure
				Debug.WriteLine("findHidDevices() -> EXCEPTION: Something went south whilst trying to get devices with matching GUIDs - giving up!");
				return false;
			}
			finally
			{
				// Clean up the unmanaged memory allocations
				if (detailDataBuffer != IntPtr.Zero)
				{
					// Free the memory allocated previously by AllocHGlobal.
					Marshal.FreeHGlobal(detailDataBuffer);
				}

				if (deviceInfoSet != IntPtr.Zero)
				{
					SetupDiDestroyDeviceInfoList(deviceInfoSet);
				}
			}

			if (deviceFound)
			{
				Debug.WriteLine(string.Format("findHidDevices() -> Found {0} devices with matching GUID", listIndex - 1));
				numberOfDevicesFound = listIndex - 2;
			}
			else Debug.WriteLine("findHidDevices() -> No matching devices found");

			return deviceFound;
		}

		/// <summary>
		/// Find the target device based on the VID and PID
		/// </summary>
		/// <remarks>This method used the 'findHidDevices' to fetch a list of attached HID devices
		/// then it searches through the list looking for a HID device with the correct VID and
		/// PID</remarks>
		public void FindDevice()
		{
			Debug.WriteLine("findTargetDevice() -> Method called");

			bool deviceFoundByGuid = false;
			String[] listOfDevicePathNames = new String[128]; // 128 is the maximum number of USB devices allowed on a single host
			int listIndex = 0;
			bool success = false;
			bool isDeviceDetected;
			int numberOfDevicesFound = 0;

			try
			{
				// Reset the device detection flag
				isDeviceDetected = false;

				// Get all the devices with the correct HID GUID
				deviceFoundByGuid = FindHIDDevices(ref listOfDevicePathNames, ref numberOfDevicesFound);

				if (deviceFoundByGuid)
				{
					Debug.WriteLine("findTargetDevice() -> Devices with matching GUID found...");
					listIndex = 0;

					do
					{
						Debug.WriteLine(string.Format("findTargetDevice() -> Performing CreateFile to listIndex {0}", listIndex));
						deviceInformation.hidHandle = CreateFile(listOfDevicePathNames[listIndex], 0, FILE_SHARE_READ | FILE_SHARE_WRITE, IntPtr.Zero, OPEN_EXISTING, 0, 0);

						if (!deviceInformation.hidHandle.IsInvalid)
						{
							deviceInformation.attributes.Size = Marshal.SizeOf(deviceInformation.attributes);
							success = HidD_GetAttributes(deviceInformation.hidHandle, ref deviceInformation.attributes);

							if (success)
							{
								Debug.WriteLine(string.Format("findTargetDevice() -> Found device with VID {0}, PID {1} and Version number {2}",
									Convert.ToString(deviceInformation.attributes.VendorID, 16),
									Convert.ToString(deviceInformation.attributes.ProductID, 16),
									Convert.ToString(deviceInformation.attributes.VersionNumber, 16)));

								//  Do the VID and PID of the device match our target device?
								if ((deviceInformation.attributes.VendorID == deviceInformation.targetVid) &&
									(deviceInformation.attributes.ProductID == deviceInformation.targetPid))
								{
									// Matching device found
									Debug.WriteLine("findTargetDevice() -> Device with matching VID and PID found!");
									isDeviceDetected = true;

									// Store the device's pathname in the device information
									deviceInformation.devicePathName = listOfDevicePathNames[listIndex];
								}
								else
								{
									// Wrong device, close the handle
									Debug.WriteLine("findTargetDevice() -> Device didn't match... Continuing...");
									isDeviceDetected = false;
									deviceInformation.hidHandle.Close();
								}
							}
							else
							{
								//  Something went rapidly south...  give up!
								Debug.WriteLine("findTargetDevice() -> Something bad happened - couldn't fill the HIDD_ATTRIBUTES, giving up!");
								isDeviceDetected = false;
								deviceInformation.hidHandle.Close();
							}
						}

						//  Move to the next device, or quit if there are no more devices to examine
						listIndex++;
					}
					while (!((isDeviceDetected || (listIndex == numberOfDevicesFound + 1))));
				}

				// If we found a matching device then we need discover more details about the attached device
				// and then open read and write handles to the device to allow communication
				if (isDeviceDetected)
				{
					// Query the HID device's capabilities (primarily we are only really interested in the 
					// input and output report byte lengths as this allows us to validate information sent
					// to and from the device does not exceed the devices capabilities.
					//
					// We could determine the 'type' of HID device here too, but since this class is only
					// for generic HID communication we don't care...
					queryDeviceCapabilities();

					if (success)
					{
						// Open the readHandle to the device
						Debug.WriteLine("findTargetDevice() -> Opening a readHandle to the device");
						deviceInformation.readHandle = CreateFile(
							deviceInformation.devicePathName,
							GENERIC_READ,
							FILE_SHARE_READ | FILE_SHARE_WRITE,
							IntPtr.Zero, OPEN_EXISTING,
							FILE_FLAG_OVERLAPPED,
							0);

						// Did we open the readHandle successfully?
						if (deviceInformation.readHandle.IsInvalid)
						{
							Debug.WriteLine("findTargetDevice() -> Unable to open a readHandle to the device!");
						}
						else
						{
							Debug.WriteLine("findTargetDevice() -> Opening a writeHandle to the device");
							deviceInformation.writeHandle = CreateFile(
								deviceInformation.devicePathName,
								GENERIC_WRITE,
								FILE_SHARE_READ | FILE_SHARE_WRITE,
								IntPtr.Zero,
								OPEN_EXISTING,
								0,
								0);

							// Did we open the writeHandle successfully?
							if (deviceInformation.writeHandle.IsInvalid)
							{
								Debug.WriteLine("findTargetDevice() -> Unable to open a writeHandle to the device!");

								// Attempt to close the writeHandle
								deviceInformation.writeHandle.Close();
							}
							else
							{
								// Device is now discovered and ready for use, update the status
								isDeviceAttached = true;
								Connected?.Invoke(this);
								//onUsbEvent(EventArgs.Empty); // Throw an event
							}
						}
					}
				}
				else
				{
					//  The device wasn't detected.
					Debug.WriteLine("findTargetDevice() -> No matching device found!");
				}
			}
			catch (Exception)
			{
				Debug.WriteLine("findTargetDevice() -> EXCEPTION: Unknown - device not found");
				isDeviceDetected = false;
			}
		}

		/// <summary>
		/// queryDeviceCapabilities - Used the hid.dll function to query the capabilities
		/// of the target device.
		/// </summary>
		private void queryDeviceCapabilities()
		{
			IntPtr preparsedData = new System.IntPtr();
			Int32 result = 0;
			Boolean success = false;

			try
			{
				// Get the preparsed data from the HID driver
				success = HidD_GetPreparsedData(deviceInformation.hidHandle, ref preparsedData);

				// Get the HID device's capabilities
				result = HidP_GetCaps(preparsedData, ref deviceInformation.capabilities);
				if ((result != 0))
				{
					Debug.WriteLine("queryDeviceCapabilities() -> Device query results:");
					Debug.WriteLine(string.Format("queryDeviceCapabilities() ->     Usage: {0}",
						Convert.ToString(deviceInformation.capabilities.Usage, 16)));
					Debug.WriteLine(string.Format("queryDeviceCapabilities() ->     Usage Page: {0}",
						Convert.ToString(deviceInformation.capabilities.UsagePage, 16)));
					Debug.WriteLine(string.Format("queryDeviceCapabilities() ->     Input Report Byte Length: {0}",
						deviceInformation.capabilities.InputReportByteLength));
					Debug.WriteLine(string.Format("queryDeviceCapabilities() ->     Output Report Byte Length: {0}",
						deviceInformation.capabilities.OutputReportByteLength));
					Debug.WriteLine(string.Format("queryDeviceCapabilities() ->     Feature Report Byte Length: {0}",
						deviceInformation.capabilities.FeatureReportByteLength));
					Debug.WriteLine(string.Format("queryDeviceCapabilities() ->     Number of Link Collection Nodes: {0}",
						deviceInformation.capabilities.NumberLinkCollectionNodes));
					Debug.WriteLine(string.Format("queryDeviceCapabilities() ->     Number of Input Button Caps: {0}",
						deviceInformation.capabilities.NumberInputButtonCaps));
					Debug.WriteLine(string.Format("queryDeviceCapabilities() ->     Number of Input Value Caps: {0}",
						deviceInformation.capabilities.NumberInputValueCaps));
					Debug.WriteLine(string.Format("queryDeviceCapabilities() ->     Number of Input Data Indices: {0}",
						deviceInformation.capabilities.NumberInputDataIndices));
					Debug.WriteLine(string.Format("queryDeviceCapabilities() ->     Number of Output Button Caps: {0}",
						deviceInformation.capabilities.NumberOutputButtonCaps));
					Debug.WriteLine(string.Format("queryDeviceCapabilities() ->     Number of Output Value Caps: {0}",
						deviceInformation.capabilities.NumberOutputValueCaps));
					Debug.WriteLine(string.Format("queryDeviceCapabilities() ->     Number of Output Data Indices: {0}",
						deviceInformation.capabilities.NumberOutputDataIndices));
					Debug.WriteLine(string.Format("queryDeviceCapabilities() ->     Number of Feature Button Caps: {0}",
						deviceInformation.capabilities.NumberFeatureButtonCaps));
					Debug.WriteLine(string.Format("queryDeviceCapabilities() ->     Number of Feature Value Caps: {0}",
						deviceInformation.capabilities.NumberFeatureValueCaps));
					Debug.WriteLine(string.Format("queryDeviceCapabilities() ->     Number of Feature Data Indices: {0}",
						deviceInformation.capabilities.NumberFeatureDataIndices));
				}
			}
			catch (Exception)
			{
				// Something went badly wrong... this shouldn't happen, so we throw an exception
				Debug.WriteLine("queryDeviceCapabilities() -> EXECEPTION: An unrecoverable error has occurred!");
				throw;
			}
			finally
			{
				// Free up the memory before finishing
				if (preparsedData != IntPtr.Zero)
				{
					success = HidD_FreePreparsedData(preparsedData);
				}
			}
		}














		//void HIDRead()
		//{
		//	HID_DEVICE[] pDevice = new HID_DEVICE[1];

		//	while (true)
		//	{
		//		Thread.Sleep(1);

		//		if (nbrDevice != FindNumberDevices() || NewRead)
		//		{
		//			nbrDevice = FindNumberDevices();
		//			pDevice = new HID_DEVICE[nbrDevice];
		//			FindKnownHidDevices(ref pDevice);

		//			var Index = 0;
		//			while (Index < nbrDevice)
		//			{
		//				var Count = 0;
		//				if (pDevice[Index].Attributes.VendorID == DEVICE_VID && DEVICE_VID != 0)
		//				{
		//					Count++;
		//				}
		//				if (pDevice[Index].Attributes.ProductID == DEVICE_PID)
		//				{
		//					Count++;
		//				}

		//				if (Count == 2)
		//				{
		//					iDeviceRead = Index;
		//					isRead = true;

		//					break;
		//				}
		//				else
		//					isRead = false;

		//				Index++;
		//			}

		//			NewRead = false;
		//		}

		//		if (isRead)
		//		{
		//			Read(pDevice[iDeviceRead]);
		//		}
		//	}
		//}
		//void HIDSend()
		//{
		//	HID_DEVICE[] pDevice = new HID_DEVICE[1];

		//	while (true)
		//	{
		//		Thread.Sleep(1);

		//		if (nbrDevice != FindNumberDevices() || NewSend)
		//		{
		//			nbrDevice = FindNumberDevices();
		//			pDevice = new HID_DEVICE[nbrDevice];
		//			FindKnownHidDevices(ref pDevice);

		//			var Index = 0;
		//			while (Index < nbrDevice)
		//			{
		//				var Count = 0;

		//				if (pDevice[Index].Attributes.VendorID == DEVICE_VIDa && DEVICE_VIDa != 0)
		//				{
		//					Count++;
		//				}
		//				if (pDevice[Index].Attributes.ProductID == DEVICE_PIDa)
		//				{
		//					Count++;
		//				}
		//				if (pDevice[Index].Caps.UsagePage == USAGE_PAGE)
		//				{
		//					Count++;
		//				}
		//				if (pDevice[Index].Caps.Usage == USAGE)
		//				{
		//					Count++;
		//				}

		//				if (Count == 4)
		//				{
		//					iDeviceSend = Index;
		//					isSend = true;

		//					break;
		//				}
		//				else
		//					isSend = false;

		//				Index++;
		//			}

		//			NewSend = false;
		//		}

		//		if (isSend)
		//		{
		//			Write(pDevice[iDeviceSend]);
		//		}
		//	}
		//}

		//void Read(HID_DEVICE HidDevice)  // Read the Report[] and 
		//								 // send it to the USB device targeted 
		//								 // by HidDevice.HidDevice
		//{
		//	Byte[] Report = new Byte[HidDevice.Caps.InputReportByteLength];
		//	UInt32 tmp = 0;

		//	try
		//	{
		//		Report[0] = REPORT_ID;
		//	}
		//	catch { }

		//	ReadFile(HidDevice.HidDevice, Report, HidDevice.Caps.InputReportByteLength, ref tmp, IntPtr.Zero);

		//	//try
		//	//{
		//	//	textBox_Read.Clear();
		//	//}
		//	//catch { }

		//	//var Index = 0;
		//	//while (Index < HidDevice.Caps.InputReportByteLength)
		//	//{
		//	//	try
		//	//	{
		//	//		textBox_Read.Text += Report[Index++].ToString("X2");
		//	//		textBox_Read.Text += ", ";
		//	//	}
		//	//	catch { }
		//	//}
		//}
		//void Write(HID_DEVICE HidDevice) // Read what the USB device has sent 
		//								 // to the PC and store the result into Report[]
		//{
		//	Byte[] Report = new Byte[HidDevice.Caps.OutputReportByteLength];
		//	UInt32 tmp = 0;

		//	try
		//	{
		//		Report[0] = REPORT_ID;
		//		Report[1] = WriteData;
		//	}
		//	catch { }

		//	WriteFile(HidDevice.HidDevice, Report, HidDevice.Caps.OutputReportByteLength, ref tmp, IntPtr.Zero);
		//}

		//int FindNumberDevices()
		//{
		//	Guid hidGuid = new Guid();
		//	SP_DEVICE_INTERFACE_DATA deviceInfoData = new SP_DEVICE_INTERFACE_DATA();
		//	int index = 0;

		//	HidD_GetHidGuid(ref hidGuid);

		//	//
		//	// Open a handle to the plug and play dev node.
		//	//
		//	SetupDiDestroyDeviceInfoList(hardwareDeviceInfo);
		//	hardwareDeviceInfo = SetupDiGetClassDevs(ref hidGuid, IntPtr.Zero, IntPtr.Zero, DIGCF_PRESENT | DIGCF_DEVICEINTERFACE);
		//	deviceInfoData.cbSize = Marshal.SizeOf(typeof(SP_DEVICE_INTERFACE_DATA));

		//	index = 0;
		//	while (SetupDiEnumDeviceInterfaces(hardwareDeviceInfo, IntPtr.Zero, ref hidGuid, index, ref deviceInfoData))
		//	{
		//		index++;
		//	}

		//	return (index);
		//}
		//int FindKnownHidDevices(ref HID_DEVICE[] HidDevices)
		//{
		//	int iHIDD;
		//	int RequiredLength;

		//	Guid hidGuid = new Guid();
		//	SP_DEVICE_INTERFACE_DATA deviceInfoData = new SP_DEVICE_INTERFACE_DATA();
		//	SP_DEVICE_INTERFACE_DETAIL_DATA functionClassDeviceData = new SP_DEVICE_INTERFACE_DETAIL_DATA();

		//	HidD_GetHidGuid(ref hidGuid);

		//	//
		//	// Open a handle to the plug and play dev node.
		//	//
		//	SetupDiDestroyDeviceInfoList(hardwareDeviceInfo);
		//	hardwareDeviceInfo = SetupDiGetClassDevs(ref hidGuid, IntPtr.Zero, IntPtr.Zero, DIGCF_PRESENT | DIGCF_DEVICEINTERFACE);
		//	deviceInfoData.cbSize = Marshal.SizeOf(typeof(SP_DEVICE_INTERFACE_DATA));

		//	iHIDD = 0;
		//	while (SetupDiEnumDeviceInterfaces(hardwareDeviceInfo, IntPtr.Zero, ref hidGuid, iHIDD, ref deviceInfoData))
		//	{
		//		RequiredLength = 0;

		//		//
		//		// allocate a function class device data structure to receive the
		//		// goods about this particular device.
		//		//
		//		SetupDiGetDeviceInterfaceDetail(hardwareDeviceInfo, ref deviceInfoData, IntPtr.Zero, 0, ref RequiredLength, IntPtr.Zero);

		//		if (IntPtr.Size == 8)
		//			functionClassDeviceData.cbSize = 8;
		//		else if (IntPtr.Size == 4)
		//			functionClassDeviceData.cbSize = 5;

		//		//
		//		// Retrieve the information from Plug and Play.
		//		//
		//		SetupDiGetDeviceInterfaceDetail(hardwareDeviceInfo, ref deviceInfoData, ref functionClassDeviceData, RequiredLength, ref RequiredLength, IntPtr.Zero);

		//		//
		//		// Open device with just generic query abilities to begin with
		//		//
		//		OpenHidDevice(functionClassDeviceData.DevicePath, ref HidDevices, iHIDD);

		//		iHIDD++;
		//	}

		//	return iHIDD;
		//}
		//void OpenHidDevice(string DevicePath, ref HID_DEVICE[] HidDevice, int iHIDD)
		//{
		//	/*++
		//          RoutineDescription:
		//          Given the HardwareDeviceInfo, representing a handle to the plug and
		//          play information, and deviceInfoData, representing a specific hid device,
		//          open that device and fill in all the relivant information in the given
		//          HID_DEVICE structure.
		//          --*/

		//	HidDevice[iHIDD].DevicePath = DevicePath;

		//	//
		//	//  The hid.dll api's do not pass the overlapped structure into deviceiocontrol
		//	//  so to use them we must have a non overlapped device.  If the request is for
		//	//  an overlapped device we will close the device below and get a handle to an
		//	//  overlapped device
		//	//
		//	CloseHandle(HidDevice[iHIDD].HidDevice);
		//	HidDevice[iHIDD].HidDevice = CreateFile(HidDevice[iHIDD].DevicePath, GENERIC_READ | GENERIC_WRITE, FILE_SHARE_READ | FILE_SHARE_WRITE, 0, OPEN_EXISTING, 0, IntPtr.Zero);
		//	HidDevice[iHIDD].Caps = new HIDP_CAPS();
		//	HidDevice[iHIDD].Attributes = new HIDD_ATTRIBUTES();

		//	//
		//	// If the device was not opened as overlapped, then fill in the rest of the
		//	//  HidDevice structure.  However, if opened as overlapped, this handle cannot
		//	//  be used in the calls to the HidD_ exported functions since each of these
		//	//  functions does synchronous I/O.
		//	//
		//	HidD_FreePreparsedData(ref HidDevice[iHIDD].Ppd);
		//	HidDevice[iHIDD].Ppd = IntPtr.Zero;
		//	HidD_GetPreparsedData(HidDevice[iHIDD].HidDevice, ref HidDevice[iHIDD].Ppd);
		//	HidD_GetAttributes(HidDevice[iHIDD].HidDevice, ref HidDevice[iHIDD].Attributes);
		//	HidP_GetCaps(HidDevice[iHIDD].Ppd, ref HidDevice[iHIDD].Caps);

		//	//MessageBox.Show(GetLastError().ToString());

		//	//
		//	// At this point the client has a choice.  It may chose to look at the
		//	// Usage and Page of the top level collection found in the HIDP_CAPS
		//	// structure.  In this way --------*it could just use the usages it knows about.
		//	// If either HidP_GetUsages or HidP_GetUsageValue return an error then
		//	// that particular usage does not exist in the report.
		//	// This is most likely the preferred method as the application can only
		//	// use usages of which it already knows.
		//	// In this case the app need not even call GetButtonCaps or GetValueCaps.
		//	//
		//	// In this example, however, we will call FillDeviceInfo to look for all
		//	//    of the usages in the device.
		//	//
		//	//FillDeviceInfo(ref HidDevice);
		//}

		//private void textBox_VID_TextChanged(object sender, EventArgs e)
		//{
		//	uint number;

		//	UInt32.TryParse(textBox_VID.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out number);

		//	DEVICE_VID = (ushort)number;
		//}
		//private void textBox_PID_TextChanged(object sender, EventArgs e)
		//{
		//	uint number;

		//	UInt32.TryParse(textBox_PID.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out number);

		//	DEVICE_PID = (ushort)number;
		//}
		//private void textBox_Send_TextChanged(object sender, EventArgs e)
		//{
		//	WriteData = byte.Parse(textBox_Send.Text);
		//}
		//private void textBox_VIDa_TextChanged(object sender, EventArgs e)
		//{
		//	uint number;

		//	UInt32.TryParse(textBox_VIDa.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out number);

		//	DEVICE_VIDa = (ushort)number;
		//}
		//private void textBox_PIDa_TextChanged(object sender, EventArgs e)
		//{
		//	uint number;

		//	UInt32.TryParse(textBox_PIDa.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out number);

		//	DEVICE_PIDa = (ushort)number;
		//}
		//private void textBox_UsagePage_TextChanged(object sender, EventArgs e)
		//{
		//	uint number;

		//	UInt32.TryParse(textBox_UsagePage.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out number);

		//	USAGE_PAGE = (ushort)number;
		//}
		//private void textBox_Usage_TextChanged(object sender, EventArgs e)
		//{
		//	uint number;

		//	UInt32.TryParse(textBox_Usage.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out number);

		//	USAGE = (ushort)number;
		//}
		//private void textBox_RID_TextChanged(object sender, EventArgs e)
		//{
		//	uint number;

		//	UInt32.TryParse(textBox_RID.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out number);

		//	REPORT_ID = (byte)number;
		//}
		//private void button_Read_Click(object sender, EventArgs e)
		//{
		//	NewRead = !NewRead;
		//}
		//private void button_Send_Click(object sender, EventArgs e)
		//{
		//	NewSend = !NewSend;
		//}

		/* void FillDeviceInfo(ref HID_DEVICE[] HidDevice)
        {
            HIDP_BUTTON_CAPS[]  buttonCaps;
            HIDP_VALUE_CAPS[]   valueCaps;
            HID_DATA[]          data;
            int                 Index, numValues;
            ushort              numCaps, usage;

            //
            // setup Input Data buffers.
            //

            //
            // Allocate memory to hold on input report
            //
            HidDevice[iHIDD].InputReportBuffer = new IntPtr[HidDevice[iHIDD].Caps.InputReportByteLength];

            //HidDevice.InputReportBuffer = Marshal.AllocHGlobal();

            //
            // Allocate memory to hold the button and value capabilities.
            // NumberXXCaps is in terms of array elements.
            //
            HidDevice[iHIDD].InputButtonCaps   = buttonCaps = new HIDP_BUTTON_CAPS[HidDevice[iHIDD].Caps.NumberInputButtonCaps];
            HidDevice[iHIDD].InputValueCaps    = valueCaps  = new HIDP_VALUE_CAPS[HidDevice[iHIDD].Caps.NumberInputValueCaps];

            //
            // Have the HidP_X functions fill in the capability structure arrays.
            //
            numCaps = HidDevice[iHIDD].Caps.NumberInputButtonCaps;
            if (numCaps > 0)
            {
                HidP_GetButtonCaps(HIDP_REPORT_TYPE.HidP_Input, buttonCaps, ref numCaps, HidDevice[iHIDD].Ppd);
            }

            numCaps = HidDevice[iHIDD].Caps.NumberInputValueCaps;
            if (numCaps > 0)
            {
                HidP_GetValueCaps(HIDP_REPORT_TYPE.HidP_Input, valueCaps, ref numCaps, HidDevice[iHIDD].Ppd);
            }

            //
            // Depending on the device, some value caps structures may represent more
            // than one value.  (A range).  In the interest of being verbose, over
            // efficient, we will expand these so that we have one and only one
            // struct _HID_DATA for each value.
            //
            // To do this we need to count up the total number of values are listed
            // in the value caps structure.  For each element in the array we test
            // for range if it is a range then UsageMax and UsageMin describe the
            // usages for this range INCLUSIVE.
            //
            numValues = 0;
            for (Index = 0; Index < HidDevice[iHIDD].Caps.NumberInputValueCaps; Index++)
            {
                if (valueCaps[Index].IsRange)
                {
                    numValues += valueCaps[Index].Range.UsageMax - valueCaps[Index].Range.UsageMin + 1;
                }
                else
                {
                    numValues++;
                }

            }

            valueCaps = HidDevice[iHIDD].InputValueCaps;

            //
            // Allocate a buffer to hold the struct _HID_DATA structures.
            // One element for each set of buttons, and one element for each value
            // found.
            //
            HidDevice[iHIDD].InputDataLength = HidDevice[iHIDD].Caps.NumberInputButtonCaps + numValues;
            HidDevice[iHIDD].InputData = data = new HID_DATA[HidDevice[iHIDD].InputDataLength];

            //
            // Fill in the button data
            //
            for (Index = 0; Index < HidDevice[iHIDD].Caps.NumberInputButtonCaps; Index++) 
            {
                data[Index].IsButtonData = true;
                data[Index].Status       = HIDP_STATUS_SUCCESS;
                data[Index].UsagePage    = buttonCaps[Index].UsagePage;

                if (buttonCaps[Index].IsRange) 
                {
                    data[Index].ButtonData.UsageMin = buttonCaps[Index].Range.UsageMin;
                    data[Index].ButtonData.UsageMax = buttonCaps[Index].Range.UsageMax;
                }
                else
                {
                    data[Index].ButtonData.UsageMin = data[Index].ButtonData.UsageMax = buttonCaps[Index].NotRange.Usage;
                }
        
                data[Index].ButtonData.MaxUsageLength = HidP_MaxUsageListLength(HIDP_REPORT_TYPE.HidP_Input, buttonCaps[Index].UsagePage, HidDevice[iHIDD].Ppd);
                //data[Index].ButtonData.Usages = new Int32[data[Index].ButtonData.MaxUsageLength];

                data[Index].ReportID = buttonCaps[Index].ReportID;
            }

            //
            // Fill in the value data
            //
            for (Index = 0; Index < HidDevice[iHIDD].Caps.NumberInputValueCaps; Index++)
            {
                if (valueCaps[Index].IsRange)
                {
                    // Never reach
                    for (usage = valueCaps[Index].Range.UsageMin; usage <= valueCaps[Index].Range.UsageMax; usage++)
                    {
                        data[Index].IsButtonData = false;
                        data[Index].Status = HIDP_STATUS_SUCCESS;
                        data[Index].UsagePage = valueCaps[Index].UsagePage;
                        data[Index].ValueData.Usage = usage;
                        data[Index].ReportID = valueCaps[Index].ReportID;
                    }
                }
                else
                {
                    data[Index].IsButtonData = false;
                    data[Index].Status = HIDP_STATUS_SUCCESS;
                    data[Index].UsagePage = valueCaps[Index].UsagePage;
                    data[Index].ValueData.Usage = valueCaps[Index].NotRange.Usage;
                    data[Index].ReportID = valueCaps[Index].ReportID;
                }
            }

            //
            // setup Output Data buffers.
            //
            HidDevice[iHIDD].OutputReportBuffer = new IntPtr[HidDevice[iHIDD].Caps.OutputReportByteLength];
            HidDevice[iHIDD].OutputButtonCaps   = buttonCaps = new HIDP_BUTTON_CAPS[HidDevice[iHIDD].Caps.NumberOutputButtonCaps];
            HidDevice[iHIDD].OutputValueCaps    = valueCaps  = new HIDP_VALUE_CAPS[HidDevice[iHIDD].Caps.NumberOutputValueCaps];

            numCaps = HidDevice[iHIDD].Caps.NumberOutputButtonCaps;

            if (numCaps > 0)
            {
                HidP_GetButtonCaps(HIDP_REPORT_TYPE.HidP_Output, buttonCaps, ref numCaps, HidDevice[iHIDD].Ppd);
            }

            numCaps = HidDevice[iHIDD].Caps.NumberOutputValueCaps;

            if (numCaps > 0)
            {
                HidP_GetValueCaps(HIDP_REPORT_TYPE.HidP_Output, valueCaps, ref numCaps, HidDevice[iHIDD].Ppd);
            }

            numValues = 0;
            for (Index = 0; Index < HidDevice[iHIDD].Caps.NumberOutputValueCaps; Index++)
            {
                if (valueCaps[Index].IsRange)
                {
                    numValues += valueCaps[Index].Range.UsageMax - valueCaps[Index].Range.UsageMin + 1;
                }
                else
                {
                    numValues++;
                }
            }

            valueCaps = HidDevice[iHIDD].OutputValueCaps;

            HidDevice[iHIDD].OutputDataLength = HidDevice[iHIDD].Caps.NumberOutputButtonCaps + numValues;
            HidDevice[iHIDD].OutputData = data = new HID_DATA[HidDevice[iHIDD].OutputDataLength];

            for (Index = 0; Index < HidDevice[iHIDD].Caps.NumberOutputButtonCaps; Index++)
            {
                data[Index].IsButtonData = true;
                data[Index].Status = HIDP_STATUS_SUCCESS;
                data[Index].UsagePage = buttonCaps[Index].UsagePage;

                if (buttonCaps[Index].IsRange)
                {
                    data[Index].ButtonData.UsageMin = buttonCaps[Index].Range.UsageMin;
                    data[Index].ButtonData.UsageMax = buttonCaps[Index].Range.UsageMax;
                }
                else
                {
                    data[Index].ButtonData.UsageMin = data[Index].ButtonData.UsageMax = buttonCaps[Index].NotRange.Usage;
                }

                data[Index].ButtonData.MaxUsageLength = HidP_MaxUsageListLength(HIDP_REPORT_TYPE.HidP_Output, buttonCaps[Index].UsagePage, HidDevice[iHIDD].Ppd);
                //data[Index].ButtonData.Usages = new short[data[Index].ButtonData.MaxUsageLength];
                data[Index].ReportID = buttonCaps[Index].ReportID;
            }

            for (Index = 0; Index < HidDevice[iHIDD].Caps.NumberOutputValueCaps; Index++)
            {
                if (valueCaps[Index].IsRange)
                {
                    // Never reach
                    for (usage = valueCaps[Index].Range.UsageMin; usage <= valueCaps[Index].Range.UsageMax; usage++)
                    {
                        data[Index].IsButtonData = false;
                        data[Index].Status = HIDP_STATUS_SUCCESS;
                        data[Index].UsagePage = valueCaps[Index].UsagePage;
                        data[Index].ValueData.Usage = usage;
                        data[Index].ReportID = valueCaps[Index].ReportID;
                    }
                }
                else
                {
                    data[Index].IsButtonData = false;
                    data[Index].Status = HIDP_STATUS_SUCCESS;
                    data[Index].UsagePage = valueCaps[Index].UsagePage;
                    data[Index].ValueData.Usage = valueCaps[Index].NotRange.Usage;
                    data[Index].ReportID = valueCaps[Index].ReportID;
                }
            }

            //
            // setup Feature Data buffers.
            //
            HidDevice[iHIDD].FeatureReportBuffer              = new IntPtr[HidDevice[iHIDD].Caps.FeatureReportByteLength];
            HidDevice[iHIDD].FeatureButtonCaps   = buttonCaps = new HIDP_BUTTON_CAPS[HidDevice[iHIDD].Caps.NumberFeatureButtonCaps];
            HidDevice[iHIDD].FeatureValueCaps    = valueCaps  = new HIDP_VALUE_CAPS[HidDevice[iHIDD].Caps.NumberFeatureValueCaps];

            numCaps = HidDevice[iHIDD].Caps.NumberFeatureButtonCaps;

            if (numCaps > 0)
            {
                HidP_GetButtonCaps(HIDP_REPORT_TYPE.HidP_Feature, buttonCaps, ref numCaps, HidDevice[iHIDD].Ppd);
            }

            numCaps = HidDevice[iHIDD].Caps.NumberFeatureValueCaps;

            if (numCaps > 0)
            {
                HidP_GetValueCaps(HIDP_REPORT_TYPE.HidP_Feature, valueCaps, ref numCaps, HidDevice[iHIDD].Ppd);
            }


            numValues = 0;
            for (Index = 0; Index < HidDevice[iHIDD].Caps.NumberFeatureValueCaps; Index++)
            {
                if (valueCaps[Index].IsRange)
                {
                    numValues += valueCaps[Index].Range.UsageMax - valueCaps[Index].Range.UsageMin + 1;
                }
                else
                {
                    numValues++;
                }
            }

            valueCaps = HidDevice[iHIDD].FeatureValueCaps;

            HidDevice[iHIDD].FeatureDataLength = HidDevice[iHIDD].Caps.NumberFeatureButtonCaps + numValues;
            HidDevice[iHIDD].FeatureData = data = new HID_DATA[HidDevice[iHIDD].FeatureDataLength];

            for (Index = 0; Index < HidDevice[iHIDD].Caps.NumberFeatureButtonCaps; Index++)
            {
                data[Index].IsButtonData = true;
                data[Index].Status = HIDP_STATUS_SUCCESS;
                data[Index].UsagePage = buttonCaps[Index].UsagePage;

                if (buttonCaps[Index].IsRange)
                {
                    data[Index].ButtonData.UsageMin = buttonCaps[Index].Range.UsageMin;
                    data[Index].ButtonData.UsageMax = buttonCaps[Index].Range.UsageMax;
                }
                else
                {
                    data[Index].ButtonData.UsageMin = data[Index].ButtonData.UsageMax = buttonCaps[Index].NotRange.Usage;
                }

                data[Index].ButtonData.MaxUsageLength = HidP_MaxUsageListLength(HIDP_REPORT_TYPE.HidP_Feature, buttonCaps[Index].UsagePage, HidDevice[iHIDD].Ppd);
                //data[Index].ButtonData.Usages = new short[data[Index].ButtonData.MaxUsageLength];

                data[Index].ReportID = buttonCaps[Index].ReportID;
            }

            for (Index = 0; Index < HidDevice[iHIDD].Caps.NumberFeatureValueCaps; Index++)
            {
                if (valueCaps[Index].IsRange)
                {
                    // Never reach
                    for (usage = valueCaps[Index].Range.UsageMin; usage <= valueCaps[Index].Range.UsageMax; usage++)
                    {
                        data[Index].IsButtonData = false;
                        data[Index].Status = HIDP_STATUS_SUCCESS;
                        data[Index].UsagePage = valueCaps[Index].UsagePage;
                        data[Index].ValueData.Usage = usage;
                        data[Index].ReportID = valueCaps[Index].ReportID;
                    }
                }
                else
                {
                    data[Index].IsButtonData = false;
                    data[Index].Status = HIDP_STATUS_SUCCESS;
                    data[Index].UsagePage = valueCaps[Index].UsagePage;
                    data[Index].ValueData.Usage = valueCaps[Index].NotRange.Usage;
                    data[Index].ReportID = valueCaps[Index].ReportID;
                }
            }
        }
        */

		/* void PackReport(IntPtr ReportBuffer, ushort ReportBufferLength, HIDP_REPORT_TYPE ReportType, HID_DATA[] Data, Int32 DataLength, IntPtr Ppd)
        {
            // /*++
            // Routine Description:
            //    This routine takes in a list of HID_DATA structures (DATA) and builds 
            //       in ReportBuffer the given report for all data values in the list that 
            //       correspond to the report ID of the first item in the list.  
            // 
            //    For every data structure in the list that has the same report ID as the first
            //       item in the list will be set in the report.  Every data item that is 
            //       set will also have it's IsDataSet field marked with TRUE.
            // 
            //    A return value of FALSE indicates an unexpected error occurred when setting
            //       a given data value.  The caller should expect that assume that no values
            //       within the given data structure were set.
            // 
            //    A return value of TRUE indicates that all data values for the given report
            //       ID were set without error.
            // --

            Int32   numUsages; // Number of usages to set for a given report.
            Int32   Index;
            Int32   CurrReportID;

            //
            // Go through the data structures and set all the values that correspond to
            //   the CurrReportID which is obtained from the first data structure 
            //   in the list
            //
            CurrReportID = Data[0].ReportID;

            for (Index = 0; Index < DataLength; Index++) 
            {
                //
                // There are two different ways to determine if we set the current data
                //    structure: 
                //    1) Store the report ID were using and only attempt to set those
                //        data structures that correspond to the given report ID.  This
                //        example shows this implementation.
                //
                //    2) Attempt to set all of the data structures and look for the 
                //        returned status value of HIDP_STATUS_INVALID_REPORT_ID.  This 
                //        error code indicates that the given usage exists but has a 
                //        different report ID than the report ID in the current report 
                //        buffer
                //
                if (Data[Index].ReportID == CurrReportID) 
                {
                    if (Data[Index].IsButtonData) 
                    {
                        numUsages = Data[Index].ButtonData.MaxUsageLength;
                        Data[Index].Status = HidP_SetUsages(ReportType,
                                                       Data[Index].UsagePage,
                                                       0,
                                                       Data[Index].ButtonData.Usages,
                                                       ref numUsages,
                                                       Ppd,
                                                       ReportBuffer,
                                                       ReportBufferLength);
                    }
                    else
                    {
                        Data[Index].Status = HidP_SetUsageValue(ReportType,
                                                           Data[Index].UsagePage,
                                                           0,
                                                           Data[Index].ValueData.Usage,
                                                           Data[Index].ValueData.Value,
                                                           Ppd,
                                                           ReportBuffer,
                                                           ReportBufferLength);
                    }
                }
            }

            //
            // At this point, all data structures that have the same ReportID as the
            //    first one will have been set in the given report.  Time to loop 
            //    through the structure again and mark all of those data structures as
            //    having been set.
            //
            for (Index = 0; Index < DataLength; Index++) 
            {
                if (CurrReportID == Data[Index].ReportID)
                {
                    Data[Index].IsDataSet = true;
                }
            }
        }*/
	}
}
