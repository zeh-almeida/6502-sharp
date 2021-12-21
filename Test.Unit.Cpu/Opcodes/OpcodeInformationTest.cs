using Cpu.Instructions.Logic;
using Cpu.Opcodes;
using Xunit;

namespace Test.Unit.Cpu.Opcodes
{
    public sealed record OpcodeInformationTest
    {
        #region Constants
        private const byte Opcode = 0x01;

        private const int Cycles = 1;

        private const int Bytes = 2;
        #endregion

        #region Properties
        private OpcodeInformation Subject { get; }
        #endregion

        #region Constructors
        public OpcodeInformationTest()
        {
            this.Subject = new OpcodeInformation(Opcode, Cycles, Bytes)
                .SetInstruction(new InclusiveOr());
        }
        #endregion

        [Fact]
        public void HashCode_Matches_True()
        {
            Assert.Equal(Opcode.GetHashCode(), this.Subject.GetHashCode());
        }

        [Fact]
        public void Equals_Object_IsTrueForInstruction()
        {
            Assert.True(this.Subject.Equals(this.Subject));
            Assert.True(this.Subject.Equals(this.Subject as object));
        }

        [Fact]
        public void Equals_Object_IsFalseForNonInstructions()
        {
            Assert.False(this.Subject.Equals(1));
        }

        [Fact]
        public void Opcode_Equals_Defined()
        {
            Assert.Equal(Opcode, this.Subject.Opcode);
        }

        [Fact]
        public void Cycles_Equals_Defined()
        {
            Assert.Equal(Cycles, this.Subject.Cycles);
        }

        [Fact]
        public void Bytes_Equals_Defined()
        {
            Assert.Equal(Bytes, this.Subject.Bytes);
        }

        [Fact]
        public void Instruction_Is_Set()
        {
            Assert.NotNull(this.Subject.Instruction);
        }

        [Fact]
        public void Instruction_SetNull_Throws()
        {
            _ = Assert.Throws<ArgumentNullException>(() => this.Subject.SetInstruction(null));
        }

        [Fact]
        public void Instruction_AlreadySet_Throws()
        {
            _ = Assert.Throws<ArgumentException>(() => this.Subject.SetInstruction(new InclusiveOr()));
        }
    }
}
