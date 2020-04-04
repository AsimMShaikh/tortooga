using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TortoogaApp.Models
{
    public class Carrier : IEntity
    {
        [Key]
        public Guid CarrierGuid { get; set; }

        public string ContactPerson { get; set; }

        public string MCDotNumber { get; set; }

        public string BusinessName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string AccountNumber { get; set; }
        public string SiteUrl { get; set; }

        //TODO: Refactor out into a address object, as complext type is not supported yet in EF
        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }
        public string Country { get; set; }
        public string PostCode { get; set; }
        public string State { get; set; }
        public string City { get; set; }

        public string CompanyBio { get; set; }

        public string CarrierShippingItems { get; set; }

        [ForeignKey("BankingDetails")]
        public Guid CarrierBankingDetailsId { get; set; }

        public CarrierBankingDetails BankingDetails { get; set; }

        //[NotMapped]
        //public Address BusinessAddress { get; set; }

        public virtual ICollection<Listing> Listings { get; set; }

        //public virtual ICollection<ApplicationUser> Users { get; set; }
    }
}