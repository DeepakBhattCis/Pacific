
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.WebPages;

namespace PacificBBExtranet.Web.Helpers
{
    public static class ViewExtensions
    {


        public static AjaxHelper CurrentAjaxHelper
        {
            get
            {
                return CurrentViewPage.Ajax;
            }
        }

        public static WebViewPage CurrentViewPage
        {
            get
            {
                return ((System.Web.Mvc.WebViewPage)WebPageContext.Current.Page);
            }
        }

        public static string CurrentController(this HtmlHelper helper)
        {
            return helper.ViewContext.RouteData.GetRequiredString("controller");
        }

        public static string Title(this HtmlHelper helper)
        {
            string viewTitle = string.Empty;

            if (!string.IsNullOrWhiteSpace(helper.ViewContext.Controller.ViewBag.ViewTitle))
            {
                viewTitle = helper.ViewContext.Controller.ViewBag.ViewTitle;
            }
            else if (!string.IsNullOrWhiteSpace(helper.ViewBag.ViewTitle))
            {
                viewTitle = helper.ViewBag.ViewTitle;
            }

            return string.Format("DPS | {0}",
                string.IsNullOrWhiteSpace(viewTitle) ? helper.CurrentController() : viewTitle);
        }

        public static MvcHtmlString ConditionalClasses(this HtmlHelper helper,bool condition, string classes)
        {
            return MvcHtmlString.Create(condition ? classes : String.Empty);
        }

        public static MvcHtmlString ConditionalString(this HtmlHelper helper, bool condition, string trueText,string falseText)
        {
            return MvcHtmlString.Create(condition ? trueText:falseText);
        }
    }


}