namespace Cpu.Forms.Serialization
{
    public sealed record EmulatorState
    {
        #region Properties
        public string ProgramPath { get; set; }

        public Memory<byte> State { get; set; }
        #endregion

        #region Constructors
        public EmulatorState()
        {
            this.ProgramPath = string.Empty;
            this.State = Array.Empty<byte>();
        }
        #endregion
    }
}
