using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TortoogaApp.ViewModels.DashboardViewModels;

namespace TortoogaApp.ViewModels.DashboardViewModels
{
    public class NotificationViewModel
    {
        public List<NewBookingViewModel> NewBooking { get; set; }

        public List<RatingReceivedViewModel> RatingReceived { get; set; }

        public List<ShipmentArrivalViewModel> ShipmentArrivals { get; set; }

        public List<ShipmentShippedViewModel> ShipmentShipped { get; set; }

        public List<ShipmentDelayedViewModel> ShipmentDelayed { get; set; }

        public List<ShipmentCancelledViewModel> ShipmentCancelled { get; set; }

        public List<NewMessageViewModel> NewMessage { get; set; }

        public List<BookingConfirmedViewModel> BookingConfirmed { get; set; }

        public List<BookingRejectedViewModel> BookingRejected { get; set; }

        public List<string> Orders { get; set; }

    }

    #region Notifications Models

    public class BaseNotificationViewModel
    {
        public string URL { get; set; }

        public Guid NotificationId { get; set; }

        public int OrderNo { get; set; }

        public string NotificationTime { get; set; }

        public bool IsRead { get; set; }

        public Guid BookingId { get; set; }

        public Guid ListingId { get; set; }


    }

    public class NewBookingViewModel : BaseNotificationViewModel
    {
        


    }

    public class RatingReceivedViewModel : BaseNotificationViewModel
    {
        public DateTime? RatedOn { get; set; }

    }

    public class ShipmentArrivalViewModel : BaseNotificationViewModel
    {
        

    }

    public class ShipmentShippedViewModel : BaseNotificationViewModel
    {
        

    }

    public class ShipmentDelayedViewModel : BaseNotificationViewModel
    {
        

    }

    public class ShipmentCancelledViewModel : BaseNotificationViewModel
    {
        
    }

    public class NewMessageViewModel : BaseNotificationViewModel
    {

    }

    public class BookingConfirmedViewModel : BaseNotificationViewModel
    {

    }

    public class BookingRejectedViewModel : BaseNotificationViewModel
    {

    }

    #endregion
}
