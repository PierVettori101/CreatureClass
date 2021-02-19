using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CreatureClass2.Models
{
    public class Message
    {
        public int Id { get; set; }

        public string SentFrom { get; set; }

        public string SentTo { get; set; }

        public string Body { get; set; }

        public DateTime TimeSent { get; set; }

        public string status { get; set; }
    }
}