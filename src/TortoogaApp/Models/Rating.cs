using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TortoogaApp.Security;

namespace TortoogaApp.Models
{
    public class Rating : IEntity
    {
        [Key]        
        public Guid RatingId { get; set; }
        
        [ForeignKey("Booking")]
        public Guid BookingID { get; set; }

        public Booking Booking { get; set; }
        
        public Guid CarrierID { get; set; }
               
        public Guid UserId { get; set; }        

        public int OverallExperience { get; set; }

        public int Service { get; set; }

        public int Communication { get; set; }

        public int Price { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsRequestedForRemoval { get; set; }

        public Nullable<DateTime> RatingAddedOn { get; set; }

    }
}
