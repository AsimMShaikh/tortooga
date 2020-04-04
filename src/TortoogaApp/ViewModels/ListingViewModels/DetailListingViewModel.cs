using System;
using System.ComponentModel.DataAnnotations;
using TortoogaApp.Models;

namespace TortoogaApp.ViewModels.ListingViewModels
{
    public class DetailListingViewModel
    {
        //TODO: Revisit later to see how to store image
        //public List<string> ImageUrls { get; set; }
        public string Title { get; set; }

        public Guid ListingGuid { get; set; }
        public string ReferenceNumber { get; set; }

        [Display(Name = "Departure")]
        public string Origin { get; set; }

        [Display(Name = "Destination")]
        public string Destination { get; set; }

        //public Dimension Dimension { get; set; }
        public double Length { get; set; }

        public double Width { get; set; }
        public double Height { get; set; }
        public double SquareFeet { get { return Length * Width; } }

        public string Description { get; set; }
        public decimal Price { get; set; }
        public ListingStatus Status { get; set; }
        public string CarrierName { get; set; }

        public Guid CarrierGuid { get; set; }

        public DateTime DepartureDate { get; set; }
        public DateTime EstimatedArrivalDate { get; set; }
        public int TransitStops { get; set; }

        //public string ContactDetails { get; set; }
        public DateTime AppraisalDateTime { get; set; }

        public string DropOffAddress { get; set; }
        public string PickUpAddress { get; set; }

        public int TotalNumberOfRatings { get; set; }

        public int CarrierRatings { get; set; }

        //public Guid? BookingGuid { get; set; }
    }
}