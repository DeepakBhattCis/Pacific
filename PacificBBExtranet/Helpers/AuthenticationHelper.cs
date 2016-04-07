using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Security.Principal;
using PacificBBExtranet.Services.Models.Account;

//Application

namespace PacificBBExtranet.Web.Helpers
{
    public static class AuthenticationHelper
    {
        public static CustomPrincipal CurrentUser()
        {
            return System.Web.HttpContext.Current.User as CustomPrincipal;
        }

        public static CustomPrincipal CurrentUser(this HtmlHelper helper)
        {
            return helper.ViewContext.HttpContext.User as CustomPrincipal;
        }

        public static IIdentity CurrentUserIdentity()
        {
            return System.Web.HttpContext.Current.User.Identity;
        }

        public static IIdentity CurrentUserIdentity(this HtmlHelper helper)
        {
            return helper.ViewContext.HttpContext.User.Identity;
        }

        public static string CurrentUserName()
        {
            return System.Web.HttpContext.Current.User.Identity.Name;
        }

        public static int CurrentUserId()
        {
            var customPrincipal = HttpContext.Current.User as CustomPrincipal;
                if (customPrincipal != null)
                    return customPrincipal.UserId;
            return -1;
        }

        public static int CurrenResortID()
        {
            var customPrincipal = HttpContext.Current.User as CustomPrincipal;
            if (customPrincipal != null)
                return customPrincipal.ResortID;
            return -1;
        }

        public static int CurrentUserId(this HtmlHelper helper)
        {
            return helper.CurrentUser().UserId;
        }

        public static string CurrentUserFirstName()
        {
            var user = CurrentUser();

            if (user == null)
                return string.Empty;

            return user.FirstName;
        }

        public static bool CurrentUserIsAuthenticated(this HtmlHelper helper)
        {
            return helper.CurrentUserIdentity().IsAuthenticated;
        }
        

        public static string ErrorCodeToString(this MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        
    }
}