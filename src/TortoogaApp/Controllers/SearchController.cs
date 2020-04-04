using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using TortoogaApp.Models;
using TortoogaApp.Services;
using TortoogaApp.ViewModels.ListingViewModels;
using TortoogaApp.ViewModels.SearchViewModels;

namespace TortoogaApp.Controllers
{
    public class SearchController : Controller
    {
        public SearchController(IDbService dbService, IMapper autoMapper)
        {
            this._dbService = dbService;
            this._autoMapper = autoMapper;
        }

        private IDbService _dbService;
        private IMapper _autoMapper;

        [HttpGet]
        [ActionName("SearchListing")]
        [AllowAnonymous]
        public IActionResult Index(SearchFilter filter = null)
        {
            var listings = _dbService.Get<Listing>(v => v.Status == ListingStatus.Active, x => x.Carrier).AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Origin))
            {
                listings = listings.Where(v => v.Origin.Contains(filter.Origin));
            }

            if (!string.IsNullOrWhiteSpace(filter.Destination))
            {
                listings = listings.Where(v => v.Destination.Contains(filter.Destination));
            }

            if (filter.ArriveByDate.HasValue)
            {
                listings = listings.Where(v => v.EstimatedArrivalDate <= filter.ArriveByDate.Value);
            }

            if (filter.ArriveByDate.HasValue)
            {
                listings = listings.Where(v => v.EstimatedArrivalDate <= filter.ArriveByDate.Value);
            }

            if (filter.MinimumHeight.HasValue)
            {
                listings = listings.Where(v => v.Height >= filter.MinimumHeight);
            }

            if (filter.MinimumWidth.HasValue)
            {
                listings = listings.Where(v => v.Width >= filter.MinimumWidth);
            }

            if (filter.MinimumLength.HasValue)
            {
                listings = listings.Where(v => v.Length >= filter.MinimumLength);
            }

            if (filter.MaximumCost.HasValue)
            {
                listings = listings.Where(v => v.Price <= filter.MaximumCost.Value);
            }

            if (filter.MinimumCost.HasValue)
            {
                listings = listings.Where(v => v.Price >= filter.MinimumCost.Value);
            }

            var listingVm = new SearchListingViewModel()
            {
                Filter = filter,
                Listings = _autoMapper.Map<List<Listing>, List<DetailListingViewModel>>(listings.ToList())
            };

            return View("Index", listingVm);
        }
    }
}