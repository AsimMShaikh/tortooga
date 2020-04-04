using System;

namespace TortoogaApp.ViewModels.SearchViewModels
{
    public class SearchFilter
    {
        public string Origin { get; set; }
        public string Destination { get; set; }

        public double? MinimumLength { get; set; }
        public double? MinimumWidth { get; set; }
        public double? MinimumHeight { get; set; }

        public DateTime? DepartureDate { get; set; }

        public DateTime? ArriveByDate { get; set; }
        public decimal? MaximumCost { get; set; }
        public decimal? MinimumCost { get; set; }
    }
}