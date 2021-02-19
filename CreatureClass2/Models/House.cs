using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CreatureClass2.Models
{
    public class House
    {
        [Key]
        public string Name { get; set; }

        public int StartLocationId { get; set; }

        public Location Location { get; set; }
    }
}