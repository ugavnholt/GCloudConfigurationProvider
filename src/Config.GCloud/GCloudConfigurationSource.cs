using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Net.Http;

namespace Microsoft.Extensions.Configuration.GCloud
{
    /// <summary>
    /// Adds google cloud metadata as a configuration source
    /// Attributes, and tags are read from the instance, and tags are read from the project
    /// The rest of the metadata is ignored.
    /// </summary>
    public class GCloudConfigurationSource : IConfigurationSource
    {
        private static GCloudConfigurationProvider _provider = null;

        /// <summary>
        /// General flag, that specified whether an attempt to read data from the cloud should be made
        /// if the cloud is known not to be available, setting this flag to false, can prevent the 
        /// delay the underlaying http request introduces when it fails
        /// </summary>
        public bool UpdateFromCloud { get; set; }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            if (_provider == null)
                _provider = new GCloudConfigurationProvider(UpdateFromCloud);

            return _provider;
        }
    }
}
