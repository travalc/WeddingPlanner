using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WeddingPlanner.Models
{
    public class RSVP : BaseEntity
    {
        [Key]
        public int id {get; set;}
        public int users_id {get; set;}
        [ForeignKey("users_id")]
        public User user {get; set;}
        public int weddings_id {get; set;}
        [ForeignKey("weddings_id")]
        public Wedding wedding {get; set;}
    }
}