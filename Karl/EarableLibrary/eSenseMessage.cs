using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EarableLibrary
{
	public class eSenseMessage
	{
		public byte Header { get; set; }
		public byte[] Data { get; set; }
		public byte Checksum
		{
			get
			{
				byte checksum = (byte)Data.Length;
				foreach (byte b in Data) checksum += b;
				return checksum;
			}
		}
		public byte PacketIndex { get; set; }
		public bool HasPacketIndex = false;

		public eSenseMessage(byte header, params byte[] data)
		{
			Header = header;
			Data = data;
		}

		public eSenseMessage(byte[] received, bool hasPacketIndex = false)
		{
			int i = 0; // parsing index
			HasPacketIndex = hasPacketIndex;
			Header = received[i++];
			if (HasPacketIndex) PacketIndex = received[i++];
			var receivedChecksum = received[i++];
			var receivedSize = received[i++];
			if (receivedSize != received.Length - i) throw new MessageError("Invalid size detected!");
			Array.Copy(sourceArray: received, destinationArray: Data, sourceIndex: i, destinationIndex: 0, length: receivedSize);
			if (Checksum != receivedChecksum) throw new MessageError("Invalid checksum detected!");
		}

		public byte[] ToByteArray()
		{
			var size = Data.Length + 3;
			if (HasPacketIndex) size++;
			var bytes = new byte[size];
			int i = 0; // count written bytes
			bytes[i++] = Header;
			if (HasPacketIndex) bytes[i++] = PacketIndex;
			bytes[i++] = Checksum;
			bytes[i++] = (byte)Data.Length;
			Data.CopyTo(bytes, i);
			return bytes;
		}

		public static implicit operator byte[](eSenseMessage m)
		{
			return m.ToByteArray();
		}

		public static explicit operator eSenseMessage(byte[] m)
		{
			return new eSenseMessage(m, false);
		}
	}

	public class MessageError : Exception
	{
		public MessageError(string message) : base(message)
		{
		}
	}
}
