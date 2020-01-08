using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace EarableLibrary
{
	class eSenseMessage
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

		public static implicit operator byte[](eSenseMessage m)
		{
			byte[] message = new byte[m.Data.Length + 3];
			message[0] = m.Header;
			message[1] = m.Checksum;
			message[2] = (byte)m.Data.Length;
			m.Data.CopyTo(message, 3);
			return message;
		}

		public static explicit operator eSenseMessage(byte[] m)
		{
			byte header = m[0];
			byte checksum = m[1];
			byte size = m[2];
			byte[] data = new byte[size];
			if (size != m.Length - 3) throw new InvalidCastException("Invalid size detected!");
			Array.Copy(sourceArray: m, destinationArray: data, sourceIndex: 3, destinationIndex: 0, length: size);
			eSenseMessage result = new eSenseMessage(header, data);
			if (result.Checksum != checksum) throw new InvalidCastException("Invalid checksum detected!");
			return result;
		}

		public static eSenseMessage ParseMessageWithPacketIndex(byte[] m)
		{
			byte header = m[0];
			byte index = m[1];
			byte checksum = m[2];
			byte size = m[3];
			byte[] data = new byte[size];
			if (size != m.Length - 4) throw new InvalidCastException("Invalid size detected!");
			Array.Copy(sourceArray: m, destinationArray: data, sourceIndex: 4, destinationIndex: 0, length: size);
			eSenseMessage result = new eSenseMessage(header, data)
			{
				HasPacketIndex = true,
				PacketIndex = index
			};
			if (result.Checksum != checksum) throw new InvalidCastException("Invalid checksum detected!");
			return result;
		}

		public void WriteTo(ICharacteristic target)
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				Debug.WriteLine("Writing to {0} from main thread...", target.Id);
				target.WriteAsync(this);
			});
		}
	}
}
