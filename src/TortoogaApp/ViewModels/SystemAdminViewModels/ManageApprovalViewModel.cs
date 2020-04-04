using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TortoogaApp.ViewModels.SystemAdminViewModels
{
    public class ManageApprovalViewModel
    {
        public Guid CarrierRegistrationGuid { get; set; }
        
        public string ContactPerson { get; set; }
        public string MCDotNumber { get; set; }

        public string BusinessName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string State { get; set; }
        public string Country { get; set; }

        public int Status { get; set; }

        public string RequestedDate { get; set; }

        public string ResponseBy { get; set; }

        public string ResponseDate { get; set; }
    }
}
