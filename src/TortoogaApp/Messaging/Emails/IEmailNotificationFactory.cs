using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TortoogaApp.Models;
using TortoogaApp.Security;
using TortoogaApp.Services;
using TortoogaApp.ViewModels.AccountViewModels;
using TortoogaApp.ViewModels.CarrierViewModels;

namespace TortoogaApp.Messaging.Emails
{
    public interface IEmailNotificationFactory
    {
        void SendUserBookingApprovedEmail(Booking booking, ApplicationUser user);

        void SendUserBookingRejectEmail(Booking booking, ApplicationUser user);

        void SendUserShipmentShippedEmail(Booking booking, ApplicationUser user);

        void SendUserShipmentDelayedEmail(Booking booking, ApplicationUser user);

        void SendUserShipmentArrivedEmail(Booking booking, ApplicationUser user);

        void SendCarrierBookingConfirmationEmail(Listing listing);

        void SendUserBookingConfirmationEmail(Booking booking, ApplicationUser user);

        void SendSignUpCongratulatoryEmail(RegisterUserViewModel model);

        void SendCarrierSignUpEmail(RegisterCarrierViewModel model);

        void SendThankYouMailToCarrier(RegisterCarrierViewModel model);
        void SendUserSecurityCode(string email, string code);

        void SendCarrierRegistrationApprovalEmail(CarrierRegistrationApprovalViewModel model);

        void SendCarrierRegistrationRejectedEmail(string Email); 

        void SendCarrierRegistrationSuccessEmail(CarrierProfileSetupViewModel model);


        void SendUserFeedbackRatingsEmail_Customer(Booking booking, ApplicationUser user);

        void SendUserFeedbackRatingsEmail_Carrier(Booking booking, ApplicationUser user);
    }
}