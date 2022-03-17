using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PPMRm.Core;
using System.ComponentModel;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace PPMRm.Web.Pages.Countries
{
    public class EditModalModel : PPMRmPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        public ICountryAppService AppService { get; set; }
        [BindProperty]

        public EditCountryViewModel Country { get; set; }

        public EditModalModel(ICountryAppService appService)
        {
            AppService = appService;
        }

        public async Task OnGetAsync()
       {
            var country = await AppService.GetAsync(Id);
            if (country == null)
                throw new BusinessException("The country does not exist");
            Country = ObjectMapper.Map<CountryDto, EditCountryViewModel>(country);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var countryDto = ObjectMapper.Map<EditCountryViewModel, UpdateCountryDto>(Country);
            await AppService.UpdateAsync(Id, countryDto);
            return NoContent();
        }
    }

    public class EditCountryViewModel
    {
        [HiddenInput]
        public string Id { get; set; }
        [DisabledInput]
        public string Name { get; set; }
        [DisplayName("Minimum MOS")]
        public int MinStock { get; set; }
        [DisplayName("Maximum MOS")]
        public int MaxStock { get; set; }
    }
}
