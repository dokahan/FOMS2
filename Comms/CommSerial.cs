using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using FOMSSubmarine;

namespace Comms
{
	class CommSerial : ICommDevice
	{
#pragma warning disable 0067
		public event ConnectedEventHandler Connected;
		public event ConnectionLostEventHandler ConnectionLost;
#pragma warning restore 0067
		public event DataReceivedEventHandler DataReceived;

		private SerialPort serialPort;
		private string _portName;
		private int _baudrate;

		public CommSerial(string portName, int baudrate)
		{
			_portName = portName;
			_baudrate = baudrate;
		}

		public string PortName
		{
			get
			{
				return _portName;
			}
			set
			{
				_portName = value;
				if (serialPort != null)
					serialPort.PortName = _portName;
			}
		}

		public int Baudrate
		{
			get
			{
				return _baudrate;
			}
			set
			{
				_baudrate = value;
				if (serialPort != null)
					serialPort.BaudRate = _baudrate;
			}
		}

		public void Initialize()
		{
			Dispose();

			try
			{
				serialPort = new SerialPort();
				serialPort.PortName = _portName;
				serialPort.BaudRate = _baudrate;
				serialPort.Parity = Parity.None;
				serialPort.DataBits = 8;
				serialPort.StopBits = StopBits.One;
				serialPort.Handshake = Handshake.None;
				serialPort.Open();
				if (serialPort.IsOpen)
				{
					serialPort.DataReceived += serialPort_DataReceived;
				}
			}
			catch (UnauthorizedAccessException)
			{
				throw new Exception("Port in Use.");
			}
			catch
			{
				throw new Exception("Unable to Connect");
			}
		}

		public void Dispose()
		{
			if (serialPort != null)
			{
				lock (serialPort)
				{
					if (serialPort.IsOpen)
					{
						serialPort.DataReceived -= serialPort_DataReceived;
						while (serialPort.BytesToRead != 0 || serialPort.BytesToWrite != 0)
						{
							serialPort.DiscardInBuffer();
							serialPort.DiscardOutBuffer();
						}
						serialPort.Close();
					}
					serialPort.Dispose();
					serialPort = null;
				}
			}
		}

		public void SendData(byte[] data, int offset, int count)
		{
			if (serialPort != null)
			{
				lock (serialPort)
				{
					if (serialPort.IsOpen)
						serialPort.Write(data, offset, count);
				}
			}
		}

		public void WaitForWriteComplete(int timeout)
		{
			try
			{
				int start = Environment.TickCount;
				while (serialPort.BytesToWrite > 0)
					if (Environment.TickCount - start > timeout)
						break;
			}
			catch
			{
				DebuggingHelper.Output("Exception in WaitForWriteComplete!");
			}
		}

		private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
		{
			if (serialPort != null)
			{
				lock (serialPort)
				{
					if (serialPort.IsOpen)
					{
						int bytesToRead = serialPort.BytesToRead;
						if (bytesToRead > 0)
						{
							try
							{
								byte[] buffer = new byte[bytesToRead];
								serialPort.Read(buffer, 0, bytesToRead);
								if (DataReceived != null)
									DataReceived(this, buffer);
							}
							catch (Exception ex)
							{
								DebuggingHelper.Trace(ex);
							}
						}
					}
				}
			}
		}
	}
}
