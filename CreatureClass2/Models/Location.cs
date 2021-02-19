using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CreatureClass2.Models
{
    public class Location
    {
        [Key]
        public int Id { get; set; }

        public string Letter { get; set; }

        public string Question { get; set; }

        public string Answer { get; set; }

        public string Creature { get; set; }

        public int PointsTo { get; set; }
    }
}