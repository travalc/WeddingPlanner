using System;
using System.Globalization;
using System.ComponentModel.DataAnnotations;
namespace WeddingPlanner.Models
{
    public class WeddingViewModel : BaseEntity
    {
        [Required]
        public string wedderOne {get; set;}

        [Required]
        public string wedderTwo {get; set;}

        [Required]
        [ValidDate]
        public DateTime date {get; set;}

        [Required]
        public string address {get; set;}

    }

    public class ValidDateAttribute : ValidationAttribute
    {
        private DateTime _today = DateTime.Now;


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            WeddingViewModel wedding = (WeddingViewModel)validationContext.ObjectInstance;
            if(_today <= wedding.date)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Date must be not be in the past");
            }
        }
    }
}