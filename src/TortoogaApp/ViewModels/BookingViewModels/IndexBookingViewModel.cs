using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TortoogaApp.Models;

namespace TortoogaApp.ViewModels.BookingViewModels
{
    public class IndexBookingViewModel
    {
        public List<Booking> Bookings { get; set; }

        public string BookingStatus { get; set; }
    }
}