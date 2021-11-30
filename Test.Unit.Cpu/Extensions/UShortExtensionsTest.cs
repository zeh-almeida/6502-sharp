using Cpu.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace Test.Unit.Cpu.Extensions
{
    public sealed record UShortExtensionsTest
    {
        [Theory]
        [InlineData(0, 0)]
        [InlineData(0b_0000_0000_0000_1111, 0b_0000_0000_0000_1111)]
        [InlineData(0b_0000_0000_1111_1111, 0b_0000_0000_1111_1111)]
        [InlineData(0b_0000_1111_1111_1111, 0b_0000_0000_1111_1111)]
        [InlineData(0b_1111_1111_1111_1111, 0b_0000_0000_1111_1111)]
        [InlineData(0b_0000_0000_0000_0001, 0b_0000_0000_0000_0001)]
        [InlineData(0b_0000_0000_0000_0010, 0b_0000_0000_0000_0010)]
        [InlineData(0b_0000_0000_0000_0100, 0b_0000_0000_0000_0100)]
        [InlineData(0b_0000_0000_0000_1000, 0b_0000_0000_0000_1000)]
        [InlineData(0b_0000_0000_0001_0000, 0b_0000_0000_0001_0000)]
        [InlineData(0b_0000_0000_0010_0000, 0b_0000_0000_0010_0000)]
        [InlineData(0b_0000_0000_0100_0000, 0b_0000_0000_0100_0000)]
        [InlineData(0b_0000_0000_1000_0000, 0b_0000_0000_1000_0000)]
        [InlineData(0b_0000_0001_0000_0000, 0b_0000_0000_0000_0000)]
        [InlineData(0b_0000_0010_0000_0000, 0b_0000_0000_0000_0000)]
        [InlineData(0b_0000_0100_0000_0000, 0b_0000_0000_0000_0000)]
        [InlineData(0b_0000_1000_0000_0000, 0b_0000_0000_0000_0000)]
        [InlineData(0b_0001_0000_0000_0000, 0b_0000_0000_0000_0000)]
        [InlineData(0b_0010_0000_0000_0000, 0b_0000_0000_0000_0000)]
        [InlineData(0b_0100_0000_0000_0000, 0b_0000_0000_0000_0000)]
        [InlineData(0b_1000_0000_0000_0000, 0b_0000_0000_0000_0000)]
        public void LeastSignificantBits_Returns(ushort data, ushort expected)
        {
            Assert.Equal(expected, data.LeastSignificantBits());
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(0b_0000_0000_0000_1111, 0b_0000_0000_0000_0000)]
        [InlineData(0b_0000_0000_1111_1111, 0b_0000_0000_0000_0000)]
        [InlineData(0b_0000_1111_1111_1111, 0b_0000_1111_0000_0000)]
        [InlineData(0b_1111_1111_1111_1111, 0b_1111_1111_0000_0000)]
        [InlineData(0b_0000_0000_0000_0001, 0b_0000_0000_0000_0000)]
        [InlineData(0b_0000_0000_0000_0010, 0b_0000_0000_0000_0000)]
        [InlineData(0b_0000_0000_0000_0100, 0b_0000_0000_0000_0000)]
        [InlineData(0b_0000_0000_0000_1000, 0b_0000_0000_0000_0000)]
        [InlineData(0b_0000_0000_0001_0000, 0b_0000_0000_0000_0000)]
        [InlineData(0b_0000_0000_0010_0000, 0b_0000_0000_0000_0000)]
        [InlineData(0b_0000_0000_0100_0000, 0b_0000_0000_0000_0000)]
        [InlineData(0b_0000_0000_1000_0000, 0b_0000_0000_0000_0000)]
        [InlineData(0b_0000_0001_0000_0000, 0b_0000_0001_0000_0000)]
        [InlineData(0b_0000_0010_0000_0000, 0b_0000_0010_0000_0000)]
        [InlineData(0b_0000_0100_0000_0000, 0b_0000_0100_0000_0000)]
        [InlineData(0b_0000_1000_0000_0000, 0b_0000_1000_0000_0000)]
        [InlineData(0b_0001_0000_0000_0000, 0b_0001_0000_0000_0000)]
        [InlineData(0b_0010_0000_0000_0000, 0b_0010_0000_0000_0000)]
        [InlineData(0b_0100_0000_0000_0000, 0b_0100_0000_0000_0000)]
        [InlineData(0b_1000_0000_0000_0000, 0b_1000_0000_0000_0000)]
        public void MostSignificantBits_Returns(ushort data, ushort expected)
        {
            Assert.Equal(expected, data.MostSignificantBits());
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(0b_0000_0000_0000_0001, 0b_0000_0001_0000_0000, 0b_0000_0001_0000_0001)]
        [InlineData(0b_0000_0000_0000_0010, 0b_0000_0010_0000_0000, 0b_0000_0010_0000_0010)]
        [InlineData(0b_0000_0000_0000_0100, 0b_0000_0100_0000_0000, 0b_0000_0100_0000_0100)]
        [InlineData(0b_0000_0000_0000_1000, 0b_0000_1000_0000_0000, 0b_0000_1000_0000_1000)]
        [InlineData(0b_0000_0000_0001_0000, 0b_0001_0000_0000_0000, 0b_0001_0000_0001_0000)]
        [InlineData(0b_0000_0000_0010_0000, 0b_0010_0000_0000_0000, 0b_0010_0000_0010_0000)]
        [InlineData(0b_0000_0000_0100_0000, 0b_0100_0000_0000_0000, 0b_0100_0000_0100_0000)]
        [InlineData(0b_0000_0000_1000_0000, 0b_1000_0000_0000_0000, 0b_1000_0000_1000_0000)]
        public void CombineSignificantBits_Returns(ushort lsb, ushort msb, ushort expected)
        {
            var result = lsb.CombineSignificantBits(msb);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(0b_1111_0000_0000_1001, true)]
        [InlineData(0b_1111_0000_0000_1000, false)]
        public void IsFirstBitSet_Returns(ushort value, bool expected)
        {
            Assert.Equal(expected, value.IsFirstBitSet());
        }

        [Theory]
        [InlineData(0b_0000_0000_0000_0001, false, 0b_0000_0000_0000_0000)]
        [InlineData(0b_1000_0000_0000_0000, false, 0b_0100_0000_0000_0000)]
        [InlineData(0b_0000_0000_0000_0001, true, 0b_1000_0000_0000_0000)]
        [InlineData(0b_1000_0000_0000_0000, true, 0b_1100_0000_0000_0000)]
        public void RotateRight_Returns(ushort value, bool isCarry, ushort expected)
        {
            Assert.Equal(expected, value.RotateRight(isCarry));
        }

        [Theory]
        [InlineData(0b_0000_0000_0000_0001, false, 0b_0000_0000_0000_0010)]
        [InlineData(0b_1000_0000_0000_0000, false, 0b_0000_0000_0000_0000)]
        [InlineData(0b_0000_0000_0000_0001, true, 0b_0000_0000_0000_0011)]
        [InlineData(0b_1000_0000_0000_0000, true, 0b_0000_0000_0000_0001)]
        public void RotateLeft_Returns(ushort value, bool isCarry, ushort expected)
        {
            Assert.Equal(expected, value.RotateLeft(isCarry));
        }

        [Theory]
        [InlineData(0b_1111_1111_1111_1111, 0, true)]
        [InlineData(0b_1111_1111_1111_1111, 1, true)]
        [InlineData(0b_1111_1111_1111_1111, 2, true)]
        [InlineData(0b_1111_1111_1111_1111, 3, true)]
        [InlineData(0b_1111_1111_1111_1111, 4, true)]
        [InlineData(0b_1111_1111_1111_1111, 5, true)]
        [InlineData(0b_1111_1111_1111_1111, 6, true)]
        [InlineData(0b_1111_1111_1111_1111, 7, true)]
        [InlineData(0b_1111_1111_1111_1111, 8, true)]
        [InlineData(0b_1111_1111_1111_1111, 9, true)]
        [InlineData(0b_1111_1111_1111_1111, 10, true)]
        [InlineData(0b_1111_1111_1111_1111, 11, true)]
        [InlineData(0b_1111_1111_1111_1111, 12, true)]
        [InlineData(0b_1111_1111_1111_1111, 13, true)]
        [InlineData(0b_1111_1111_1111_1111, 14, true)]
        [InlineData(0b_1111_1111_1111_1111, 15, true)]
        [InlineData(0b_1111_1111_1111_1110, 0, false)]
        [InlineData(0b_1111_1111_1111_1101, 1, false)]
        [InlineData(0b_1111_1111_1111_1011, 2, false)]
        [InlineData(0b_1111_1111_1111_0111, 3, false)]
        [InlineData(0b_1111_1111_1110_1111, 4, false)]
        [InlineData(0b_1111_1111_1101_1111, 5, false)]
        [InlineData(0b_1111_1111_1011_1111, 6, false)]
        [InlineData(0b_1111_1111_0111_1111, 7, false)]
        [InlineData(0b_1111_1110_1111_1111, 8, false)]
        [InlineData(0b_1111_1101_1111_1111, 9, false)]
        [InlineData(0b_1111_1011_1111_1111, 10, false)]
        [InlineData(0b_1111_0111_1111_1111, 11, false)]
        [InlineData(0b_1110_1111_1111_1111, 12, false)]
        [InlineData(0b_1101_1111_1111_1111, 13, false)]
        [InlineData(0b_1011_1111_1111_1111, 14, false)]
        [InlineData(0b_0111_1111_1111_1111, 15, false)]
        public void IsBitSet_Returns(ushort value, ushort index, bool expected)
        {
            Assert.Equal(expected, value.IsBitSet(index));
        }

        [Theory]
        [InlineData(0b_0000_0000_0000_0000, true)]
        [InlineData(0b_0000_0000_0000_0001, false)]
        public void IsZero_Returns(ushort value, bool expected)
        {
            Assert.Equal(expected, value.IsZero());
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(16)]
        public void IsBitSet_Throws(int index)
        {
            _ = Assert.Throws<ArgumentOutOfRangeException>(() => ushort.MinValue.IsBitSet(index));
        }

        [Theory]
        [InlineData(0x0001, 0x7F, 0x0080)]
        [InlineData(0x1000, 0x80, 0x0F80)]
        public void BranchAddress_Executes(ushort value, byte offset, ushort expected)
        {
            Assert.Equal(expected, value.BranchAddress(offset));
        }

        [Theory]
        [ClassData(typeof(SignificantBitsData))]
        public void SignificantBits_Returns(ushort value, Tuple<byte, byte> expected)
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
                        (ushort)0,
                        Tuple.Create((byte)0, (byte)0)
                    },
                    #endregion

                    #region LSBs
                    new object[]
                    {
                        (ushort)0b_0000_0000_0000_0001,
                        Tuple.Create((byte)0b_0000_0001, (byte)0b_0000_0000)
                    },

                    new object[]
                    {
                        (ushort)0b_0000_0000_0000_0010,
                        Tuple.Create((byte)0b_0000_0010, (byte)0b_0000_0000)
                    },

                    new object[]
                    {
                        (ushort)0b_0000_0000_0000_0100,
                        Tuple.Create((byte)0b_0000_0100, (byte)0b_0000_0000)
                    },

                    new object[]
                    {
                        (ushort)0b_0000_0000_0000_1000,
                        Tuple.Create((byte)0b_0000_1000, (byte)0b_0000_0000)
                    },

                    new object[]
                    {
                        (ushort)0b_0000_0000_0001_0000,
                        Tuple.Create((byte)0b_0001_0000, (byte)0b_0000_0000)
                    },

                    new object[]
                    {
                        (ushort)0b_0000_0000_0010_0000,
                        Tuple.Create((byte)0b_0010_0000, (byte)0b_0000_0000)
                    },

                    new object[]
                    {
                        (ushort)0b_0000_0000_0100_0000,
                        Tuple.Create((byte)0b_0100_0000, (byte)0b_0000_0000)
                    },

                    new object[]
                    {
                        (ushort)0b_0000_0000_1000_0000,
                        Tuple.Create((byte)0b_1000_0000, (byte)0b_0000_0000)
                    },
	                #endregion

                    #region MSBs
                    new object[]
                    {
                        (ushort)0b_0000_0001_0000_0000,
                        Tuple.Create((byte)0b_0000_0000, (byte)0b_0000_0001)
                    },

                    new object[]
                    {
                        (ushort)0b_0000_0010_0000_0000,
                        Tuple.Create((byte)0b_0000_0000, (byte)0b_0000_0010)
                    },

                    new object[]
                    {
                        (ushort)0b_0000_0100_0000_0000,
                        Tuple.Create((byte)0b_0000_0000, (byte)0b_0000_0100)
                    },

                    new object[]
                    {
                        (ushort)0b_0000_1000_0000_0000,
                        Tuple.Create((byte)0b_0000_0000, (byte)0b_0000_1000)
                    },

                    new object[]
                    {
                        (ushort)0b_0001_0000_0000_0000,
                        Tuple.Create((byte)0b_0000_0000, (byte)0b_0001_0000)
                    },

                    new object[]
                    {
                        (ushort)0b_0010_0000_0000_0000,
                        Tuple.Create((byte)0b_0000_0000, (byte)0b_0010_0000)
                    },

                    new object[]
                    {
                        (ushort)0b_0100_0000_0000_0000,
                        Tuple.Create((byte)0b_0000_0000, (byte)0b_0100_0000)
                    },

                    new object[]
                    {
                        (ushort)0b_1000_0000_0000_0000,
                        Tuple.Create((byte)0b_0000_0000, (byte)0b_1000_0000)
                    },
	                #endregion
                }.GetEnumerator();
            }
        }
    }
}
