using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TortoogaApp.Models
{
    public class Listing : IEntity
    {
        [Key]
        public Guid ListingGuid { get; set; }

        public string Title { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }

        //Complex type not support in ef7 yet
        //public Dimension Dimension { get; set; }
        public double Length { get; set; }

        public double Width { get; set; }
        public double Height { get; set; }

        [NotMapped]
        public double SquareFeet { get { return Length * Width; } }

        public string Description { get; set; }
        public decimal Price { get; set; }
        public ListingStatus Status { get; set; }

        public Guid CarrierGuid { get; set; }
        public Carrier Carrier { get; set; }

        public DateTime DepartureDate { get; set; }
        public DateTime EstimatedArrivalDate { get; set; }
        public int TransitStops { get; set; }
        public string ContactDetails { get; set; }
        public DateTime AppraisalDateTime { get; set; }

        //TODO: Complex type not support yet, change it to Address object later
        public string DropOffAddress { get; set; }

        public string PickUpAddress { get; set; }

        public bool IsDeleted { get; set; }

        public string ReferenceNumber { get; set; }
    }

    public enum ListingStatus
    {
        Booked = 0,
        Active = 1,
        Inactive = 2,
        Cancelled = 3,
    }
}