using System;

namespace GlobalConfiguration
{
    public class GlobalConfig : IGlobalConfig
    {
        /// <inheritdoc/>
        public bool IsDevEnv
        {
            get
            {
                return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
            }
        }
    }
}
