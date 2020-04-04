using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TortoogaApp.ViewModels.CarrierViewModels
{
    public class RegisterCarrierViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Company Name")]
        public string BusinessName { get; set; }

        [Required]        
        [Display(Name = "Contact Person")]
        public string ContactPerson { get; set; }

        [Required]
        [Display(Name = "MC/Dot Number")]
        public string MCDotNumber { get; set; }

        [Required]
        [RegularExpression(@"^([0-9\(\)\/\+ \-]*)$", ErrorMessage = "Entered number is not valid.")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "State")]
        public string StateCode { get; set; }

        [Required]
        [Display(Name = "Country")]
        public string CountryCode { get; set; }


        public List<SelectListItem> Countries { get; set; }

        public List<SelectListItem> Provincestates { get; set; }


    }
}
