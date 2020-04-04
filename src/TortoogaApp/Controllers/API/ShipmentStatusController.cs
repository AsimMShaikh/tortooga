using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TortoogaApp.Services;
using AutoMapper;
using TortoogaApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using TortoogaApp.Security;
using Microsoft.Extensions.Options;
using TortoogaApp.ViewModels.ShipmentStatusViewModels;
using System.Linq;
using TortoogaApp.Models;

namespace TortoogaApp.Controllers
{
    [Route("api/[controller]")]
    public class ShipmentStatusController : Controller
    {
        #region Global Variables


        private IDbService _dbService;
        private IMapper _autoMapper;
        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        private AppSettings _appsettings;
        private IDashboardService _dashboardService;

        #endregion

        #region Constructor

        public ShipmentStatusController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IDbService dbService, IOptions<AppSettings> appsettings, IDashboardService dashboardService, IMapper autoMapper)
        {
            this._context = context;
            this._userManager = userManager;
            this._dbService = dbService;
            this._appsettings = appsettings.Value;
            this._dashboardService = dashboardService;
            this._autoMapper = autoMapper;
        }

        #endregion

        #region Methods


        // POST api/values
        [HttpPost]
        public ContentResult UpdateShipmentStatus([FromQuery]string booking_reference, [FromBody]ShipmentStatusRequestViewModel model)
        {
            var data = string.Empty;
            try
            {
                if (!(string.IsNullOrWhiteSpace(booking_reference)))
                {
                    if (_dbService.GetSingle<Booking>(x => x.ReferenceNumber == booking_reference) != null) //If the Booking Is Found Then Only Go Inside
                    {

                        if (model != null)
                        {
                            int status = -1;

                            try
                            {
                                status = (int)Enum.Parse(typeof(ShipmentStatusEnum), model.Status);
                            }
                            catch
                            {
                                ShipmentStatusResponseViewModel temp = new ShipmentStatusResponseViewModel();
                                temp.IsSucceeded = false;
                                temp.Message = "Shipment Status is not recognized";
                                data = CommonService.Json.Serialize(new { result = temp });
                            }

                            //Need to Make the Entry in the Table for the shipment Status                     

                            var NewShipmentDetails = new ShipmentStatus();
                            NewShipmentDetails.BookingReferenceNo = booking_reference;
                            NewShipmentDetails.Latitude = model.Latitude;
                            NewShipmentDetails.Longitude = model.Longitude;
                            NewShipmentDetails.CurrentLocation = model.Current_Location;
                            NewShipmentDetails.EstimatedArrivalDate = model.Estimated_Arrival_Date;
                            NewShipmentDetails.Status = status;
                            NewShipmentDetails.ShipmentStatusUpdateOn = DateTime.Now;

                            _dbService.Add(NewShipmentDetails);

                            var details = (from book in _context.Bookings
                                           join list in _context.Listings on book.ListingGuid equals list.ListingGuid
                                           where book.ReferenceNumber == booking_reference
                                           select new
                                           {
                                               book.CarrierGuid,
                                               list.ListingGuid
                                           }).FirstOrDefault();


                            int notificationstatus = -1;
                            string Desc = string.Empty;
                            if (status == (int)ShipmentStatusEnum.completed)
                            {
                                notificationstatus = (int)NotificationTypeEnum.ShipmentArrival;
                                Desc = "Shipment Arrival";
                            }
                            if (status == (int)ShipmentStatusEnum.shipped)
                            {
                                notificationstatus = (int)NotificationTypeEnum.ShipmentShipped;
                                Desc = "Shipment Shipped";
                            }
                            if (status == (int)ShipmentStatusEnum.delayed)
                            {
                                notificationstatus = (int)NotificationTypeEnum.ShipmentDelayed;
                                Desc = "Shipment Delayed";
                            }
                            if (status == (int)ShipmentStatusEnum.cancelled)
                            {
                                notificationstatus = (int)NotificationTypeEnum.ShipmentCancelled;
                                Desc = "Shipment Cancelled";
                            }

                            if (notificationstatus != -1)
                            {
                                //Make the entry in the notifications table
                                //Notification Entry
                                var newnotification = new Notifications()
                                {
                                    NotificationDescription = Desc,
                                    NotificationDateTime = DateTime.Now,
                                    NotificationType = notificationstatus,
                                    ReleventUserId = details.CarrierGuid, // As Notification is to be shown to carrier
                                    RelevantNotificationRefId = details.ListingGuid,
                                    IsRead = false,
                                    ReadDateTime = null
                                };
                                _dbService.Add(newnotification);
                            }

                            ShipmentStatusResponseViewModel response = new ShipmentStatusResponseViewModel();
                            response.IsSucceeded = true;
                            data = CommonService.Json.Serialize(new { result = response });
                        }
                        else
                        {
                            ShipmentStatusResponseViewModel response = new ShipmentStatusResponseViewModel();
                            response.IsSucceeded = false;
                            response.Message = "Bad Request. Data Not in Proper Format";

                            data = CommonService.Json.Serialize(new { result = response });
                        }
                    }
                    else
                    {
                        ShipmentStatusResponseViewModel response = new ShipmentStatusResponseViewModel();
                        response.IsSucceeded = false;
                        response.Message = "Bad Request. No Bookings found for the booking reference number";
                        data = CommonService.Json.Serialize(new { result = response });
                    }
                }
                else
                {
                    ShipmentStatusResponseViewModel response = new ShipmentStatusResponseViewModel();
                    response.IsSucceeded = false;
                    response.Message = "Reference No is Required";

                    data = CommonService.Json.Serialize(new { result = response });
                }
            }
            catch (Exception ex)
            {

                data = CommonService.Json.Serialize(new { });
            }
            return Content(data, "application/json");
        }




        #endregion
    }
}