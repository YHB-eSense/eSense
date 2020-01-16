using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EarableLibrary
{
	public class ESenseMessage
	{
		public byte Header { get; set; }
		public byte[] Data { get; set; }
		public byte Checksum
		{
			get
			{
				var checksum = (byte)Data.Length;
				foreach (byte b in Data) checksum += b;
				return checksum;
			}
		}
		public byte PacketIndex { get; set; }
		public bool HasPacketIndex { get; set; }

		public ESenseMessage(byte header, params byte[] data)
		{
			Header = header;
			Data = data;
			PacketIndex = 0;
		}

		public ESenseMessage(byte[] received, bool hasPacketIndex = false)
		{
			int i = 0; // parsing index
			HasPacketIndex = hasPacketIndex;
			Header = received[i++];
			if (hasPacketIndex) PacketIndex = received[i++];
			var receivedChecksum = received[i++];
			var dataSize = received[i++];
			if (dataSize != received.Length - i) throw new MessageError("Invalid size detected!");
			Data = new byte[dataSize];
			Array.Copy(sourceArray: received, destinationArray: Data, sourceIndex: i, destinationIndex: 0, length: dataSize);
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

		public static implicit operator byte[](ESenseMessage m)
		{
			return m.ToByteArray();
		}

		public static explicit operator ESenseMessage(byte[] m)
		{
			return new ESenseMessage(m, false);
		}
	}

	public class MessageError : Exception
	{
		public MessageError(string message) : base(message)
		{
		}
	}
}
