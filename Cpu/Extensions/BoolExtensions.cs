namespace Cpu.Extensions
{
    /// <summary>
    /// Additional functionality to the <see cref="bool"/> struct
    /// </summary>
    public static class BoolExtensions
    {
        /// <summary>
        /// Returns the bool value as a bynary
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <returns>1 if true, 0 otherwise</returns>
        public static int AsBin(this bool value)
        {
            return value ? 1 : 0;
        }
    }
}
