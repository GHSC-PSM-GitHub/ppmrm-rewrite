using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PPMRm.Core;
using PPMRm.Products;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.Domain.Repositories;

namespace PPMRm.Web.Pages.Countries
{
    public class EditModalModel : PPMRmPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        public ICountryAppService AppService { get; set; }
        public IRepository<Product, string> ProductRepository { get; set; }
        [BindProperty]

        public EditCountryViewModel Country { get; set; }
        [BindProperty]
        public List<SelectListItem> Products { get; set; }

        public EditModalModel(ICountryAppService appService, IRepository<Product, string> productRepository)
        {
            AppService = appService;
            ProductRepository = productRepository;
        }

        public async Task OnGetAsync()
       {
            Products = (await ProductRepository.ToListAsync()).OrderBy(p => p.Name).Select(p => new SelectListItem { Value = p.Id, Text = p.Name }).ToList();
            var country = await AppService.GetDetailsAsync(Id);
            if (country == null)
                throw new BusinessException("The country does not exist");
            Country = ObjectMapper.Map<UpdateCountryDto, EditCountryViewModel>(country);
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
        [DisplayName("Default Products")]
        [SelectItems("Products")]
        public List<string> ProductIds { get; set; }
    }
}
