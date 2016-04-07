
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using System.Web;
using System.Web.Script.Serialization;
using PacificBBExtranet.Services.Models.Account;
// Application 

namespace PacificBBExtranet.Web.Helpers
{
    public static class FormsAuthHelper
    {
        public class FormsAuthException : ApplicationException
        {
            public FormsAuthException(string message, Exception innerException)
                : base(message, innerException) { }
        }

        const int _timeoutMinutes = 120;

        public static bool RefreshContextUser(
            HttpContextBase context)
        {
            try
            {
                var principalModel = FormsAuthHelper.GetCurrentPrincipalUser(context.Request);

                if (principalModel != null)
                {
                    var authTicket = FormsAuthHelper.GetCurrentAuthTicket(context.Request);

                    context.User = new CustomPrincipal(authTicket, principalModel);

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new FormsAuthException(ex.Message, ex);
            }
        }

        public static void UpdateContextUser(
            HttpContextBase context,
            PrincipalModel principalModel)
        {
            try
            {
                var existingTicket = FormsAuthHelper.GetCurrentAuthTicket(context.Request);

                var serializer = new JavaScriptSerializer();

                var userData = serializer.Serialize(principalModel);

                var newAuthTicket = new FormsAuthenticationTicket(
                     existingTicket.Version,
                     existingTicket.Name,
                     existingTicket.IssueDate,
                     existingTicket.Expiration,
                     existingTicket.IsPersistent,
                     userData);

                context.User = new CustomPrincipal(newAuthTicket, principalModel);

                var cookie = CreateCookie(newAuthTicket);

                context.Response.Cookies.Add(cookie);
            }
            catch (Exception ex)
            {
                throw new FormsAuthException(ex.Message, ex);
            }
        }

        public static void SetContextUser(
            HttpContextBase context,
            bool keepSignedIn,
            string userName,
            PrincipalModel principalModel)
        {
            try
            {
                var serializer = new JavaScriptSerializer();

                var userData = serializer.Serialize(principalModel);

                var expire = DateTime.Now.AddMinutes(_timeoutMinutes);

                FormsAuthenticationTicket authTicket =
                    keepSignedIn ?
                    new FormsAuthenticationTicket(1, userName, DateTime.Now, expire, true, userData) :
                    new FormsAuthenticationTicket(1, userName, DateTime.Now, expire, false, userData);

                HttpCookie authCookie = CreateCookie(authTicket);

                context.Response.Cookies.Add(authCookie);
            }
            catch (Exception ex)
            {
                throw new FormsAuthException(ex.Message, ex);
            }
        }

        public static PrincipalModel GetCurrentPrincipalUser(
            HttpRequestBase request)
        {
            try
            {
                var authCookie = request.Cookies[FormsAuthentication.FormsCookieName];

                if (authCookie != null)
                {
                    var authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                    var serializer = new JavaScriptSerializer();

                    return serializer.Deserialize<PrincipalModel>(authTicket.UserData);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new FormsAuthException(ex.Message, ex);
            }
        }

        // Private Helpers

        private static FormsAuthenticationTicket GetCurrentAuthTicket(
            HttpRequestBase request)
        {
            var authCookie = request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie != null)
            {
                return FormsAuthentication.Decrypt(authCookie.Value);
            }

            return null;
        }

        private static HttpCookie CreateCookie(FormsAuthenticationTicket ticket)
        {
            var encTicket = FormsAuthentication.Encrypt(ticket);

            return new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
        }
    }
}