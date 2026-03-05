using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using FOMSSubmarine;

namespace Comms
{
	class CommEthernet : ICommDevice
	{
		public event ConnectedEventHandler Connected;
		public event ConnectionLostEventHandler ConnectionLost;
		public event DataReceivedEventHandler DataReceived;

		private TcpClient tcpClient;
		private byte[] readBuffer = new byte[1024];
		private string hostname;
		private ushort port;

		public CommEthernet(string hostname, ushort port)
		{
			this.hostname = hostname;
			this.port = port;
		}

		public void Initialize()
		{
			tcpClient = new TcpClient(AddressFamily.InterNetwork);
			tcpClient.BeginConnect(hostname, port, new AsyncCallback(tcpClient_ConnectCallback), tcpClient);
		}

		public void Dispose()
		{
			if (tcpClient != null)
			{
				tcpClient.Close();
				tcpClient = null;
			}
		}

		public void SendData(byte[] data, int offset, int count)
		{
			if (tcpClient != null && tcpClient.Connected)
			{
				NetworkStream networkStream = tcpClient.GetStream();
				networkStream.Write(data, offset, count);
			}
		}

		private void tcpClient_ConnectCallback(IAsyncResult ar)
		{
			try
			{
				TcpClient tcpClient = (TcpClient)ar.AsyncState;
				if (tcpClient.Client != null)
                {
					tcpClient.EndConnect(ar);

					Connected?.Invoke(this);

					NetworkStream networkStream = tcpClient.GetStream();
					networkStream.BeginRead(readBuffer, 0, readBuffer.Length, new AsyncCallback(tcpClient_ReadCallback), tcpClient);
				}
			}
			catch (Exception ex)
			{
				DebuggingHelper.Trace(ex);
			}
		}

		private void tcpClient_ReadCallback(IAsyncResult ar)
		{
			TcpClient tcpClient = ar.AsyncState as TcpClient;

			if (tcpClient == null)
			{
				return;
			}
			else
			{
				int readSize = 0;
				NetworkStream networkStream = null;
				try
				{
					networkStream = tcpClient.GetStream();
					readSize = networkStream.EndRead(ar);
				}
				catch
				{
					readSize = 0;
				}
				if (readSize == 0)
				{
					tcpClient.Close();
					this.tcpClient = null;
					if (ConnectionLost != null)
						ConnectionLost(this);
				}
				else
				{
					byte[] buffer = new byte[readSize];
					Array.Copy(readBuffer, buffer, buffer.Length);
					if (DataReceived != null)
						DataReceived(this, buffer);
					try
					{
						networkStream.BeginRead(readBuffer, 0, readBuffer.Length,
												new AsyncCallback(tcpClient_ReadCallback), tcpClient);
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
