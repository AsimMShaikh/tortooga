using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using TortoogaApp.Models;

namespace TortoogaApp.Security
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser<Guid>, IEntity
    {
        [ForeignKey("Carrier")]
        public Guid? CarrierGuid { get; set; }

        public virtual Carrier Carrier { get; set; }
       
        public string FirstName { get; set; }
       
        public string LastName { get; set; }
        
        public Nullable<DateTime> BirthDate { get; set; }
           
       
        public string MobileNumber { get; set; }
     
        public string TimeZoneId { get; set; }
              
        
        public string AddressLine1 { get; set; }

      
        public string AddressLine2 { get; set; }

        
        public string PostCode { get; set; }

        
        public string State { get; set; }
        
        public string Suburb { get; set; }

        public string Country { get; set; }

        public bool IsDisabled { get; set; }

        public bool IsDeleted { get; set; }

    }
}