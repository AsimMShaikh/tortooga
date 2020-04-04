using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TortoogaApp.ViewModels.CarrierViewModels
{
    public class UserRatingsViewModel
    {

        public string CarrirerName { get; set; }

        public int OverallExperience { get; set; }

        public int Service { get; set; }

        public int Communication { get; set; }

        public int Price { get; set; }

        public Guid RatingID { get; set; }

        public Guid CarrierID { get; set; }

        public bool IsDeleted { get; set; }

        public string UserName { get; set; }

        public DateTime? RatingAddedOn { get; set; }

        public string ImageUrl { get; set; }

    }
}
