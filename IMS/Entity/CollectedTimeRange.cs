using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IMS.Entity
{
    [Serializable]
    public class CollectedTimeRange : BaseEntity
    {
        public string Name { get; set; }

        public virtual List<Relic> Relics { get; set; }
    }
}
