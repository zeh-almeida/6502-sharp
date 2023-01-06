using Cpu.Opcodes;
using Xunit;

namespace Test.Unit.Cpu.Opcodes;

public sealed record OpcodeLoaderTest
{
    #region Properties
    private OpcodeLoader Subject { get; }
    #endregion

    #region Constructors
    public OpcodeLoaderTest()
    {
        this.Subject = new OpcodeLoader();
    }
    #endregion

    [Fact]
    public async Task LoadAsync_Executes()
    {
        var result = await this.Subject.LoadAsync();

        Assert.NotNull(result);
        Assert.NotEmpty(result.Opcodes);
    }
}