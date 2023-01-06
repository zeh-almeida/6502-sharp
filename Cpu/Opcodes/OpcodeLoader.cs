using System.Text.Json;

namespace Cpu.Opcodes
{
    /// <summary>
    /// Loads available opcodes from the configuration
    /// </summary>
    public sealed record OpcodeLoader
    {
        #region Properties
        /// <summary>
        /// Loaded Opcodes. May be empty.
        /// <see cref="OpcodeInformation"/>
        /// </summary>
        public IEnumerable<OpcodeInformation> Opcodes { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes the loader
        /// </summary>
        public OpcodeLoader()
        {
            this.Opcodes = Array.Empty<OpcodeInformation>();
        }
        #endregion

        /// <summary>
        /// Loads all available opcodes.
        /// </summary>
        /// <returns>Current instance for method chaining</returns>
        public async Task<OpcodeLoader> LoadAsync()
        {
            await this.LoadJsonData();
            return this;
        }

        private async Task LoadJsonData()
        {
            var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);

            using var stream = new MemoryStream(InstructionDefinition.Values);
            var result = await JsonSerializer.DeserializeAsync<IEnumerable<OpcodeInformation>>(stream, options);

            if (result is null)
            {
                throw new Exception();
            }

            this.Opcodes = result.ToArray();
        }
    }
}
