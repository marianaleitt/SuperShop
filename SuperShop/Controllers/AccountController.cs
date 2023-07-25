using Microsoft.AspNetCore.Mvc;
using SuperShop.Helpers;

namespace SuperShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;

        public AccountController(IUserHelper userHelper)
        {
            _userHelper = userHelper;
        }

        public IActionResult Login()
        {
            if(User.Identity.IsAuthenticated) 
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
    }
}
