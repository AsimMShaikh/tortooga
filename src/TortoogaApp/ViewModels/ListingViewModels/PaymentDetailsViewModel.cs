using System.ComponentModel.DataAnnotations;

namespace TortoogaApp.ViewModels.ListingViewModels
{
    public class PayListingViewModel
    {
        public PayListingViewModel()
        {
            BookListingVm = new BookListingViewModel();
            ListingDetailVm = new DetailListingViewModel();
        }

        [Display(Name = "Name on Card"), Required(AllowEmptyStrings = false)]
        public string CardHolderName { get; set; }

        [Display(Name = "Card Number"), Required]
        public string CardNumber { get; set; }

        [Display(Name = "Card Type"), EnumDataType(typeof(CreditCardType))]
        public CreditCardAttribute CardType { get; set; }

        [Display(Name = "Expiration Date"), Required(AllowEmptyStrings = false)]
        public int ExpiryMonth { get; set; }

        [Display(Name = "Expiration Date"), Required(AllowEmptyStrings = false)]
        public int ExpiryYear { get; set; }

        [Display(Name = "CVV"), Required(AllowEmptyStrings = false), MaxLength(4)]
        public string Cvv { get; set; }

        public BookListingViewModel BookListingVm { get; set; }
        public DetailListingViewModel ListingDetailVm { get; set; }
    }
}