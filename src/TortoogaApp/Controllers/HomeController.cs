using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TortoogaApp.Security;

namespace TortoogaApp.Controllers
{
    public class HomeController : Controller
    {
        private UserManager<ApplicationUser> _userManager;

        public HomeController(UserManager<ApplicationUser> userManager)
        {
            this._userManager = userManager;
        }

        public IActionResult Index()
        {

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        //[Authorize]
        //public IActionResult Dashboard()
        //{
            
        //    if (User.IsInRole(RoleType.CARRIER_ADMIN))
        //    {
        //        return View("AdminDashboard");
        //    }

            
        //    return View("UserDashboard");
        //}
    }
}