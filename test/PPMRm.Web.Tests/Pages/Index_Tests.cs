using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace PPMRm.Pages
{
    public class Index_Tests : PPMRmWebTestBase
    {
        [Fact]
        public async Task Welcome_Page()
        {
            var response = await GetResponseAsStringAsync("/");
            response.ShouldNotBeNull();
        }
    }
}
