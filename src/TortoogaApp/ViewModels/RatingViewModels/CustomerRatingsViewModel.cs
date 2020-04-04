using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TortoogaApp.ViewModels.RatingViewModels
{
    public class CustomerRatingsViewModel
    {
        public string Origin { get; set; }

        public string Destination { get; set; }

        public string CarrirerName { get; set; }

        public string BookingReferenceNumber { get; set; }

        public string ListingReferenceNumber { get; set; }

        public Guid BookingID { get; set; }

        public Guid CarrierID { get; set; }

        public int Count { get; set; }

        public int? Average { get; set; }

        public Guid UserId { get; set; }

        public string UserProfileImagePath { get; set; }

        public DateTime? RatingAddedOn { get; set; }

        public Guid RatingID { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string MobileNumber { get; set; }

        public string FullName { get; set; }

        public DateTime DepartedDate { get; set; }

        public DateTime ArrivalDate { get; set; }

        public int Rating { get; set; }


        public string CarrierLogoPath { get; set; }
    }

}
