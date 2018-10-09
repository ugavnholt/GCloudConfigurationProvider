using System;
using Xunit;
using System.Collections;

namespace Microsoft.Extensions.Configuration.GCloud.Test
{
    public class GCloudTest
    {
        [Fact]
        public void LoadKeyValuePairsFromDictionary()
        {
            var testData =
                "GCloud:Attributes:Test-attribute with some value\n" +
                "GCloud:Attributes:another-Test-Attribute \n" + // no value
                "GCloud:Attributes:another-Test-Attribute\n" + // another no value
                "GCloud:Attributes:Test-ATTRIBUTE reassigned\n";

            var gcloudConfigSrc = new GCloudConfigurationProvider(false);

            gcloudConfigSrc.Load(testData);

            Assert.Equal("reassigned", gcloudConfigSrc.Get("GCloud:Attributes:Test-ATTRIBUTE"));
        }

    }
}
