using Cpu.Extensions;
using System.Collections;
using Xunit;

namespace Test.Unit.Cpu.Extensions
{
    public sealed record ByteExtensionsTest
    {
        [Theory]
        [InlineData(0, 0)]
        [InlineData(0b_1111_1111, 0b_0000_1111)]
        [InlineData(0b_0000_0001, 0b_0000_0001)]
        [InlineData(0b_0000_0010, 0b_0000_0010)]
        [InlineData(0b_0000_0100, 0b_0000_0100)]
        [InlineData(0b_0000_1000, 0b_0000_1000)]
        [InlineData(0b_0001_0000, 0b_0000_0000)]
        [InlineData(0b_0010_0000, 0b_0000_0000)]
        [InlineData(0b_0100_0000, 0b_0000_0000)]
        [InlineData(0b_1000_0000, 0b_0000_0000)]
        public void LeastSignificant_Returns(byte data, byte expected)
        {
            Assert.Equal(expected, data.LeastSignificantBits());
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(0b_1111_1111, 0b_1111_0000)]
        [InlineData(0b_0000_0001, 0b_0000_0000)]
        [InlineData(0b_0000_0010, 0b_0000_0000)]
        [InlineData(0b_0000_0100, 0b_0000_0000)]
        [InlineData(0b_0000_1000, 0b_0000_0000)]
        [InlineData(0b_0001_0000, 0b_0001_0000)]
        [InlineData(0b_0010_0000, 0b_0010_0000)]
        [InlineData(0b_0100_0000, 0b_0100_0000)]
        [InlineData(0b_1000_0000, 0b_1000_0000)]
        public void MostSignificant_Returns(byte data, byte expected)
        {
            Assert.Equal(expected, data.MostSignificantBits());
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(0b_0000_0001, 0b_0001_0000, 0b_0001_0001)]
        [InlineData(0b_0000_0010, 0b_0010_0000, 0b_0010_0010)]
        [InlineData(0b_0000_0100, 0b_0100_0000, 0b_0100_0100)]
        [InlineData(0b_0000_1000, 0b_1000_0000, 0b_1000_1000)]
        public void CombineSignificantBits_Returns(byte lsb, byte msb, byte expected)
        {
            var result = lsb.CombineSignificantBits(msb);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(0b_0000_0001, 0b_0001_0000, 0b_0001_0000_0000_0001)]
        [InlineData(0b_0000_0010, 0b_0010_0000, 0b_0010_0000_0000_0010)]
        [InlineData(0b_0000_0100, 0b_0100_0000, 0b_0100_0000_0000_0100)]
        [InlineData(0b_0000_1000, 0b_1000_0000, 0b_1000_0000_0000_1000)]
        public void CombineBytes_Returns(byte lsb, byte msb, ushort expected)
        {
            var result = lsb.CombineBytes(msb);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(0b_1111_1001, true)]
        [InlineData(0b_0111_1000, false)]
        public void IsFirstBitSet_Returns(byte value, bool expected)
        {
            Assert.Equal(expected, value.IsFirstBitSet());
        }

        [Theory]
        [InlineData(0b_1111_1001, true)]
        [InlineData(0b_0111_1000, false)]
        public void IsLastBitSet_Returns(byte value, bool expected)
        {
            Assert.Equal(expected, value.IsLastBitSet());
        }

        [Theory]
        [InlineData(0b_1111_1111, 0, true)]
        [InlineData(0b_1111_1111, 1, true)]
        [InlineData(0b_1111_1111, 2, true)]
        [InlineData(0b_1111_1111, 3, true)]
        [InlineData(0b_1111_1111, 4, true)]
        [InlineData(0b_1111_1111, 5, true)]
        [InlineData(0b_1111_1111, 6, true)]
        [InlineData(0b_1111_1111, 7, true)]
        [InlineData(0b_1111_1110, 0, false)]
        [InlineData(0b_1111_1101, 1, false)]
        [InlineData(0b_1111_1011, 2, false)]
        [InlineData(0b_1111_0111, 3, false)]
        [InlineData(0b_1110_1111, 4, false)]
        [InlineData(0b_1101_1111, 5, false)]
        [InlineData(0b_1011_1111, 6, false)]
        [InlineData(0b_0111_1111, 7, false)]
        public void IsBitSet_Returns(byte value, byte index, bool expected)
        {
            Assert.Equal(expected, value.IsBitSet(index));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(8)]
        public void IsBitSet_Throws(int index)
        {
            _ = Assert.Throws<ArgumentOutOfRangeException>(() => byte.MinValue.IsBitSet(index));
        }

        [Theory]
        [InlineData(0b_0000_0000, true)]
        [InlineData(0b_0000_0001, false)]
        public void IsZero_Returns(byte value, bool expected)
        {
            Assert.Equal(expected, value.IsZero());
        }

        [Theory]
        [InlineData(0b_0000_0001, false, 0b_0000_0000)]
        [InlineData(0b_1000_0000, false, 0b_0100_0000)]
        [InlineData(0b_0000_0001, true, 0b_1000_0000)]
        [InlineData(0b_1000_0000, true, 0b_1100_0000)]
        public void RotateRight_Returns(byte value, bool isCarry, byte expected)
        {
            Assert.Equal(expected, value.RotateRight(isCarry));
        }

        [Theory]
        [InlineData(0b_0000_0001, false, 0b_0000_0010)]
        [InlineData(0b_1000_0000, false, 0b_0000_0000)]
        [InlineData(0b_0000_0001, true, 0b_0000_0011)]
        [InlineData(0b_1000_0000, true, 0b_0000_0001)]
        public void RotateLeft_Returns(byte value, bool isCarry, byte expected)
        {
            Assert.Equal(expected, value.RotateLeft(isCarry));
        }

        [Theory]
        [InlineData(0b_0000_0001, 1)]
        [InlineData(0b_0000_0010, 2)]
        [InlineData(0b_0000_0100, 4)]
        public void ToBCD_Returns(byte value, byte expected)
        {
            Assert.Equal(expected, value.ToBCD());
        }

        [Theory]
        [InlineData(0x0F)]
        [InlineData(0x1F)]
        [InlineData(0x2F)]
        public void ToBCD_LargeLsb_Throws(byte value)
        {
            _ = Assert.Throws<InvalidOperationException>(() => value.ToBCD());
        }

        [Theory]
        [InlineData(1, 0b_0000_0001)]
        [InlineData(2, 0b_0000_0010)]
        [InlineData(4, 0b_0000_0100)]
        public void ToHex_Returns(byte value, byte expected)
        {
            Assert.Equal(expected, value.ToHex());
        }

        [Theory]
        [ClassData(typeof(SignificantBitsData))]
        public void SignificantBits_Returns(byte value, Tuple<byte, byte> expected)
        {
            var result = value.SignificantBits();
            Assert.Equal(expected.ToValueTuple(), result);
        }

        private sealed record SignificantBitsData : IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            public IEnumerator<object[]> GetEnumerator()
            {
                return new List<object[]>
                {
                    #region Zero
                    new object[]
                    {
                        (byte)0,
                        Tuple.Create((byte)0, (byte)0)
                    },
                    #endregion

                    #region LSBs
                    new object[]
                    {
                        (byte)0b_0000_0001,
                        Tuple.Create((byte)0b_0000_0001, (byte)0b_0000_0000)
                    },

                    new object[]
                    {
                        (byte)0b_0000_0010,
                        Tuple.Create((byte)0b_0000_0010, (byte)0b_0000_0000)
                    },

                    new object[]
                    {
                        (byte)0b_0000_0100,
                        Tuple.Create((byte)0b_0000_0100, (byte)0b_0000_0000)
                    },

                    new object[]
                    {
                        (byte)0b_0000_1000,
                        Tuple.Create((byte)0b_0000_1000, (byte)0b_0000_0000)
                    },
	                #endregion

                    #region MSBs
                    new object[]
                    {
                        (byte)0b_0001_0000,
                        Tuple.Create((byte)0b_0000_0000, (byte)0b_0001_0000)
                    },

                    new object[]
                    {
                        (byte)0b_0010_0000,
                        Tuple.Create((byte)0b_0000_0000, (byte)0b_0010_0000)
                    },

                    new object[]
                    {
                        (byte)0b_0100_0000,
                        Tuple.Create((byte)0b_0000_0000, (byte)0b_0100_0000)
                    },

                    new object[]
                    {
                        (byte)0b_1000_0000,
                        Tuple.Create((byte)0b_0000_0000, (byte)0b_1000_0000)
                    },
	                #endregion
                }.GetEnumerator();
            }
        }
    }
}
