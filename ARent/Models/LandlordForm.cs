using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ARent.Models
{
    public class LandlordForm
    {
        [Required(ErrorMessage = "Address Required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "City Required")]
        public string City { get; set; }

        [Required(ErrorMessage = "State Required")]
        public string State { get; set; }

        [Required(ErrorMessage = "Zip Required")]
        public string Zip { get; set; }

        [Required(ErrorMessage = "ContactName Required")]
        public string ContactName { get; set; }

        [Required(ErrorMessage = "Email Required")]
        [StringLength(50, ErrorMessage = "The email is too long")]
        public string Email { get; set; }

        [StringLength(20, ErrorMessage = "The number is too long")]
        public string PhoneNumber { get; set; }

        public double Deposit { get; set; }

        [Range(0, 100000, ErrorMessage = "The rent is not valid number")]
        public double Rent { get; set; }
        public IList<string> PictureUrls { get; set; }

        [Required(ErrorMessage = "This is an invalid address")]
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public string Notes { get; set; }
    }
}