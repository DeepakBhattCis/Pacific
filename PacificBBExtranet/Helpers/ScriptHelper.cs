
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Optimization;
using PacificBBExtranet.Web.UserControls;
using Microsoft.SqlServer.Server;
using PacificBBExtranet.Web.UserControls;

namespace PacificBBExtranet.Web.Helpers
{
    public static class ScriptHelper
    {
        public static class MarkupIDs
        {
            public const string AjaxLoadingOverlay = "ajaxLoadingOverlay";

        }


        public static class Functions
        {
            public const string HideModalFunction = "HideModal";
            public const string RefreshIssues = "RefreshIssuesGrid";//OK
            public const string HideLoadingOverlay = "HideLoadingOverlay";
        }

        //public static class SharedAjaxOptions
        //{
        //    /////////////////////////////////////
        //    public static AjaxOptions AjaxMessageIssuesOptions = new AjaxOptions()
        //    {
        //        InsertionMode = InsertionMode.Replace,
        //        UpdateTargetId = ScriptHelper.Panels.MessageTreePanel,
        //        LoadingElementId = ScriptHelper.MarkupIDs.AjaxLoadingOverlay,
        //        OnSuccess = ScriptHelper.Functions.HideModalFunction
        //    };
        //}

        #region Script Controls

        /// <summary>
        /// 
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="actionUri"></param>
        /// <param name="containerId">Container to be reloaded</param>
        /// <param name="filterControlIds"></param>
        /// <param name="containerDataAttribute">The Attribue name set in the Container.  The content of this attribute will be added to the URL query string</param>
        /// <param name="loadingText"></param>
        /// <returns></returns>
        public static MvcHtmlString UcScriptContentLoader(this HtmlHelper helper,
            string actionUri, string containerId, string[] filterControlIds = null, string containerDataAttribute = null,
            string loadingText = null,
            bool loadingTargetMini = false,
            int setdelay = 0)
        {
            var model = new ScriptContentLoaderModel()
            {
                ActionUri = actionUri,
                ContainerId = containerId,
                FilterControlIDs = filterControlIds,
                LoadingText = loadingText ?? string.Empty,
                LoadingTargetMini = loadingTargetMini,
                Delay = setdelay
            };

            return helper.UserControlPartialInternal("_ScriptContentLoader", model);
        }

        public static IHtmlString RenderDashboardBundle()
        {
            return Scripts.Render("~/Scripts/modules/dashboard");
        }

        public static IHtmlString RenderDocumentsBundle()
        {
            return Scripts.Render("~/Scripts/modules/documents");
        }

        public static IHtmlString RenderScriptAccount()
        {
            return Scripts.Render("~/Scripts/modules/account");
        }

        public static IHtmlString RenderScriptFileUpload()
        {
            return Scripts.Render("~/Scripts/fileupload");
        }

        internal static MvcHtmlString ScriptPartialInternal(this HtmlHelper helper, string viewName, object model)
        {
            return helper.Partial(ScriptPartialPathInternal(viewName), model);
        }
        private static string ScriptPartialPathInternal(string viewName)
        {
            return string.Format("~/Views/Scripts/{0}.cshtml", viewName);
        }

        public static IHtmlString RenderJsTreeScriptAndStyle()
        {
            var script = Scripts.Render("~/Scripts/treeview");

            var style = Styles.Render("~/Content/themes/jsTree/default/style.css");

            return MvcHtmlString.Create(script.ToString() + style.ToString());
        }
        #endregion

        public static int GetDayOfWeek(this HtmlHelper helper,DateTime date)
        {
            var dayofTheWeek = date.DayOfWeek;
            switch (dayofTheWeek)
            {
                case DayOfWeek.Monday:
                    return 1;
                case DayOfWeek.Tuesday:
                    return 2;
                case DayOfWeek.Wednesday:
                    return 3;
                case DayOfWeek.Thursday:
                    return 4;
                case DayOfWeek.Friday:
                    return 5;
                case DayOfWeek.Saturday:
                    return 6;
                case DayOfWeek.Sunday:
                    return 7;
                default:
                    return 0;
            }
        }

        public static int GetWeekToStart(this HtmlHelper helper,int day)
        {
            return (day / 7 )+ 1;
        }
    }
}