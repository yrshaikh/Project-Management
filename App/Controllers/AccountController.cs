using System.Threading.Tasks;
using System.Web.Mvc;
using App.Models;
using App.Services;

namespace App.Controllers
{
    // http://stackoverflow.com/questions/18594316/custom-authentication-and-asp-net-mvc
    // http://stackoverflow.com/questions/7217105/how-can-i-manually-create-a-authentication-cookie-instead-of-the-default-method
    [Authorize]
    public class AccountController : Controller
    {
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ModelState.AddModelError("", "Invalid login attempt.");
            return View(model);
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserService service = new UserService();
                if (!service.IsEmailRegistered(model.Email))
                {
                    service.Register(model);
                    // create auth cookie and log him in
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("Email", "Email address is already taken.");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        
        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
        #endregion
    }
}