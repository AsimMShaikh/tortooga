using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TortoogaApp.Services;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TortoogaApp.Messaging.Emails;
using TortoogaApp.Security;
using Microsoft.AspNetCore.Authorization;
using TortoogaApp.Models;
using TortoogaApp.ViewModels.CarrierViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.IO;
using Microsoft.WindowsAzure.Storage;
using TortoogaApp.ViewModels.UserViewModels;
using TortoogaApp.Data;
using TortoogaApp.ViewModels.RatingViewModels;
using System.Globalization;

namespace TortoogaApp.Controllers
{
    [Route("/Carrier")]
    public class CarrierController : BaseController
    {
        private IDbService _dbService;
        private IMapper _autoMapper;
        private UserManager<ApplicationUser> _userManager;
        private IEmailNotificationFactory _emailNotificationFactory;
        private AppSettings _appSettings;
        private IBlobService _iBlobService;
        private ApplicationDbContext _context;

        public CarrierController(IDbService dbService, IMapper autoMapper, UserManager<ApplicationUser> userManager, IEmailNotificationFactory emailNotificationFactory, IOptions<AppSettings> appSettings, IBlobService blobService, ApplicationDbContext context, ICommonservice commonservice)
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

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            var carrier = _dbService.GetSingle<Carrier>(v => v.CarrierGuid == currentUser.CarrierGuid);

            var carrierProfile = new CarrierProfileViewModel();

            carrierProfile = _autoMapper.Map<Carrier, CarrierProfileViewModel>(carrier);

                carrierProfile.CarrierGuid = carrier.CarrierGuid;
                var countryList = _dbService.Get<Country>(null).ToList();
                carrierProfile.Countries = new List<SelectListItem>();
                foreach (var item in countryList)
                {
                    carrierProfile.Countries.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
                }

            carrierProfile.Provincestates = new List<SelectListItem>();
            var carrierCountry = _dbService.Get<Country>(v => v.Name == carrier.Country, x => x.ProvinceStates).FirstOrDefault();


            var stateVal = _dbService.GetSingle<ProvinceState>(v => v.Name == carrier.State);
            if(stateVal  != null)
            {
                carrierProfile.State = stateVal.Name;
            }

            //if (carrierCountry != null)
            //{

            foreach (var item in carrierCountry.ProvinceStates)
            {
                carrierProfile.Provincestates.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }

        //}


        var objBankDetails = _dbService.GetSingle<CarrierBankingDetails>(v => v.CarrierBankingDetailsId == carrier.CarrierBankingDetailsId);

            if (objBankDetails != null)
            {
                carrierProfile.BankAccountNumber = objBankDetails.AccountNumber;
                carrierProfile.BankIdentificationNumber = objBankDetails.BankIdentificationNumber;
                carrierProfile.AccountName = objBankDetails.AccountName;
            }

            var objImage = _dbService.GetSingle<CompanyLogo>(v => v.CarrierGuid == carrier.CarrierGuid && v.isDeleted == false);
            if (objImage != null)
                carrierProfile.ImageUrl = objImage.ImageUri;
            ViewBag.AppUrl = _appSettings.BaseUrl;
            ViewBag.ImageContainerPath = _appSettings.ImageBlobContainerPath + _appSettings.CompanyLogoContainer;

            return View("Index", carrierProfile);
        }

        [Route("{id}/ViewBio")]
        [AllowAnonymous]
        public async Task<IActionResult> ViewBio(Guid id)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var carrier = _dbService.GetSingle<Carrier>(v => v.CarrierGuid == id);

            var carrierProfile = new CarrierBioViewModel()
            {
                CarrierGuid = carrier.CarrierGuid.ToString(),
                BusinessName = carrier.BusinessName,
                CompanyBio = carrier.CompanyBio,
                Email = carrier.Email,
                PhoneNumber = carrier.PhoneNumber,
                AddressLine1 = carrier.AddressLine1,
                AddressLine2 = carrier.AddressLine2,
                City = carrier.City,
                PostCode = carrier.PostCode,
                Country = carrier.Country,
                State  = carrier.State
            };
            //if (carrier.Country != null)
            //    carrierProfile.Country = _dbService.GetSingle<Country>(v => v.Id == Convert.ToInt32(carrier.Country)).Name;
            //if (carrier.State != null)
            //    carrierProfile.State = _dbService.GetSingle<ProvinceState>(v => v.Id == Convert.ToInt32(carrier.State)).Name;
            carrierProfile.ShippingItemsList = new List<string>();
            if (carrier.CarrierShippingItems != null)
            {
                carrierProfile.ShippingItemsList = carrier.CarrierShippingItems.Split(',').ToList();
            }

            var objImage = _dbService.GetSingle<CompanyLogo>(v => v.CarrierGuid == carrier.CarrierGuid && v.isDeleted == false);
            if (objImage != null)
                carrierProfile.ImageUrl = objImage.ImageUri;

            var RatingDetails = _dbService.Get<Rating>(v => v.CarrierID == carrier.CarrierGuid && v.IsDeleted == false).ToList();
            int OverallExperience = 0;
            if (RatingDetails.Count > 0)
            {
                //int count_five = RatingDetails.Where(x => x.OverallExperience == 5 || x.Communication == 5 || x.Service == 5 || x.Price == 5).Count();
                //int count_four = RatingDetails.Where(x => x.OverallExperience == 4 || x.Communication == 4 || x.Service == 4 || x.Price == 4).Count();
                //int count_Three = RatingDetails.Where(x => x.OverallExperience == 3 || x.Communication == 3 || x.Service == 3 || x.Price == 3).Count();
                //int count_Two = RatingDetails.Where(x => x.OverallExperience == 2 || x.Communication == 2 || x.Service == 2 || x.Price == 2).Count();
                //int count_One = RatingDetails.Where(x => x.OverallExperience == 1 || x.Communication == 1 || x.Service == 1 || x.Price == 1).Count();

                int count_five = RatingDetails.Where(x => x.OverallExperience == 5).Count();
                int count_four = RatingDetails.Where(x => x.OverallExperience == 4).Count();
                int count_Three = RatingDetails.Where(x => x.OverallExperience == 3).Count();
                int count_Two = RatingDetails.Where(x => x.OverallExperience == 2).Count();
                int count_One = RatingDetails.Where(x => x.OverallExperience == 1).Count();

                carrierProfile.TotalNumberOfRatings = RatingDetails.Count;

                double ovalue = Convert.ToDouble((5 * count_five + 4 * count_four + 3 * count_Three + 2 * count_Two + 1 * count_One) / (count_five + count_four + count_Three + count_Two + count_One));
                double percent = ((ovalue / 5) * 100);

                OverallExperience = (int)Math.Round(ovalue, MidpointRounding.AwayFromZero);
                var OverAllPercent = percent;
            }
            carrierProfile.CarrierRatings = OverallExperience;

            ViewBag.AppUrl = _appSettings.BaseUrl;
            ViewBag.ImageContainerPath = _appSettings.ImageBlobContainerPath + _appSettings.CompanyLogoContainer;
            return View(carrierProfile);
        }

        [HttpPost]
        [ActionName(nameof(Upload))]
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
                        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_appSettings.StorageConnectionString);
                        var container = _iBlobService.GetImageBlobContainer(_appSettings.CompanyLogoContainer, _appSettings.StorageConnectionString);
                        string imageName = Guid.NewGuid().ToString() + fileExtension;
                        _iBlobService.SaveImageToBlob(container, fileBytes, imageName);
                        imageName = _appSettings.ImageBlobContainerPath + _appSettings.CompanyLogoContainer + "/" + imageName;
                        var userImage = new UserImageModel()
                        {
                            ImageUri = imageName,
                            bytes = fileBytes,
                            Extension = fileExtension,
                            size = file.Length,
                            ContentType = file.ContentType
                        };

                        var carrier = _dbService.GetSingle<Carrier>(v => v.CarrierGuid == currentUser.CarrierGuid);
                        if (carrier != null)
                        {
                            var objImage = _dbService.GetSingle<CompanyLogo>(v => v.CarrierGuid == carrier.CarrierGuid && v.isDeleted == false);
                            if (objImage != null)
                            {
                                objImage.isDeleted = true;
                                _dbService.Update(objImage);
                            }
                        }

                        var imageDB = _autoMapper.Map<UserImageModel, CompanyLogo>(userImage);

                        imageDB.CarrierGuid = carrier.CarrierGuid;
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

        [HttpPost]
        [Route("Save")]
        [Authorize(Roles = RoleType.CARRIER_ADMIN)]
        public async Task<IActionResult> Save(CarrierProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(HttpContext.User);
                var carrier = _dbService.GetSingle<Carrier>(v => v.CarrierGuid == currentUser.CarrierGuid);
                var carrierCountry = _dbService.GetSingle<Country>(v => v.Id == Int32.Parse(model.Country));
                var carrierState = _dbService.GetSingle<ProvinceState>(v => v.Id == Int32.Parse(model.State));

                carrier.AddressLine1 = model.AddressLine1;
                carrier.AddressLine2 = model.AddressLine2;
                carrier.Country = carrierCountry.Name;
                carrier.State = carrierState.Name;
                carrier.CompanyBio = model.CompanyBio;
                carrier.CarrierShippingItems = model.CarrierShippingItems.Remove(model.CarrierShippingItems.Length - 1);
                carrier.MCDotNumber = model.MCDotNumber;
                carrier.SiteUrl = model.SiteUrl;
                carrier.PostCode = model.PostCode;
                carrier.City = model.City;
                carrier.PhoneNumber = model.PhoneNumber;
                carrier.ContactPerson = model.ContactPerson;

                var bankDetails = _dbService.GetSingle<CarrierBankingDetails>(v => v.CarrierBankingDetailsId == carrier.CarrierBankingDetailsId);
                if (bankDetails != null)
                {
                    bankDetails.AccountName = model.AccountName;
                    bankDetails.AccountNumber = model.BankAccountNumber;
                    bankDetails.BankIdentificationNumber = model.BankIdentificationNumber;
                    _dbService.Update(bankDetails);
                }
                else
                {
                    var newBankDetails = new CarrierBankingDetails()
                    {
                        AccountName = model.AccountName,
                        AccountNumber = model.BankAccountNumber,
                        BankIdentificationNumber = model.BankIdentificationNumber,
                    };
                    
                    _dbService.Add(newBankDetails);

                    carrier.CarrierBankingDetailsId = newBankDetails.CarrierBankingDetailsId;
                }

                _dbService.Update(carrier);

                var result = await _userManager.UpdateAsync(currentUser);
                if (result.Succeeded)
                {
                    return Json(new { result = "Redirect", url = Url.Action("Index", "Dashboard") });
                    //return RedirectToAction(nameof(Index));
                }
                AddErrors(result);

            }
            return View(model);
        }

        /// <summary>
        /// To Fetch the Carrier Ratings List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUserRatingsList")]
        public async Task<ContentResult> GetUserRatingsList(string id)
        {
            var data = string.Empty;
            try
            {
                var currentUser = await _userManager.GetUserAsync(HttpContext.User);

                string errorMessage = string.Empty;
                string uri = string.Empty;
                var urlParamenters = new Dictionary<string, string>();
                var parameters = GetParameters(new List<string>() { "" });

                var RatingDetails = _dbService.Get<Rating>(v => v.CarrierID.ToString() == id && v.IsDeleted == false).ToList();

                var tempRatings = (from rat in _context.Ratings
                                   join carr in _context.Carriers on rat.CarrierID equals carr.CarrierGuid
                                   join user in _context.Users on rat.UserId equals user.Id
                                   join temp in _context.ProfileImages on rat.UserId equals temp.UserId
                                   into profgrp
                                   from prof in profgrp.DefaultIfEmpty()
                                   where (rat.CarrierID.ToString() == id && rat.IsDeleted == false && prof.isDeleted == false)
                                   select new UserRatingsViewModel
                                   {
                                       CarrirerName = carr.BusinessName,
                                       OverallExperience = rat.OverallExperience,
                                       Service = rat.Service,
                                       Communication = rat.Communication,
                                       Price = rat.Price,
                                       UserName = user.UserName,
                                       RatingAddedOn = rat.RatingAddedOn,
                                       RatingID = rat.RatingId,
                                       ImageUrl = prof.ImageUri//prof.ImageUri
                                   });

                int count_five = tempRatings.Where(x => x.OverallExperience == 5 || x.Communication == 5 || x.Service == 5 || x.Price == 5).Count();
                int count_four = tempRatings.Where(x => x.OverallExperience == 4 || x.Communication == 4 || x.Service == 4 || x.Price == 4).Count();
                int count_Three = tempRatings.Where(x => x.OverallExperience == 3 || x.Communication == 3 || x.Service == 3 || x.Price == 3).Count();
                int count_Two = tempRatings.Where(x => x.OverallExperience == 2 || x.Communication == 2 || x.Service == 2 || x.Price == 2).Count();
                int count_One = tempRatings.Where(x => x.OverallExperience == 1 || x.Communication == 1 || x.Service == 1 || x.Price == 1).Count();

                int totalcount = count_five + count_four + count_Three + count_Two + count_One;

                if (totalcount != 0)
                {
                    double ovalue = Convert.ToDouble((5 * count_five + 4 * count_four + 3 * count_Three + 2 * count_Two + 1 * count_One) / (count_five + count_four + count_Three + count_Two + count_One));
                    double percent = ((ovalue / 5) * 100);

                    var OverallExperience = (int)Math.Round(ovalue, MidpointRounding.AwayFromZero);
                    var OverAllPercent = percent;
                }

                #region Filteration

                if (parameters.ContainsKey("Value") && parameters["Value"] != null && parameters["Value"] != string.Empty)
                {
                    var value = parameters["Value"].ToString();
                }

                var total = tempRatings.Count();

                #endregion Filteration

                #region Pagination and OutPut

                var pageNo = Convert.ToInt32(parameters["page_number"]);
                var pageSize = Convert.ToInt32(parameters["results_per_page"]);
                var CompletedRatings = tempRatings.Skip((pageNo - 1) * (pageSize)).Take(pageSize).ToList();

                #endregion Pagination and OutPut

                var res = from model in CompletedRatings
                          select new string[]
                             {
                                     model.CarrirerName,
                                     model.OverallExperience.ToString(),
                                     model.Service.ToString(),
                                     model.Communication.ToString(),
                                     model.Price.ToString(),
                                     model.RatingID.ToString(),
                                     model.UserName,
                                     model.RatingAddedOn.Value.ToString("yyyy/MM/dd"),
                                     model.ImageUrl
                             };

                data = CommonService.Json.Serialize(new { iTotalRecords = res.Count(), iTotalDisplayRecords = res.Count(), aaData = res });
            }
            catch (Exception ex)
            {
                data = CommonService.Json.Serialize(new { iTotalRecords = 0, iTotalDisplayRecords = 0, aaData = new string[] { } });
            }
            return Content(data, "application/json");
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