using EarableLibrary;
using Xunit;

namespace UnitTesting.EarableLibraryTests
{
	public class BLEMessageTests
	{
		[Theory]
		[InlineData(new byte[] { 1, 0 }, new short[] { 256 }, false)]
		[InlineData(new byte[] { 1, 0 }, new short[] { 1 }, true)]
		[InlineData(new byte[] { 1, 1 }, new short[] { 257 }, false)]
		[InlineData(new byte[] { 255, 255 }, new short[] { -1 })]
		[InlineData(new byte[] { 128, 0 }, new short[] { short.MinValue })]
		[InlineData(new byte[] { 127, 255 }, new short[] { short.MaxValue })]
		[InlineData(new byte[] { 1, 2, 3, 4 }, new short[] { 256 * 1 + 2, 256 * 3 + 4 }, false)]
		[InlineData(new byte[] { 1, 2, 3, 4 }, new short[] { 256 * 2 + 1, 256 * 4 + 3 }, true)]
		[InlineData(new byte[] { 1, 0, 2, 0, 3 }, new short[] { 256, 512 }, false)]
		[InlineData(new byte[] { 1, 0, 2, 0, 3 }, new short[] { 1, 2 }, true)]
		public void DataAsShortArrayTest(byte[] data, short[] expected, bool bigEndian = false)
		{
			var msg = new ESenseMessage
			{
				Data = data
			};
			var actual = msg.DataAsShortArray(bigEndian);
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(new byte[0], 0)]
		[InlineData(new byte[] { 1 }, 1, (byte)1)]
		[InlineData(new byte[] { 1, 2, 3 }, 4, (byte)5)]
		[InlineData(new byte[] { 255, 255 }, 255)]
		public void RecodeTest(byte[] data, byte header, byte? index = null)
		{
			var encoderMessage = ConstructMessage(data, header, index);
			var encoded = encoderMessage.Encode();

			var decoderMsg = index.HasValue ? new IndexedESenseMessage() : new ESenseMessage();
			decoderMsg.Decode(encoded);

			Assert.Equal(encoderMessage, decoderMsg);
		}

		[Theory]
		[InlineData(new byte[0], 0, null, 0)]
		public void ChecksumTest(byte[] data, byte header, byte? index, byte expectedChecksum)
		{
			var msg = ConstructMessage(data, header, index);
			Assert.Equal(expectedChecksum, msg.Checksum);
		}

		[Theory]
		[InlineData(new byte[0], 0, null, new byte[] { 0, 0, 0 })]
		public void EncodeTest(byte[] data, byte header, byte? index, byte[] rawMessage)
		{
			var msg = ConstructMessage(data, header, index);
			Assert.Equal(rawMessage, msg.Encode());
		}

		[Theory]
		[InlineData(new byte[] { 0, 0, 0 }, new byte[0], 0, null)]
		public void DecodeTest(byte[] rawMessage, byte[] data, byte header, byte? index)
		{
			var msg = index.HasValue ? new IndexedESenseMessage() : new ESenseMessage();
			msg.Decode(rawMessage);
			Assert.Equal(header, msg.Header);
			Assert.Equal(data, msg.Data);
		}

		[Theory]
		[InlineData(new byte[] { 1, 2, 3 })]
		public void RawMessageTest(byte[] data)
		{
			var encoderMsg = new RawMessage
			{
				Data = data
			};
			Assert.Equal(data, encoderMsg.Encode());

			var decoderMsg = new RawMessage();
			decoderMsg.Decode(data);
			Assert.Equal(data, decoderMsg.Data);
		}

		[Fact]
		public void ConstructorTest()
		{
			Assert.Equal(new IndexedESenseMessage(), new IndexedESenseMessage(0, null, 0));
			Assert.Equal(new ESenseMessage(), new ESenseMessage(0, null));
		}

		[Fact]
		public void ImplicitConversionTest()
		{
			BLEMessage msg = new RawMessage
			{
				Data = new byte[] { 1, 2, 3 }
			};
			byte[] encoded = msg;
			Assert.Equal(msg.Data, encoded);
		}

		[Fact]
		public void EqualsTest()
		{
			BLEMessage msgA = new ESenseMessage(), msgB = new RawMessage(), msgC = new IndexedESenseMessage();
			Assert.Equal(msgA.Data, msgB.Data);
			Assert.Equal(msgB.Data, msgC.Data);
			Assert.NotEqual(msgA, msgB);
			Assert.NotEqual(msgB, msgC);
			Assert.NotEqual(msgC, msgA);
			Assert.False(msgA.Equals(null));
		}

		[Theory]
		[InlineData(new byte[] { 0, 0, 0, 0 }, false)] // invalid size
		[InlineData(new byte[] { 0, 0, 1, 1 }, false)] // invalid checksum
		[InlineData(new byte[] { 0, 0, 0, 0, 0 }, true)] // invalid size
		[InlineData(new byte[] { 0, 0, 0, 1, 1 }, true)] // invalid checksum
		public void InvalidDecodeTest(byte[] rawMessage, bool isIndexed)
		{
			var msg = isIndexed ? new IndexedESenseMessage() : new ESenseMessage();
			var error = Assert.Throws<MessageError>(() =>
			{
				msg.Decode(rawMessage);
			});
		}

		private ESenseMessage ConstructMessage(byte[] data, byte header, byte? index = null)
		{
			if (index.HasValue)
			{
				return new IndexedESenseMessage
				{
					PacketIndex = index.Value,
					Header = header,
					Data = data
				};
			}
			else
			{
				return new ESenseMessage
				{
					Header = header,
					Data = data
				};
			}
		}
	}
}
