using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TortoogaApp.ViewModels.DashboardViewModels
{
    public class ShipmentStatusViewModel
    {
        public string BookingReferenceNo { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public string CurrentLocation { get; set; }

        public DateTime? EstimatedArrivalDate { get; set; }

        public string Status { get; set; }

        public DateTime ShipmentStatusUpdateOn { get; set; }

        public string Origin { get; set;  }

        public string Destination { get; set; }

        public double LatDecimal { get; set; }

        public double LongDecimal { get; set; }
    }

}
