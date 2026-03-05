using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Comms;
using System.Threading;

namespace FOMSSubmarine
{
	class OSWControl : IDisposable, IProtocolProcessor
	{
		public enum OSWModel
		{
			PPI_OSWL144,
			LIGHTech_LT900,
			LIGHTech_LT1100,
			FOD_5510
		}
		public static readonly List<string> ModelNames = new List<string>
		{
			"PPI OSWL-144",
			"LIGHTech LT900",
			"LIGHTech LT1100",
			"AFL 2000"
		};

		private Communications communications;

		#region define delegate, event
		public delegate void ConnectionStatusChangedEventHandler(OSWControl sender, ConnectionStatus connectionStatus);
		public event ConnectionStatusChangedEventHandler ConnectionStatusChanged;

		public delegate void OutputMessageEventHandler(OSWControl sender, string response);
		public event OutputMessageEventHandler OutputMessage;
		#endregion

		private string response = "";
		private AutoResetEvent waitResponse = new AutoResetEvent(false);
		private bool responseReceived;

		private string strReceiving = "";

		public OSWControl(OSWModel model)
		{
			Model = model;

			communications = new Communications(this);
			communications.ConnectionStatusChanged += communications_ConnectionStatusChanged;
			communications.TimeoutForConnecting = 8000;
		}

		public OSWModel Model { get; private set; }

		public ConnectionStatus ConnectionStatus
		{
			get
			{
				return communications.ConnectionStatus;
			}
		}

		public bool IsConnected
		{
			get
			{
				return ConnectionStatus == ConnectionStatus.Connected;
			}
		}

		public bool Connect(string portName, int baudrate)
		{
			strReceiving = "";
			return communications.Connect(portName, baudrate, false);
		}

		public bool Connect(ushort vid, ushort pid)
		{
			strReceiving = "";
			return communications.Connect(vid, pid);
		}

		public void Disconnect()
		{
			communications.Disconnect();
		}

		public string ModelDescription { get; set; }
		public string CurrentPort { get; set; }

		public string Response { get => response; }

		public void SwitchChannel(int channel)
		{
			if (Model == OSWModel.PPI_OSWL144)
			{
				string command = "SP " + string.Format("{0:D3}", channel);
				SendCommand(command);
			}
			else if (Model == OSWModel.LIGHTech_LT900)
			{
				string command = "SWITCH:" + channel.ToString();
				SendCommand(command);
			}
			else if (Model == OSWModel.LIGHTech_LT1100)
			{
				string command = "SWITCH:" + channel.ToString();
				SendCommand(command);
			}
			else if (Model == OSWModel.FOD_5510)
			{
				int ch = channel - 1;
				if (ch >= 0 && ch < 12)
				{
					string command = "1 1 " + ch.ToString("X2");
					SendCommand(command);
				}
				else
				{
					DebuggingHelper.Output("Out of channel range for FOD 5510 OSW!");
				}
			}
		}

		#region IDisposable
		public void Dispose()
		{
			Disconnect();

			communications.Dispose();
		}
		#endregion

		#region IProtocolProcessor
		public void SendDiscoveryCommand()
		{
			if (Model == OSWModel.PPI_OSWL144)
			{
				SendCommand("HO");    // Go Home (Reset)
			}
			else if (Model == OSWModel.LIGHTech_LT900)
			{
				SendCommand("*RST");	// Reset
			}
			else if (Model == OSWModel.LIGHTech_LT1100)
			{
				SendCommand("*RST");    // Reset
				SendCommand("*IDN?");
			}
			else if (Model == OSWModel.FOD_5510)
			{
				ModelDescription = "FOD 5510";
				communications.EstablishConnection();
			}
		}

		public void ProcessReceivedData(byte[] data)
		{
			string str;
			strReceiving += Encoding.ASCII.GetString(data);
			int index = strReceiving.IndexOf('\r');
			if (index != -1)
			{
				++index;
				str = strReceiving.Substring(0, index);
				if (index < strReceiving.Length)
				{
					strReceiving = strReceiving.Substring(index, strReceiving.Length - index);
				}
				else
				{
					strReceiving = "";
				}
			}
			else
			{
				return;
			}

			if (ConnectionStatus == ConnectionStatus.Connected)
			{
				lock (response)
				{
					response = str;
				}
			}
			else if (ConnectionStatus == ConnectionStatus.Connecting)
			{
				if (Model == OSWModel.PPI_OSWL144)
				{
					if (str.StartsWith("OK"))
					{
						SendCommand("CP");
					}
					else if (str.StartsWith("CP"))
					{
						if (str.Length >= 6)
						{
							CurrentPort = str.Substring(3, 3);
							communications.EstablishConnection();
							response = str;
						}
					}
				}
				else if (Model == OSWModel.LIGHTech_LT900)
				{
					if (str.StartsWith("0,OK"))
					{
						SendCommand("IDN?");
					}
					else if (str.StartsWith("LT900"))
					{
						ModelDescription = str;
						communications.EstablishConnection();
					}
				}
				else if (Model == OSWModel.LIGHTech_LT1100)
				{
					if (str.StartsWith("LT1100"))
					{
						ModelDescription = str;
						communications.EstablishConnection();
					}
				}
			}

			waitResponse.Set();
			responseReceived = true;
		}

		public void ClearPendingData()
		{
		}
		#endregion

		public void SendCommand(string command)
		{
			OutputMessage?.Invoke(this, command);

			try
			{
				if (communications.ConnectionType == ConnectionType.HID)
				{
					string[] strs = command.Split(' ');
					List<byte> bytes = new List<byte>();
					foreach (string str in strs)
					{
						byte b;
						try
						{
							b = Convert.ToByte(str, 16);
						}
						catch
						{
							b = 0;
						}
						bytes.Add(b);
					}
					communications.SendData(bytes.ToArray(), 0, bytes.Count);
				}
				else
				{
					command += '\r';
					byte[] bytes = Encoding.UTF8.GetBytes(command);
					communications.SendData(bytes, 0, bytes.Length);
				}
			}
			catch
			{
				Disconnect();
				MessageBox.Show("Connection disconnected.\nReconnect please.", "Error",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		public void WaitResponse(int timeout = 180000)
		{
			//waitResponse.Reset();
			responseReceived = false;

			int interval = 10;
			int count = timeout / interval;
			for (int i = 0; i < count; i++)
			{
				//if (waitResponse.WaitOne(interval))
				Utilities.Delay(interval);
				if (responseReceived)
				{
					// response received
					return;
				}

				Application.DoEvents();
			}
			throw new Exception("OSW Timeout occured. Timeout = " + timeout.ToString() + "msec");
		}

		private void communications_ConnectionStatusChanged(Communications sender, ConnectionStatus connectionStatus)
		{
			ConnectionStatusChanged?.Invoke(this, connectionStatus);
		}
	}
}
