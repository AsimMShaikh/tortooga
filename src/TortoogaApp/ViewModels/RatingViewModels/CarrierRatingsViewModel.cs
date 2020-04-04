using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TortoogaApp.ViewModels.RatingViewModels
{
    public class CarrierRatingsViewModel
    {
        public string CompanyName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public double OverAllPercent { get; set; }

        public double PricePercent { get; set; }

        public double ServicePercent { get; set; }

        public double CommunicationPercent { get; set; }

        public int OverallExperience { get; set; }

        public int Service { get; set; }

        public int Communication { get; set; }

        public int Price { get; set; }

        public string CompanyProfileImageURL { get; set; }

        public string AccountNumber { get; set; }
    }
}
