using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Comms;
using System.Threading;

namespace FOMSSubmarine
{
	class OTDRControl : IDisposable, IProtocolProcessor
	{
		public enum OTDRModel
		{
			Anritsu_MT9083,
			VIAVI_SmartOTDR
		}
		public static readonly List<string> ModelNames = new List<string>
		{
			"Anritsu MT9083",
			"VIAVI SmartOTDR"
		};

		private Communications communications;

		#region define delegate, event
		public delegate void ConnectionStatusChangedEventHandler(OTDRControl sender, ConnectionStatus connectionStatus);
		public event ConnectionStatusChangedEventHandler ConnectionStatusChanged;

		public delegate void OutputMessageEventHandler(OTDRControl sender, string response);
		public event OutputMessageEventHandler OutputMessage;

		public delegate void DataReceivedEventHandler(OTDRControl sender, byte[] data);
		public event DataReceivedEventHandler DataReceived;
		#endregion

		private string response = "";
		private AutoResetEvent waitResponse = new AutoResetEvent(false);
		private bool responseReceived;

		public OTDRControl(OTDRModel model)
		{
			Model = model;

			communications = new Communications(this);
			communications.ConnectionStatusChanged += communications_ConnectionStatusChanged;
		}

		public OTDRModel Model { get; private set; }

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

		public bool IsReceivedDataCallbackEnabled { get; set; }

		public bool Connect(string hostname, ushort port)
		{
			return communications.Connect(hostname, port);
		}

		public void Disconnect()
		{
			communications.Disconnect();
		}

		public string ModelDescription { get; set; }

		public string Response { get => response; }

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
			if (Model == OTDRModel.Anritsu_MT9083)
			{
				SendCommand("*CLS;WAI;*RST");
				SendCommand("*IDN?");
			}
			else if (Model == OTDRModel.VIAVI_SmartOTDR)
			{
				SendCommand("*REM");
				SendCommand("*IDN?");
			}
		}

		public void ProcessReceivedData(byte[] data)
		{
			if (IsReceivedDataCallbackEnabled)
			{
				DataReceived?.Invoke(this, data);
				return;
			}

			string str = Encoding.ASCII.GetString(data);
			str = str.TrimEnd();

			Console.WriteLine("Rcv OK... : " + str);

			if (ConnectionStatus == ConnectionStatus.Connected)
			{
				lock (response)
				{
					response = str;
				}
			}
			else if (ConnectionStatus == ConnectionStatus.Connecting)
			{
				if (Model == OTDRModel.Anritsu_MT9083)
				{
					if (str.StartsWith("ANRITSU"))
					{
						communications.EstablishConnection();
					}

					string[] strs = str.Split(new string[] { ", " }, StringSplitOptions.None);
					ModelDescription =
						"Maker : " + strs[0] + Environment.NewLine +
						"Model : " + strs[1] + Environment.NewLine +
						"S/N : " + strs[2] + Environment.NewLine;
				}
				else if (Model == OTDRModel.VIAVI_SmartOTDR)
				{
					if (str.StartsWith("VIAVI"))
					{
						communications.EstablishConnection();
					}

					string[] strs = str.Split(new string[] { "," }, StringSplitOptions.None);
					ModelDescription =
						"Maker : " + strs[0] + Environment.NewLine +
						"Model : " + strs[1] + Environment.NewLine +
						"S/N :";
					for (int i = 2; i < strs.Length; ++i)
					{
						ModelDescription += " " + strs[i];
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
				command += Environment.NewLine;
				byte[] bytes = Encoding.UTF8.GetBytes(command);
				communications.SendData(bytes, 0, bytes.Length);
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
				Application.DoEvents();
				if (responseReceived)
				{
					// response received
					return;
				}
			}
			throw new Exception("OTDR Timeout occured. Timeout = " + timeout.ToString() + "msec");
		}

		private void communications_ConnectionStatusChanged(Communications sender, ConnectionStatus connectionStatus)
		{
			ConnectionStatusChanged?.Invoke(this, connectionStatus);
		}
	}
}
