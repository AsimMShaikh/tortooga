using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TortoogaApp.ViewModels.AccountViewModels
{
    public class CarrierProfileSetupViewModel
    {
        public Guid CarrierRegistrationGuid { get; set; }

        public string Email { get; set; }

        //public string ContactPerson { get; set; }

        //public string MCDotNumber { get; set; }

        //public string PhoneNumber { get; set; }

        //[Required]
        //[Display(Name = "State")]
        //public string State { get; set; }

        [Display(Name = "Account Number")]
        public string AccountNumber { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Old Password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Company Name")]
        public string BusinessName { get; set; }

        [Required]
        [Display(Name = "Company Bio")]
        public string CompanyBio { get; set; }

        [Required]
        [Display(Name = "What we ship")]
        public string CarrierShippingItems { get; set; }

        [Required]
        [Display(Name = "Site Url")]
        public string SiteUrl { get; set; }

        [Required]
        [Display(Name = "Address Line 1")]
        public string AddressLine1 { get; set; }

        [Display(Name = "Address Line 2")]
        public string AddressLine2 { get; set; }

        //[Display(Name = "Country")]
        //public string Country { get; set; }
        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Post Code")]
        public string PostCode { get; set; }

        [Required]
        [Display(Name = "Bank A/c name")]
        public string BankAccountName { get; set; }

        [Required]
        [Display(Name = "Bank Identification Number")]
        public string BankIdentificationNumber { get; set; }

        [Required]
        [Display(Name = "Bank A/c No")]
        public string BankAccountNumber { get; set; }


        public string CompanyLogoImage { get; set; }
    }
}