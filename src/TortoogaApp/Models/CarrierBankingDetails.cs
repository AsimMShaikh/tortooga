using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TortoogaApp.Models
{
    public class CarrierBankingDetails : IEntity
    {
        [Key]
        public Guid CarrierBankingDetailsId { get; set; }

        public string AccountName { get; set; }
        public string BankIdentificationNumber { get; set; }
        public string AccountNumber { get; set; }

        public Carrier Carrier { get; set; }
    }
}