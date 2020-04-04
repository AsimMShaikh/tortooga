using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TortoogaApp.Security;
using TortoogaApp.Services;
using AutoMapper;
using TortoogaApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using TortoogaApp.ViewModels.DashboardViewModels;

namespace TortoogaApp.Controllers
{
    public class DashboardController : Controller
    {
        #region Global Variables

        private IMapper _autoMapper;
        private IDbService _dbService;
        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        private IViewRenderingService _renderingService;
        private IActionContextAccessor _actionContextAccessor;
        private AppSettings _appsettings;
        private IDashboardService _dashboardService;

        #endregion

        #region Constructor

        public DashboardController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IDbService dbService, IViewRenderingService viewrenderingservice, IActionContextAccessor actionContextAccessor, IOptions<AppSettings> appsettings, IDashboardService dashboardService)
        {
            this._context = context;
            this._userManager = userManager;
            this._dbService = dbService;
            this._renderingService = viewrenderingservice;
            this._actionContextAccessor = actionContextAccessor;
            this._appsettings = appsettings.Value;
            this._dashboardService = dashboardService;
        }


        #endregion


        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.AppUrl = _appsettings.BaseUrl;

            if (User.IsInRole(RoleType.CARRIER_ADMIN))
            {
                ViewBag.IsAdmin = "True";
                ViewBag.CarrierId = currentUser.CarrierGuid;
                return View("AdminDashboard");
            }
            else
            {
                ViewBag.UserId = currentUser.Id;
                return View("UserDashboard");
            }
        }

        #region Carrier Dashboard Methods

        public ContentResult GetCarrierDashboardData(Guid CarrierId)
        {
            var data = string.Empty;
            try
            {
                if (User.IsInRole(RoleType.CARRIER_ADMIN))
                {
                    CarrierDashboardViewModel dashboardmodel = _dashboardService.GetCarrierDashboardData(CarrierId);
                    data = CommonService.Json.Serialize(new { data = dashboardmodel, Success = true });
                }
                else
                {
                    data = CommonService.Json.Serialize(new { });
                }
            }
            catch (Exception ex)
            {
                data = CommonService.Json.Serialize(new { });
            }
            return Content(data, "application/json");
        }

        public ContentResult UpdateBookingStatus(Guid BookingId, string Status)
        {
            var data = string.Empty;
            bool isUpdated = _dashboardService.UpdatePendingBookingStatus(BookingId, Status);
            if (isUpdated)
            {
                data = CommonService.Json.Serialize(new { Success = true });
            }
            else
            {
                data = CommonService.Json.Serialize(new { Success = false });
            }
            return Content(data, "application/json");
        }

        /// <summary>
        /// To Fetch the customer ratings to be rendered on the chart
        /// </summary>
        /// <param name="CarrierId"></param>
        /// <returns></returns>
        public ContentResult GetCustomerRatings(Guid CarrierId)
        {
            var data = string.Empty;
            CustomerRatingsDashboardViewModel custmerratings = _dashboardService.GetCustomerRatings(CarrierId);
            if (custmerratings != null)
            {
                data = CommonService.Json.Serialize(new { Success = true, data = custmerratings });
            }
            else
            {
                data = CommonService.Json.Serialize(new { Success = false, data = "" });
            }
            return Content(data, "application/json");
        }

        #endregion

        #region User Dashboard Methods

        public ContentResult GetUserDashboardData(Guid UserId)
        {
            var data = string.Empty;
            try
            {
                UserDashboardViewModel dashboardmodel = _dashboardService.GetUserDashboardData(UserId);
                data = CommonService.Json.Serialize(new { data = dashboardmodel, Success = true });
            }
            catch (Exception ex)
            {
                data = CommonService.Json.Serialize(new { data = "" });
            }
            return Content(data, "application/json");
        }

        #endregion

        #region Misc Methods

        public ContentResult UpdateNotificationToRead(Guid NotificationId)
        {
            var data = string.Empty;
            bool isUpdated = _dashboardService.UpdateNotificationToRead(NotificationId);
            if (isUpdated)
            {
                data = CommonService.Json.Serialize(new { Success = true });
            }
            else
            {
                data = CommonService.Json.Serialize(new { Success = false });
            }
            return Content(data, "application/json");

        } 

        #endregion

    }
}