using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IMS.Entity
{
    [Serializable]
    public class Age : BaseEntity
    {
        public string Name { get; set; }

        public Age Parent { get; set; }

        public virtual List<Age> Children { get; set; }
    }
}
