using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TortoogaApp.Security;

namespace TortoogaApp.Models
{//TODO: Include all those auditable properties like datecreated, dateupdated, updatedby, createdby?
    public class Booking : IEntity
    {
        [Key]
        public Guid BookingGuid { get; set; }

        public Guid ListingGuid { get; set; }
        public Listing Listing { get; set; }

        [ForeignKey("CreatedByUser")]
        public Guid UserId { get; set; }

        public ApplicationUser CreatedByUser { get; set; }
        public Guid CarrierGuid { get; set; }
        public BookingStatus Status { get; set; }

        public string ReferenceNumber { get; set; }

        public double BookedLength { get; set; }
        public double BookedWidth { get; set; }
        public double BookedHeight { get; set; }
        public decimal BookingAmount { get; set; }

        public string PaymentReferenceNumber { get; set; }
        public bool IsDeleted { get; set; }

        public DateTime? BookingDate { get; set; }
    }

    public enum BookingStatus
    {
        BookedButNotConfirmed = 0,
        BookedAndConfirmed = 1,
        Rejected = 2,
        Cancelled = 3,
        Completed = 4,
        Shipped = 5,
        Delayed = 6
    }
}