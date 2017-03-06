using System;
using System.Configuration;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using App.Models.User;

namespace App.Utilities
{
    public class CookieManager
    {
        private static readonly string UserDetailsCookieName = ConfigurationManager.AppSettings["cookie:userdetails:name"];
        private static readonly DateTime Expiry = DateTime.Now.AddHours(6);
        public static void SetAuthCookie(string email, object obj, bool isPersistent = true)
        {
            var ticket = new FormsAuthenticationTicket(1, email, DateTime.Now, Expiry, false, string.Empty);
            HttpContext.Current.Response.Cookies.Set(new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket)) { Expires = Expiry });

            string userData = new JavaScriptSerializer().Serialize(obj);
            var cookie = new HttpCookie(UserDetailsCookieName) {Value = userData, Expires = Expiry};
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public static AuthenticatedUserModel GetAuthenticatedUserDetails()
        {
            var userData = HttpContext.Current.Request.Cookies[UserDetailsCookieName].Value;
            return new JavaScriptSerializer().Deserialize<AuthenticatedUserModel>(userData);
        }
    }
}