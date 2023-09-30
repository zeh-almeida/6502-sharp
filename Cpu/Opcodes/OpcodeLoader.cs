using Cpu.Opcodes.Exceptions;
using System.Collections;
using System.Text.Json;

namespace Cpu.Opcodes;

/// <summary>
/// Loads available opcodes from the configuration
/// </summary>
public sealed record OpcodeLoader
{
    #region Constants
    private const int OpcodeAmount = byte.MaxValue;

    private static JsonSerializerOptions JsonOptions { get; } = new JsonSerializerOptions(JsonSerializerDefaults.Web);
    #endregion

    #region Properties
    /// <summary>
    /// Loaded Opcodes. May be empty.
    /// <see cref="IOpcodeInformation"/>
    /// </summary>
    public IEnumerable<IOpcodeInformation> Opcodes { get; private set; }

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
        this.Opcodes = Array.Empty<IOpcodeInformation>();
        this.Loader = loader;
    }
    #endregion

    /// <summary>
    /// Loads all available opcodes.
    /// </summary>
    /// <returns>Current instance for method chaining</returns>
    public async Task<OpcodeLoader> LoadAsync()
    {
        await this.ReadResourcesAsync().ConfigureAwait(false);
        return this;
    }

#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.
    private async Task ReadResourcesAsync()
    {
        var resourceSet = this.Loader.LoadInstructions();
        var foundValues = new HashSet<IOpcodeInformation>(OpcodeAmount);

        foreach (DictionaryEntry item in resourceSet)
        {
            if (item.Value is not byte[] bytes || bytes.Length == 0)
            {
                var value = item.Key.ToString();
                throw new MisconfiguredOpcodeException(value);
            }

            using var stream = new MemoryStream(bytes);

            // Must use concrete type when deserializing because you cannot instantiate interfaces
            var result = await JsonSerializer.DeserializeAsync<IEnumerable<OpcodeInformation>>(stream, JsonOptions).ConfigureAwait(false);

            foreach (var opcode in result)
            {
                if (!foundValues.Add(opcode))
                {
                    throw new DuplicateOpcodeException(opcode.Opcode);
                }
            }
        }

        if (foundValues.Count == 0)
        {
            throw new MisconfiguredOpcodeException(nameof(resourceSet));
        }

        this.Opcodes = foundValues;
    }
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
}
