using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ARent.Models
{
    public class Contact
    {
        [Required(ErrorMessage = "ContactName Required")]
        [StringLength(1,ErrorMessage = "The Name is too long")]
        public string ContactName { get; set; }
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email Required")]
        public string Email { get; set; }
    }
}