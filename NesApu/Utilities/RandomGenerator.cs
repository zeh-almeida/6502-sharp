namespace NesApu.Utilities;

/// <summary>
/// Implements <see cref="IRandomGenerator"/> using the plain old <see cref="System.Random"/>
/// </summary>
public sealed record RandomGenerator : IRandomGenerator
{
    #region Constants
    private static Random Random { get; } = new();
    #endregion

    /// <inheritdoc/>
    public double NextDouble()
    {
#pragma warning disable SCS0005 // Not for crypto purposes
        return Random.NextDouble();
#pragma warning restore SCS0005 // Not for crypto purposes
    }
}
