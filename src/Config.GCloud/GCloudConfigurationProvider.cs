using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Extensions.Configuration.GCloud
{
    public class GCloudConfigurationProvider : ConfigurationProvider
    {
        private const string _prefix = "GCloud:";
        private readonly bool _updateFromCloud;

        public GCloudConfigurationProvider() : this(true)
        {

        }

        public GCloudConfigurationProvider(bool updateFromCloud)
        {
            _updateFromCloud = updateFromCloud;
        }

        public override void Load()
        {
            // Set default values of mandatory parameters
            Data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            if (!_updateFromCloud)
                return;

            var loadTasks = new Task<string>[]
            {
                GetUrlAsStringSafeAsync("http://metadata.google.internal/computeMetadata/v1/project/attributes/?recursive=true&alt=text"),
                GetUrlAsStringSafeAsync("http://metadata.google.internal/computeMetadata/v1/instance/attributes/?recursive=true&alt=text"),
                GetUrlAsStringSafeAsync("http://metadata.google.internal/computeMetadata/v1/instance/tags/?recursive=true&alt=text")
            };

            Task.WhenAll<string>(loadTasks).Wait();

            ParseResult(loadTasks[0].Result, "GCloud:Project:Attributes:");
            ParseResult(loadTasks[1].Result, "GCloud:Instance:Attributes:");
            ParseResult(loadTasks[2].Result, "GCloud:Instance:Tags:");
        }

        internal void Load(string cloudData)
        {
            Data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            ParseResult(cloudData, string.Empty);
        }

        /// <summary>
        /// Parses the result of a cloud metadata query, format is expected to be one variable declaration on each line (separated by \n)
        /// and the variable name separated by space, followed by the value 
        /// </summary>
        /// <param name="result">The result returned from a cloud REST call</param>
        private void ParseResult(string result, string sourceType)
        {
            if (string.IsNullOrWhiteSpace(result))
                return;
            var lines = result.Split(new char[] { '\n' });
            foreach(var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var parts = line.Split(new char[] { ' ' }, 2);
                if (string.IsNullOrWhiteSpace(parts[0]) || parts.Length == 1)
                    continue;
                Data[sourceType + parts[0]] = parts[1];
            }
        }

        /// <summary>
        /// Queries a url with the google cloud metadata header variable
        /// returns the response body on success, or otherwise null
        /// </summary>
        /// <param name="url">The URL to query</param>
        /// <returns></returns>
        private static async Task<string> GetUrlAsStringSafeAsync(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new InvalidOperationException("Invalid operation, specified url was empty");

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Metadata-Flavor", "Google");
                try
                {
                    using (var r = await client.GetAsync(url))
                    {
                        if (r.IsSuccessStatusCode)
                            return await r.Content.ReadAsStringAsync();
                    }
                }
                catch (Exception) { }
            }
            return null;
        }
    }
}
