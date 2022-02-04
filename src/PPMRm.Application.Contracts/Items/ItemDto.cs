using System;
using System.Collections.Generic;
using System.Text;

namespace PPMRm.Items
{
    public class ItemDto
    {
        public string Id { get; set; }

        public string Name { get; set; }
        public string BaseUnit { get; set; }

        public decimal BaseUnitMultiplier { get; set; }
        public string TracerCategory { get; set; }
        public string UOM { get; set; }
        public int? NumberOfTreatments { get; set; }
    }
}
