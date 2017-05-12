using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IMS.Entity
{
    [Serializable]
    public class Grain : BaseEntity
    {
        public string Name { get; set; }

        public Grain Parent { get; set; }

        public virtual List<Grain> Children { get; set; }

    }
}
