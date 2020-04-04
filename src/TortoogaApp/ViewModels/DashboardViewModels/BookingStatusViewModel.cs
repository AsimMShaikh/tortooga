using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TortoogaApp.ViewModels.DashboardViewModels
{
    public class BookingStatusViewModel
    {
        public string BookingReferencenNo { get; set; }

        public string Origin { get; set; }

        public string Destination { get; set; }

        public string Status { get; set; }

        public DateTime? BookingDate { get; set; }

        public Guid ListingID { get; set; }
    }
}
