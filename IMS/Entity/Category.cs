using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IMS.Entity
{
    [Serializable]
    public class Category : BaseEntity
    {
        public string Name { get; set; }
    }
}