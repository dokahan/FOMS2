using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Comms
{
	public delegate void ConnectedEventHandler(ICommDevice sender);
	public delegate void ConnectionLostEventHandler(ICommDevice sender);
	public delegate void DataReceivedEventHandler(ICommDevice sender, byte[] data);

	public interface ICommDevice
	{
		event ConnectedEventHandler Connected;
		event ConnectionLostEventHandler ConnectionLost;
		event DataReceivedEventHandler DataReceived;

		void Initialize();
		void Dispose();

		void SendData(byte[] data, int offset, int count);
	}
}
