namespace NesApu.Utilities;

/// <summary>
/// Generates a random number
/// </summary>
public interface IRandomGenerator
{
    /// <summary>
    /// Generates a random <see cref="double"/> value
    /// </summary>
    /// <returns><see cref="double"/> value</returns>
    double NextDouble();
}
