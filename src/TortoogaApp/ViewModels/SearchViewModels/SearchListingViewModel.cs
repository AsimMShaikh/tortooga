using System.Collections.Generic;
using TortoogaApp.ViewModels.ListingViewModels;

namespace TortoogaApp.ViewModels.SearchViewModels
{
    public class SearchListingViewModel
    {
        public SearchFilter Filter { get; set; }
        public List<DetailListingViewModel> Listings { get; set; }
    }
}