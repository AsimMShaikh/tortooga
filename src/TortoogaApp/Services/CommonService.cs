using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using TortoogaApp.Data;
using TortoogaApp.Models;
using TortoogaApp.ViewModels.BadgeCountsViewModels;

namespace TortoogaApp.Services
{

    public interface ICommonservice
    {
        BadgeCountViewModel GetBadgeCounts(Guid UserId, Guid? CarrierId, bool IsCarrierAdmin);
    }

    public class CommonService : ICommonservice
    {
        private ApplicationDbContext _context;
        private IDbService _dbService;

        public CommonService(ApplicationDbContext context, IDbService dbService)
        {
            this._context = context;
            this._dbService = dbService;
        }

        public static string GenerateXML(Dictionary<string, string> filters)
        {
            XElement resultXML = new XElement("Parameters", from item in filters select new XElement(item.Key, item.Value));
            return resultXML.ToString();
        }

        /// <summary>
        /// For Fetching the BadgeCounts To be shown to User
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public BadgeCountViewModel GetBadgeCounts(Guid UserId, Guid? CarrierId, bool IsCarrierAdmin)
        {
            BadgeCountViewModel model = new BadgeCountViewModel();

            var ratingscount = 0;            
            if (IsCarrierAdmin)
            {
                var BookingDetails = _dbService.Get<Booking>(v => v.CarrierGuid == CarrierId && v.IsDeleted == false).ToList();
                ratingscount = _dbService.Get<Rating>(v => v.CarrierID == CarrierId && v.IsDeleted == false).Count();

                model.PendingBookings = BookingDetails.Where(x => x.Status == BookingStatus.BookedButNotConfirmed && x.IsDeleted == false).Count();

                model.ProcessingBookings = BookingDetails.Where(x => x.Status == BookingStatus.BookedAndConfirmed && x.IsDeleted == false).Count();

                model.ShippingBookings = BookingDetails.Where(x => x.Status == BookingStatus.Shipped && x.IsDeleted == false).Count();
            }
            else
            {
                ratingscount = _dbService.Get<Notifications>(v => v.NotificationType == (int)NotificationTypeEnum.RatingReceived && v.ReleventUserId == UserId && v.IsRead == false).Count();
            }

            model.RatingsReceived = ratingscount;

            return model;
        }


        /// <summary>
        /// For Json Serialization and Deserialization
        /// </summary>
        public static class Json
        {
            /// <summary>
            /// Method to convert json string to custom object
            /// </summary>
            /// <typeparam name="T">Type of custom object to be returned</typeparam>
            /// <param name="jsonData">Json string to convert</param>
            /// <returns>Custom object</returns>
            public static T Deserialize<T>(string jsonData)
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonData);
            }

            /// <summary>
            /// Method to convert custom object to json string
            /// </summary>
            /// <typeparam name="T">Type of custom object to be converted</typeparam>
            /// <param name="entity">Custom object to convert</param>
            /// <returns>Json string</returns>
            public static string Serialize<T>(T entity)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(entity);
            }

            /// <summary>
            /// Method to convert json string to custom object
            /// </summary>
            /// <param name="jsonData">Json string to convert</param>
            /// <param name="t">Type of custom object to be returned</param>
            /// <returns>Custom object</returns>
            public static dynamic Deserialize(string jsonData, Type t)
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject(jsonData, t);
            }
        }

        /// <summary>
        /// For fetching the GenericResponse Of Any Model When returing the Partial View as String
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class GenericJsonResponse<T>
        {
            public GenericJsonResponse()
            {
                IsSucceed = false;
                Message = string.Empty;
                Data = default(T);
            }
            public bool IsSucceed { get; set; }
            public string Message { get; set; }
            public T Data { get; set; }
        }

        /// <summary>
        /// For Converting the Lat/Long From Decimal to Degrees
        /// </summary>
        public class GeoAngle
        {
            public bool IsNegative { get; set; }
            public int Degrees { get; set; }
            public int Minutes { get; set; }
            public int Seconds { get; set; }
            public int Milliseconds { get; set; }

            public static GeoAngle FromDouble(double angleInDegrees)
            {
                //ensure the value will fall within the primary range [-180.0..+180.0]
                while (angleInDegrees < -180.0)
                    angleInDegrees += 360.0;

                while (angleInDegrees > 180.0)
                    angleInDegrees -= 360.0;

                var result = new GeoAngle();

                //switch the value to positive
                result.IsNegative = angleInDegrees < 0;
                angleInDegrees = Math.Abs(angleInDegrees);

                //gets the degree
                result.Degrees = (int)Math.Floor(angleInDegrees);
                var delta = angleInDegrees - result.Degrees;

                //gets minutes and seconds
                var seconds = (int)Math.Floor(3600.0 * delta);
                result.Seconds = seconds % 60;
                result.Minutes = (int)Math.Floor(seconds / 60.0);
                delta = delta * 3600.0 - seconds;

                //gets fractions
                result.Milliseconds = (int)(1000.0 * delta);

                return result;
            }

            public override string ToString()
            {
                var degrees = this.IsNegative
                    ? -this.Degrees
                    : this.Degrees;

                return string.Format(
                    "{0}° {1:00}' {2:00}\"",
                    degrees,
                    this.Minutes,
                    this.Seconds);
            }

            public string ToString(string format)
            {
                switch (format)
                {
                    case "NS":
                        return string.Format(
                            "{0}° {1:00}' {2:00}\".{3:000} {4}",
                            this.Degrees,
                            this.Minutes,
                            this.Seconds,
                            this.Milliseconds,
                            this.IsNegative ? 'S' : 'N');

                    case "WE":
                        return string.Format(
                            "{0}° {1:00}' {2:00}\".{3:000} {4}",
                            this.Degrees,
                            this.Minutes,
                            this.Seconds,
                            this.Milliseconds,
                            this.IsNegative ? 'W' : 'E');

                    default:
                        throw new NotImplementedException();
                }
            }
        }
    }




}
