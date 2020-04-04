using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TortoogaApp.Messaging.Emails;
using TortoogaApp.Models;
using TortoogaApp.Security;
using TortoogaApp.Services;
using TortoogaApp.Utility;
using TortoogaApp.ViewModels;
using TortoogaApp.ViewModels.AccountViewModels;
using TortoogaApp.ViewModels.CarrierViewModels;
using TortoogaApp.ViewModels.UserViewModels;

namespace TortoogaApp.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailNotificationFactory _emailNotificationFactory;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;
        private IMapper _autoMapper;
        private IDbService _dbService;
        private readonly AppSettings _appSettings;
        private IBlobService _blobService;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailNotificationFactory emailNotificationFactory,
            ISmsSender smsSender,
            IMapper autoMapper,
            ILoggerFactory loggerFactory,
            IDbService dbService,
            IOptions<AppSettings> appSettings,
            IBlobService blobService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailNotificationFactory = emailNotificationFactory;
            _smsSender = smsSender;
            _autoMapper = autoMapper;
            _logger = loggerFactory.CreateLogger<AccountController>();
            _dbService = dbService;
            _appSettings = appSettings.Value;
            _blobService = blobService;
        }

        //
        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                //var isExist = _dbService.Get<ApplicationUser>(v => v.Email == model.Email).FirstOrDefault();



                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation(1, "User logged in.");
                    return RedirectToLocal(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning(2, "User account locked out.");
                    return View("Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/Register
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult RegisterCarrier(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            RegisterCarrierViewModel model = new RegisterCarrierViewModel();
            ViewBag.AppUrl = _appSettings.BaseUrl;
            ViewBag.Title = "Register Carrier";
            return View(model);
        }

        //
        // GET: /Account/Register
        [HttpGet]
        [AllowAnonymous]
        public IActionResult RegisterUser(string returnUrl = null)
        {
            RegisterUserViewModel model = new RegisterUserViewModel();
            //var countryList = _dbService.Get<Country>(null).ToList();
            //model.Countries = new List<SelectListItem>();
            //foreach (var item in countryList)
            //{
            //    model.Countries.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            //}

            ViewBag.AppUrl = _appSettings.BaseUrl;
            ViewBag.Title = "Register User";
            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                    // Send an email with this link
                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                    //await _emailSender.SendEmailAsync(model.Email, "Confirm your account",
                    //    $"Please confirm your account by clicking this link: <a href='{callbackUrl}'>link</a>");
                    //_emailNotificationFactory.SendSignUpCongratulatoryEmail(model);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation(3, "User created a new account with password.");
                    return RedirectToLocal(returnUrl);
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterUser(RegisterUserViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    BirthDate = model.BirthDate,
                    PhoneNumber = model.PhoneNumber,
                    MobileNumber = model.MobileNumber,
                    TimeZoneId = model.TimeZoneId,
                    AddressLine1 = model.AddressLine1,
                    AddressLine2 = model.AddressLine2,
                    PostCode = model.PostCode,
                    State = model.StateCode,
                    Suburb = model.Suburb,
                    Country = model.CountryCode,
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    _emailNotificationFactory.SendSignUpCongratulatoryEmail(model);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation(3, "User created a new account with password.");
                    return RedirectToLocal(returnUrl);
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterCarrier(RegisterCarrierViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (model.CountryCode.ToString() == "0" || model.StateCode.ToString() == "0")
            {
                ModelState.AddModelError(string.Empty, "Country State Required");
            }

            if (await _userManager.FindByEmailAsync(model.Email) != null)
            {
                ModelState.AddModelError(string.Empty, "Email exist, please choose another email");
            }

            if (ModelState.IsValid)
            {
                var carrier = new CarrierRegistration
                {
                    Email = model.Email,

                    ContactPerson = model.ContactPerson,
                    TempPassword = General.GetRandomAlphanumericString(8),
                    BusinessName = model.BusinessName,
                    MCDotNumber = model.MCDotNumber,
                    State = model.StateCode,
                    Country = model.CountryCode,
                    PhoneNumber = model.PhoneNumber,
                    Status = CarrierRegistrationStatus.Pending,
                    RequestDate = DateTime.UtcNow

                };
                _dbService.Add(carrier);
                _emailNotificationFactory.SendThankYouMailToCarrier(model);
                _emailNotificationFactory.SendCarrierSignUpEmail(model);
                _logger.LogInformation(3, "Carrier created a new account.");
                //return RedirectToLocal(returnUrl);
                return Json(new { result = "Success" , url = Url.Action("Login", "Account") });
                
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public ContentResult GetAllStatesByCountryCode(int code)
        {
            var stateList = _dbService.Get<ProvinceState>(v => v.Country.Id == code).ToList();
            var ListStates = new List<SelectListItem>();
            foreach (var item in stateList)
            {
                ListStates.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }
            return Content(JsonConvert.SerializeObject(ListStates), "application/json");
        }

        [HttpGet]
        [AllowAnonymous]
        public ContentResult GetAllCountries()
        {
            var countryList = _dbService.Get<Country>(null).ToList();
            var ListCountries = new List<SelectListItem>();
            foreach (var item in countryList)
            {
                ListCountries.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }
            return Content(JsonConvert.SerializeObject(ListCountries), "application/json");
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation(4, "User logged out.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        //
        // GET: /Account/ExternalLoginCallback
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                return View(nameof(Login));
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (result.Succeeded)
            {
                _logger.LogInformation(5, "User logged in with {Name} provider.", info.LoginProvider);
                return RedirectToLocal(returnUrl);
            }
            if (result.RequiresTwoFactor)
            {
                return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl });
            }
            if (result.IsLockedOut)
            {
                return View("Lockout");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["LoginProvider"] = info.LoginProvider;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        _logger.LogInformation(6, "User created an account using {Name} provider.", info.LoginProvider);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }

        // GET: /Account/ConfirmEmail
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                // Send an email with this link
                //var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                //var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                //await _emailSender.SendEmailAsync(model.Email, "Reset Password",
                //   $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");
                //return View("ForgotPasswordConfirmation");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(AccountController.ResetPasswordConfirmation), "Account");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(AccountController.ResetPasswordConfirmation), "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/SendCode
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl = null, bool rememberMe = false)
        {
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            var userFactors = await _userManager.GetValidTwoFactorProvidersAsync(user);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return View("Error");
            }

            // Generate the token and send it
            var code = await _userManager.GenerateTwoFactorTokenAsync(user, model.SelectedProvider);
            if (string.IsNullOrWhiteSpace(code))
            {
                return View("Error");
            }

            var message = "Your security code is: " + code;
            if (model.SelectedProvider == "Email")
            {
                _emailNotificationFactory.SendUserSecurityCode(await _userManager.GetEmailAsync(user), code);
            }
            else if (model.SelectedProvider == "Phone")
            {
                await _smsSender.SendSmsAsync(await _userManager.GetPhoneNumberAsync(user), message);
            }

            return RedirectToAction(nameof(VerifyCode), new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/VerifyCode
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyCode(string provider, bool rememberMe, string returnUrl = null)
        {
            // Require that the user has already logged in via username/password or external login
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes.
            // If a user enters incorrect codes for a specified amount of time then the user account
            // will be locked out for a specified amount of time.
            var result = await _signInManager.TwoFactorSignInAsync(model.Provider, model.Code, model.RememberMe, model.RememberBrowser);
            if (result.Succeeded)
            {
                return RedirectToLocal(model.ReturnUrl);
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning(7, "User account locked out.");
                return View("Lockout");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid code.");
                return View(model);
            }
        }      

        [HttpPost]
        [AllowAnonymous]
        public async Task<string> Upload()
        {
            
            var files = Request.Form.Files;
            foreach (var file in files)
            {
                if (file.Length > 0)
                {                    
                    var parsedContent = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
                    var fileName = file.FileName;

                    string fileExtension = Path.GetExtension(fileName);
                    string targetFolder = Path.Combine(_appSettings.ContentRootPath, @"wwwroot\images\CompanyLogo\");
                    var filePath = targetFolder + fileName;
                                  
                    using (var fileStream = file.OpenReadStream())
                    using (var ms = new MemoryStream())
                    {
                        fileStream.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        try
                        {
                            FileStream fileObj = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                            ms.WriteTo(fileObj);
                            fileObj.Flush();                        
                            
                        }
                        catch(Exception ex)
                        {

                        }
                        
                     
                    }
                }
            }
            
            return null;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult SetUpCarrierProfile(string id)
        {
            var result = _dbService.GetSingle<CarrierRegistration>(v => v.CarrierRegistrationGuid.ToString() == id);
            var viewModel = new CarrierProfileSetupViewModel();
            if (result != null)
            {
                if (result.Status == CarrierRegistrationStatus.Completed)
                {
                    return RedirectToAction(nameof(Login));
                }
                //TODO: Need to handle Pending and Rejected application by directing them to some explaination page
                viewModel.Email = result.Email;
                viewModel.CarrierRegistrationGuid = result.CarrierRegistrationGuid;
                viewModel.BusinessName = result.BusinessName;
            }

            return View(viewModel);
        }
       

        [HttpGet]
        [AllowAnonymous]
        public ContentResult ValidateCarrierCredentials()
        {
            var result = _dbService.GetSingle<CarrierRegistration>(v => v.CarrierRegistrationGuid.ToString() == "64dd8b65-6b29-442e-8ce8-08d4348c5dc6");
            var viewModel = new CarrierProfileSetupViewModel();
            if (result != null)
            {
                viewModel.Email = result.Email;
            }
            ViewBag.BusinessName = result.BusinessName;
            return null;
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetUpCarrierProfile(CarrierProfileSetupViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var result = _dbService.GetSingle<CarrierRegistration>(v => v.Email == model.Email && v.Status == CarrierRegistrationStatus.Approved && v.CarrierRegistrationGuid == model.CarrierRegistrationGuid);
                if (result != null)
                {
                    var countryState = _dbService.GetSingle<Country>(v => v.Id == Int32.Parse(result.Country), x => x.ProvinceStates);
                    if (result.TempPassword == model.OldPassword)
                    {
                        var bankDetails = new CarrierBankingDetails
                        {
                            BankIdentificationNumber = model.BankIdentificationNumber,
                            AccountName = model.BankAccountName,
                            AccountNumber = model.BankAccountNumber
                        };
                        _dbService.Add(bankDetails);

                        var carrier = new Carrier
                        {
                            Email = model.Email,
                            ContactPerson = result.ContactPerson,
                            BusinessName = result.BusinessName,
                            MCDotNumber = result.MCDotNumber,
                            State = countryState.ProvinceStates.FirstOrDefault(v => v.Id == Int32.Parse(result.State)).Name,
                            Country = countryState.Name,
                            PhoneNumber = result.PhoneNumber,
                            CompanyBio = model.CompanyBio,
                            CarrierShippingItems = model.CarrierShippingItems.Remove(model.CarrierShippingItems.Length - 1),
                            AccountNumber = General.GetRandomAlphanumericString(6),
                            AddressLine1 = model.AddressLine1,
                            AddressLine2 = model.AddressLine2,
                            City = model.City,
                            PostCode = model.PostCode,
                            SiteUrl = model.SiteUrl,
                            CarrierBankingDetailsId = bankDetails.CarrierBankingDetailsId
                        };
                        _dbService.Add(carrier);

                        if (model.CompanyLogoImage != null)
                        {

                            string targetFolder = Path.Combine(_appSettings.ContentRootPath, @"wwwroot\images\CompanyLogo\");
                            var filePath = targetFolder + model.CompanyLogoImage;
                            var fileName = Path.GetFileName(filePath);
                            var fileExtension = Path.GetExtension(fileName);

                            try
                            {
                                byte[] buff = null;
                                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                                BinaryReader br = new BinaryReader(fs);
                                long numBytes = new FileInfo(filePath).Length;
                                buff = br.ReadBytes((int)numBytes);


                                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_appSettings.StorageConnectionString);
                                var container = _blobService.GetImageBlobContainer(_appSettings.CompanyLogoContainer, _appSettings.StorageConnectionString);
                                string imageName = Guid.NewGuid().ToString() + fileExtension;
                                _blobService.SaveImageToBlob(container, buff, imageName);
                                imageName = _appSettings.ImageBlobContainerPath + _appSettings.CompanyLogoContainer + "/" + imageName;
                                var userImage = new UserImageModel()
                                {
                                    ImageUri = imageName,
                                    bytes = buff,
                                    Extension = fileExtension,
                                    size = numBytes,
                                    ContentType = ""
                                };


                                var imageDB = _autoMapper.Map<UserImageModel, CompanyLogo>(userImage);

                                imageDB.CarrierGuid = carrier.CarrierGuid;
                                imageDB.isDeleted = false;

                                _dbService.Add(imageDB);

                                if (imageDB.ImageGuid == Guid.Empty)
                                {
                                    throw new Exception("Db Error");
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }

                        var user = new ApplicationUser()
                        {
                            Email = model.Email,
                            UserName = model.Email,
                            CarrierGuid = carrier.CarrierGuid
                        };
                        var userResult = await _userManager.CreateAsync(user, model.Password);

                        if (userResult.Succeeded)
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            await _userManager.AddToRoleAsync(user, RoleType.CARRIER_ADMIN);
                            _emailNotificationFactory.SendCarrierRegistrationSuccessEmail(model);
                           
                            result.Status = CarrierRegistrationStatus.Completed;
                            _dbService.Update(result);
                            _logger.LogInformation(3, "Carrier created a new account with password.");
                            
                            return Json(new { result = "Redirect", url = Url.Action("Index", "Dashboard") });
                            //return RedirectToAction(nameof(Index));
                            
                           //return RedirectToLocal(returnUrl);
                        }
                        else
                        {
                            throw new Exception("Carrier registration failed");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid password");
                        return View("SetUpCarrierProfile", model);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "No record found with given Email ID or your request has not been approved.");
                    return View("SetUpCarrierProfile", model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View("SetUpCarrierProfile", model);
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion Helpers
    }
}