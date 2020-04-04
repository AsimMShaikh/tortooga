using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.IO;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace TortoogaApp.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link http://go.microsoft.com/fwlink/?LinkID=532713
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        private AppSettings _appSettings;

        public AuthMessageSender(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = ContructEmailMessage(email, subject, message);

            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);
                client.Connect("smtp.gmail.com", 587);

                client.Authenticate("developer.development.email", _appSettings.DevelopmentEmailCredential);

                client.Send(emailMessage);
                client.Disconnect(true);
            }

            return Task.FromResult(0);
        }

        /// <summary>
        /// Validates the server certificate.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="certificate">The certificate.</param>
        /// <param name="chain">The chain.</param>
        /// <param name="sslPolicyErrors">The SSL policy errors.</param>
        /// <returns></returns>
        private bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;

            // if there are errors in the certificate chain, look at each error to determine the cause.
            if ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateChainErrors) != 0)
            {
                if (chain != null && chain.ChainStatus != null)
                {
                    foreach (var status in chain.ChainStatus)
                    {
                        if ((certificate.Subject == certificate.Issuer) && (status.Status == X509ChainStatusFlags.UntrustedRoot))
                        {
                            // self-signed certificates with an untrusted root are valid.
                            continue;
                        }
                        else if (status.Status != X509ChainStatusFlags.NoError)
                        {
                            // if there are any other errors in the certificate chain, the certificate is invalid,
                            // so the method returns false.
                            return false;
                        }
                    }

                    // When processing reaches this line, the only errors in the certificate chain are
                    // untrusted root errors for self-signed certificates. These certificates are valid
                    // for default Exchange server installations, so return true.
                    return true;
                }
                return false;
            }

            return false;
        }

        private MimeMessage ContructEmailMessage(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("tortooga-bookin-noreply", "booking-noreply@tortooga.com"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("plain") { Text = message };

            return emailMessage;
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    /// <summary>
    /// Used for development
    /// </summary>
    /// <seealso cref="TortoogaApp.Services.IEmailSender" />
    public class DevelopmentMessageSender : IEmailSender
    {
        private AppSettings _appSettings;

        public DevelopmentMessageSender(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            var MAILROOT = _appSettings.MailRoot;
            var emailMessage = ContructEmailMessage(email, subject, message);

            var emailPath = MAILROOT + subject + ".eml";
            using (StreamWriter data = File.CreateText(emailPath))
            {
                emailMessage.WriteTo(data.BaseStream);
            }

            return Task.FromResult(0);
        }

        //TODO: need to refactor out as there are duplicate
        private MimeMessage ContructEmailMessage(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("tortooga-bookin-noreply", "booking-noreply@tortooga.com"));
            //emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.To.Add(new MailboxAddress("", "developer.development.email@gmail.com"));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("plain") { Text = message };

            return emailMessage;
        }
    }
}