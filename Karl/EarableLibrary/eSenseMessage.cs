using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EarableLibrary
{
	/// <summary>
	/// Utility for encoding and decoding messages from and to the eSense.
	/// See official eSense BLE specification for details about the used protocol:
	/// https://www.esense.io/share/eSense-BLE-Specification.pdf
	/// </summary>
	public class ESenseMessage
	{
		/// <summary>
		/// Message header
		/// </summary>
		public byte Header { get; set; }

		/// <summary>
		/// Message data
		/// </summary>
		public byte[] Data { get; set; }

		/// <summary>
		/// Calculated checksum
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
		/// Consecutive packet counter used in some messages.
		/// Meaningless if <see cref="HasPacketIndex"/> is false.
		/// </summary>
		public byte PacketIndex { get; set; }

		/// <summary>
		/// Whether this message contains an packet index or not.
		/// </summary>
		public bool HasPacketIndex { get; set; }

		/// <summary>
		/// Construct a new ESenseMessage.
		/// </summary>
		/// <param name="header">Message header</param>
		/// <param name="data">Message data</param>
		public ESenseMessage(byte header, params byte[] data)
		{
			Header = header;
			Data = data;
			PacketIndex = 0;
		}

		/// <summary>
		/// Construct a new ESenseMessage from an encoded byte array.
		/// </summary>
		/// <param name="received">The encoded byte array</param>
		/// <param name="hasPacketIndex">Whether the message received contains a packet-index-byte or not</param>
		/// <exception cref="MessageError">When the received message is invalid (wrong size or checksum)</exception>
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

		/// <summary>
		/// Encode this message into a byte array.
		/// </summary>
		/// <returns></returns>
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

		/// <summary>
		/// See <see cref="ToByteArray"/>.
		/// </summary>
		/// <param name="message">Message to be encoded</param>
		public static implicit operator byte[](ESenseMessage message)
		{
			return message.ToByteArray();
		}

		/// <summary>
		/// Decode the byte array to an ESenseMessage (same as calling <see cref="ESenseMessage"/>).
		/// </summary>
		/// <param name="array">Encoded byte array</param>
		public static explicit operator ESenseMessage(byte[] array)
		{
			return new ESenseMessage(array, false);
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
