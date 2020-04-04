using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TortoogaApp.Data;
using TortoogaApp.Models;
using TortoogaApp.ViewModels.DashboardViewModels;

namespace TortoogaApp.Services
{
    public interface IDashboardService
    {
        CarrierDashboardViewModel GetCarrierDashboardData(Guid CarrierId);
        List<PendingBookingsViewModel> GetPendingBookings(Guid CarrierId);
        List<BookingReportViewModel> GetBookingReports(Guid CarrierId);
        bool UpdatePendingBookingStatus(Guid BookingId, string Action);
        CustomerRatingsDashboardViewModel GetCustomerRatings(Guid CarrierId);

        bool UpdateNotificationToRead(Guid NotificationId);

        //User Dashboard Methods
        UserDashboardViewModel GetUserDashboardData(Guid UserId);

        string calculateDifferenceInTime(DateTime source, DateTime Destination);
    }


    public class DashboardService : IDashboardService
    {
        private ApplicationDbContext _context;
        private IDbService _dbService;

        public DashboardService(ApplicationDbContext context, IDbService dbService)
        {
            this._context = context;
            this._dbService = dbService;
        }

        #region Carrrier Dashboard Methods

        public CarrierDashboardViewModel GetCarrierDashboardData(Guid CarrierId)
        {
            CarrierDashboardViewModel dashboardmodel = new CarrierDashboardViewModel();

            dashboardmodel.AccountSummary = GetAccountSummaryDetails(CarrierId);
            dashboardmodel.PendingBookingsModel = GetPendingBookings(CarrierId);
            dashboardmodel.Notifications = GetNotificationsForUser(CarrierId, true);
            dashboardmodel.BookingReports = GetBookingReports(CarrierId);


            return dashboardmodel;
        }

        /// <summary>
        /// Fetch the Accouunt Summary Details
        /// </summary>
        /// <param name="CarrierId"></param>
        /// <returns></returns>
        public AccountSummaryViewModel GetAccountSummaryDetails(Guid CarrierId)
        {
            AccountSummaryViewModel accounsummary = new AccountSummaryViewModel();

            int month = DateTime.Now.Month;

            var bookings = (from book in _context.Bookings
                            where book.IsDeleted == false
                            select book).ToList();

            accounsummary.AwaitingApproval = bookings.Where(x => x.CarrierGuid == CarrierId && x.Status == BookingStatus.BookedButNotConfirmed).Count();

            accounsummary.InTransit = bookings.Where(x => x.CarrierGuid == CarrierId && x.Status == BookingStatus.Shipped).Count();

            accounsummary.Completed = bookings.Where(x => x.CarrierGuid == CarrierId && x.Status == BookingStatus.Completed).Count();

            accounsummary.TotalMonthBookings = bookings.Where(x => x.CarrierGuid == CarrierId && x.BookingDate.Value.Month == month).Count();

            accounsummary.TotalMonthRevenue = bookings.Where(x => x.CarrierGuid == CarrierId && x.BookingDate.Value.Month == month
                                        && x.Status != BookingStatus.BookedButNotConfirmed
                                        && x.Status != BookingStatus.Rejected
                                        && x.Status != BookingStatus.Cancelled)
                                       .Sum(x => x.BookingAmount);

            accounsummary.CurrentEscrowTotal = 0;

            accounsummary.TotalReviewsCurrentMonth = (from rat in _context.CustomerRatings
                                                      where rat.RatingAddedOn.Value.Month == month && rat.IsDeleted == false && rat.CarrierID == CarrierId
                                                      select rat).Count();

            accounsummary.AverageProfitPerft = 0;

            return accounsummary;
        }

        /// <summary>
        /// For fetching the Pending Booking Details
        /// </summary>
        /// <param name="CarrierId"></param>
        /// <returns></returns>
        public List<PendingBookingsViewModel> GetPendingBookings(Guid CarrierId)
        {

            var pendingbookings = (from book in _context.Bookings
                                   join list in _context.Listings on book.ListingGuid equals list.ListingGuid
                                   where book.CarrierGuid == CarrierId && book.Status == BookingStatus.BookedButNotConfirmed && book.IsDeleted == false && list.IsDeleted == false
                                   select new PendingBookingsViewModel
                                   {
                                       Listing = list.Title,
                                       Origin = list.Origin,
                                       Destination = list.Destination,
                                       Amount = book.BookingAmount,
                                       BookingID = book.BookingGuid,
                                       BookingDate = book.BookingDate,
                                       ListingID = list.ListingGuid,

                                   }).OrderByDescending(x => x.BookingDate).Take(10).ToList();

            return pendingbookings;
        }

        /// <summary>
        /// For Fetching the Booking Reports Details
        /// </summary>
        /// <param name="CarrierId"></param>
        /// <returns></returns>
        public List<BookingReportViewModel> GetBookingReports(Guid CarrierId)
        {

            var BookingReports = (from book in _context.Bookings
                                  join list in _context.Listings on book.ListingGuid equals list.ListingGuid
                                  join user in _context.Users on book.UserId equals user.Id
                                  where book.CarrierGuid == CarrierId && book.Status != BookingStatus.BookedButNotConfirmed && book.IsDeleted == false && list.IsDeleted == false
                                  select new BookingReportViewModel
                                  {
                                      Date = list.EstimatedArrivalDate,
                                      Details = list.Description,
                                      BookedBy = user.UserName,
                                      Origin = list.Origin,
                                      Destination = list.Destination,
                                      Size = list.Length * list.Width,
                                      Status = Enum.GetName(typeof(BookingStatus), book.Status),
                                      Amount = book.BookingAmount,
                                      BookingID = book.BookingGuid,
                                      BookingDate = book.BookingDate,
                                      ListingID = list.ListingGuid,

                                  }).OrderByDescending(x => x.BookingDate).Take(10).ToList();

            return BookingReports;
        }

        /// <summary>
        /// To Update the Booking Status
        /// </summary>
        /// <param name="BookingId"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public bool UpdatePendingBookingStatus(Guid BookingId, string Status)
        {

            var booking = _dbService.GetSingle<Booking>(v => v.BookingGuid == BookingId);
            if (Status == "Approve")
                booking.Status = BookingStatus.BookedAndConfirmed;
            else
                booking.Status = BookingStatus.Rejected;

            try
            {
                _dbService.Update(booking);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        /// <summary>
        /// To Update the Notification Status to Read
        /// </summary>
        /// <param name="NotificationId"></param>
        /// <returns></returns>
        public bool UpdateNotificationToRead(Guid NotificationId)
        {
            var notify = _dbService.GetSingle<Notifications>(v => v.NotificationId == NotificationId);
            notify.IsRead = true;
            notify.ReadDateTime = DateTime.Now;
            try
            {
                _dbService.Update(notify);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// To Fetch the Ratings to be shown on the Chart
        /// </summary>
        /// <param name="CarrierId"></param>
        /// <returns></returns>
        public CustomerRatingsDashboardViewModel GetCustomerRatings(Guid CarrierId)
        {
            CustomerRatingsDashboardViewModel customerratings = new CustomerRatingsDashboardViewModel();

            var RatingDetails = _dbService.Get<Rating>(v => v.CarrierID == CarrierId && v.IsDeleted == false).ToList();

            if (RatingDetails != null && RatingDetails.Count > 0)
            {
                #region Overall Experience

                customerratings.CountOfFive = RatingDetails.Where(x => x.OverallExperience == 5).Count();
                customerratings.CountOfFour = RatingDetails.Where(x => x.OverallExperience == 4).Count();
                customerratings.CountOfThree = RatingDetails.Where(x => x.OverallExperience == 3).Count();
                customerratings.CountOfTwo = RatingDetails.Where(x => x.OverallExperience == 2).Count();
                customerratings.CountOfOne = RatingDetails.Where(x => x.OverallExperience == 1).Count();

                #endregion
            }

            return customerratings;
        }



        #endregion

        #region User Dashboard Methods

        public UserDashboardViewModel GetUserDashboardData(Guid UserId)
        {
            UserDashboardViewModel dashboardmodel = new UserDashboardViewModel();
            dashboardmodel.ShipmentStatus = GetShipmentStatusDetails(UserId);
            dashboardmodel.Notifications = GetNotificationsForUser(UserId, false);
            dashboardmodel.BookingStatus = GetBookingStatusDetails(UserId);

            return dashboardmodel;
        }

        public List<BookingStatusViewModel> GetBookingStatusDetails(Guid UserId)
        {
            var BookingStatus = (from book in _context.Bookings
                                 join list in _context.Listings on book.ListingGuid equals list.ListingGuid
                                 where book.UserId == UserId && book.IsDeleted == false && list.IsDeleted == false
                                 select new BookingStatusViewModel
                                 {
                                     Origin = list.Origin,
                                     Destination = list.Destination,
                                     BookingReferencenNo = book.ReferenceNumber,
                                     Status = Enum.GetName(typeof(BookingStatus), book.Status),
                                     BookingDate = book.BookingDate,
                                     ListingID = list.ListingGuid,

                                 }).OrderByDescending(x => x.BookingDate).Take(10).ToList();

            return BookingStatus;
        }

        public List<ShipmentStatusViewModel> GetShipmentStatusDetails(Guid UserId)
        {

            var tmp = (from x in _context.ShipmentStatus
                       orderby x.ShipmentStatusUpdateOn descending
                       select x);

            var finalshipmentdetails = tmp.DistinctBy(x => x.BookingReferenceNo).Take(10).ToList();

            var ShipmentStatus = (from book in _context.Bookings
                                  join shipment in finalshipmentdetails on book.ReferenceNumber equals shipment.BookingReferenceNo
                                  join list in _context.Listings on book.ListingGuid equals list.ListingGuid
                                  where book.UserId == UserId && book.IsDeleted == false
                                  select new ShipmentStatusViewModel
                                  {
                                      BookingReferenceNo = shipment.BookingReferenceNo,
                                      ShipmentStatusUpdateOn = shipment.ShipmentStatusUpdateOn,
                                      Latitude = CommonService.GeoAngle.FromDouble(shipment.Latitude).ToString("NS"),
                                      Longitude = CommonService.GeoAngle.FromDouble(shipment.Longitude).ToString("WE"),
                                      CurrentLocation = shipment.CurrentLocation,
                                      EstimatedArrivalDate = shipment.EstimatedArrivalDate,
                                      Status = Enum.GetName(typeof(ShipmentStatusEnum), shipment.Status),
                                      Origin = list.Origin,
                                      Destination = list.Destination,
                                      LatDecimal = shipment.Latitude,
                                      LongDecimal = shipment.Longitude

                                  }).OrderByDescending(x => x.ShipmentStatusUpdateOn).Take(10).ToList();



            return ShipmentStatus;

        }

        #endregion

        #region Misc Methods


        public NotificationViewModel GetNotificationsForUser(Guid Userid, bool iscarrieradmin)
        {
            NotificationViewModel notifications = new NotificationViewModel();

            var temp = (from notify in _context.Notifications
                        where notify.ReleventUserId == Userid
                        select
                        notify).OrderByDescending(x => x.NotificationDateTime).Take(5).ToList();

            notifications.NewBooking = new List<NewBookingViewModel>();
            notifications.RatingReceived = new List<RatingReceivedViewModel>();
            notifications.ShipmentArrivals = new List<ShipmentArrivalViewModel>();
            notifications.ShipmentShipped = new List<ShipmentShippedViewModel>();
            notifications.ShipmentCancelled = new List<ShipmentCancelledViewModel>();
            notifications.ShipmentDelayed = new List<ShipmentDelayedViewModel>();
            notifications.NewMessage = new List<NewMessageViewModel>();
            notifications.BookingConfirmed = new List<BookingConfirmedViewModel>();
            notifications.BookingRejected = new List<BookingRejectedViewModel>();

            notifications.Orders = new List<string>();

            for (int i = 0; i < temp.Count(); i++)
            {
                switch (temp[i].NotificationType)
                {
                    case 0: // NewBooking
                        NewBookingViewModel newbookingmodel = new NewBookingViewModel();
                        newbookingmodel.BookingId = temp[i].RelevantNotificationRefId;
                        newbookingmodel.URL = "/Listing/" + newbookingmodel.BookingId + "/View";
                        newbookingmodel.OrderNo = i + 1;
                        notifications.Orders.Add(Enum.GetName(typeof(NotificationTypeEnum), temp[i].NotificationType));
                        newbookingmodel.NotificationTime = calculateDifferenceInTime(temp[i].NotificationDateTime, DateTime.Now);
                        newbookingmodel.IsRead = temp[i].IsRead;
                        newbookingmodel.NotificationId = temp[i].NotificationId;
                        notifications.NewBooking.Add(newbookingmodel);
                        break;

                    case 1: //RatingReceived
                        RatingReceivedViewModel ratingreceived = new RatingReceivedViewModel();
                        if (iscarrieradmin)
                            ratingreceived.URL = "/Ratings/Index?From=Notifications";
                        else
                            ratingreceived.URL = "/Ratings/RatingsReceived?From=Notifications";
                        ratingreceived.OrderNo = i + 1;
                        notifications.Orders.Add(Enum.GetName(typeof(NotificationTypeEnum), temp[i].NotificationType));
                        ratingreceived.NotificationTime = calculateDifferenceInTime(temp[i].NotificationDateTime, DateTime.Now);
                        ratingreceived.IsRead = temp[i].IsRead;
                        ratingreceived.NotificationId = temp[i].NotificationId;
                        notifications.RatingReceived.Add(ratingreceived);
                        break;

                    case 2: //Shipment Arrival
                        ShipmentArrivalViewModel shipmentarrival = new ShipmentArrivalViewModel();
                        shipmentarrival.BookingId = temp[i].RelevantNotificationRefId;
                        shipmentarrival.URL = "/Listing/" + shipmentarrival.BookingId + "/View";
                        shipmentarrival.OrderNo = i + 1;
                        notifications.Orders.Add(Enum.GetName(typeof(NotificationTypeEnum), temp[i].NotificationType));
                        shipmentarrival.NotificationTime = calculateDifferenceInTime(temp[i].NotificationDateTime, DateTime.Now);
                        shipmentarrival.IsRead = temp[i].IsRead;
                        shipmentarrival.NotificationId = temp[i].NotificationId;
                        notifications.ShipmentArrivals.Add(shipmentarrival);
                        break;

                    case 3: //NewMessage
                        NewMessageViewModel newmessage = new NewMessageViewModel();
                        //shipmentarrival.BookingId = temp[i].RelevantNotificationRefId;
                        //shipmentarrival.URL = "/Listing/" + shipmentarrival.BookingId + "/View";
                        newmessage.OrderNo = i + 1;
                        notifications.Orders.Add(Enum.GetName(typeof(NotificationTypeEnum), temp[i].NotificationType));
                        newmessage.NotificationTime = calculateDifferenceInTime(temp[i].NotificationDateTime, DateTime.Now);
                        newmessage.IsRead = temp[i].IsRead;
                        newmessage.NotificationId = temp[i].NotificationId;
                        notifications.NewMessage.Add(newmessage);
                        break;

                    case 4: // ShipmentShipped
                        ShipmentShippedViewModel shipmentshipped = new ShipmentShippedViewModel();
                        shipmentshipped.BookingId = temp[i].RelevantNotificationRefId;
                        shipmentshipped.URL = "/Listing/" + shipmentshipped.BookingId + "/View";
                        shipmentshipped.OrderNo = i + 1;
                        notifications.Orders.Add(Enum.GetName(typeof(NotificationTypeEnum), temp[i].NotificationType));
                        shipmentshipped.NotificationTime = calculateDifferenceInTime(temp[i].NotificationDateTime, DateTime.Now);
                        shipmentshipped.IsRead = temp[i].IsRead;
                        shipmentshipped.NotificationId = temp[i].NotificationId;
                        notifications.ShipmentShipped.Add(shipmentshipped);
                        break;

                    case 5: // ShipmentDelayed
                        ShipmentDelayedViewModel shipmentdelayed = new ShipmentDelayedViewModel();
                        shipmentdelayed.BookingId = temp[i].RelevantNotificationRefId;
                        shipmentdelayed.URL = "/Listing/" + shipmentdelayed.BookingId + "/View";
                        shipmentdelayed.OrderNo = i + 1;
                        notifications.Orders.Add(Enum.GetName(typeof(NotificationTypeEnum), temp[i].NotificationType));
                        shipmentdelayed.NotificationTime = calculateDifferenceInTime(temp[i].NotificationDateTime, DateTime.Now);
                        shipmentdelayed.IsRead = temp[i].IsRead;
                        shipmentdelayed.NotificationId = temp[i].NotificationId;
                        notifications.ShipmentDelayed.Add(shipmentdelayed);
                        break;

                    case 6: // ShipmentCancelled
                        ShipmentCancelledViewModel shipmentcancelled = new ShipmentCancelledViewModel();
                        shipmentcancelled.BookingId = temp[i].RelevantNotificationRefId;
                        shipmentcancelled.URL = "/Listing/" + shipmentcancelled.BookingId + "/View";
                        shipmentcancelled.OrderNo = i + 1;
                        notifications.Orders.Add(Enum.GetName(typeof(NotificationTypeEnum), temp[i].NotificationType));
                        shipmentcancelled.NotificationTime = calculateDifferenceInTime(temp[i].NotificationDateTime, DateTime.Now);
                        shipmentcancelled.IsRead = temp[i].IsRead;
                        shipmentcancelled.NotificationId = temp[i].NotificationId;
                        notifications.ShipmentCancelled.Add(shipmentcancelled);
                        break;
                    case 7: // BookingConfirmed
                        BookingConfirmedViewModel bookingconfirmed = new BookingConfirmedViewModel();
                        bookingconfirmed.BookingId = temp[i].RelevantNotificationRefId;
                        bookingconfirmed.URL = "/Listing/" + bookingconfirmed.BookingId + "/View";
                        bookingconfirmed.OrderNo = i + 1;
                        notifications.Orders.Add(Enum.GetName(typeof(NotificationTypeEnum), temp[i].NotificationType));
                        bookingconfirmed.NotificationTime = calculateDifferenceInTime(temp[i].NotificationDateTime, DateTime.Now);
                        bookingconfirmed.IsRead = temp[i].IsRead;
                        bookingconfirmed.NotificationId = temp[i].NotificationId;
                        notifications.BookingConfirmed.Add(bookingconfirmed);
                        break;

                    case 8: // BookingRejected
                        BookingRejectedViewModel bookingrejected = new BookingRejectedViewModel();
                        bookingrejected.BookingId = temp[i].RelevantNotificationRefId;
                        bookingrejected.URL = "/Listing/" + bookingrejected.BookingId + "/View";
                        bookingrejected.OrderNo = i + 1;
                        notifications.Orders.Add(Enum.GetName(typeof(NotificationTypeEnum), temp[i].NotificationType));
                        bookingrejected.NotificationTime = calculateDifferenceInTime(temp[i].NotificationDateTime, DateTime.Now);
                        bookingrejected.IsRead = temp[i].IsRead;
                        bookingrejected.NotificationId = temp[i].NotificationId;
                        notifications.BookingRejected.Add(bookingrejected);
                        break;
                }
            }

            return notifications;
        }

        public string calculateDifferenceInTime(DateTime SourceDateTime, DateTime DestinationDatetime)
        {
            if (SourceDateTime.Day == DestinationDatetime.Day) //Need to show the difference in Hours or minutes
            {
                if (SourceDateTime.Hour == DestinationDatetime.Hour) // Same hour Then Difference is in Minutes
                {
                    return (DestinationDatetime.Minute - SourceDateTime.Minute) + " minutes ago";
                }
                else //As hours are different then difference in Hours
                {
                    return (DestinationDatetime.Hour - SourceDateTime.Hour) + " hours ago";
                }
            }
            else //Need to show the diff in days
            {
                return (DestinationDatetime.Day - SourceDateTime.Day) + " day(s) ago";
            }

        }

        #endregion
    }

    public static class CustomExt
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }

}
