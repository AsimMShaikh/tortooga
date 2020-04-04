using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TortoogaApp.Services;
using AutoMapper;
using TortoogaApp.ViewModels.RatingViewModels;
using TortoogaApp.Data;
using TortoogaApp.Security;
using Microsoft.AspNetCore.Identity;
using TortoogaApp.Models;
using static TortoogaApp.Services.CommonService;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace TortoogaApp.Controllers
{
    public class RatingsController : BaseController
    {
        #region Global Variables

        private IMapper _autoMapper;
        private IDbService _dbService;
        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        private IViewRenderingService _renderingService;
        private IActionContextAccessor _actionContextAccessor;
        private AppSettings _appsettings;

        #endregion

        #region Constructor

        public RatingsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IDbService dbService, IViewRenderingService viewrenderingservice, IActionContextAccessor actionContextAccessor, IOptions<AppSettings> appsettings, ICommonservice commonservice)
            : base(commonservice, userManager)
        {
            this._context = context;
            this._userManager = userManager;
            this._dbService = dbService;
            this._renderingService = viewrenderingservice;
            this._actionContextAccessor = actionContextAccessor;
            this._appsettings = appsettings.Value;
        }


        #endregion

        #region View Loading

        /// <summary>
        /// For Customer Rating the Carriers and Carriers Receiving the Ratings
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index(string From)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.AppUrl = _appsettings.BaseUrl;

            //If the User is Carrier Admin
            if (await _userManager.IsInRoleAsync(currentUser, RoleType.CARRIER_ADMIN))
            {
                var carrierDetails = _dbService.GetSingle<Carrier>(v => v.CarrierGuid == currentUser.CarrierGuid);

                CarrierRatingsViewModel carrierratingmodel = new CarrierRatingsViewModel();
                carrierratingmodel.CompanyName = carrierDetails.BusinessName;
                carrierratingmodel.Email = carrierDetails.Email;
                carrierratingmodel.PhoneNumber = carrierDetails.PhoneNumber;
                carrierratingmodel.AccountNumber = carrierDetails.AccountNumber;

                var RatingDetails = _dbService.Get<Rating>(v => v.CarrierID == currentUser.CarrierGuid && v.IsDeleted == false).ToList();

                if (RatingDetails != null && RatingDetails.Count > 0)
                {

                    #region Overall Experience

                    int count_five = RatingDetails.Where(x => x.OverallExperience == 5).Count();
                    int count_four = RatingDetails.Where(x => x.OverallExperience == 4).Count();
                    int count_Three = RatingDetails.Where(x => x.OverallExperience == 3).Count();
                    int count_Two = RatingDetails.Where(x => x.OverallExperience == 2).Count();
                    int count_One = RatingDetails.Where(x => x.OverallExperience == 1).Count();

                    double value = Convert.ToDouble((5 * count_five + 4 * count_four + 3 * count_Three + 2 * count_Two + 1 * count_One) / (count_five + count_four + count_Three + count_Two + count_One));
                    double percent = ((value / 5) * 100);

                    carrierratingmodel.OverallExperience = (int)Math.Round(value, MidpointRounding.AwayFromZero);
                    carrierratingmodel.OverAllPercent = percent;

                    #endregion

                    #region Price

                    count_five = RatingDetails.Where(x => x.Price == 5).Count();
                    count_four = RatingDetails.Where(x => x.Price == 4).Count();
                    count_Three = RatingDetails.Where(x => x.Price == 3).Count();
                    count_Two = RatingDetails.Where(x => x.Price == 2).Count();
                    count_One = RatingDetails.Where(x => x.Price == 1).Count();

                    value = Convert.ToDouble((5 * count_five + 4 * count_four + 3 * count_Three + 2 * count_Two + 1 * count_One) / (count_five + count_four + count_Three + count_Two + count_One));
                    percent = ((value / 5) * 100);

                    carrierratingmodel.Price = (int)Math.Round(value, MidpointRounding.AwayFromZero);
                    carrierratingmodel.PricePercent = percent;

                    #endregion

                    #region Service

                    count_five = RatingDetails.Where(x => x.Service == 5).Count();
                    count_four = RatingDetails.Where(x => x.Service == 4).Count();
                    count_Three = RatingDetails.Where(x => x.Service == 3).Count();
                    count_Two = RatingDetails.Where(x => x.Service == 2).Count();
                    count_One = RatingDetails.Where(x => x.Service == 1).Count();

                    value = Convert.ToDouble((5 * count_five + 4 * count_four + 3 * count_Three + 2 * count_Two + 1 * count_One) / (count_five + count_four + count_Three + count_Two + count_One));
                    percent = ((value / 5) * 100);

                    carrierratingmodel.Service = (int)Math.Round(value, MidpointRounding.AwayFromZero);
                    carrierratingmodel.ServicePercent = percent;

                    #endregion

                    #region communication

                    count_five = RatingDetails.Where(x => x.Communication == 5).Count();
                    count_four = RatingDetails.Where(x => x.Communication == 4).Count();
                    count_Three = RatingDetails.Where(x => x.Communication == 3).Count();
                    count_Two = RatingDetails.Where(x => x.Communication == 2).Count();
                    count_One = RatingDetails.Where(x => x.Communication == 1).Count();

                    value = Convert.ToDouble((5 * count_five + 4 * count_four + 3 * count_Three + 2 * count_Two + 1 * count_One) / (count_five + count_four + count_Three + count_Two + count_One));
                    percent = ((value / 5) * 100);

                    carrierratingmodel.Communication = (int)Math.Round(value, MidpointRounding.AwayFromZero);
                    carrierratingmodel.CommunicationPercent = percent;

                    #endregion
                }
                else
                {
                    carrierratingmodel.OverallExperience = 0;
                    carrierratingmodel.OverAllPercent = 0;


                    carrierratingmodel.Price = 0;
                    carrierratingmodel.PricePercent = 0;

                    carrierratingmodel.Service = 0;
                    carrierratingmodel.ServicePercent = 0;

                    carrierratingmodel.Communication = 0;
                    carrierratingmodel.CommunicationPercent = 0;

                }

                //var objImage = _dbService.GetSingle<CompanyLogo>(v => v.CarrierGuid == carrierDetails.CarrierGuid && v.isDeleted == false);
                //if (objImage != null)
                //    carrierratingmodel.CompanyProfileImageURL = objImage.ImageUri;
                //ViewBag.AppUrl = _appsettings.BaseUrl;
                //ViewBag.ImageContainerPath = _appsettings.ImageBlobContainerPath + appSettings.CompanyLogoContainer;

                if (From == "Page") //Make the Ratings Received Notifications as IsRead if Not Read
                {
                    var ratingsNotReadIds = (from notify in _context.Notifications
                                             where notify.NotificationType == (int)NotificationTypeEnum.RatingReceived && notify.IsRead == false && notify.ReleventUserId == currentUser.CarrierGuid
                                             select notify.NotificationId).ToList();

                    if (ratingsNotReadIds != null && ratingsNotReadIds.Count > 0)
                    {
                        var updatedrecords = _context.Notifications.Where(f => ratingsNotReadIds.Contains(f.NotificationId)).ToList();
                        updatedrecords.ForEach(a =>
                                            {
                                                a.IsRead = true;
                                                a.ReadDateTime = DateTime.Now;
                                            });
                        _context.SaveChanges();
                    }

                }

                //Need to fetch the Carrier Logo
                return View("CarrierRatings", carrierratingmodel);
            }
            else //Normal User
            {
                var PendingBookings = new List<FeedBackRatingsViewModel>();
                PendingBookings = GetPendingRatingsList(currentUser);

                //Need to fetch the carrier images
                foreach (var rating in PendingBookings)
                {
                    rating.CarrierLogoPath = string.Empty;
                }

                return View("YourFeedbackRatings", PendingBookings);
            }

        }


        /// <summary>
        /// For Carrier Rating the Customers and Customers Receiving the Ratings
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> RatingsReceived(string From)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.AppUrl = _appsettings.BaseUrl;

            //If the User is Carrier Admin
            if (await _userManager.IsInRoleAsync(currentUser, RoleType.CARRIER_ADMIN))
            {
                var PendingRatings = GetCarrierPendingRatings(currentUser);

                foreach (var rating in PendingRatings)
                {
                    var objImage = _dbService.GetSingle<ProfileImage>(v => v.UserId == rating.UserId && v.isDeleted == false);
                    if (objImage != null)
                        rating.UserProfileImagePath = objImage.ImageUri;

                    rating.Count = _context.CustomerRatings.Where(x => x.UserId == rating.UserId).Count();
                    double sumvalue = (_context.CustomerRatings.Where(x => x.UserId == rating.UserId).Sum(x => x.Rating));
                    if (rating.Count != 0)
                        rating.Average = (int)Math.Round(sumvalue / rating.Count, MidpointRounding.AwayFromZero);
                    else
                        rating.Average = 0;

                    //ViewBag.AppUrl = _appsettings.BaseUrl;

                }

                return View("~/Views/CustomerRatings/RatingsGivenByCarrier.cshtml", PendingRatings);

            }
            else //Normal User
            {
                var userDetails = _dbService.GetSingle<ApplicationUser>(v => v.Id == currentUser.Id);

                CustomerRatingsViewModel customerModel = new CustomerRatingsViewModel();
                customerModel.UserName = userDetails.UserName;
                customerModel.FullName = userDetails.FirstName + " " + userDetails.LastName;
                customerModel.Email = userDetails.Email;
                customerModel.MobileNumber = userDetails.MobileNumber;

                customerModel.Count = _context.CustomerRatings.Where(x => x.UserId == currentUser.Id).Count();
                double sumvalue = _context.CustomerRatings.Where(x => x.UserId == userDetails.Id).Sum(x => x.Rating);

                if (customerModel.Count != 0)
                    customerModel.Average = (int)Math.Round(sumvalue / customerModel.Count, MidpointRounding.AwayFromZero);
                else
                    customerModel.Average = 0;

                var objImage = _dbService.GetSingle<ProfileImage>(v => v.UserId == currentUser.Id && v.isDeleted == false);
                if (objImage != null)
                    customerModel.UserProfileImagePath = objImage.ImageUri;

                if (From == "Page") //Make the Ratings Received Notifications as IsRead if Not Read
                {
                    var ratingsNotReadIds = (from notify in _context.Notifications
                                             where notify.NotificationType == (int)NotificationTypeEnum.RatingReceived && notify.IsRead == false && notify.ReleventUserId == currentUser.Id
                                             select notify.NotificationId).ToList();

                    if (ratingsNotReadIds != null && ratingsNotReadIds.Count > 0)
                    {
                        var updatedrecords = _context.Notifications.Where(f => ratingsNotReadIds.Contains(f.NotificationId)).ToList();
                        updatedrecords.ForEach(a =>
                        {
                            a.IsRead = true;
                            a.ReadDateTime = DateTime.Now;
                        });
                        _context.SaveChanges();
                    }

                }

                return View("~/Views/CustomerRatings/RatingsReceivedByCustomer.cshtml", customerModel);
            }


        }


        #endregion

        #region Customer Submit        

        /// <summary>
        /// Fetching the Pending Ratings List - For Customers
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        protected List<FeedBackRatingsViewModel> GetPendingRatingsList(ApplicationUser currentUser)
        {
            var tempCompletedBookings = _context.Ratings.Where(x => x.UserId == currentUser.Id && x.IsDeleted == false).Select(p => p.BookingID);

            var PendingBookings = (from book in _context.Bookings
                                   join list in _context.Listings on book.ListingGuid equals list.ListingGuid
                                   join carr in _context.Carriers on book.CarrierGuid equals carr.CarrierGuid
                                   where !tempCompletedBookings.Contains(book.BookingGuid) && book.Status == BookingStatus.Completed && book.UserId == currentUser.Id
                                   select new FeedBackRatingsViewModel
                                   {
                                       Origin = list.Origin,
                                       Destination = list.Destination,
                                       CarrirerName = carr.BusinessName,
                                       BookingReferenceNumber = book.ReferenceNumber,
                                       DepartedDate = list.DepartureDate,
                                       ArrivalDate = list.EstimatedArrivalDate,
                                       BookingID = book.BookingGuid,
                                       CarrierID = carr.CarrierGuid

                                   }).ToList();

            return PendingBookings;

        }

        /// <summary>
        /// For User Perspective Completed Ratings Given By him
        /// </summary>
        /// <returns></returns>
        public async Task<ContentResult> GetCompletedRatingsList()
        {
            var data = string.Empty;
            try
            {
                var currentUser = await _userManager.GetUserAsync(HttpContext.User);

                string errorMessage = string.Empty;
                string uri = string.Empty;
                var urlParamenters = new Dictionary<string, string>();
                var parameters = GetParameters(new List<string>() { "Origin", "Destination", "CarrierName", "Reference", "DepartureDate", "ArrivalDate" });

                var tempRatings = (from rat in _context.Ratings
                                   join book in _context.Bookings on rat.BookingID equals book.BookingGuid
                                   join list in _context.Listings on book.ListingGuid equals list.ListingGuid
                                   join carr in _context.Carriers on book.CarrierGuid equals carr.CarrierGuid
                                   where rat.UserId == currentUser.Id
                                   select new FeedBackRatingsViewModel
                                   {
                                       Origin = list.Origin,
                                       Destination = list.Destination,
                                       CarrirerName = carr.BusinessName,
                                       BookingReferenceNumber = book.ReferenceNumber,
                                       DepartedDate = list.DepartureDate,
                                       ArrivalDate = list.EstimatedArrivalDate,
                                       OverallExperience = rat.OverallExperience,
                                       Service = rat.Service,
                                       Communication = rat.Communication,
                                       Price = rat.Price,
                                       RatingID = rat.RatingId

                                   });

                var total = tempRatings.Count();

                #region Sort

                string SortColumn = parameters["sort_by"].ToString();
                string SortDir = parameters["sort_order"].ToString();

                switch (SortColumn)
                {
                    case "Origin":
                        if (SortDir == "desc")
                        {
                            tempRatings = tempRatings.OrderByDescending(x => x.Origin);
                        }
                        else
                        {
                            tempRatings = tempRatings.OrderBy(x => x.Origin);
                        }
                        break;
                    case "Destination":
                        if (SortDir == "desc")
                        {
                            tempRatings = tempRatings.OrderByDescending(x => x.Destination);
                        }
                        else
                        {
                            tempRatings = tempRatings.OrderBy(x => x.Destination);
                        }
                        break;
                    case "Reference":
                        if (SortDir == "desc")
                        {
                            tempRatings = tempRatings.OrderByDescending(x => x.BookingReferenceNumber);
                        }
                        else
                        {
                            tempRatings = tempRatings.OrderBy(x => x.BookingReferenceNumber);
                        }
                        break;
                    case "DepartureDate":
                        if (SortDir == "desc")
                        {
                            tempRatings = tempRatings.OrderByDescending(x => x.DepartedDate);
                        }
                        else
                        {
                            tempRatings = tempRatings.OrderBy(x => x.DepartedDate);
                        }
                        break;
                    case "ArrivalDate":
                        if (SortDir == "desc")
                        {
                            tempRatings = tempRatings.OrderByDescending(x => x.ArrivalDate);
                        }
                        else
                        {
                            tempRatings = tempRatings.OrderBy(x => x.ArrivalDate);
                        }
                        break;
                }

                #endregion


                #region Pagination and OutPut

                var pageNo = Convert.ToInt32(parameters["page_number"]);
                var pageSize = Convert.ToInt32(parameters["results_per_page"]);
                var CompletedRatings = tempRatings.Skip((pageNo - 1) * (pageSize)).Take(pageSize).ToList();

                #endregion

                var res = from model in CompletedRatings
                          select new string[]
                             {
                                     model.Origin,
                                     model.Destination,
                                     model.CarrirerName,
                                     model.BookingReferenceNumber,
                                     model.DepartedDate.ToString("yyyy/MM/dd"),
                                     model.ArrivalDate.ToString("yyyy/MM/dd"),
                                     model.OverallExperience.ToString(),
                                     model.Service.ToString(),
                                     model.Communication.ToString(),
                                     model.Price.ToString(),
                                     model.RatingID.ToString()
                             };

                data = CommonService.Json.Serialize(new { iTotalRecords = res.Count(), iTotalDisplayRecords = res.Count(), aaData = res });
            }
            catch (Exception ex)
            {
                data = CommonService.Json.Serialize(new { iTotalRecords = 0, iTotalDisplayRecords = 0, aaData = new string[] { } });
            }
            return Content(data, "application/json");
        }


        /// <summary>
        /// For User Submitting his ratings to Carrier
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> SubmitRatings(FeedBackRatingsViewModel model)
        {
            #region Post Data    

            string errorMessage = string.Empty;
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            var RatingObj = new Rating();
            RatingObj.BookingID = model.BookingID;
            RatingObj.CarrierID = model.CarrierID;
            RatingObj.OverallExperience = model.OverallExperience;
            RatingObj.Communication = model.Communication;
            RatingObj.Service = model.Service;
            RatingObj.Price = model.Price;
            RatingObj.UserId = currentUser.Id;
            RatingObj.IsDeleted = false;
            RatingObj.IsRequestedForRemoval = false;
            RatingObj.RatingAddedOn = DateTime.Today;

            _dbService.Add(RatingObj);

            #endregion

            if (RatingObj.RatingId == Guid.Empty)
            {
                throw new Exception("Db Error");
            }
            else
            {
                #region Notification

                //Adding the Entry in the notifications Table                
                var newnotification = new Notifications()
                {
                    NotificationDescription = "New Ratings For Carrier",
                    NotificationDateTime = DateTime.Now,
                    NotificationType = (int)NotificationTypeEnum.RatingReceived,
                    ReleventUserId = model.CarrierID, // As Notification is to be shown to carrier
                    RelevantNotificationRefId = RatingObj.RatingId,
                    IsRead = false,
                    ReadDateTime = null
                };
                _dbService.Add(newnotification);

                #endregion


                var result = new GenericJsonResponse<FeedBackRatingsViewModel>();
                var PendingBookings = new List<FeedBackRatingsViewModel>();
                PendingBookings = GetPendingRatingsList(currentUser);
                string stringFeedBackRatingsModel = string.Empty;

                stringFeedBackRatingsModel = _renderingService.RenderPartialView(_actionContextAccessor.ActionContext, "~/Views/Ratings/_PendingRatings.cshtml", PendingBookings);
                result.IsSucceed = true;

                return Json(new { result, stringFeedBackRatingsModel });
            }

        }


        #endregion      

        #region Carrier Receiving 

        /// <summary>
        /// To Fetch the Carrier Ratings List - to see ratings given by user
        /// </summary>
        /// <returns></returns>
        public async Task<ContentResult> GetCarrierRatingsList()
        {
            var data = string.Empty;
            try
            {
                var currentUser = await _userManager.GetUserAsync(HttpContext.User);

                string errorMessage = string.Empty;
                string uri = string.Empty;
                var urlParamenters = new Dictionary<string, string>();
                var parameters = GetParameters(new List<string>() { "" });

                var tempRatings = (from rat in _context.Ratings
                                   join book in _context.Bookings on rat.BookingID equals book.BookingGuid
                                   join list in _context.Listings on book.ListingGuid equals list.ListingGuid
                                   join carr in _context.Carriers on book.CarrierGuid equals carr.CarrierGuid
                                   join user in _context.Users on rat.UserId equals user.Id
                                   where rat.CarrierID == currentUser.CarrierGuid
                                   select new FeedBackRatingsViewModel
                                   {
                                       Origin = list.Origin,
                                       Destination = list.Destination,
                                       CarrirerName = carr.BusinessName,
                                       BookingReferenceNumber = book.ReferenceNumber,
                                       DepartedDate = list.DepartureDate,
                                       ArrivalDate = list.EstimatedArrivalDate,
                                       OverallExperience = rat.OverallExperience,
                                       Service = rat.Service,
                                       Communication = rat.Communication,
                                       Price = rat.Price,
                                       ListingReferenceNumber = list.ReferenceNumber,
                                       UserName = user.UserName,
                                       RatingAddedOn = rat.RatingAddedOn,
                                       RatingID = rat.RatingId

                                   });



                #region Filteration

                if (parameters.ContainsKey("Value") && parameters["Value"] != null && parameters["Value"] != string.Empty)
                {
                    var value = parameters["Value"].ToString();

                    var date = new DateTime();
                    try
                    {
                        date = DateTime.ParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        tempRatings = tempRatings.Where(p => value.Contains(p.Origin) || value.Contains(p.Destination) || value.Contains(p.CarrirerName) || value.Contains(p.ListingReferenceNumber) || value.Contains(p.BookingReferenceNumber) || value.Contains(p.UserName) || p.ArrivalDate == date || p.DepartedDate == date);
                    }
                    catch (Exception ex)
                    {
                        tempRatings = tempRatings.Where(p => value.Contains(p.Origin) || value.Contains(p.Destination) || value.Contains(p.CarrirerName) || value.Contains(p.ListingReferenceNumber) || value.Contains(p.BookingReferenceNumber) || value.Contains(p.UserName));
                    }
                }

                var total = tempRatings.Count();

                #endregion

                #region Pagination and OutPut

                var pageNo = Convert.ToInt32(parameters["page_number"]);
                var pageSize = Convert.ToInt32(parameters["results_per_page"]);
                var CompletedRatings = tempRatings.Skip((pageNo - 1) * (pageSize)).Take(pageSize).ToList();

                #endregion

                var res = from model in CompletedRatings
                          select new string[]
                             {
                                     model.Origin,
                                     model.Destination,
                                     model.CarrirerName,
                                     model.ListingReferenceNumber,
                                     model.BookingReferenceNumber,
                                     model.DepartedDate.ToString("yyyy/MM/dd"),
                                     model.ArrivalDate.ToString("yyyy/MM/dd"),
                                     model.OverallExperience.ToString(),
                                     model.Service.ToString(),
                                     model.Communication.ToString(),
                                     model.Price.ToString(),
                                     model.RatingID.ToString(),
                                     model.UserName,
                                     model.RatingAddedOn.Value.ToString("yyyy/MM/dd"),
                             };

                data = CommonService.Json.Serialize(new { iTotalRecords = res.Count(), iTotalDisplayRecords = res.Count(), aaData = res });
            }
            catch (Exception ex)
            {
                data = CommonService.Json.Serialize(new { iTotalRecords = 0, iTotalDisplayRecords = 0, aaData = new string[] { } });
            }
            return Content(data, "application/json");
        }

        #endregion

        #region Carrier Submit Methods



        /// <summary>
        /// Get Carrier Pending Ratings List
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        protected List<CustomerRatingsViewModel> GetCarrierPendingRatings(ApplicationUser currentUser)
        {
            var tempCompletedRatings = _context.CustomerRatings.Where(x => x.CarrierID == currentUser.CarrierGuid && x.IsDeleted == false).Select(p => p.BookingID);

            var PendingRatings = (from book in _context.Bookings
                                  join list in _context.Listings on book.ListingGuid equals list.ListingGuid
                                  join carr in _context.Carriers on book.CarrierGuid equals carr.CarrierGuid
                                  join user in _context.Users on book.UserId equals user.Id
                                  //join temp in _context.ProfileImages on user.Id equals temp.UserId into profgrp
                                  //from prof in profgrp.DefaultIfEmpty()
                                  where !tempCompletedRatings.Contains(book.BookingGuid) && book.Status == BookingStatus.Completed && book.CarrierGuid == currentUser.CarrierGuid
                                  select new CustomerRatingsViewModel
                                  {
                                      Origin = list.Origin,
                                      Destination = list.Destination,
                                      CarrirerName = carr.BusinessName,
                                      BookingReferenceNumber = book.ReferenceNumber,
                                      ListingReferenceNumber = list.ReferenceNumber,
                                      UserName = user.UserName,
                                      DepartedDate = list.DepartureDate,
                                      ArrivalDate = list.EstimatedArrivalDate,
                                      //CountAvg = ratedGrp.Count(),
                                      UserId = book.UserId,
                                      BookingID = book.BookingGuid,
                                      CarrierID = carr.CarrierGuid,
                                      //UserProfileImagePath = prof.ImageUri

                                  }).ToList();

            return PendingRatings;

        }

        /// <summary>
        /// For fetching the customers past ratings list
        /// </summary>
        /// <returns></returns>
        public async Task<ContentResult> GetCustomerPastRatingsList()
        {
            var data = string.Empty;
            try
            {
                var currentUser = await _userManager.GetUserAsync(HttpContext.User);

                string errorMessage = string.Empty;
                string uri = string.Empty;
                var urlParamenters = new Dictionary<string, string>();
                var parameters = GetParameters(new List<string>() { "" });

                var tempRatings = (from rat in _context.CustomerRatings
                                   join book in _context.Bookings on rat.BookingID equals book.BookingGuid
                                   join list in _context.Listings on book.ListingGuid equals list.ListingGuid
                                   join carr in _context.Carriers on rat.CarrierID equals carr.CarrierGuid
                                   join user in _context.Users on rat.UserId equals user.Id
                                   where rat.CarrierID == currentUser.CarrierGuid
                                   select new CustomerRatingsViewModel
                                   {
                                       Origin = list.Origin,
                                       Destination = list.Destination,
                                       CarrirerName = carr.BusinessName,
                                       BookingReferenceNumber = book.ReferenceNumber,
                                       ListingReferenceNumber = list.ReferenceNumber,
                                       RatingAddedOn = rat.RatingAddedOn,
                                       UserName = user.UserName,
                                       Rating = rat.Rating,
                                       RatingID = rat.RatingId,
                                       UserId = rat.UserId,
                                       DepartedDate = list.DepartureDate,
                                       ArrivalDate = list.EstimatedArrivalDate,
                                   });



                #region Filteration

                if (parameters.ContainsKey("Value") && parameters["Value"] != null && parameters["Value"] != string.Empty)
                {
                    var value = parameters["Value"].ToString();
                    tempRatings = tempRatings.Where(p => value.Contains(p.Origin) || value.Contains(p.Destination) || value.Contains(p.CarrirerName) || value.Contains(p.ListingReferenceNumber) || value.Contains(p.BookingReferenceNumber) || value.Contains(p.UserName));
                }

                var total = tempRatings.Count();

                #endregion

                #region Pagination and OutPut

                var pageNo = Convert.ToInt32(parameters["page_number"]);
                var pageSize = Convert.ToInt32(parameters["results_per_page"]);
                var PastRatings = tempRatings.Skip((pageNo - 1) * (pageSize)).Take(pageSize).ToList();

                foreach (var rating in PastRatings)
                {
                    var objImage = _dbService.GetSingle<ProfileImage>(v => v.UserId == rating.UserId && v.isDeleted == false);
                    if (objImage != null)
                        rating.UserProfileImagePath = objImage.ImageUri;
                }


                #endregion

                var res = from model in PastRatings
                          select new string[]
                             {
                                     model.Origin,
                                     model.Destination,
                                     model.CarrirerName,
                                     model.ListingReferenceNumber,
                                     model.BookingReferenceNumber,
                                     model.RatingAddedOn.Value.ToString("yyyy/MM/dd"),
                                     model.UserName,
                                     model.Rating.ToString(),
                                     model.UserProfileImagePath,
                                     model.RatingID.ToString(),
                                     model.DepartedDate.ToString("yyyy/MM/dd"),
                                     model.ArrivalDate.ToString("yyyy/MM/dd"),
                             };

                data = CommonService.Json.Serialize(new { iTotalRecords = res.Count(), iTotalDisplayRecords = res.Count(), aaData = res });
            }
            catch (Exception ex)
            {
                data = CommonService.Json.Serialize(new { iTotalRecords = 0, iTotalDisplayRecords = 0, aaData = new string[] { } });
            }
            return Content(data, "application/json");
        }

        /// <summary>
        /// For Carrier Submitting the Ratings to Customers
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> SubmitCustomerRatings(CustomerRatingsViewModel model)
        {
            #region Post Data    

            string errorMessage = string.Empty;
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            var CustomerRatingObj = new CustomerRatings();
            CustomerRatingObj.BookingID = model.BookingID;
            CustomerRatingObj.CarrierID = model.CarrierID;
            CustomerRatingObj.Rating = model.Rating;
            CustomerRatingObj.UserId = model.UserId;
            CustomerRatingObj.IsDeleted = false;
            CustomerRatingObj.RatingAddedOn = DateTime.Today;

            _dbService.Add(CustomerRatingObj);

            #endregion

            if (CustomerRatingObj.RatingId == Guid.Empty)
            {
                throw new Exception("Db Error");
            }
            else
            {
                #region Notification

                //Adding the Entry in the notifications Table                
                var newnotification = new Notifications()
                {
                    NotificationDescription = "New Ratings For Customer",
                    NotificationDateTime = DateTime.Now,
                    NotificationType = (int)NotificationTypeEnum.RatingReceived,
                    ReleventUserId = model.UserId, // As Notification is to be shown to carrier
                    RelevantNotificationRefId = CustomerRatingObj.RatingId,
                    IsRead = false,
                    ReadDateTime = null
                };
                _dbService.Add(newnotification);

                #endregion

                var result = new GenericJsonResponse<CustomerRatingsViewModel>();
                var PendingRatings = new List<CustomerRatingsViewModel>();
                PendingRatings = GetCarrierPendingRatings(currentUser);

                foreach (var rating in PendingRatings)
                {
                    rating.Count = _context.CustomerRatings.Where(x => x.UserId == rating.UserId).Count();
                    if (rating.Count != 0)
                        rating.Average = _context.CustomerRatings.Where(x => x.UserId == rating.UserId).Sum(x => x.Rating) / rating.Count;
                    else
                        rating.Average = 0;
                    //ViewBag.ImageContainerPath = _appsettings.ProfileImageBlobContainerPath + _appsettings.ProfileImageContainer;
                }

                string stringCustomerRatingModel = string.Empty;

                stringCustomerRatingModel = _renderingService.RenderPartialView(_actionContextAccessor.ActionContext, "~/Views/CustomerRatings/_CustomerPendingRatings.cshtml", PendingRatings);
                result.IsSucceed = true;

                return Json(new { result, stringCustomerRatingModel });
            }

        }



        #endregion

        #region Customer Receiving

        /// <summary>
        /// To Fetch the User Ratings List - to see ratings given by Carrier
        /// </summary>
        /// <returns></returns>
        public async Task<ContentResult> GetCustomerReceivedRatingsList()
        {
            var data = string.Empty;
            try
            {
                var currentUser = await _userManager.GetUserAsync(HttpContext.User);

                string errorMessage = string.Empty;
                string uri = string.Empty;
                var urlParamenters = new Dictionary<string, string>();
                var parameters = GetParameters(new List<string>() { "" });

                var tempRatings = (from rat in _context.CustomerRatings
                                   join book in _context.Bookings on rat.BookingID equals book.BookingGuid
                                   join list in _context.Listings on book.ListingGuid equals list.ListingGuid
                                   join carr in _context.Carriers on book.CarrierGuid equals carr.CarrierGuid
                                   where rat.UserId == currentUser.Id
                                   select new CustomerRatingsViewModel
                                   {
                                       Origin = list.Origin,
                                       Destination = list.Destination,
                                       CarrirerName = carr.BusinessName,
                                       BookingReferenceNumber = book.ReferenceNumber,
                                       ListingReferenceNumber = list.ReferenceNumber,
                                       DepartedDate = list.DepartureDate,
                                       ArrivalDate = list.EstimatedArrivalDate,
                                       RatingAddedOn = rat.RatingAddedOn,
                                       RatingID = rat.RatingId,
                                       CarrierID = carr.CarrierGuid,
                                       Rating = rat.Rating,
                                   });



                #region Filteration

                if (parameters.ContainsKey("Value") && parameters["Value"] != null && parameters["Value"] != string.Empty)
                {
                    var value = parameters["Value"].ToString();

                    var date = new DateTime();
                    try
                    {
                        date = DateTime.ParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        tempRatings = tempRatings.Where(p => value.Contains(p.Origin) || value.Contains(p.Destination) || value.Contains(p.CarrirerName) || value.Contains(p.ListingReferenceNumber) || value.Contains(p.BookingReferenceNumber) || value.Contains(p.CarrirerName) || p.ArrivalDate == date || p.DepartedDate == date);
                    }
                    catch (Exception ex)
                    {
                        tempRatings = tempRatings.Where(p => value.Contains(p.Origin) || value.Contains(p.Destination) || value.Contains(p.CarrirerName) || value.Contains(p.ListingReferenceNumber) || value.Contains(p.BookingReferenceNumber) || value.Contains(p.CarrirerName));
                    }
                }

                var total = tempRatings.Count();

                #endregion

                #region Pagination and OutPut

                var pageNo = Convert.ToInt32(parameters["page_number"]);
                var pageSize = Convert.ToInt32(parameters["results_per_page"]);
                var CompletedRatings = tempRatings.Skip((pageNo - 1) * (pageSize)).Take(pageSize).ToList();

                foreach (var rating in CompletedRatings)
                {
                    //var objImage = _dbService.GetSingle<ProfileImage>(v => v.UserId == rating.UserId && v.isDeleted == false);
                    //if (objImage != null)
                    //    rating.UserProfileImagePath = objImage.ImageUri;

                    //Fetch the Carrier Images
                }

                #endregion

                //Fetch the carrier Image Path

                var res = from model in CompletedRatings
                          select new string[]
                             {
                                     model.Origin,
                                     model.Destination,
                                     model.CarrirerName,
                                     model.ListingReferenceNumber,
                                     model.BookingReferenceNumber,
                                     model.DepartedDate.ToString("yyyy/MM/dd"),
                                     model.ArrivalDate.ToString("yyyy/MM/dd"),
                                     model.RatingID.ToString(),
                                     model.RatingAddedOn.Value.ToString("yyyy/MM/dd"),
                                     model.Rating.ToString(),
                                     model.CarrierLogoPath,
                             };

                data = CommonService.Json.Serialize(new { iTotalRecords = res.Count(), iTotalDisplayRecords = res.Count(), aaData = res });
            }
            catch (Exception ex)
            {
                data = CommonService.Json.Serialize(new { iTotalRecords = 0, iTotalDisplayRecords = 0, aaData = new string[] { } });
            }
            return Content(data, "application/json");
        }

        #endregion

    }
}