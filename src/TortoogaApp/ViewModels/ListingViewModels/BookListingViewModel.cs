using System.ComponentModel.DataAnnotations;

namespace TortoogaApp.ViewModels.ListingViewModels
{
    public class BookListingViewModel
    {
        [Display(Name = "Required Dimension")]
        public double Length { get; set; }

        public double Width { get; set; }
        public double Height { get; set; }

        public double SquareFeet { get { return Length * Width; } }

        [Range(typeof(bool), "true", "true")]
        public bool TermsAndConditions { get; set; }
    }
}