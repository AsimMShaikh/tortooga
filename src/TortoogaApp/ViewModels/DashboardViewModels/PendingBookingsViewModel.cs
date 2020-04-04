using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TortoogaApp.ViewModels.DashboardViewModels
{
    public class PendingBookingsViewModel
    {

        public Guid BookingID { get; set; }

        public Guid ListingID { get; set; }

        public string Listing { get; set; }

        public string Origin { get; set; }

        public string Destination { get; set; }

        public decimal Amount { get; set; }

        public DateTime? BookingDate { get; set; }

    }
}
