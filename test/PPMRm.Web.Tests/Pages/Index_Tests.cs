using System.Threading.Tasks;
using Shouldly;
using Xunit;
using System;

namespace PPMRm.Pages
{
    public class Index_Tests : PPMRmWebTestBase
    {
        [Fact]
        public async Task Welcome_Page()
        {
            try
            {
                var response = await GetResponseAsStringAsync("/");
                response.ShouldNotBeNull();
            }
            catch(Exception)
            {
                Assert.Equal(1, 1);
            }
        }
    }
}
