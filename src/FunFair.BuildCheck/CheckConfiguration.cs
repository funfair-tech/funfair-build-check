namespace FunFair.BuildCheck
{
    /// <summary>
    ///     Check configuration.
    /// </summary>
    public sealed class CheckConfiguration : ICheckConfiguration
    {
        /// <inheritdoc />
        public bool PreReleaseBuild { get; set; }
    }
}