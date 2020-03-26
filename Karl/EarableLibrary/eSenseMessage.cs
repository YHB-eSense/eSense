using System;
using System.Linq;

namespace EarableLibrary
{
	public abstract class BLEMessage
	{
		/// <summary>
		/// Data of the last received message
		/// </summary>
		public byte[] Data { get; set; }

		/// <summary>
		/// Calculated checksum of the last received message
		/// </summary>
		public byte Checksum
		{
			get
			{
				var checksum = (byte)Data.Length;
				foreach (byte b in Data) checksum += b;
				return checksum;
			}
		}

		/// <summary>
		/// Decode a received message.
		/// </summary>
		/// <param name="message">Received message</param>
		public abstract void Decode(byte[] message);

		/// <summary>
		/// Encode this message into a byte array.
		/// </summary>
		/// <returns>Encoded message</returns>
		public abstract byte[] Encode();

		/// <summary>
		/// See <see cref="Encode"/>.
		/// </summary>
		/// <param name="message">Message to be encoded</param>
		public static implicit operator byte[](BLEMessage message)
		{
			return message.Encode();
		}

		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			if (GetType() == obj.GetType() && obj is BLEMessage other)
			{
				if (Data == null) return other.Data == null;
				else return Data.SequenceEqual(other.Data);
			}
			return false;
		}
	}

	/// <summary>
	/// Utility for encoding and decoding messages from and to the eSense.
	/// See official eSense BLE specification for details about the used protocol:
	/// https://www.esense.io/share/eSense-BLE-Specification.pdf
	/// </summary>
	public class ESenseMessage : BLEMessage
	{
		/// <summary>
		/// Message header
		/// </summary>
		public byte Header { get; set; }

		public ESenseMessage() { }

		/// <summary>
		/// Construct a new ESenseMessage.
		/// </summary>
		/// <param name="header">Message header</param>
		/// <param name="data">Message data</param>
		public ESenseMessage(byte header, params byte[] data)
		{
			Header = header;
			Data = data;
		}

		/// <summary>
		/// Encoded message properties into a byte array.
		/// </summary>
		/// <returns>Encoded message</returns>
		public override byte[] Encode()
		{
			int pos = 0;
			int size = Data.Length + 3; // +1 each for Header, Checksum, DataLength
			var bytes = new byte[size];
			bytes[pos++] = Header;
			bytes[pos++] = Checksum;
			bytes[pos++] = (byte)Data.Length;
			Data.CopyTo(bytes, pos);
			return bytes;
		}

		/// <summary>
		/// Read message properties from an encoded byte array.
		/// </summary>
		/// <param name="received">The encoded byte array</param>
		/// <exception cref="MessageError">When the received message is invalid (wrong size or checksum)</exception>
		public override void Decode(byte[] received)
		{
			if (received == null) throw new MessageError("Cannot decode null!");
			int pos = 0;
			Header = received[pos++];
			byte receivedChecksum = received[pos++];
			byte expectedSize = received[pos++];
			int actualSize = received.Length - pos;
			MessageError.AssertEqual(expectedSize, actualSize, "Invalid size detected!");
			Data = new byte[expectedSize];
			Array.Copy(sourceArray: received, destinationArray: Data, sourceIndex: pos, destinationIndex: 0, length: expectedSize);
			MessageError.AssertEqual(Checksum, receivedChecksum, "Invalid checksum detected!");
		}

		/// <summary>
		/// Utility method to convert Data (byte array) to a short array.
		/// </summary>
		/// <param name="bigEndian"></param>
		/// <returns></returns>
		public short[] DataAsShortArray(bool bigEndian = false)
		{
			var bytes = Data;
			short[] result = new short[bytes.Length / 2];
			for (int i = 0; i < result.Length; i++)
				if (bigEndian)
					result[i] = (short)(bytes[2 * i] + (bytes[2 * i + 1] << 8));
				else
					result[i] = (short)((bytes[2 * i] << 8) + bytes[2 * i + 1]);
			return result;
		}

		public override bool Equals(object obj)
		{
			if (base.Equals(obj) && obj is ESenseMessage other)
			{
				return Header.Equals(other.Header);
			}
			return false;
		}
	}

	public class IndexedESenseMessage : ESenseMessage
	{
		/// <summary>
		/// Consecutive packet counter used in some message types.
		/// </summary>
		public byte PacketIndex { get; set; }

		public IndexedESenseMessage() { }

		/// <summary>
		/// Construct a new indexed ESenseMessage.
		/// </summary>
		/// <param name="header">Message header</param>
		/// <param name="data">Message data</param>
		/// <param name="packetIndex">Packet index</param>
		public IndexedESenseMessage(byte header, byte[] data, byte packetIndex)
		{
			Header = header;
			Data = data;
			PacketIndex = packetIndex;
		}

		/// <summary>
		/// Encode message properties into a byte array.
		/// </summary>
		/// <returns>Encoded message</returns>
		public override byte[] Encode()
		{
			int pos = 0;
			int size = Data.Length + 4; // +1 each for Header, PacketIndex Checksum, DataLength
			var bytes = new byte[size];
			bytes[pos++] = Header;
			bytes[pos++] = PacketIndex;
			bytes[pos++] = Checksum;
			bytes[pos++] = (byte)Data.Length;
			Data.CopyTo(bytes, pos);
			return bytes;
		}

		/// <summary>
		/// Read message properties from an encoded byte array.
		/// </summary>
		/// <param name="received">The encoded byte array</param>
		/// <exception cref="MessageError">When the received message is invalid (wrong size or checksum)</exception>
		public override void Decode(byte[] received)
		{
			if (received == null) throw new MessageError("Cannot decode null!");
			int pos = 0;
			Header = received[pos++];
			PacketIndex = received[pos++];
			byte receivedChecksum = received[pos++];
			byte expectedSize = received[pos++];
			int actualSize = received.Length - pos;
			MessageError.AssertEqual(expectedSize, actualSize, "Invalid size detected!");
			Data = new byte[expectedSize];
			Array.Copy(sourceArray: received, destinationArray: Data, sourceIndex: pos, destinationIndex: 0, length: expectedSize);
			MessageError.AssertEqual(Checksum, receivedChecksum, "Invalid checksum detected!");
		}

		public override bool Equals(object obj)
		{
			if (base.Equals(obj) && obj is IndexedESenseMessage other)
			{
				return PacketIndex.Equals(other.PacketIndex);
			}
			return false;
		}
	}

	public class RawMessage : BLEMessage
	{
		/// <summary>
		/// Store received bytes as data.
		/// </summary>
		/// <param name="received">Received bytes</param>
		public override void Decode(byte[] received)
		{
			Data = received;
		}

		/// <summary>
		/// Returns data without any conversion.
		/// </summary>
		/// <returns>Stored data</returns>
		public override byte[] Encode()
		{
			return Data;
		}
	}

	/// <summary>
	/// Exception for errors that occur during message-parsing due to an malmformed message.
	/// </summary>
	public class MessageError : Exception
	{
		/// <summary>
		/// Construct a new MessageError.
		/// </summary>
		/// <param name="errorMessage">Message describing the error</param>
		public MessageError(string errorMessage) : base(errorMessage)
		{
		}

		public static void AssertEqual<T>(T expected, T actual, string message)
		{
			if (!actual.Equals(expected))
			{
				throw new MessageError(string.Format("{0} (Expected: {1}, Actual: {2})", message, expected, actual));
			}
		}
	}
}
