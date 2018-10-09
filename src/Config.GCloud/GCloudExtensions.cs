using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration.GCloud;


namespace Microsoft.Extensions.Configuration
{
    public static class GCloudExtensions
    {
        /// <summary>
        /// Adds an <see cref="IConfigurationProvider"/> that reads configuration values from environment variables.
        /// </summary>
        /// <param name="configurationBuilder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddGCloudMetaData(this IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Add(new GCloudConfigurationSource());
            return configurationBuilder;
        }

        /// <summary>
        /// Adds an <see cref="IConfigurationProvider"/> that reads configuration values from environment variables.
        /// </summary>
        /// <param name="configurationBuilder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddGCloudMetaData(this IConfigurationBuilder configurationBuilder, bool enableLoadingFromCloud)
        {
            configurationBuilder.Add(new GCloudConfigurationSource { UpdateFromCloud = enableLoadingFromCloud });
            return configurationBuilder;
        }
    }
}
