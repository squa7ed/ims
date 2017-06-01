using IMS.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace IMS.Common
{
    public static class Utils
    {

        public static bool CanSave(this Relic relic)
        {
            return (relic.RootAge != null || relic.SecondaryAge != null || relic.ThirdAge != null || relic.FourthAge != null) &&
            relic.TotalAmount > 0 &&
            relic.Category != null &&
            relic.CollectedTimeRange != null &&
            relic.DisabilityLevel != null &&
            (relic.RootGrain != null || relic.SecondaryGrain != null || relic.ThirdGrain != null) &&
            relic.Level != null &&
            relic.Name != null &&
            relic.RelicId != null &&
            relic.IdType != null &&
            relic.Source != null &&
            relic.WeightRange != null;
        }
    }
}
