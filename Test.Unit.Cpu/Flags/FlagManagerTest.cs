using Cpu.Flags;
using Xunit;

namespace Test.Unit.Cpu.Flags
{
    public sealed record FlagManagerTest
    {
        #region Properties
        private FlagManager Subject { get; }
        #endregion

        #region Constructors
        public FlagManagerTest()
        {
            this.Subject = new FlagManager();
        }
        #endregion

        [Fact]
        public void WriteRead_Carry_Successful()
        {
            const bool value = true;

            this.Subject.IsCarry = value;
            var result = this.Subject.IsCarry;

            Assert.Equal(value, result);
        }

        [Fact]
        public void WriteRead_Zero_Successful()
        {
            const bool value = true;

            this.Subject.IsZero = value;
            var result = this.Subject.IsZero;

            Assert.Equal(value, result);
        }

        [Fact]
        public void WriteRead_InterruptDisable_Successful()
        {
            const bool value = true;

            this.Subject.IsInterruptDisable = value;
            var result = this.Subject.IsInterruptDisable;

            Assert.Equal(value, result);
        }

        [Fact]
        public void WriteRead_DecimalMode_Successful()
        {
            const bool value = true;

            this.Subject.IsDecimalMode = value;
            var result = this.Subject.IsDecimalMode;

            Assert.Equal(value, result);
        }

        [Fact]
        public void WriteRead_BreakCommand_Successful()
        {
            const bool value = true;

            this.Subject.IsBreakCommand = value;
            var result = this.Subject.IsBreakCommand;

            Assert.Equal(value, result);
        }

        [Fact]
        public void WriteRead_Overflow_Successful()
        {
            const bool value = true;

            this.Subject.IsOverflow = value;
            var result = this.Subject.IsOverflow;

            Assert.Equal(value, result);
        }

        [Fact]
        public void WriteRead_Negative_Successful()
        {
            const bool value = true;

            this.Subject.IsNegative = value;
            var result = this.Subject.IsNegative;

            Assert.Equal(value, result);
        }

        #region Save/Load
        [Fact]
        public void Serialize_State_Writes()
        {
            this.Subject.IsCarry = true;
            this.Subject.IsZero = true;
            this.Subject.IsInterruptDisable = true;
            this.Subject.IsDecimalMode = true;
            this.Subject.IsBreakCommand = true;
            this.Subject.IsOverflow = true;
            this.Subject.IsNegative = true;

            var result = this.Subject.Save();
            Assert.Equal(0x7F, result);
        }

        [Fact]
        public void Deserialize_State_Loads()
        {
            this.Subject.Load(0x7F);

            Assert.True(this.Subject.IsCarry);
            Assert.True(this.Subject.IsZero);
            Assert.True(this.Subject.IsInterruptDisable);
            Assert.True(this.Subject.IsDecimalMode);
            Assert.True(this.Subject.IsBreakCommand);
            Assert.True(this.Subject.IsOverflow);
            Assert.True(this.Subject.IsNegative);
        }
        #endregion
    }
}
