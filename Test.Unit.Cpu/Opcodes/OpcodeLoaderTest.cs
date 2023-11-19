using Cpu.Opcodes;
using Cpu.Opcodes.Exceptions;
using Moq;
using System.Collections;
using System.Resources;
using System.Text;
using Xunit;

namespace Test.Unit.Cpu.Opcodes;

public sealed record OpcodeLoaderTest
{
    #region Constants
    private const string DuplicateData = "[ { \"opcode\": 16, \"bytes\": 2, \"instructionQualifier\": \"Cpu.Instructions.Branches.BranchPositive\", \"minimumCycles\": 5, \"maximumCycles\": 0 }, { \"opcode\": 16, \"bytes\": 2, \"instructionQualifier\": \"Cpu.Instructions.Branches.BranchPositive\", \"minimumCycles\": 5, \"maximumCycles\": 0 } ]";
    #endregion

    [Fact]
    public async Task LoadAsync_Executes()
    {
        var subject = new OpcodeLoader();
        var result = await subject.LoadAsync();

        Assert.NotNull(result);
        Assert.NotEmpty(result.Opcodes);
    }

    [Fact]
    public async Task LoadAsync_No_Resources_Throws()
    {
        var sets = new Mock<ResourceSet>();
        var loader = new Mock<ResourceLoader>();

        var enumerator = new Dictionary<object, object>() as IDictionary;

        _ = sets
            .Setup(m => m.GetEnumerator())
            .Returns(enumerator.GetEnumerator());

        _ = loader
            .Setup(m => m.Load(It.Ref<ResourceManager>.IsAny))
            .Returns(sets.Object);

        var subject = new OpcodeLoader(loader.Object);
        _ = await Assert.ThrowsAsync<MisconfiguredOpcodeException>(subject.LoadAsync);
    }

    [Fact]
    public async Task LoadAsync_Empty_Resource_Throws()
    {
        var sets = new Mock<ResourceSet>();
        var loader = new Mock<ResourceLoader>();

        var enumerator = new Dictionary<object, object>() {
            { "Test", Array.Empty<byte>() },
        } as IDictionary;

        _ = sets
            .Setup(m => m.GetEnumerator())
            .Returns(enumerator.GetEnumerator());

        _ = loader
            .Setup(m => m.Load(It.Ref<ResourceManager>.IsAny))
            .Returns(sets.Object);

        var subject = new OpcodeLoader(loader.Object);
        _ = await Assert.ThrowsAsync<MisconfiguredOpcodeException>(subject.LoadAsync);
    }

    [Fact]
    public async Task LoadAsync_Bad_Resource_Type_Throws()
    {
        var sets = new Mock<ResourceSet>();
        var loader = new Mock<ResourceLoader>();

        var enumerator = new Dictionary<object, object>() {
            { "Test", "Not a Byte Array" },
        } as IDictionary;

        _ = sets
            .Setup(m => m.GetEnumerator())
            .Returns(enumerator.GetEnumerator());

        _ = loader
            .Setup(m => m.Load(It.Ref<ResourceManager>.IsAny))
            .Returns(sets.Object);

        var subject = new OpcodeLoader(loader.Object);
        _ = await Assert.ThrowsAsync<MisconfiguredOpcodeException>(subject.LoadAsync);
    }

    [Fact]
    public async Task LoadAsync_Duplicate_Opcode_Throws()
    {
        var sets = new Mock<ResourceSet>();
        var loader = new Mock<ResourceLoader>();

        var enumerator = new Dictionary<object, object>() {
            { "Test", Encoding.UTF8.GetBytes(DuplicateData) },
        } as IDictionary;

        _ = sets
            .Setup(m => m.GetEnumerator())
            .Returns(enumerator.GetEnumerator());

        _ = loader
            .Setup(m => m.Load(It.Ref<ResourceManager>.IsAny))
            .Returns(sets.Object);

        var subject = new OpcodeLoader(loader.Object);
        _ = await Assert.ThrowsAsync<DuplicateOpcodeException>(subject.LoadAsync);
    }
}