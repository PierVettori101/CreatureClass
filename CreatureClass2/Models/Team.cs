using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CreatureClass2.Models
{
    public class Team: ApplicationUser
    {
        
        public int Score { get; set; }

        [ForeignKey("House")]
        public string HouseName { get; set; }

        public House House { get; set; }


        public int LocationId { get; set; }

        public Location Location { get; set; }

        public int Counter { get; set; }

        public DateTime RegisteredAt { get; set; }

        public List<Message> Messages { get; set; }

        public Team()
        {
            Messages = new List<Message>();
        }
    }
}