using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPMRm.DataSync
{
    public class ItemEto
    {
        public string ItemId { get; set; }
        public string ProductId { get; set; }
        public string SupplierName { get; set; }
        public string ManufacturerName { get; set; }
        public string ManufacturerGTIN { get; set; }
        public string TracerCategory { get; set; }
        public string ProductName { get; set; }
        public int? ProductNumberOfTreatments { get; set; }
        public string UOM { get; set; }
        public string BaseUnit { get; set; }
        public decimal BaseUnitMultiplier { get; set; }
        public decimal? CatalogPrice { get; set; }
        public string CountryOfOrigin { get; set; }
        public string ManufacturerLocation { get; set; }
    }

    public static class ItemMapping
    {
        public readonly static Dictionary<string, string> Mapping = new Dictionary<string, string>
        {
            { "Artemether/Lumefantrine 20/120 mg Dispersible Tablet, 1 x 6x1 Blister Pack Tablets", "" },
            { "Artemether/Lumefantrine 20/120 mg Dispersible Tablet, 30 x 6x1 Blister Pack Tablets", "" },
            { "Artemether/Lumefantrine 20/120 mg Tablet, 30 x 6x1 Blister Pack Tablets", "" },
            {"", "" },
            {"", "" },
            {"", "" },
            {"", "" }
        };
    }
}
