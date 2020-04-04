using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TortoogaApp.Data;
using Microsoft.AspNetCore.Identity;
using TortoogaApp.Security;
using TortoogaApp.Services;
using Microsoft.Extensions.Options;
using TortoogaApp.ViewModels.NotificationsList;

namespace TortoogaApp.Controllers
{
    public class NotificationsController : BaseController
    {
        #region Global Variables

        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        private AppSettings _appsettings;
        private IDashboardService _dashboardService;

        #endregion

        #region Constructor

        public NotificationsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IOptions<AppSettings> appsettings, ICommonservice commonservice, IDashboardService dashboardService)
            : base(commonservice, userManager)
        {
            this._context = context;
            this._userManager = userManager;
            this._appsettings = appsettings.Value;
            this._dashboardService = dashboardService;
        }


        #endregion

        #region Methods

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ContentResult> GetNotificationsList()
        {
            var data = string.Empty;
            try
            {
                var currentUser = await _userManager.GetUserAsync(HttpContext.User);
                Guid? id = Guid.Empty;

                //If the User is Carrier Admin
                bool iscarrieradmin = false;
                if (await _userManager.IsInRoleAsync(currentUser, RoleType.CARRIER_ADMIN))
                {
                    id = currentUser.CarrierGuid;
                    iscarrieradmin = true;
                }
                else
                {
                    id = currentUser.Id;
                }
                string errorMessage = string.Empty;
                string uri = string.Empty;
                var urlParamenters = new Dictionary<string, string>();
                var parameters = GetParameters(new List<string>() { "", "ReceivedOn", "Description", "IsRead", "ReadOn" });

                var notifications = (from notify in _context.Notifications
                                     where notify.ReleventUserId == id
                                     select new NotificationListViewModel
                                     {
                                         NotificationType = GetNotificationString(notify.NotificationType, notify.RelevantNotificationRefId, false, iscarrieradmin),
                                         ReceivedOn = _dashboardService.calculateDifferenceInTime(notify.NotificationDateTime, DateTime.Now),
                                         Description = notify.NotificationDescription,
                                         IsRead = notify.IsRead,
                                         ReadOn = notify.ReadDateTime,
                                         NotificationDateTime = notify.NotificationDateTime,
                                         URL = GetNotificationString(notify.NotificationType, notify.RelevantNotificationRefId, true, iscarrieradmin),
                                         NotificationId = notify.NotificationId

                                     });

                #region Filteration

                if (parameters.ContainsKey("Value") && parameters["Value"] != null && parameters["Value"] != string.Empty)
                {
                    var value = parameters["Value"].ToString();
                    notifications = notifications.Where(p => value.Contains(p.NotificationType.ToLower()) || value.Contains(p.ReceivedOn.ToLower()) || value.Contains(p.Description.ToLower()) || value.Contains(p.IsRead.ToString().ToLower()));                    
                }
                
                #endregion

                var total = notifications.Count();

                #region Sort

                string SortColumn = parameters["sort_by"].ToString();
                string SortDir = parameters["sort_order"].ToString();

                switch (SortColumn)
                {
                    case "ReceivedOn":
                        if (SortDir == "desc")
                        {
                            notifications = notifications.OrderByDescending(x => x.NotificationDateTime);
                        }
                        else
                        {
                            notifications = notifications.OrderBy(x => x.NotificationDateTime);
                        }
                        break;
                    case "Description":
                        if (SortDir == "desc")
                        {
                            notifications = notifications.OrderByDescending(x => x.Description);
                        }
                        else
                        {
                            notifications = notifications.OrderBy(x => x.Description);
                        }
                        break;
                    case "IsRead":
                        if (SortDir == "desc")
                        {
                            notifications = notifications.OrderByDescending(x => x.IsRead);
                        }
                        else
                        {
                            notifications = notifications.OrderBy(x => x.IsRead);
                        }
                        break;
                    case "ReadOn":
                        if (SortDir == "desc")
                        {
                            notifications = notifications.OrderByDescending(x => x.ReadOn);
                        }
                        else
                        {
                            notifications = notifications.OrderBy(x => x.ReadOn);
                        }
                        break;
                }

                #endregion


                #region Pagination and OutPut

                var pageNo = Convert.ToInt32(parameters["page_number"]);
                var pageSize = Convert.ToInt32(parameters["results_per_page"]);
                var notificationslist = notifications.Skip((pageNo - 1) * (pageSize)).Take(pageSize).ToList();

                #endregion

                var res = from model in notificationslist
                          select new string[]
                             {
                                     model.NotificationType,
                                     model.ReceivedOn,
                                     model.Description,
                                     model.IsRead.ToString(),
                                     model.ReadOn.HasValue ? model.ReadOn.Value.ToString("yyyy/MM/dd hh:mm:ss") : "",
                                     model.URL,
                                     model.NotificationId.ToString(),
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

        #region Misc Methods

        private string GetNotificationString(int notificationtype, Guid relevantid, bool IsUrlRequired, bool iscarrieradmin)
        {
            string notificationstring = string.Empty;
            string URL = string.Empty;

            switch (notificationtype)
            {
                case 0: // NewBooking
                    notificationstring = "New Booking";
                    URL = "/Listing/" + relevantid + "/View";
                    break;

                case 1: //RatingReceived                    
                    notificationstring = "Ratings Received";
                    if (iscarrieradmin)
                        URL = "/Ratings/Index?From=Notifications";
                    else
                        URL = "/Ratings/RatingsReceived?From=Notifications";
                    break;

                case 2: //Shipment Arrival
                    notificationstring = "Shipment Arrival";
                    URL = "/Listing/" + relevantid + "/View";
                    break;

                case 3: //NewMessage
                    notificationstring = "New Message";
                    break;

                case 4: // ShipmentShipped
                    notificationstring = "Shipment Shipped";
                    URL = "/Listing/" + relevantid + "/View";
                    break;

                case 5: // ShipmentDelayed
                    notificationstring = "Shipment Delayed";
                    URL = "/Listing/" + relevantid + "/View";
                    break;

                case 6: // ShipmentCancelled
                    notificationstring = "Shipment Cancelled";
                    URL = "/Listing/" + relevantid + "/View";
                    break;
                case 7: // BookingConfirmed
                    notificationstring = "Booking Confirmed";
                    URL = "/Listing/" + relevantid + "/View";
                    break;

                case 8: // BookingRejected
                    notificationstring = "Booking Rejected";
                    URL = "/Listing/" + relevantid + "/View";
                    break;

            }

            if (IsUrlRequired)
                return URL;
                        
            return notificationstring;
        }

        #endregion
    }


}