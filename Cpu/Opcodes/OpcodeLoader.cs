using Cpu.Opcodes.Exceptions;
using System.Collections;
using System.Globalization;
using System.Text.Json;

namespace Cpu.Opcodes
{
    /// <summary>
    /// Loads available opcodes from the configuration
    /// </summary>
    public sealed record OpcodeLoader
    {
        #region Constants
        private const int OpcodeAmount = byte.MaxValue;
        #endregion

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
            await this.ReadAll();
            return this;
        }

        private async Task ReadAll()
        {
            var resourceSet = InstructionDefinition
                .ResourceManager
                .GetResourceSet(CultureInfo.CurrentUICulture, true, true);

            if (resourceSet is null)
            {
                return;
            }

            var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            var foundValues = new HashSet<OpcodeInformation>(OpcodeAmount);

            foreach (DictionaryEntry item in resourceSet)
            {
                if (item.Value is not byte[] bytes || !bytes.Any())
                {
                    var value = item.Key.ToString()
                        ?? throw new KeyNotFoundException("Cannot load resource");

                    throw new MisconfiguredOpcodeException(value);
                }

                using var stream = new MemoryStream(bytes);

                var result = await JsonSerializer.DeserializeAsync<IEnumerable<OpcodeInformation>>(stream, options)
                    ?? throw new Exception();

                foreach (var opcode in result)
                {
                    if (foundValues.Contains(opcode))
                    {
                        throw new DuplicateOpcodeException(opcode.Opcode);
                    }

                    _ = foundValues.Add(opcode);
                }
            }

            this.Opcodes = foundValues;
        }
    }
}
