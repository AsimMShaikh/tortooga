using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TortoogaApp.ViewModels.ShipmentStatusViewModels
{
    public class ShipmentStatusRequestViewModel
    {
        public string BookingReferenceNo { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string Current_Location { get; set; }

        public DateTime Estimated_Arrival_Date { get; set; }

        public string Status { get; set; }

    }

    public class ShipmentStatusResponseViewModel
    {
        public bool IsSucceeded { get; set; }

        public string Message { get; set; }
    }

}
