using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TortoogaApp.ViewModels.DashboardViewModels
{
    public class BookingReportViewModel
    {
        public Guid BookingID { get; set; }

        public Guid ListingID { get; set; }

        public DateTime Date { get; set; }

        public string Details { get; set; }

        public string BookedBy { get; set; }

        public string Origin { get; set; }

        public string Destination { get; set; }

        public double Size { get; set; }

        public string Status { get; set; }

        public decimal Amount { get; set; }

        public DateTime? BookingDate { get; set; }


    }
}
