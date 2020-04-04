using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TortoogaApp.ViewModels.BadgeCountsViewModels
{
    public class BadgeCountViewModel
    {
        public int RatingsReceived { get; set; }

        public int PendingBookings { get; set; }

        public int ProcessingBookings { get; set; }

        public int ShippingBookings { get; set; }
    }
}
