using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TortoogaApp.Services;
using AutoMapper;
using TortoogaApp.Messaging.Emails;
using TortoogaApp.Security;
using Microsoft.AspNetCore.Identity;
using TortoogaApp.Data;
using Microsoft.Extensions.Options;
using TortoogaApp.Models;
using TortoogaApp.ViewModels.SystemAdminViewModels;
using Microsoft.AspNetCore.Authorization;
using TortoogaApp.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using TortoogaApp.ViewModels.UserViewModels;
using Microsoft.Net.Http.Headers;
using System.IO;
using Microsoft.WindowsAzure.Storage;

namespace TortoogaApp.Controllers
{
    public class SystemAdminController : BaseController
    {
        private IDbService _dbService;
        private IMapper _autoMapper;
        private UserManager<ApplicationUser> _userManager;
        private IEmailNotificationFactory _emailNotificationFactory;
        private AppSettings _appSettings;
        private IBlobService _iBlobService;
        private ApplicationDbContext _context;

        public SystemAdminController(IDbService dbService, IMapper autoMapper, UserManager<ApplicationUser> userManager, IEmailNotificationFactory emailNotificationFactory, IOptions<AppSettings> appSettings, IBlobService blobService, ApplicationDbContext context, ICommonservice commonservice)
            : base(commonservice, userManager)
        {
            this._context = context;
            this._dbService = dbService;
            this._autoMapper = autoMapper;
            this._userManager = userManager;
            this._emailNotificationFactory = emailNotificationFactory;
            this._appSettings = appSettings.Value;
            this._iBlobService = blobService;
        }

        #region Manage Approvals

        [Authorize(Roles = RoleType.SYSTEM_ADMIN)]
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.AppUrl = _appSettings.BaseUrl;

         
            return View();
        }

        /// <summary>
        /// For All Carrer Registration related data
        /// </summary>
        /// <returns></returns>
        public async Task<ContentResult> GetAllApprovalsList()
        {
            var data = string.Empty;
            try
            {
                var currentUser = await _userManager.GetUserAsync(HttpContext.User);

                string errorMessage = string.Empty;
                string uri = string.Empty;
                var urlParamenters = new Dictionary<string, string>();
                var parameters = GetParameters(new List<string>() { "BusinessName", "ContactPerson", "PhoneNumber", "Email", "MCDotNumber", "Country", "State","Status", "RequestDate", "ResponseBy", "ResponseDate","CarrierRegistrationGuid" });
                var carrierRegistrationDetails = _dbService.Get<CarrierRegistration>(null);

                #region Sorting

                if (parameters.ContainsKey("sort_by") && parameters["sort_by"] != null && parameters["sort_by"] != string.Empty)
                {
                    var value = parameters["sort_by"].ToString();
                    
                    try
                    {

                        carrierRegistrationDetails = carrierRegistrationDetails.OrderByDescending(x=>x.RequestDate).ToList();
                    }
                    catch (Exception ex)
                    {
                        carrierRegistrationDetails = carrierRegistrationDetails.ToList();
                    }
                }                

                #endregion

                if (carrierRegistrationDetails.Count() > 0)
                {
                    var res = from model in carrierRegistrationDetails
                              select new string[]
                                 {
                                     model.BusinessName,
                                     model.ContactPerson,
                                     model.PhoneNumber,
                                     model.Email,
                                     model.MCDotNumber,
                                     model.Country == null? "" : _dbService.Get<Country>(v=>v.Id == Int32.Parse(model.Country)).FirstOrDefault().Name,
                                     model.State== null? "" : _dbService.Get<ProvinceState>(v=>v.Id == Int32.Parse(model.State)).FirstOrDefault().Name,
                                     Enum.GetName(typeof(CarrierRegistrationStatus),model.Status),
                                     model.RequestDate != null ? model.RequestDate.Value.ToString("yyyy/MM/dd") : "",
                                     model.ResponseBy != null ?   _dbService.Get<ApplicationUser>(u=>u.Id == model.ResponseBy).FirstOrDefault().UserName : "",
                                     model.ResponseDate != null ? model.ResponseDate.Value.ToString("yyyy/MM/dd") : "",
                                     model.CarrierRegistrationGuid.ToString()
                                 };

                    data = CommonService.Json.Serialize(new { iTotalRecords = res.Count(), iTotalDisplayRecords = res.Count(), aaData = res });
                }
                else
                {
                    data = CommonService.Json.Serialize(new { iTotalRecords = 0, iTotalDisplayRecords = 0, aaData = new string[] { } });
                }
            }
            catch (Exception ex)
            {
                data = CommonService.Json.Serialize(new { iTotalRecords = 0, iTotalDisplayRecords = 0, aaData = new string[] { } });
            }
            return Content(data, "application/json");
        }

        
        [Authorize(Roles = RoleType.SYSTEM_ADMIN)]
        public async Task<ContentResult> ApproveRegistrationRequest(Guid regId)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var data = string.Empty;                               
            var result = _dbService.GetSingle<CarrierRegistration>(v => v.CarrierRegistrationGuid == regId);
            if (result != null)
            {
                var viewModel = new CarrierRegistrationApprovalViewModel()
                {
                    Email = result.Email,
                    BusinessName = result.BusinessName,
                    ContactPerson = result.ContactPerson,
                    LoginLink = _appSettings.BaseUrl + "/Account/SetUpCarrierProfile?id=" + result.CarrierRegistrationGuid,
                    TempPassword = result.TempPassword,
                };
                _emailNotificationFactory.SendCarrierRegistrationApprovalEmail(viewModel);

                result.Status = CarrierRegistrationStatus.Approved;
                result.ResponseBy = currentUser.Id;
                result.ResponseDate = DateTime.UtcNow;
                _dbService.Update(result);
                return Content(CommonService.Json.Serialize(new { Success = true }), "application/json");
            }
            
            return Content(CommonService.Json.Serialize(new { Success = false }), "application/json");
        }


        
        [Authorize(Roles = RoleType.SYSTEM_ADMIN)]
        public async Task<ContentResult> RejectRegistrationRequest(Guid regId)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var data = string.Empty;
            var result = _dbService.GetSingle<CarrierRegistration>(v => v.CarrierRegistrationGuid == regId);
            if (result != null)
            {
                _emailNotificationFactory.SendCarrierRegistrationRejectedEmail(result.Email);
                result.Status = CarrierRegistrationStatus.Rejected;
                result.ResponseBy = currentUser.Id;
                result.ResponseDate = DateTime.UtcNow;
                _dbService.Update(result);
                return Content(CommonService.Json.Serialize(new { Success = true }), "application/json");
            }
            return Content(CommonService.Json.Serialize(new { Success = false }), "application/json");
        }

        #endregion

        #region Manage Users


        #region Manage Carrier users
        [Authorize(Roles = RoleType.SYSTEM_ADMIN)]
        public async Task<IActionResult> ManageCarrierUsers()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            CarrierUsersListViewModel viewmodel = new CarrierUsersListViewModel();
           
            viewmodel.Carriers = new List<SelectListItem>();
            var carrierList = _dbService.Get<Carrier>(null).ToList();
            foreach (var item in carrierList)
            {
                viewmodel.Carriers.Add(new SelectListItem() { Text = item.BusinessName, Value = item.CarrierGuid.ToString() });
            }

            viewmodel.Countries = new List<SelectListItem>();
            var countrylist = _dbService.Get<Country>(null).ToList();
            foreach (var item in countrylist)
            {
                viewmodel.Countries.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }
            

            ViewBag.AppUrl = _appSettings.BaseUrl;


            return View("~/Views/SystemAdmin/CarrierUsers.cshtml",viewmodel);
        }

        /// <summary>
        /// For All Carrer Registration related data
        /// </summary>
        /// <returns></returns>
        public async Task<ContentResult> GetAllCarrierUsers()
        {
            var data = string.Empty;
            try
            {
                var currentUser = await _userManager.GetUserAsync(HttpContext.User);

                string errorMessage = string.Empty;
                string uri = string.Empty;
                var urlParamenters = new Dictionary<string, string>();
                var parameters = GetParameters(new List<string>() { "Email","FullName", "PhoneNumber", "Country", "State", "CarrierName" });
               // var carrierUsers = _dbService.Get<ApplicationUser>(x=>x.CarrierGuid != null && x.IsDeleted == false);

                if(Request.Query["CarrierGuid"] != "")
                {
                    var carrierVal = Request.Query["CarrierGuid"];
                    if (!string.IsNullOrWhiteSpace(carrierVal))
                    {
                        //searchTxt = CheckIfDate(searchTxt);                
                        parameters.Add("Carrier", string.IsNullOrEmpty(carrierVal) ? string.Empty : carrierVal.ToString().ToLower());
                    }
                }


                if (Request.Query["Country"] != "")
                {
                    var countryVal = Request.Query["Country"];
                    if (!string.IsNullOrWhiteSpace(countryVal))
                    {
                        //searchTxt = CheckIfDate(searchTxt);                
                        parameters.Add("Country", string.IsNullOrEmpty(countryVal) ? string.Empty : countryVal.ToString().ToLower());
                    }
                }

                if (Request.Query["showDisabled"] != "")
                {
                    var allowDisabled = Request.Query["showDisabled"];
                    if (!string.IsNullOrWhiteSpace(allowDisabled))
                    {
                        //searchTxt = CheckIfDate(searchTxt);                
                        parameters.Add("showDisabled", string.IsNullOrEmpty(allowDisabled) ? string.Empty : allowDisabled.ToString().ToLower());
                    }
                }

                var tempUsers = (from carUser in _context.Users
                                 join Carrier in _context.Carriers on carUser.CarrierGuid equals Carrier.CarrierGuid                                 
                                 where carUser.CarrierGuid  != null && carUser.IsDeleted != true
                                   select new CarrierUsersListViewModel
                                   {
                                       UserId = carUser.Id,
                                       Email = carUser.Email,
                                       FullName = carUser.FirstName + " " + carUser.LastName,
                                       PhoneNumber = carUser.PhoneNumber,                                       
                                       Country = carUser.Country != null ? carUser.Country : "",
                                       State = carUser.State != null ? carUser.State: "",                                     
                                       CarrierName = Carrier.BusinessName,
                                       Disabled = carUser.IsDisabled.ToString(),
                                       CarrierId = Carrier.CarrierGuid
                                      
                                   });

                #region Filteration



                


                if (parameters.ContainsKey("Carrier") && parameters["Carrier"] != null && parameters["Carrier"] != string.Empty)
                {
                    var selectedVal = parameters["Carrier"].ToString();
                    if (selectedVal != "0")
                    {
                        tempUsers = tempUsers.Where(p => p.CarrierId.ToString() == selectedVal);
                    }
                }

                if (parameters.ContainsKey("Country") && parameters["Country"] != null && parameters["Country"] != string.Empty)
                {
                    var selectedCountry = parameters["Country"].ToString();
                    if (selectedCountry != "0")
                    {
                        tempUsers = tempUsers.Where(p => p.Country.ToString() == selectedCountry);
                    }
                }

                if (parameters.ContainsKey("showDisabled") && parameters["showDisabled"] != null && parameters["showDisabled"] != string.Empty)
                {
                    var showDisabled = parameters["showDisabled"].ToString();    
                    if(showDisabled == "false")                
                        tempUsers = tempUsers.Where(p => p.Disabled == "False");
                    
                }

                if (parameters.ContainsKey("Value") && parameters["Value"] != null && parameters["Value"] != string.Empty)
                {
                    var value = parameters["Value"].ToString();
                    tempUsers = tempUsers.Where(p => p.Email.Contains(value) || p.FullName.Contains(value) || p.Country.Contains(value) || p.State.Contains(value) || p.PhoneNumber.Contains(value) || p.CarrierName.Contains(value));
                }


                var carrierUsersList = tempUsers.ToList();
                //var finalusers = carrierUsers.ToList();


                #endregion

                if (carrierUsersList.Count() > 0)
                {
                    var res = from model in carrierUsersList
                              select new string[]
                                 {
                                     model.Email,
                                     model.FullName,                                     
                                     model.PhoneNumber,
                                     model.Country != "" ? _dbService.Get<Country>(v=>v.Id == Int32.Parse(model.Country)).FirstOrDefault().Name : "",
                                     model.State != "" ? _dbService.Get<ProvinceState>(v=>v.Id == Int32.Parse(model.State)).FirstOrDefault().Name : "",
                                     model.CarrierName,
                                     model.Disabled,
                                     model.UserId.ToString()
                                 };

                    data = CommonService.Json.Serialize(new { iTotalRecords = res.Count(), iTotalDisplayRecords = res.Count(), aaData = res });
                }
                else
                {
                    data = CommonService.Json.Serialize(new { iTotalRecords = 0, iTotalDisplayRecords = 0, aaData = new string[] { } });
                }
            }
            catch (Exception ex)
            {
                data = CommonService.Json.Serialize(new { iTotalRecords = 0, iTotalDisplayRecords = 0, aaData = new string[] { } });
            }
            return Content(data, "application/json");
        }


        public IActionResult AddEditCarrierUser(string guid)
        {
            var userProfile = new AddEditCarrierUserViewModel();
            ViewBag.Mode = "Add";
            if (guid != null && guid != "")
            {
                var carrierUser = _dbService.GetSingle<ApplicationUser>(p => p.Id.ToString() == guid && p.IsDeleted == false);

                userProfile.Email = carrierUser.Email;                
                userProfile.FirstName = carrierUser.FirstName;
                userProfile.LastName = carrierUser.LastName;
                userProfile.BirthDate = carrierUser.BirthDate != null ? carrierUser.BirthDate.Value.ToString("yyyy/MM/dd") : "";
                userProfile.TimeZoneId = carrierUser.TimeZoneId;
                userProfile.PhoneNumber = carrierUser.PhoneNumber;
                userProfile.MobileNumber = carrierUser.MobileNumber;
                userProfile.AddressLine1 = carrierUser.AddressLine1;
                userProfile.AddressLine2 = carrierUser.AddressLine2;
                userProfile.PostCode = carrierUser.PostCode;
                userProfile.StateCode = carrierUser.State;
                userProfile.Suburb = carrierUser.Suburb;
                userProfile.CountryCode = carrierUser.Country;
                userProfile.isDisabled = carrierUser.IsDisabled;                
                userProfile.UserID = carrierUser.Id;

                var objImage = _dbService.GetSingle<ProfileImage>(v => v.UserId == userProfile.UserID && v.isDeleted == false);
                if (objImage != null)
                    userProfile.ImageUrl = objImage.ImageUri;
                ViewBag.AppUrl = _appSettings.BaseUrl;
                ViewBag.ImageContainerPath = _appSettings.ImageBlobContainerPath + _appSettings.ProfileImageContainer;

                ViewBag.Mode = "Edit";
            }


            var carrierList = _dbService.Get<Carrier>(null).ToList();
            userProfile.Carriers = new List<SelectListItem>();
            foreach (var item in carrierList)
            {
                userProfile.Carriers.Add(new SelectListItem() { Text = item.BusinessName, Value = item.CarrierGuid.ToString() });
            }
            

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

            

            return View("~/Views/SystemAdmin/AddEditCarrierUser.cshtml", userProfile);
        }


        [HttpPost]
        public IActionResult AddEditCarrierUser(AddEditCarrierUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser carrierUser;
                if (model.UserID != Guid.Empty)
                {
                    carrierUser = _dbService.GetSingle<ApplicationUser>(p => p.Id == model.UserID);
                }
                else
                {
                    carrierUser = new ApplicationUser();
                    carrierUser.Email = model.Email;
                }

                carrierUser.PhoneNumber = model.PhoneNumber;
                carrierUser.FirstName = model.FirstName;
                carrierUser.LastName = model.LastName;
                carrierUser.MobileNumber = model.MobileNumber;
                carrierUser.AddressLine1 = model.AddressLine1;
                carrierUser.AddressLine2 = model.AddressLine2;
                carrierUser.PostCode = model.PostCode;
                carrierUser.State = model.StateCode;
                carrierUser.Suburb = model.Suburb;
                carrierUser.Country = model.CountryCode;
                carrierUser.TimeZoneId = model.TimeZoneId;
                carrierUser.BirthDate = Convert.ToDateTime(model.BirthDate);
                carrierUser.IsDisabled = model.isDisabled;               
                if (model.CarrierGuid != null)
                {
                    carrierUser.CarrierGuid = new Guid(model.CarrierGuid);
                }

                if (carrierUser.Id != Guid.Empty)
                    _dbService.Update(carrierUser);
                else
                    _dbService.Add(carrierUser);

                return RedirectToAction("ManageCarrierUsers");

            }

            return View(model);

        }

        [HttpPost]
        public async Task<string> Upload(string userId)
        {          
            var currentUser = _dbService.GetSingle<ApplicationUser>(p => p.Id.ToString() == userId);
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


        /// <summary>
        /// Disable User
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]        
        public IActionResult DisableUser(string id)
        {            
            var carrierUser = _dbService.GetSingle<ApplicationUser>(p => p.Id.ToString() == id && p.IsDisabled == false);
            if (carrierUser != null)
            {
                try
                {
                    carrierUser.IsDisabled = true;
                    _dbService.Update(carrierUser);
                    
                }
                catch (Exception ex)
                {
                    return Json(new { result = "false", Message = ex.Message });
                }
                return Json(new { result = "true", Message = "User disabled" });
            }
            else
            {                                
                return Json(new { result = "false", Message = "User not found" });
            }
        }

        /// <summary>
        /// Enable User
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult EnableUser(string id)
        {
            var carrierUser = _dbService.GetSingle<ApplicationUser>(p => p.Id.ToString() == id && p.IsDisabled == true);
            if (carrierUser != null)
            {
                try
                {
                    carrierUser.IsDisabled = false;
                    _dbService.Update(carrierUser);

                }
                catch (Exception ex)
                {
                    return Json(new { result = "false", Message = ex.Message });
                }
                return Json(new { result = "true", Message = "User Enabled" });
            }
            else
            {
                return Json(new { result = "false", Message = "User not found" });
            }
        }

        /// <summary>
        /// Disable User
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult DeleteUser(string id)
        {
            var carrierUser = _dbService.GetSingle<ApplicationUser>(p => p.Id.ToString() == id);
            if (carrierUser != null)
            {
                try
                {
                    carrierUser.IsDeleted = true;
                    _dbService.Update(carrierUser);

                }
                catch (Exception ex)
                {
                    return Json(new { result = "false", Message = ex.Message });
                }
                return Json(new { result = "true", url = Url.Action("ManageCarrierUsers", "SystemAdmin") });
            }
            else
            {
                return Json(new { result = "false", Message = ""});
               
            }
        }

        public ContentResult ListCarriers()
        {           
            var carriers = _dbService.Get<Carrier>(null).ToList();
            return Content(CommonService.Json.Serialize(carriers), "application/json");
        }

        public ContentResult ListCountries()
        {
            var countries = _dbService.Get<Country>(null).ToList();
            return Content(CommonService.Json.Serialize(countries), "application/json");
        }




        #endregion
        #region Manage Shipper Users
        public async Task<IActionResult> ManageShipperUsers()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.AppUrl = _appSettings.BaseUrl;


            return View();
        }



        #endregion
        #endregion

    }
}