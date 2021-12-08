namespace Cpu.Forms.Serialization
{
    public sealed record EmulatorState
    {
        #region Properties
        public string ProgramPath { get; set; }

        public IEnumerable<byte> State { get; set; }
        #endregion
    }
}
