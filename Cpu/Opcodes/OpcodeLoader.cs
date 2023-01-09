using Cpu.Opcodes.Exceptions;
using System.Collections;
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

        private ResourceLoader Loader { get; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes the loader
        /// </summary>
        public OpcodeLoader()
            : this(new ResourceLoader())
        {
        }

        /// <summary>
        /// Initializes the loader
        /// </summary>
        /// <param name="loader"><see cref="ResourceLoader"/> to read from</param>
        public OpcodeLoader(ResourceLoader loader)
        {
            this.Opcodes = Array.Empty<OpcodeInformation>();
            this.Loader = loader;
        }
        #endregion

        /// <summary>
        /// Loads all available opcodes.
        /// </summary>
        /// <returns>Current instance for method chaining</returns>
        public async Task<OpcodeLoader> LoadAsync()
        {
            await this.ReadResourcesAsync();
            return this;
        }

#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.
        private async Task ReadResourcesAsync()
        {
            var resourceSet = this.Loader.LoadInstructions();

            var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            var foundValues = new HashSet<OpcodeInformation>(OpcodeAmount);

            foreach (DictionaryEntry item in resourceSet)
            {
                if (item.Value is not byte[] bytes || !bytes.Any())
                {
                    var value = item.Key.ToString();
                    throw new MisconfiguredOpcodeException(value);
                }

                using var stream = new MemoryStream(bytes);
                var result = await JsonSerializer.DeserializeAsync<IEnumerable<OpcodeInformation>>(stream, options);

                foreach (var opcode in result)
                {
                    if (!foundValues.Add(opcode))
                    {
                        throw new DuplicateOpcodeException(opcode.Opcode);
                    }
                }
            }

            if (!foundValues.Any())
            {
                throw new MisconfiguredOpcodeException(nameof(resourceSet));
            }

            this.Opcodes = foundValues;
        }
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
    }
}
