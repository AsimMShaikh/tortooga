using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TortoogaApp.ViewModels.AccountViewModels
{
    public class RegisterUserViewModel
    {

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "User Name")]
        public string Username { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]        
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

      

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Birthdate")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<DateTime> BirthDate { get; set; }

        [Required]
        [RegularExpression(@"^([0-9\(\)\/\+ \-]*)$", ErrorMessage = "Entered number is not valid.")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Mobile Number")]
        [RegularExpression(@"^([0-9\(\)\/\+ \-]*)$", ErrorMessage = "Entered mobile number is not valid.")]
        public string MobileNumber { get; set; }

        [Required]        
        public string TimeZoneId { get; set; }

        
        [Display(Name = "Timezone")]
        public TimeZoneInfo TimeZone
        {
            get { return TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId); }
            set { TimeZoneId = value.Id; }
        }

        [Display(Name = "Address Line 1")]
        public string AddressLine1  { get; set; }

        [Display(Name = "Address Line 2")]
        public string AddressLine2 { get; set; }

        [Display(Name = "PostCode")]
        public string PostCode { get; set; }

        [Display(Name = "State")]
        public string StateCode { get; set; }


        [Display(Name = "Suburb")]
        public string Suburb { get; set; }


        [Display(Name = "Country")]
        public string CountryCode { get; set; }

        public List<SelectListItem> Countries { get; set; }

        public List<SelectListItem> Provincestates { get; set; }
        public IFormFile UserImage { get; set; }


    }
}
