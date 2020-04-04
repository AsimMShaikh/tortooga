using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace TortoogaApp.ViewComponents
{
    public class LoginModal : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var vm = new ViewModels.AccountViewModels.LoginViewModel()
            {
                ReturnUrl = HttpContext.Request.Path + HttpContext.Request.QueryString
            };

            return View("Default", vm);
        }
    }
}