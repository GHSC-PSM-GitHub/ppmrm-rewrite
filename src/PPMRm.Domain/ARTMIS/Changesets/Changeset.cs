using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPMRm.ARTMIS.Changesets
{
    public class Changeset
    {
        public string Id { get; set; }
        public int Version { get; set; }
        public DateTime Date { get; set; }
        public int PeriodId { get; set; }


        public void Apply(OrderLineEvent @event)
        {
            if (Version == 0) // First time only
            {
                Date = DateTime.ParseExact(@event.FileName.Substring(0, ARTMISConsts.TimestampLength), ARTMISConsts.DateTimeFormat, null);
                PeriodId = Convert.ToInt32(@event.FileName.Substring(0, ARTMISConsts.PeriodIdLength));
            }
                
            Version++;
        }
    }
}
