using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TortoogaApp.ViewModels.NotificationsList
{
    public class NotificationListViewModel
    {
        public Guid NotificationId { get; set; }

        public string NotificationType { get; set; }

        public string ReceivedOn { get; set; }

        public string Description { get; set; }

        public bool IsRead { get; set; }

        public DateTime? ReadOn { get; set; }

        public DateTime NotificationDateTime { get; set; }

        public string URL { get; set; }
    }
}
