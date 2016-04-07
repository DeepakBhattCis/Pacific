using PacificBBExtranet.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace PacificBBExtranet.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ModelBinders.Binders.Add(typeof(DateTime), new DateTimeBinder());
            ModelBinders.Binders.Add(typeof(DateTime?), new DateTimeBinder());
        }

        /// <summary>
        /// To Override the default HttpContext User with our custom User class once the login has be authenticated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            // becuase we redirect to logout if failure, 
            // we dont want to keep authenticating requests.
            if (Response.IsRequestBeingRedirected)
            {
                return;
            }

            bool success = false;

            try
            {
                success = FormsAuthHelper.RefreshContextUser(
                    new HttpContextWrapper(HttpContext.Current));

            }
            catch (FormsAuthHelper.FormsAuthException ex)
            {
                // trap, log, supress
                // we dont want to be blasted w/ emails when a users cookie is corrupt
                //log.Error(ex);
            }
            finally
            {
                /* if (!success)
                 {
                     WebSecurity.Logout();
                 }*/
                //  HttpContext.Current.

            }
        }


        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

            // Get the exception object.
            Exception exc = Server.GetLastError();

        }
    }





    }
