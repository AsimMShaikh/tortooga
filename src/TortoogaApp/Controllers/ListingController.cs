using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TortoogaApp.Messaging.Emails;
using TortoogaApp.Models;
using TortoogaApp.Security;
using TortoogaApp.Services;
using TortoogaApp.ViewModels.ListingViewModels;

namespace TortoogaApp.Controllers
{
    [Route("/Listing")]
    public class ListingController : Controller
    {
        private IDbService _dbService;
        private IMapper _autoMapper;
        private UserManager<ApplicationUser> _userManager;
        private IEmailNotificationFactory _emailNotificationFactory;

        public ListingController(IDbService dbService, IMapper autoMapper, UserManager<ApplicationUser> userManager, IEmailNotificationFactory emailNotificationFactory)
        {
            this._dbService = dbService;
            this._autoMapper = autoMapper;
            this._userManager = userManager;
            this._emailNotificationFactory = emailNotificationFactory;
        }

        /// <summary>
        /// View Listing Details
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Route("{id}/View")]
        public async Task<IActionResult> View(Guid id)
        {
            var listing = _dbService.GetSingle<Listing>(v => v.ListingGuid == id);
            int OverallExperience = 0;
            if (listing == null)
            {
                return NotFound();
            }

            var listingDetailsVm = _autoMapper.Map<Listing, DetailListingViewModel>(listing);


            var carrier = _dbService.GetSingle<Carrier>(v => v.CarrierGuid == listingDetailsVm.CarrierGuid);

            if(carrier != null)
            {
                listingDetailsVm.CarrierName = carrier.BusinessName;
                listingDetailsVm.CarrierGuid = carrier.CarrierGuid;
                var RatingDetails = _dbService.Get<Rating>(v => v.CarrierID == carrier.CarrierGuid && v.IsDeleted == false).ToList();

                if (RatingDetails.Count > 0)
                {                   

                    int count_five = RatingDetails.Where(x => x.OverallExperience == 5).Count();
                    int count_four = RatingDetails.Where(x => x.OverallExperience == 4).Count();
                    int count_Three = RatingDetails.Where(x => x.OverallExperience == 3).Count();
                    int count_Two = RatingDetails.Where(x => x.OverallExperience == 2).Count();
                    int count_One = RatingDetails.Where(x => x.OverallExperience == 1).Count();

                    listingDetailsVm.TotalNumberOfRatings = RatingDetails.Count;

                    double ovalue = Convert.ToDouble((5 * count_five + 4 * count_four + 3 * count_Three + 2 * count_Two + 1 * count_One) / (count_five + count_four + count_Three + count_Two + count_One));
                    double percent = ((ovalue / 5) * 100);



                    OverallExperience = (int)Math.Round(ovalue, MidpointRounding.AwayFromZero);
                    var OverAllPercent = percent;


                }
                listingDetailsVm.CarrierRatings = OverallExperience;
            }


            return View(listingDetailsVm);
        }

        /// <summary>
        /// Retrieves all the listings of the carrier
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = RoleType.CARRIER_ADMIN)]
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            var listings = _dbService.Get<Listing>(v => v.CarrierGuid == currentUser.CarrierGuid).ToList();
            var indexListingVm = new IndexListingViewModel()
            {
                Listings = _autoMapper.Map<List<Listing>, List<DetailListingViewModel>>(listings),
            };

            return View(indexListingVm);
        }

        /// <summary>
        /// Adds this instance.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = RoleType.CARRIER_ADMIN)]
        [Route("Add")]
        public IActionResult Add()
        {
            return View(new ListingViewModel());
        }

        /// <summary>
        /// Adds this instance.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = RoleType.CARRIER_ADMIN)]
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add(ListingViewModel addListingVm)
        {
            //TODO: Add client side validations as well for these dates,this will be later stage
            if (addListingVm.EstimatedArrivalDate <= DateTime.Today || addListingVm.DepartureDate <= DateTime.Today || addListingVm.EstimatedArrivalDate <= DateTime.Today)
            {
                ModelState.AddModelError(string.Empty, "Dates must not be today");
            }

            if (addListingVm.EstimatedArrivalDate <= addListingVm.DepartureDate)
            {
                ModelState.AddModelError(string.Empty, "Departure Date must be before Estimated Arrival Date");
            }

            if (addListingVm.AppraisalDate >= addListingVm.DepartureDate)
            {
                ModelState.AddModelError(string.Empty, "Appraisal Date must be before Departure Date");
            }

            if (!ModelState.IsValid)
            {
                return View(addListingVm);
            }

            //TODO: image upload do later, still haven't figure out how
            //TODO: need to do cargo restriction as well, this will be later stage
            var listing = _autoMapper.Map<ListingViewModel, Listing>(addListingVm);

            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            listing.CarrierGuid = currentUser.CarrierGuid.Value;
            listing.ReferenceNumber = GenerateReferenceNumber();

            _dbService.Add(listing);

            if (listing.ListingGuid == Guid.Empty)
            {
                throw new Exception("Db Error");
            }

            return RedirectToAction(nameof(Edit), new { @id = listing.ListingGuid });
        }

        /// <summary>
        /// Get Edit Page
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Authorize(Roles = RoleType.CARRIER_ADMIN)]
        [HttpGet]
        [Route("{id}/Edit")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var listing = _dbService.GetSingle<Listing>(v => v.ListingGuid == id && !v.IsDeleted);

            if (listing == null)
            {
                return NotFound();
            }

            var listingVm = _autoMapper.Map<Listing, ListingViewModel>(listing);

            return View("Edit", listingVm);
        }

        /// <summary>
        /// Edits the specified listing view model.
        /// </summary>
        /// <param name="listingViewModel">The listing view model.</param>
        /// <returns></returns>
        [Authorize(Roles = RoleType.CARRIER_ADMIN)]
        [HttpPost]
        [Route("{id}/Edit")]
        public async Task<IActionResult> Edit(ListingViewModel listingViewModel)
        {
            //TODO: Add client side validations as well for these dates,this will be later stage
            if (listingViewModel.EstimatedArrivalDate <= DateTime.Today || listingViewModel.DepartureDate <= DateTime.Today || listingViewModel.EstimatedArrivalDate <= DateTime.Today)
            {
                ModelState.AddModelError(string.Empty, "Dates must not be today");
            }

            if (listingViewModel.EstimatedArrivalDate <= listingViewModel.DepartureDate)
            {
                ModelState.AddModelError(string.Empty, "Departure Date must be before Estimated Arrival Date");
            }

            if (listingViewModel.AppraisalDate >= listingViewModel.DepartureDate)
            {
                ModelState.AddModelError(string.Empty, "Appraisal Date must be before Departure Date");
            }

            if (!ModelState.IsValid)
            {
                return View(listingViewModel);
            }

            var listing = _dbService.GetSingle<Listing>(v => v.ListingGuid == listingViewModel.ListingGuid.Value && !v.IsDeleted);

            if (listing == null)
            {
                return NotFound();
            }

            var updatedlisting = _autoMapper.Map<ListingViewModel, Listing>(listingViewModel, listing);

            if (_dbService.Update(updatedlisting) == 0)
            {
                throw new Exception("Update Failed");
            }

            return View(nameof(Edit), listingViewModel);
        }

        /// <summary>
        /// Pays for the Listing
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="bookListingVm">The book listing vm.</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [Route("{id}/Payment")]
        public IActionResult PayListing(Guid id, BookListingViewModel bookListingVm)
        {
            var listing = _dbService.GetSingle<Listing>(v => v.ListingGuid == id, x => x.Carrier);

            if (listing == null)
            {
                return NotFound();
            }

            //HACK: Return bad request for now
            if (!ModelState.IsValid || (listing.SquareFeet < bookListingVm.SquareFeet) || bookListingVm.SquareFeet <= 0)
            {
                return BadRequest();
            }

            //TODO: Perhaps also show how much space the fella wanna book and the original space, and the final price should adjust if we were to implement the leftover space thing
            var detailListingVm = _autoMapper.Map<Listing, DetailListingViewModel>(listing);

            var payListingVm = new PayListingViewModel()
            {
                ListingDetailVm = detailListingVm,
                BookListingVm = bookListingVm
            };

            return View(payListingVm);
        }

        /// <summary>
        /// Books the listing.
        /// </summary>
        /// <param name="listingId">The listing identifier.</param>
        /// <param name="payListingVm">The pay listing vm.</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BookListing(Guid listingId, PayListingViewModel payListingVm)
        {
            var listing = _dbService.GetSingle<Listing>(v => v.ListingGuid == listingId && v.Status == ListingStatus.Active, x => x.Carrier);

            if (listing == null)
            {
                return NotFound();
            }

            if (listing.Height < payListingVm.BookListingVm.Height ||
               listing.Width < payListingVm.BookListingVm.Width ||
               listing.Length < payListingVm.BookListingVm.Length ||
               listing.SquareFeet < payListingVm.BookListingVm.SquareFeet)
            {
                return Forbid();
            }

            //TODO: Server side verification with payment gateway to "hold" the amount, only capture when the confirmation is being made at carrier side
            var paymentReferenceNumber = Guid.NewGuid().ToString(); //Simulate payment reference

            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var newBooking = new Booking()
            {
                Listing = listing,
                CarrierGuid = listing.CarrierGuid,
                Status = BookingStatus.BookedButNotConfirmed,
                ReferenceNumber = GenerateReferenceNumber(),
                CreatedByUser = currentUser,
                BookingAmount = payListingVm.ListingDetailVm.Price,
                BookedHeight = payListingVm.BookListingVm.Height,
                BookedLength = payListingVm.BookListingVm.Length,
                BookedWidth = payListingVm.BookListingVm.Width,
                PaymentReferenceNumber = paymentReferenceNumber,
                //Added for Booking Date
                BookingDate = DateTime.Now
            };

            _dbService.Add(newBooking);
            listing.Status = ListingStatus.Booked;
            _dbService.Update(listing);
                      
            if (newBooking.BookingGuid == Guid.Empty)
            {
                throw new Exception("Db Error, booking creation failed");
            }

            //Notification Entry For the Notification Booking
            var newnotification = new Notifications()
            {
                NotificationDescription = "New Booking",
                NotificationDateTime = DateTime.Now,
                NotificationType = (int)NotificationTypeEnum.NewBooking,
                ReleventUserId = listing.CarrierGuid, // As Notification is to be shown to carrier
                RelevantNotificationRefId = listing.ListingGuid,
                IsRead = false,
                ReadDateTime = null
            };
            _dbService.Add(newnotification);

            _emailNotificationFactory.SendCarrierBookingConfirmationEmail(listing);
            _emailNotificationFactory.SendUserBookingConfirmationEmail(newBooking, currentUser);

            return RedirectToAction(nameof(BookingSummary), new { @id = newBooking.BookingGuid });
        }

        /// <summary>
        /// Shows boooking summary page.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Authorize]
        [ActionName("BookingSummary")]
        [Route("{id}/BookingSummary")]
        public async Task<IActionResult> BookingSummary(Guid id)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var booking = _dbService.GetSingle<Booking>(v => v.BookingGuid == id && v.UserId == currentUser.Id, x => x.CreatedByUser, x => x.Listing);

            if (booking == null)
            {
                return NotFound();
            }

            var bookingSummaryVm = _autoMapper.Map<Booking, BookingSummaryViewModel>(booking);

            return View(bookingSummaryVm);
        }

        private string GenerateReferenceNumber()
        {
            return new Random().Next(1, 1000000000).ToString("000000000");
        }
    }
}