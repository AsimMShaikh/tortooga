using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TortoogaApp.ViewModels.DashboardViewModels
{
    public class AccountSummaryViewModel
    {

        public int AwaitingApproval { get; set; }

        public int InTransit { get; set; }

        public int Completed { get; set; }

        public int TotalMonthBookings { get; set; }

        public decimal TotalMonthRevenue { get; set; }

        public decimal CurrentEscrowTotal { get; set; }

        public int TotalReviewsCurrentMonth { get; set; }

        public decimal AverageProfitPerft { get; set; }
    }
}
