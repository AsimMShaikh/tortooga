using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TortoogaApp.ViewModels.CarrierViewModels
{
    public class CarrierProfileViewModel
    {

        public Guid CarrierGuid { get; set; }
        public string Email { get; set; }

        [Required]
        [Display(Name = "Company Name")]
        public string BusinessName { get; set; }

        
        [Display(Name = "Contact Person")]
        public string ContactPerson { get; set; }

        
        [Display(Name = "MC/Dot Number")]
        public string MCDotNumber { get; set; }

        
        [RegularExpression(@"^([0-9\(\)\/\+ \-]*)$", ErrorMessage = "Entered number is not valid.")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }


        [Display(Name = "Company Bio")]
        public string CompanyBio { get; set; }

       
        [Display(Name = "What we ship")]
        public string CarrierShippingItems { get; set; }


       
        [Display(Name = "State")]
        public string State { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string AccountNumber { get; set; }

      
        [Display(Name = "Site Url")]
        public string SiteUrl { get; set; }

       
        [Display(Name = "Address Line 1")]
        public string AddressLine1 { get; set; }

        [Display(Name = "Address Line 2")]
        public string AddressLine2 { get; set; }

       
        [Display(Name = "Country")]
        public string Country { get; set; }


        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "Post Code")]
        public string PostCode { get; set; }

        [Display(Name = "Bank A/C Name")]
        public string AccountName { get; set; }

        [Display(Name = "Bank Identification Number")]
        public string BankIdentificationNumber { get; set; }

        [Display(Name = "Bank A/C Number")]
        public string BankAccountNumber { get; set; }


        public string ImageUrl { get; set; }

        public List<SelectListItem> Countries { get; set; }

        public List<SelectListItem> Provincestates { get; set; }

        public List<string> ShippingItemsList { get; set; }


        


    }
}
