using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TortoogaApp.Messaging.Emails;
using TortoogaApp.Models;
using TortoogaApp.Security;
using TortoogaApp.Services;
using TortoogaApp.ViewModels.BookingViewModels;

namespace TortoogaApp.Controllers
{
    [Authorize]
    [Route("/Booking")]
    public class BookingController : Controller
    {
        private IDbService _dbService;
        private IMapper _autoMapper;
        private UserManager<ApplicationUser> _userManager;
        private IEmailNotificationFactory _emailNotificationFactory;

        public BookingController(IDbService dbService, IMapper autoMapper, UserManager<ApplicationUser> userManager, IEmailNotificationFactory emailNotificationFactory)
        {
            this._dbService = dbService;
            this._autoMapper = autoMapper;
            this._userManager = userManager;
            this._emailNotificationFactory = emailNotificationFactory;
        }

        [Route("ManageBookings")]
        public async Task<IActionResult> ManageBookings(string status = null)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var bookings = _dbService.Get<Booking>(v => v.CarrierGuid == currentUser.CarrierGuid && !v.IsDeleted, x => x.Listing, x => x.CreatedByUser);

            if (!string.IsNullOrEmpty(status))
            {//TODO: the status should be refactored out as an object then passed in from view
                switch (status)
                {
                    case "pending":
                        bookings = bookings.Where(v => v.Status == BookingStatus.BookedButNotConfirmed);
                        break;

                    case "processing":
                        bookings = bookings.Where(v => v.Status == BookingStatus.BookedAndConfirmed);
                        break;

                    case "shipped":
                        bookings = bookings.Where(v => v.Status == BookingStatus.Shipped);
                        break;

                    case "rejected":
                        bookings = bookings.Where(v => v.Status == BookingStatus.Rejected);
                        break;

                    case "cancelled":
                        bookings = bookings.Where(v => v.Status == BookingStatus.Cancelled);
                        break;

                    case "completed":
                        bookings = bookings.Where(v => v.Status == BookingStatus.Completed);
                        break;
                }
            }

            var indexBookingVm = new IndexBookingViewModel() { Bookings = bookings.ToList(), BookingStatus = status };

            return View(nameof(ManageBookings), indexBookingVm);
        }

        [Route("UserBookings")]
        public async Task<IActionResult> UserBookings()
        {//TODO: Separate out user booking page and Admin booking page
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var bookings = _dbService.Get<Booking>(v => v.UserId == currentUser.Id, x => x.Listing, x => x.CreatedByUser);

            var indexBookingVm = new IndexBookingViewModel() { Bookings = bookings.ToList() };

            return View(nameof(UserBookings), indexBookingVm);
        }

        [Route("{id}/Approve")]
        [ActionName(nameof(Approve))]
        public async Task<IActionResult> Approve(Guid id)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var booking = _dbService.GetSingle<Booking>(v => v.CarrierGuid == currentUser.CarrierGuid && v.BookingGuid == id, x => x.Listing, x => x.CreatedByUser);

            if (booking.Status != BookingStatus.BookedButNotConfirmed)
            {
                //HACK: Handle those edge case later, perhaps tell them they can't do shit
                return BadRequest();
            }

            booking.Status = BookingStatus.BookedAndConfirmed;

            _dbService.Update(booking);

            #region Notification 

            //Need to add the Notification Entry for the customer to notify about the booking confirmation
            var newnotification = new Notifications()
            {
                NotificationDescription = "Booking Confirmed",
                NotificationDateTime = DateTime.Now,
                NotificationType = (int)NotificationTypeEnum.BookingConfirmed,
                ReleventUserId = booking.UserId, // As Notification is to be shown to User
                RelevantNotificationRefId = booking.ListingGuid,
                IsRead = false,
                ReadDateTime = null
            };
            _dbService.Add(newnotification);

            #endregion

            var userThatBookedTheBooking = _dbService.GetSingle<ApplicationUser>(v => v.Id == booking.UserId);
            _emailNotificationFactory.SendUserBookingApprovedEmail(booking, userThatBookedTheBooking);

            return RedirectToAction(nameof(ManageBookings));
        }

        [Route("{id}/Reject")]
        [ActionName(nameof(Reject))]
        public async Task<IActionResult> Reject(Guid id)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var booking = _dbService.GetSingle<Booking>(v => v.CarrierGuid == currentUser.CarrierGuid && v.BookingGuid == id, x => x.Listing, x => x.CreatedByUser);

            if (booking.Status != BookingStatus.BookedButNotConfirmed)
            {
                //HACK: Handle those edge case later, perhaps tell them they can't do shit
                return BadRequest();
            }

            booking.Status = BookingStatus.Rejected;

            _dbService.Update(booking);

            #region Notification 

            //Need to add the Notification Entry for the customer to notify about the booking confirmation
            var newnotification = new Notifications()
            {
                NotificationDescription = "Booking Rejected",
                NotificationDateTime = DateTime.Now,
                NotificationType = (int)NotificationTypeEnum.BookingRejected,
                ReleventUserId = booking.UserId, // As Notification is to be shown to User
                RelevantNotificationRefId = booking.ListingGuid,
                IsRead = false,
                ReadDateTime = null
            };
            _dbService.Add(newnotification);

            #endregion

            var userThatBookedTheBooking = _dbService.GetSingle<ApplicationUser>(v => v.Id == booking.UserId);
            _emailNotificationFactory.SendUserBookingRejectEmail(booking, userThatBookedTheBooking);

            return RedirectToAction(nameof(ManageBookings));
        }

        [Route("{id}/Shipped")]
        [ActionName(nameof(Shipped))]
        public async Task<IActionResult> Shipped(Guid id)
        {//TODO: SHIPMENT should be an entity on its on
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var booking = _dbService.GetSingle<Booking>(v => v.CarrierGuid == currentUser.CarrierGuid && v.BookingGuid == id, x => x.Listing, x => x.CreatedByUser);

            if (booking.Status != BookingStatus.BookedAndConfirmed)
            {
                //HACK: Handle those edge case later, perhaps tell them they can't do shit
                return BadRequest();
            }

            booking.Status = BookingStatus.Shipped;

            _dbService.Update(booking);

            #region Notification 

            //Need to add the Notification Entry for the customer to notify about the booking confirmation
            var newnotification = new Notifications()
            {
                NotificationDescription = "Shipment Shipped",
                NotificationDateTime = DateTime.Now,
                NotificationType = (int)NotificationTypeEnum.ShipmentShipped,
                ReleventUserId = booking.UserId, // As Notification is to be shown to User
                RelevantNotificationRefId = booking.ListingGuid,
                IsRead = false,
                ReadDateTime = null
            };
            _dbService.Add(newnotification);

            #endregion

            var userThatBookedTheBooking = _dbService.GetSingle<ApplicationUser>(v => v.Id == booking.UserId);
            _emailNotificationFactory.SendUserShipmentShippedEmail(booking, userThatBookedTheBooking);

            return RedirectToAction(nameof(ManageBookings));
        }

        [Route("{id}/Delayed")]
        [ActionName(nameof(Delayed))]
        public async Task<IActionResult> Delayed(Guid id)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var booking = _dbService.GetSingle<Booking>(v => v.CarrierGuid == currentUser.CarrierGuid && v.BookingGuid == id, x => x.Listing, x => x.CreatedByUser);

            if (booking.Status != BookingStatus.BookedAndConfirmed)
            {
                //HACK: Handle those edge case later, perhaps tell them they can't do shit
                return BadRequest();
            }

            booking.Status = BookingStatus.Delayed;

            _dbService.Update(booking);

            #region Notification 

            //Need to add the Notification Entry for the customer to notify about the booking confirmation
            var newnotification = new Notifications()
            {
                NotificationDescription = "Shipment Delayed",
                NotificationDateTime = DateTime.Now,
                NotificationType = (int)NotificationTypeEnum.ShipmentDelayed,
                ReleventUserId = booking.UserId, // As Notification is to be shown to User
                RelevantNotificationRefId = booking.ListingGuid,
                IsRead = false,
                ReadDateTime = null
            };
            _dbService.Add(newnotification);

            #endregion

            var userThatBookedTheBooking = _dbService.GetSingle<ApplicationUser>(v => v.Id == booking.UserId);
            _emailNotificationFactory.SendUserShipmentDelayedEmail(booking, userThatBookedTheBooking);

            return RedirectToAction(nameof(ManageBookings));
        }

        [Route("{id}/Completed")]
        [ActionName(nameof(Completed))]
        public async Task<IActionResult> Completed(Guid id)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var booking = _dbService.GetSingle<Booking>(v => v.CarrierGuid == currentUser.CarrierGuid && v.BookingGuid == id, x => x.Listing, x => x.CreatedByUser);

            if (booking.Status != BookingStatus.Shipped)
            {
                //HACK: Handle those edge case later, perhaps tell them they can't do shit
                return BadRequest();
            }

            booking.Status = BookingStatus.Completed;

            _dbService.Update(booking);

            #region Notification 

            //Need to add the Notification Entry for the customer to notify about the booking confirmation
            var newnotification = new Notifications()
            {
                NotificationDescription = "Shipment Arrival",
                NotificationDateTime = DateTime.Now,
                NotificationType = (int)NotificationTypeEnum.ShipmentArrival,
                ReleventUserId = booking.UserId, // As Notification is to be shown to User
                RelevantNotificationRefId = booking.ListingGuid,
                IsRead = false,
                ReadDateTime = null
            };
            _dbService.Add(newnotification);

            #endregion

            var userThatBookedTheBooking = _dbService.GetSingle<ApplicationUser>(v => v.Id == booking.UserId);
            _emailNotificationFactory.SendUserShipmentArrivedEmail(booking, userThatBookedTheBooking);

            //Feedback Ratings Email to Customer
            _emailNotificationFactory.SendUserFeedbackRatingsEmail_Customer(booking, userThatBookedTheBooking);

            //FeedBack Ratings Email To Carrier
            _emailNotificationFactory.SendUserFeedbackRatingsEmail_Carrier(booking, currentUser);

            return RedirectToAction(nameof(ManageBookings));
        }
    }
}