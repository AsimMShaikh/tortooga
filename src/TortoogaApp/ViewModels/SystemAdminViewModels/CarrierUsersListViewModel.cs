using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TortoogaApp.ViewModels.SystemAdminViewModels
{
    public class CarrierUsersListViewModel
    {
        public Guid UserID { get; set; }
       
        public string Email { get; set; }

        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Country { get; set; }
        public string State { get; set; }

        public string Disabled { get; set; }
                                           
        public List<SelectListItem> Carriers { get; set; }

        public List<SelectListItem> Countries { get; set; }

        public List<SelectListItem> Provincestates { get; set; }
                            
        public string CarrierName { get; set; }

        public Guid CarrierId { get; set; }

        public Guid UserId { get; set; }
     
    }
}
