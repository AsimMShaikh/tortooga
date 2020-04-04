using System;

namespace TortoogaApp.ViewModels.ListingViewModels
{
    public class BookingSummaryViewModel
    {
        public string ReferenceNumber { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime DepartureDate { get; set; }
        public string BusinessName { get; set; }
        public string UserEmail { get; set; }
    }
}