namespace GlobalConfiguration
{
    public interface IGlobalConfig
    {
        /// <summary>
        /// Indicates whether it's development environment.
        /// </summary>
        bool IsDevEnv { get; }
    }
}
