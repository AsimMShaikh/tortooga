using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TortoogaApp.Models
{
    public class CustomerRatings : IEntity
    {
        [Key]
        public Guid RatingId { get; set; }

        [ForeignKey("Booking")]
        public Guid BookingID { get; set; }

        public Booking Booking { get; set; }

        public Guid CarrierID { get; set; }

        public Guid UserId { get; set; }

        public int Rating { get; set; }

        public bool IsDeleted { get; set; }

        public Nullable<DateTime> RatingAddedOn { get; set; }
    }
}
