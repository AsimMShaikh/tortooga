using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TortoogaApp.Services;
using TortoogaApp.Security;
using AutoMapper;
using TortoogaApp.Messaging.Emails;
using Microsoft.AspNetCore.Identity;
using TortoogaApp.ViewModels.UserViewModels;
using System.Globalization;
using TortoogaApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.IO;
using Microsoft.AspNet.Http;
using Microsoft.Net.Http.Headers;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.AspNetCore.Hosting;

namespace TortoogaApp.Controllers
{
    [Authorize]
    public class UserProfileController : Controller
    {
        private IDbService _dbService;
        private IMapper _autoMapper;
        private UserManager<ApplicationUser> _userManager;
        private IEmailNotificationFactory _emailNotificationFactory;
        private readonly AppSettings _appSettings;
        private IBlobService _iBlobService;
        private IHostingEnvironment _iHostingEnvironment;

        public UserProfileController(IDbService dbService, IMapper autoMapper, UserManager<ApplicationUser> userManager, IEmailNotificationFactory emailNotificationFactory, IOptions<AppSettings> appSettings, IBlobService blobService, IHostingEnvironment hostingEnvironment)
        {
            this._dbService = dbService;
            this._autoMapper = autoMapper;
            this._userManager = userManager;
            this._emailNotificationFactory = emailNotificationFactory;
            _appSettings = appSettings.Value;
            this._iBlobService = blobService;
            _iHostingEnvironment = hostingEnvironment;
        } 

        public async Task<IActionResult> Index()
        {          
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            

            var userProfile = new UserProfileViewModel()
            {
                Email = currentUser.Email,
                UserName = currentUser.UserName,
                FirstName = currentUser.FirstName,
                LastName = currentUser.LastName,
                BirthDate = currentUser.BirthDate != null ? currentUser.BirthDate.Value.ToString("yyyy/MM/dd") : "",                
                TimeZoneId = currentUser.TimeZoneId,
                PhoneNumber = currentUser.PhoneNumber,
                MobileNumber = currentUser.MobileNumber,
                AddressLine1 = currentUser.AddressLine1,
                AddressLine2 = currentUser.AddressLine2,
                PostCode = currentUser.PostCode,
                StateCode = currentUser.State,
                Suburb = currentUser.Suburb,
                CountryCode = currentUser.Country,               
            };

            var countryList = _dbService.Get<Country>(null).ToList();
            userProfile.Countries = new List<SelectListItem>();
            foreach (var item in countryList)
            {
                userProfile.Countries.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }

            userProfile.Provincestates = new List<SelectListItem>();
            if (userProfile.CountryCode != null)
            {
                var stateList = _dbService.Get<ProvinceState>(v => v.Country.Id == Int32.Parse(userProfile.CountryCode)).ToList();
                foreach (var item in stateList)
                {
                    userProfile.Provincestates.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
                }
            }
            

            var objImage = _dbService.GetSingle<ProfileImage>(v => v.UserId == currentUser.Id && v.isDeleted == false);
            if (objImage != null)
                userProfile.ImageUrl = objImage.ImageUri;
            ViewBag.AppUrl = _appSettings.BaseUrl;
            ViewBag.ImageContainerPath = _appSettings.ImageBlobContainerPath + _appSettings.ProfileImageContainer;
            return View(userProfile);
            
        }

        [HttpGet]
        [AllowAnonymous]
        public ContentResult GetAllStatesByCountryCode(string code)
        {
            var stateList = _dbService.Get<ProvinceState>(v => v.Country.Id == Int32.Parse(code)).ToList();
            var ListStates = new List<SelectListItem>();
            foreach (var item in stateList)
            {
                ListStates.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }
            return Content(JsonConvert.SerializeObject(ListStates), "application/json");
        }

        [HttpPost]
        public async Task<IActionResult> Save(UserProfileViewModel model)
        {
            var files = HttpContext.Request.Form.Files;
         
          
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (ModelState.IsValid)
            { 
               
                currentUser.PhoneNumber = model.PhoneNumber;
                currentUser.FirstName = model.FirstName;
                currentUser.LastName = model.LastName;                
                currentUser.MobileNumber = model.MobileNumber;
                currentUser.AddressLine1 = model.AddressLine1;
                currentUser.AddressLine2 = model.AddressLine2;
                currentUser.PostCode = model.PostCode;
                currentUser.State = model.StateCode;
                currentUser.Suburb = model.Suburb;
                currentUser.Country = model.CountryCode;
                currentUser.TimeZoneId = model.TimeZoneId;
                currentUser.BirthDate = Convert.ToDateTime(model.BirthDate);

                var result = await _userManager.UpdateAsync(currentUser);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Dashboard");
                }
                AddErrors(result);                
               
            }

            return View(model);

        }

        [HttpPost]
        public async Task<string> Upload()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var files = Request.Form.Files;            
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    

                    var parsedContent = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
                    var fileName = file.FileName;

                   

                    string fileExtension = Path.GetExtension(fileName);


                    using (var fileStream = file.OpenReadStream())
                    using (var ms = new MemoryStream())
                    {
                        fileStream.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        //string ImageBytes = Convert.ToBase64String(fileBytes);
                        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_appSettings.StorageConnectionString);
                        var container = _iBlobService.GetImageBlobContainer(_appSettings.ProfileImageContainer, _appSettings.StorageConnectionString);
                        string imageName = Guid.NewGuid().ToString() + fileExtension;
                        _iBlobService.SaveImageToBlob(container, fileBytes, imageName);
                        imageName = _appSettings.ImageBlobContainerPath + _appSettings.ProfileImageContainer + "/" + imageName;
                        var userImage = new UserImageModel()
                        {
                            ImageUri = imageName,
                            bytes = fileBytes,
                            Extension = fileExtension,
                            size = file.Length,
                            ContentType = file.ContentType
                        };


                        var objImage = _dbService.GetSingle<ProfileImage>(v => v.UserId == currentUser.Id && v.isDeleted == false);
                        if (objImage != null)
                        {
                            objImage.isDeleted = true;
                            _dbService.Update(objImage);
                        }

                        var imageDB = _autoMapper.Map<UserImageModel, ProfileImage>(userImage);


                        imageDB.UserId = currentUser.Id;
                        imageDB.isDeleted = false;

                        _dbService.Add(imageDB);

                        if (imageDB.ImageGuid == Guid.Empty)
                        {
                            throw new Exception("Db Error");
                        }

                    }                    

                }
            }           
            return null;
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }     
    }

}