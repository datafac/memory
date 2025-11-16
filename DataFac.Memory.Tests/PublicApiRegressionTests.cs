using PublicApiGenerator;
using Shouldly;
using System;
using System.Threading.Tasks;
using VerifyXunit;
using Xunit;

namespace DataFac.Memory.Tests
{
    public class PublicApiRegressionTests
    {
        [Fact]
        public void VersionCheck()
        {
            ThisAssembly.AssemblyVersion.ShouldBe("0.16.0.0");
        }

        [Fact]
#if NET7_0_OR_GREATER
        public async Task CheckPublicApiNet70()
#else
        public async Task CheckPublicApiNet48()
#endif
        {
            // act
            var options = new ApiGeneratorOptions()
            {
                IncludeAssemblyAttributes = false
            };
            string currentApi = ApiGenerator.GeneratePublicApi(typeof(Octets).Assembly, options);

            // assert
            await Verifier.Verify(currentApi);
        }
    }
}
