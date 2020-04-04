using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TortoogaApp.ViewModels.RatingViewModels
{
    public class FeedBackRatingsViewModel
    {
        public string Origin { get; set; }

        public string Destination { get; set; }

        public string CarrirerName { get; set; }

        [Display(Name = "Booking Ref.")]
        public string BookingReferenceNumber { get; set; }

        [Display(Name = "Listing Ref.")]
        public string ListingReferenceNumber { get; set; }

        public int OverallExperience { get; set; }

        public int Service { get; set; }

        public int Communication { get; set; }

        public int Price { get; set; }

        public DateTime DepartedDate { get; set; }

        public DateTime ArrivalDate { get; set; }

        public Guid RatingID { get; set; }

        public Guid BookingID { get; set; }

        public Guid CarrierID { get; set; }

        public bool IsDeleted { get; set; }

        public string UserName { get; set; }

        public DateTime? RatingAddedOn { get; set; }

        public string CarrierLogoPath { get; set; }


    }
}
