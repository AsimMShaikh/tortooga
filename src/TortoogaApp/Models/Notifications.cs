using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TortoogaApp.Models
{
    public class Notifications : IEntity
    {
        [Key]
        public Guid NotificationId { get; set; }

        public string NotificationDescription { get; set; }

        public DateTime NotificationDateTime { get; set; }

        public int NotificationType { get; set; }

        public Guid ReleventUserId { get; set; }

        public Guid RelevantNotificationRefId { get; set; }

        public bool IsRead { get; set; }

        public DateTime? ReadDateTime { get; set; }
    }

    public enum NotificationTypeEnum
    {
        NewBooking = 0,
        RatingReceived = 1,
        ShipmentArrival = 2,
        NewMessage = 3,
        ShipmentShipped = 4,
        ShipmentDelayed = 5,
        ShipmentCancelled = 6,
        BookingConfirmed = 7,
        BookingRejected = 8
    }
}
