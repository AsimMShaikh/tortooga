using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TortoogaApp.ViewModels.CarrierViewModels
{
    public class CarrierBioViewModel
    {
        public string CarrierGuid { get; set; }
        public string Email { get; set; }

        public string BusinessName { get; set; }

        public string PhoneNumber { get; set; }

        public string CompanyBio { get; set; }

        public List<string> ShippingItemsList { get; set; }

        public string ImageUrl { get; set; }

        public string Country { get; set; }

        public string State { get; set; }

        public string AddressLine1 { get; set; }

  
        public string AddressLine2 { get; set; }

        public string City { get; set; }

        public string PostCode { get; set; }

        public int CarrierRatings { get; set; }

        public int TotalNumberOfRatings { get; set; }



    }
}
