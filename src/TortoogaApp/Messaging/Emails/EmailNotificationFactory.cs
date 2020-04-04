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
    public class EmailNotificationFactory : IEmailNotificationFactory
    {
        private IEmailSender _emailSender;

        public EmailNotificationFactory(IEmailSender emailSender)
        {
            this._emailSender = emailSender;
        }

        public void SendUserBookingApprovedEmail(Booking booking, ApplicationUser user)
        {
            var subject = $"Your Tortooga booking has been approved";
            var message = $"Your booking of {booking.ReferenceNumber}, {booking.Listing.Origin} - {booking.Listing.Destination} departing on {booking.Listing.DepartureDate.ToString("yyyy-MM-dd")} has been approved"
                + $"\n\nAmount ${booking.BookingAmount} will now be charged to your account.\n"
                + $"\nDrop off details:\n--------------------------------------------------\n"
                + $"\nAddress: {booking.Listing.DropOffAddress}\n"
                + $"\nDate Time: {booking.Listing.AppraisalDateTime}\n"
                + $"--------------------------------------------------\n"
                + $"\nIf you have any enqueries please contact {booking.Listing.ContactDetails}\n"
                + "You can login to Tortooga to manage your bookings";
            _emailSender.SendEmailAsync(user.Email, subject, message);
        }

        public void SendUserBookingRejectEmail(Booking booking, ApplicationUser user)
        {
            var subject = $"Your Tortooga booking has been rejected";
            var message = $"Your booking of {booking.ReferenceNumber}, {booking.Listing.Origin} - {booking.Listing.Destination} departing on {booking.Listing.DepartureDate.ToString("yyyy-MM-dd")} has been rejected"
                + $"\n\nReason\n--------------------------------------------------\n"
                + $"Destination country shipping restrictions\n"
                + $"--------------------------------------------------\n"
                + $"\nFor more information please contact {booking.Listing.ContactDetails}\n"
                + "You can login to Tortooga to manage your bookings";
            _emailSender.SendEmailAsync(user.Email, subject, message);
        }

        public void SendUserShipmentShippedEmail(Booking booking, ApplicationUser user)
        {
            var subject = $"Your Tortooga shipment has been shipped";
            var message = $"Your shipment of {booking.ReferenceNumber}, {booking.Listing.Origin} - {booking.Listing.Destination} departing on {booking.Listing.DepartureDate.ToString("yyyy-MM-dd")} has been shipped"
                + $"\nThe estimated arrival date will be {booking.Listing.EstimatedArrivalDate.ToString("yyyy-MM-dd")}\n"
                + $"\nWe will keep you posted with updates\n"
                + $"\nIf you have any enqueries please contact {booking.Listing.ContactDetails}\n"
                + "You can login to Tortooga to manage your bookings";
            _emailSender.SendEmailAsync(user.Email, subject, message);
        }

        public void SendUserShipmentDelayedEmail(Booking booking, ApplicationUser user)
        {
            var subject = $"Your Tortooga shipment has been delayed";
            var message = $"Your shipment of {booking.ReferenceNumber}, {booking.Listing.Origin} - {booking.Listing.Destination} departing on {booking.Listing.DepartureDate.ToString("yyyy-MM-dd")} has been delayed"
                + $"\nThe estimated arrival date has been delayed to {booking.Listing.EstimatedArrivalDate.ToString("yyyy-MM-dd")}\n"
                + $"\nWe will keep you posted with updates\n"
                + $"\nFor more information please contact {booking.Listing.ContactDetails}\n"
                + "You can login to Tortooga to manage your bookings";
            _emailSender.SendEmailAsync(user.Email, subject, message);
        }

        public void SendUserShipmentArrivedEmail(Booking booking, ApplicationUser user)
        {
            var subject = $"Your Tortooga shipment has arrived at destination";
            var message = $"Your shipment of {booking.ReferenceNumber}, {booking.Listing.Origin} - {booking.Listing.Destination} has arrived at destination"
                + $"\nPlease proceed to the address below to collect your goods, you are advised to contact them to pre-arrange a pick up time to avoid unneccessary delays:\n"
                + $"\nPick up:\n--------------------------------------------------\n"
                + $"\nAddress: {booking.Listing.PickUpAddress}\n"
                + $"\nContact: {booking.Listing.ContactDetails}\n"
                + $"--------------------------------------------------\n"
                + $"\nIf you have any enqueries please contact {booking.Listing.ContactDetails}\n"
                + "You can login to Tortooga to manage your bookings";
            _emailSender.SendEmailAsync(user.Email, subject, message);
        }

        public void SendCarrierBookingConfirmationEmail(Listing listing)
        {
            //First send to carrier
            var subject = $"PENDING-{listing.Origin}-{listing.Destination}-{listing.ReferenceNumber}";
            var message = $"Listing {listing.ListingGuid} has been booked and pending confirmation\n\n" +
                "Please login to Tortooga to manage bookings";
            _emailSender.SendEmailAsync(listing.Carrier.Email, subject, message);
        }

        public void SendUserBookingConfirmationEmail(Booking booking, ApplicationUser user)
        {
            //First send to carrier
            var subject = $"Tortooga-Booking-Confirmation";
            var message = $"You have booked {booking.Listing.Origin} - {booking.Listing.Destination}, REF: ({booking.ReferenceNumber}) departing on {booking.Listing.DepartureDate.ToString("yyyy-MM-dd")}"
                + "\nYour booking is pending for confirmation"
                + $"\n\n${booking.BookingAmount} will only be charged after your booking is approved.\n"
                + $"\nIf you have any enqueries please contact {booking.Listing.ContactDetails}\n"
                + "You can login to Tortooga to manage your bookings";
            _emailSender.SendEmailAsync(user.Email, subject, message);
        }

        public void SendSignUpCongratulatoryEmail(RegisterUserViewModel model)
        {
            //First send to carrier
            var subject = $"Welcome to Tortooga!";
            var message = $"Hi {model.Email}, Welcome to Tortooga\n\n" +
                "You can now ship your belongings without paying hefty price with us.\n\n" +
                "We will be the next freight/shipping industry changer!\n\n" +
                "Thanks for supporting us!";
            _emailSender.SendEmailAsync(model.Email, subject, message);
        }

        public void SendUserSecurityCode(string email, string code)
        {
            var subject = $"Security Code";
            var message = "Your security code is: " + code;
            _emailSender.SendEmailAsync(email, subject, message);
        }

        public void SendCarrierSignUpEmail(RegisterCarrierViewModel model)
        {
            var subject = $"New Carrier Registered!";
            var message = $"Hi admin,\n\n" +
                $" A new carrier {model.BusinessName} is registered to Tortooga!\n\n" +
                "Please approve/deny registration request.\n\n" +
                "Thanks!";
            _emailSender.SendEmailAsync(model.Email, subject, message);
        }

        public void SendThankYouMailToCarrier(RegisterCarrierViewModel model)
        {
            var subject = $"Thank you fro Registration!";
            var message = $"Hi {model.Email}, Welcome to Tortooga\n\n" +
                "Your registration request has been noted down.\n\n" +
                "You will soon receive approval/denial email from Tortooga!\n\n" +
                "Thanks for supporting us!";
            _emailSender.SendEmailAsync(model.Email, subject, message);
        }
        

        /// <summary>
        /// Sending the Feedback Email To Customer
        /// </summary>
        /// <param name="booking"></param>
        /// <param name="user"></param>
        public void SendUserFeedbackRatingsEmail_Customer(Booking booking, ApplicationUser user)
        {
            var subject = $"Tortooga Shipment - Give us your Feedback";
            var message = $"Thank you for using Tortooga Shipment !\n\n"
                + $"Your shipment of {booking.ReferenceNumber}, {booking.Listing.Origin} - {booking.Listing.Destination} has arrived at destination.\n"
                + $"\nHope you have liked working with us\n"
                + $"\nPlease log into your account and rate us on your latest shipments.\n";
            _emailSender.SendEmailAsync(user.Email, subject, message);
        }

        public void SendCarrierRegistrationApprovalEmail(CarrierRegistrationApprovalViewModel model)
        {
            var subject = $"Registration Approved!";
            var message = $"Hi {model.BusinessName}, Welcome to Tortooga\n\n" +
                "Please click the following link to login into the system.\n\n" +
                $" {model.LoginLink} \n\n" +
                "Use below password for first time login!\n\n" +
                $" {model.TempPassword}\n\n" +
                "Thanks for supporting us!";
            _emailSender.SendEmailAsync(model.Email, subject, message);
        }

        public void SendCarrierRegistrationSuccessEmail(CarrierProfileSetupViewModel model)
        {
            var subject = $"Registration Completed!";
            var message = $"Hi {model.Email}, Welcome to Tortooga\n\n" +
                "You can now list your spaces using Tortooga.\n\n" +
                "We will be the next freight/shipping industry changer!\n\n" +
                "Thanks for supporting us!";
            _emailSender.SendEmailAsync(model.Email, subject, message);
        }


        public void SendCarrierRegistrationRejectedEmail(string Email)
        {
            var subject = $"Registration Rejected!";
            var message = $"Hi {Email}, \n\n" +
                "Your registration request to Tortooga has been denied/rejected by the Tortooga Admin .\n\n" +
                $"\n\nReason\n--------------------------------------------------\n" + 
                "Thanks for supporting us!";
            _emailSender.SendEmailAsync(Email, subject, message);
        }


        /// <summary>
        /// Sending the Feedback Email To Carrier
        /// </summary>
        /// <param name="booking"></param>
        /// <param name="user"></param>
        public void SendUserFeedbackRatingsEmail_Carrier(Booking booking, ApplicationUser user)
        {
            var subject = $"Tortooga Shipment - Rate Your Customer";
            var message = $"The shipment of {booking.ReferenceNumber}, {booking.Listing.Origin} - {booking.Listing.Destination} has arrived at destination.\n"               
                + $"\nPlease log into your account and give ratings to your customer.\n";
            _emailSender.SendEmailAsync(user.Email, subject, message);
        }
    }
}