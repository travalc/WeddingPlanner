using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace WeddingPlanner.Models
{
    public class Wedding : BaseEntity
    {
        [Key]
        public int id {get; set;}
        public int creator_id {get; set;}
        public string wedderOne {get; set;}
        public string wedderTwo {get; set;}
        public string address {get; set;}
        public List<RSVP> rsvps {get; set;}
        public DateTime date {get; set;}
        public DateTime created_at {get; set;}
        public DateTime updated_at {get; set;}

        public Wedding()
        {
            rsvps = new List<RSVP>();
        }
    }
}