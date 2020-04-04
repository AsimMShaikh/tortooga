using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TortoogaApp
{
    public class AppSettings
    {
        public string BaseUrl { get; set; }

        public string ContentRootPath { get; set; }

        public string StorageConnectionString { get; set; }

        public string ProfileImageContainer { get; set; }

        public string CompanyLogoContainer { get; set; }

        public string ImageBlobContainerPath { get; set; }

        public string MailRoot { get; set; }

        public string DevelopmentEmailCredential { get; set; }
    }

    public static class BaseUrls
    {
        public static string Api { get; set; }
        public static string Web { get; set; }
    }
}