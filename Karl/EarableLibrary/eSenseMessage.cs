using System;

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
			int pos = 0;
			Header = received[pos++];
			var receivedChecksum = received[pos++];
			var dataSize = received[pos++];
			if (dataSize != received.Length - pos) throw new MessageError("Invalid size detected!");
			Data = new byte[dataSize];
			Array.Copy(sourceArray: received, destinationArray: Data, sourceIndex: pos, destinationIndex: 0, length: dataSize);
			if (Checksum != receivedChecksum) throw new MessageError("Invalid checksum detected!");
		}

		/// <summary>
		/// Utility method to convert Data (byte array) to a short array.
		/// </summary>
		/// <param name="bigEndian"></param>
		/// <returns></returns>
		public short[] DataAsShortArray(bool bigEndian = true)
		{
			var bytes = Data;
			short[] result = new short[bytes.Length / 2];
			for (int i = 0; i < result.Length; i++)
				if (bigEndian)
					result[i] = (short)(bytes[i] << 8 + bytes[i + 1]);
				else
					result[i] = (short)(bytes[i + 1] << 8 + bytes[i]);
			return result;
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
			int pos = 0;
			Header = received[pos++];
			PacketIndex = received[pos++];
			var receivedChecksum = received[pos++];
			var dataSize = received[pos++];
			if (dataSize != received.Length - pos) throw new MessageError("Invalid size detected!");
			Data = new byte[dataSize];
			Array.Copy(sourceArray: received, destinationArray: Data, sourceIndex: pos, destinationIndex: 0, length: dataSize);
			if (Checksum != receivedChecksum) throw new MessageError("Invalid checksum detected!");
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
	}
}
