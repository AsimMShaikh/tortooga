using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TortoogaApp.ViewModels.UserViewModels
{
    public class UserProfileViewModel
    {
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Display(Name = "Birthdate")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/mm/dd}")]
        public string BirthDate { get; set; }

        [Required]
        public string TimeZoneId { get; set; }


        [Display(Name = "Timezone")]
        public TimeZoneInfo TimeZone
        {
            get { return TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId); }
            set { TimeZoneId = value.Id; }
        }

        [Required]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered phone number is not valid.")]
        [RegularExpression(@"^([0-9\(\)\/\+ \-]*)$", ErrorMessage = "Entered number is not valid.")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Mobile Number")]
        [RegularExpression(@"^([0-9\(\)\/\+ \-]*)$", ErrorMessage = "Entered number is not valid.")]
        public string MobileNumber { get; set; }

        [Display(Name = "Address Line 1")]
        public string AddressLine1 { get; set; }

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

        //[Display(Name = "Profile Image")]
        //public IFormFile UserImage { get; set; }

       // public UserImageModel ImageVal { get; set; }

        public string ImageUrl { get; set; }

    }


    public class UserImageModel
    {
        public Guid? ImageGuid { get; set; }
        public string ImageUri { get; set; }
        public byte[] bytes { get; set; }
        public long size { get; set; }

        public string Extension { get; set; }

        public string ContentType { get; set; }
              
    }
}
