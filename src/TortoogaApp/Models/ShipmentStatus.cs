using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TortoogaApp.Models
{
    public class ShipmentStatus : IEntity
    {
        [Key]
        public Guid ShipmentStatusId { get; set; }

        public string BookingReferenceNo { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string CurrentLocation { get; set; }

        public DateTime? EstimatedArrivalDate { get; set; }

        public int Status { get; set; }   
        
        public DateTime ShipmentStatusUpdateOn { get; set; }     
    }

    public enum ShipmentStatusEnum
    {
        shipped = 0,
        delayed = 1,
        cancelled = 2,
        completed = 3,
    }

}
