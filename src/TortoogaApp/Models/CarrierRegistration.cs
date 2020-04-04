using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TortoogaApp.Models
{
    public class CarrierRegistration : IEntity
    {
        [Key]
        public Guid CarrierRegistrationGuid { get; set; }

        public string TempPassword { get; set; }

        public string ContactPerson { get; set; }
        public string MCDotNumber { get; set; }

        public string BusinessName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string State { get; set; }
        public string Country { get; set; }

        public CarrierRegistrationStatus Status { get; set; }

        public Guid? ResponseBy { get; set; }

        public DateTime? RequestDate { get; set; }

        public DateTime? ResponseDate { get; set; }

    }

    public enum CarrierRegistrationStatus
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2,
        Completed = 3
    }
}
