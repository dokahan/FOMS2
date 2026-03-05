using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using FOMSSubmarine;

namespace Comms
{
	public enum ConnectionType
	{
		Serial,
		Ethernet,
		HID
	}

	public enum ConnectionStatus
	{
		Disconnected,
		Connecting,
		Connected
	}

	public interface IProtocolProcessor
	{
		void SendDiscoveryCommand();
		void ProcessReceivedData(byte[] data);
		void ClearPendingData();
	}

	public class Communications : IDisposable
	{
		public delegate void ConnectionStatusChangedEventHandler(Communications sender, ConnectionStatus connectionStatus);
		public event ConnectionStatusChangedEventHandler ConnectionStatusChanged;

		private ConnectionType _connectionType;
		private ICommDevice commDevice;
		private ConnectionStatus _connectionStatus;

		private List<int> listBaudrateForAutoSearch = new List<int> { 115200, 9600, 57600, 38400, 28800, 19200, 14400, 4800, 2400, 1200, 600 };
		private bool searchingBaudrate = false;
		private int indexSearchingBaudrate = 0;
		private System.Threading.Timer timerConnection;
		private bool shouldClearBuffer;

		private BlockingCollection<byte[]> packets = new BlockingCollection<byte[]>();
		private CancellationTokenSource ctsProcessReceivedData;
		private Task taskProcessReceivedData;
		private IProtocolProcessor protocolProcessor;

		private bool isNoResponseMessageBoxOn = true;

		public Communications(IProtocolProcessor processor)
		{
			protocolProcessor = processor;
			_connectionStatus = ConnectionStatus.Disconnected;

			ctsProcessReceivedData = new CancellationTokenSource();
			taskProcessReceivedData = Task.Factory.StartNew(() => ProcessReceivedData(ctsProcessReceivedData.Token),
				ctsProcessReceivedData.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
		}

		public void Dispose()
		{
			if (commDevice != null)
			{
				commDevice.Dispose();
				commDevice = null;
			}

			if (ctsProcessReceivedData != null)
				ctsProcessReceivedData.Cancel();

			if (taskProcessReceivedData != null)
			{
				taskProcessReceivedData.Wait();
				taskProcessReceivedData.Dispose();
				taskProcessReceivedData = null;
			}

			if (ctsProcessReceivedData != null)
			{
				ctsProcessReceivedData.Dispose();
				ctsProcessReceivedData = null;
			}
		}

		public ConnectionType ConnectionType { get { return _connectionType; } }
		public ConnectionStatus ConnectionStatus { get { return _connectionStatus; } }
		public int TimeoutForConnecting { get; set; } = 1000;

		public int Baudrate
		{
			get
			{
				int baudrate = 0;
				if (_connectionType == ConnectionType.Serial)
				{
					if (commDevice != null)
					{
						var commSerial = commDevice as CommSerial;
						baudrate = commSerial.Baudrate;
					}
				}
				return baudrate;
			}
			set
			{
				if (_connectionType == ConnectionType.Serial)
				{
					if (commDevice != null)
					{
						var commSerial = commDevice as CommSerial;
						commSerial.Baudrate = value;
					}
				}
			}
		}

		public bool Connect(string portName, int baudrate, bool shouldDisplayMessageBox)
		{
			if (_connectionStatus != ConnectionStatus.Disconnected)
				return false;

			_connectionType = ConnectionType.Serial;
			isNoResponseMessageBoxOn = shouldDisplayMessageBox;

			if (commDevice != null)
				commDevice.Dispose();

			if (baudrate == 0)
			{
				indexSearchingBaudrate = 0;
				baudrate = listBaudrateForAutoSearch[indexSearchingBaudrate];
				searchingBaudrate = true;
			}
			else
			{
				searchingBaudrate = false;
			}
			CommSerial commSerial = new CommSerial(portName, baudrate);

			try
			{
				commSerial.Initialize();
			}
			catch (Exception ex)
			{
				if (shouldDisplayMessageBox)
					MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}

			_connectionStatus = ConnectionStatus.Connecting;
			if (ConnectionStatusChanged != null)
				ConnectionStatusChanged(this, _connectionStatus);

			timerConnection = new System.Threading.Timer(TimerCallback_ReplyTimeout);
			shouldClearBuffer = true;
			timerConnection.Change(200, Timeout.Infinite);

			commDevice = commSerial;
			commDevice.Connected += commDevice_Connected;
			commDevice.ConnectionLost += commDevice_ConnectionLost;
			commDevice.DataReceived += commDevice_DataReceived;

			return true;
		}

		public bool Connect(string hostname, ushort port)
		{
			if (_connectionStatus != ConnectionStatus.Disconnected)
				return false;

			_connectionType = ConnectionType.Ethernet;

			if (commDevice != null)
				commDevice.Dispose();

			CommEthernet commEthernet = new CommEthernet(hostname, port);

			try
			{
				commEthernet.Initialize();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}

			_connectionStatus = ConnectionStatus.Connecting;
			if (ConnectionStatusChanged != null)
				ConnectionStatusChanged(this, _connectionStatus);

			timerConnection = new System.Threading.Timer(TimerCallback_ReplyTimeout);
			timerConnection.Change(4000, Timeout.Infinite);

			commDevice = commEthernet;
			commDevice.Connected += commDevice_Connected;
			commDevice.ConnectionLost += commDevice_ConnectionLost;
			commDevice.DataReceived += commDevice_DataReceived;

			return true;
		}

		public bool Connect(ushort vid, ushort pid)
		{
			if (_connectionStatus != ConnectionStatus.Disconnected)
				return false;

			_connectionType = ConnectionType.HID;

			if (commDevice != null)
				commDevice.Dispose();

			CommHID commHID = new CommHID(vid, pid);

			_connectionStatus = ConnectionStatus.Connecting;
			if (ConnectionStatusChanged != null)
				ConnectionStatusChanged(this, _connectionStatus);

			timerConnection = new System.Threading.Timer(TimerCallback_ReplyTimeout);
			timerConnection.Change(4000, Timeout.Infinite);

			commDevice = commHID;
			commDevice.Connected += commDevice_Connected;
			commDevice.ConnectionLost += commDevice_ConnectionLost;
			commDevice.DataReceived += commDevice_DataReceived;

			commHID.Initialize();

			return true;
		}

		public void EstablishConnection()
		{
			_connectionStatus = ConnectionStatus.Connected;
			if (ConnectionStatusChanged != null)
				ConnectionStatusChanged(this, _connectionStatus);
		}

		public void Disconnect(bool internally = false)
		{
			if (_connectionStatus == ConnectionStatus.Disconnected)
				return;

			commDevice.Connected -= commDevice_Connected;
			commDevice.ConnectionLost -= commDevice_ConnectionLost;
			commDevice.DataReceived -= commDevice_DataReceived;
			commDevice.Dispose();

			_connectionStatus = ConnectionStatus.Disconnected;
			if (internally == false)
			{
				if (ConnectionStatusChanged != null)
					ConnectionStatusChanged(this, _connectionStatus);
			}
		}

		public void SendData(byte[] data, int offset, int count)
		{
			if (commDevice != null)
			{
				commDevice.SendData(data, offset, count);
			}
		}

		public void WaitForWriteComplete(int timeout)
		{
			if (_connectionType == ConnectionType.Serial)
			{
				if (commDevice != null)
				{
					var commSerial = commDevice as CommSerial;
					commSerial.WaitForWriteComplete(timeout);
				}
			}
		}

		private void TimerCallback_ReplyTimeout(object state)
		{
			try
			{
				((System.Threading.Timer)state).Dispose();
				timerConnection = null;
				if (_connectionStatus == ConnectionStatus.Connecting)
				{
					_connectionStatus = ConnectionStatus.Disconnected;
					if (_connectionType == ConnectionType.Serial)
					{
						if (shouldClearBuffer)
						{
							byte[] packet;
							while (packets.TryTake(out packet))
								;
							protocolProcessor.ClearPendingData();
							_connectionStatus = ConnectionStatus.Connecting;
							protocolProcessor.SendDiscoveryCommand();
							timerConnection = new System.Threading.Timer(TimerCallback_ReplyTimeout);
							timerConnection.Change(TimeoutForConnecting, Timeout.Infinite);
							shouldClearBuffer = false;
							return;
						}
						else
						{
							if (searchingBaudrate)
							{
								if (++indexSearchingBaudrate < listBaudrateForAutoSearch.Count)
								{
									var commSerial = commDevice as CommSerial;
									commSerial.Baudrate = listBaudrateForAutoSearch[indexSearchingBaudrate];
									_connectionStatus = ConnectionStatus.Connecting;
									protocolProcessor.SendDiscoveryCommand();
									timerConnection = new System.Threading.Timer(TimerCallback_ReplyTimeout);
									timerConnection.Change(1000, Timeout.Infinite);
									if (ConnectionStatusChanged != null)
										ConnectionStatusChanged(this, _connectionStatus);
									return;
								}
								else
								{
									searchingBaudrate = false;
								}
							}
						}
					}

					commDevice.Dispose();

					if (isNoResponseMessageBoxOn)
						MessageBox.Show("No Response.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

					if (ConnectionStatusChanged != null)
						ConnectionStatusChanged(this, _connectionStatus);
				}
			}
			catch (Exception ex)
			{
				DebuggingHelper.Trace(ex);
			}
		}

		private void commDevice_Connected(ICommDevice sender)
		{
			protocolProcessor.SendDiscoveryCommand();
		}

		private void commDevice_ConnectionLost(ICommDevice sender)
		{
			Disconnect();
		}

		private void commDevice_DataReceived(ICommDevice sender, byte[] data)
		{
			//DebuggingHelper.Output("Encueue:{0}, {1}", packets.Count, data.Length);
			//string output = ">>";
			//for (int i = 0; i < data.Length; ++i)ANRITSU
			//{
			//    output += data[i].ToString("X2");
			//    output += " ";
			//}
			//DebuggingHelper.Output(output);
			packets.Add(data);
		}

		private void ProcessReceivedData(CancellationToken ct)
		{
			while (ct.IsCancellationRequested == false)
			{
				try
				{
					byte[] data = packets.Take(ct);
					//DebuggingHelper.Output("Take:{0}, {1}:{2:X2}", packets.Count, data.Length, data[0]);
					protocolProcessor.ProcessReceivedData(data);
				}
				catch (OperationCanceledException)
				{
					return;
				}
				catch (Exception ex)
				{
					DebuggingHelper.Trace(ex);
					return;
				}
			}
		}
	}
}
