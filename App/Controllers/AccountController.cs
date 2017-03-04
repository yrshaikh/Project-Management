using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using App.Models;
using App.Services;

namespace App.Controllers
{
    // http://stackoverflow.com/questions/18594316/custom-authentication-and-asp-net-mvc
    // http://stackoverflow.com/questions/7217105/how-can-i-manually-create-a-authentication-cookie-instead-of-the-default-method
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
                SetAuthCookie(model.Email);
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
                    SetAuthCookie(model.Email);
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

        public void SetAuthCookie(string email, bool isPersistent=true)
        {
            string userData = "";

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
              1,                                     // ticket version
              email,                              // authenticated username
              DateTime.Now,                          // issueDate
              DateTime.Now.AddMinutes(30),           // expiryDate
              isPersistent,                          // true to persist across browser sessions
              userData,                              // can be used to store additional user data
              FormsAuthentication.FormsCookiePath);  // the path for the cookie

            // Encrypt the ticket using the machine key
            string encryptedTicket = FormsAuthentication.Encrypt(ticket);

            // Add the cookie to the request to save it
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            cookie.HttpOnly = true;
            Response.Cookies.Add(cookie);
        }

        [Route("signout")]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}