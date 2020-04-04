using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TortoogaApp.ViewModels.DashboardViewModels
{
    public class CarrierDashboardViewModel
    {
        public Guid UserId { get; set; }

        public AccountSummaryViewModel AccountSummary { get; set; }

        public List<PendingBookingsViewModel> PendingBookingsModel { get; set; }

        public List<BookingReportViewModel> BookingReports { get; set; }

        public NotificationViewModel Notifications { get; set; }
    }
}
