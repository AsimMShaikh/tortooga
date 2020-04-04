using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TortoogaApp.Security;

namespace TortoogaApp.Models
{
    public class ProfileImage :IEntity
    {
        [Key]
        public Guid ImageGuid { get; set; }
               
        [ForeignKey("ImageForUser")]
        public Guid UserId { get; set; }

        public ApplicationUser ImageForUser { get; set; }

        public string ImageUri { get; set; }
        public byte[] bytes { get; set; }
        public long size { get; set; }

        public string Extension { get; set; }

        public string ContentType { get; set; }
               
    
        public Guid CreatedBy { get; set; }

        public Nullable<DateTime> CreatedOn { get; set; }

        public bool isDeleted { get; set; }




    }
}
