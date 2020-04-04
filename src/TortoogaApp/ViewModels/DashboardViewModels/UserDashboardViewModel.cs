using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TortoogaApp.ViewModels.DashboardViewModels
{
    public class UserDashboardViewModel
    {
        public List<BookingStatusViewModel> BookingStatus { get; set; }

        public List<ShipmentStatusViewModel> ShipmentStatus { get; set; }

        public NotificationViewModel Notifications { get; set; }
    }
}
