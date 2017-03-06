using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using App.Models;
using App.Models.User;
using App.Services;
using App.Utilities;

namespace App.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        [HttpGet]
        [Route("signin")]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [Route("signin")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (_userService.Authenticate(model.Email, model.Password))
            {
                AuthenticatedUserModel authenticatedUser = _userService.GetUserDetails(model.Email);
                CookieManager.SetAuthCookie(model.Email, authenticatedUser, model.RememberMe);
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid login attempt.");
            return View(model);
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        [Route("signup")]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [Route("signup")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!_userService.IsEmailRegistered(model.Email))
                {
                    _userService.Register(model);
                    AuthenticatedUserModel authenticatedUser = _userService.GetUserDetails(model.Email);

                    CookieManager.SetAuthCookie(model.Email, authenticatedUser);
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

        [Route("signout")]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}