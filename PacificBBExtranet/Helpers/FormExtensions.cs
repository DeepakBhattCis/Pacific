#region ﻿Copyright (c) 2015 The Capital Consulting Group LLC. All rights reserved.

/*
 * All information contained herein is, and remains
 * the property of The Capital Consulting Group LLC,
 * The intellectual and technical concepts contained
 * herein are proprietary to The Capital Consulting Group LLC
 * may be covered by U.S. and Foreign Patents,
 * patents in process, and are protected by trade secret or copyright law.
 * Dissemination, usage, reproduction, and/or modification of this material
 * is strictly forbidden unless prior written permission is obtained
 * from The Capital Consulting Group LLC.
 * 
 * Unauthorized copying of this file, via any medium is strictly prohibited.
 * Proprietary and confidential.
*/

#endregion // Copyright

// System
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Mvc.Ajax;
using System.Linq.Expressions;
using System.Web.Optimization;
using System.Web.Routing;
using PacificBBExtranet.Utils.ObjectExtensions;

namespace PacificBBExtranet.Web.Helpers
{
    public static class FormExtensions
    {
        public static string ControlNameFor<TModel, TProperty>(this HtmlHelper<TModel> helper, 
            Expression<Func<TModel, TProperty>> expression)
        {
            return ControlNameFor(expression);
        }

        public static string ControlNameFor<TModel, TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            return ExpressionHelper.GetExpressionText(expression);
        }

        public static MvcHtmlString HiddenFor<TModel, TProperty>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression, bool includeId)
        {
            if (includeId)
                return helper.HiddenFor(expression, new { id = helper.IdFor(expression) });
            else
                return helper.HiddenFor(expression);
        }

        /* public static MvcForm UcBeginDialogForm(this HtmlHelper helper,
            string actionName,
            string controllerName,
            string updateTargetId,
            bool closeOnSuccess = true,
            bool validationExcludePropertyErrors = true,
            string formId = null,
            bool disableAutoComplete = false)
         {
             return helper.UcBeginForm(
                 actionName, 
                 controllerName, 
                 updateTargetId, 
                 null, 
                 validationExcludePropertyErrors,
                 disableAutoComplete: disableAutoComplete, 
                 onComplete:closeOnSuccess ? 
                     ScriptHelper.Functions.HideModalFunction : null);
         }
         */


        public static MvcForm UcBeginForm(this HtmlHelper helper,
           string actionName,
           string controllerName,
           string updateTargetId,
           string formId = null,
           bool validationExcludePropertyErrors = true,
           bool disableAutoComplete = false,
           string loadingText = "Processing...")
        {
            return helper.UcBeginForm(
                actionName,
                controllerName,
                updateTargetId,
                null,
                validationExcludePropertyErrors,
                disableAutoComplete: disableAutoComplete,
                formId: formId);
        }

        //If you need to use this version you will need to set the routeValues parameter (null if you dont need it)
        //becasue if we set routeValues = null as default value here the call would be ambiguous with the above one.
        public static MvcForm UcBeginForm(this HtmlHelper helper,
            string actionName,
            string controllerName,
            string updateTargetId,
            object routeValues,
            bool validate = true,
            bool validationExcludePropertyErrors = true,
            string onSuccessScriptDelegate = null,
            string formId = null,
            string loadingElementId = null,
            string httpMethod = "POST",
            string cssForm = null,
            bool disableAutoComplete = false,
            string onComplete = null,
            bool showLoadingElementOnTargetID = true,
            IDictionary<string, object> additionalHtmlAttributes = null,
            string loadingText = "Processing...")
        {
            var ajaxHelper = ViewExtensions.CurrentAjaxHelper;

            if (string.IsNullOrWhiteSpace(formId))
                formId = Guid.NewGuid().ToString();

            var htmlAttributes = new RouteValueDictionary(
                new
                {
                    @class = cssForm == null ? helper.StyleFormHorizontal() : cssForm,
                    @role = "form",
                    @id = formId
                });

            if (disableAutoComplete)
                htmlAttributes.Add("autocomplete", "off");

            if (additionalHtmlAttributes != null)
            {
                foreach (var additionalAttr in additionalHtmlAttributes)
                {
                    htmlAttributes[additionalAttr.Key] = additionalAttr.Value;
                }
            }

            RouteValueDictionary newRouteValues;
            if (routeValues is RouteValueDictionary)
                newRouteValues = routeValues as RouteValueDictionary;
            else
            {
                newRouteValues = routeValues != null ?
                    new RouteValueDictionary(routeValues) :
                    new RouteValueDictionary();
            }

            //DialogExtensions.RememberDialogTarget(helper, newRouteValues);

            var form = ajaxHelper.BeginForm(
                actionName: actionName,
                controllerName: controllerName,
                routeValues: newRouteValues,
                ajaxOptions: new AjaxOptions()
                {
                    HttpMethod = httpMethod,
                    InsertionMode = InsertionMode.Replace,
                    UpdateTargetId = updateTargetId,
                    OnSuccess = onSuccessScriptDelegate,
                    LoadingElementId = loadingElementId,
                    OnComplete = onComplete,
                    OnBegin = showLoadingElementOnTargetID ? "AjaxHelper.SetLoadingOnTarget('#" + updateTargetId + "', '', '" + loadingText + "')" : null
                },
                htmlAttributes: htmlAttributes
            );

            var token = helper.AntiForgeryToken();

            helper.ViewContext.Writer.Write(token.ToString());

            if (validate)
            {
                var validation = helper.ValidationSummary(validationExcludePropertyErrors);

                if (validation != null)
                    helper.ViewContext.Writer.Write(validation.ToString());
            }

            if (helper.ViewContext.RequestContext.HttpContext.Request.IsAjaxRequest())
            {
                // client side validation only works on document.ready
                // so ajax rendered forms do not have client side validation
                // as a result, we add this setup script if its an ajax request only
                string setupValidationScript = "<script type='text/javascript'> " +
                        "$(function () { " +
                        "    $.validator.unobtrusive.parse('#" + formId + "'); " +
                        "}); " +
                    "</script>";

                helper.ViewContext.Writer.Write(setupValidationScript);
            }

            return form;
        }


        /* public static MvcForm UcBeginDialogForm(this HtmlHelper helper,
             string actionName,
             string controllerName,
             string updateTargetId,
             object routeValues,
             bool validate = true,
             bool validationExcludePropertyErrors = true,
             string onSuccessScriptDelegate = null,
             string formId = null,
             string loadingElementId = null,
             string httpMethod = "POST",
             string cssForm = null,
             bool disableAutoComplete = false,
             string onComplete = null)
         {
             return helper.UcBeginForm(actionName,controllerName,updateTargetId,routeValues,validate,
                 validationExcludePropertyErrors,onSuccessScriptDelegate,formId,loadingElementId,httpMethod,cssForm,
                 disableAutoComplete, onComplete: ScriptHelper.Functions.HideModalFunction);
         }*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="iconClass">fa fa-icon class for the prepend icon</param>
        /// <returns></returns>
        public static MvcHtmlString UcActionLinkIconButton(this HtmlHelper helper,
            string buttonText,string iconClass,string url,
            object htmlAttributes = null, string buttonsize = "btn-xs")
        {
            var attrs = htmlAttributes != null ? HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes) : null;
            return UcActionLinkIconButton(helper, buttonText, iconClass, url, attrs,buttonsize);
        }
        /*
        /// <summary>
        /// Generate a empty link with a css icon
        /// </summary>        
        /// <returns></returns>
        public static MvcHtmlString UcActionLinkIcon(this HtmlHelper helper,string iconClass="icon",string url="#",string anchorStyle=null)
        {
            return helper.UcActionLinkIconText(null, iconClass, url, anchorStyle);
        }*/

        public static MvcHtmlString UcActionLinkIconText(this HtmlHelper helper, string linktext,
            string iconClass = "icon", string url = "#", string anchorStyle = null)
        {
            var Url = new UrlHelper(helper.ViewContext.RequestContext);
            var a = new TagBuilder("a");
            
            var text = new TagBuilder("span") {InnerHtml = linktext};
            var icon = new TagBuilder("span");
            icon.AddCssClass(iconClass);

            a.InnerHtml = icon.ToString() +  (String.IsNullOrEmpty(linktext)?String.Empty:text.ToString());
            
            if (!String.IsNullOrEmpty(anchorStyle))
                a.Attributes.Add("style", anchorStyle);
            a.Attributes.Add("href", Url.Content(url));
            return new MvcHtmlString(a.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString UcActionLinkIconButton(this HtmlHelper helper, string buttonText, string iconClass, string url, IDictionary<string,object> htmlAttributes,string buttonsize="btn-xs")
        {
            var icontag = new TagBuilder("span");
            icontag.AddCssClass(iconClass);
            var actionLink = new TagBuilder("a");
            actionLink.AddCssClass("btn");
            actionLink.AddCssClass("btn-default");
            actionLink.AddCssClass(buttonsize);
            if(htmlAttributes!=null)
                actionLink.MergeAttributes(htmlAttributes);
            actionLink.Attributes["href"] = url;
            actionLink.InnerHtml = icontag.ToString(TagRenderMode.Normal) + " " + buttonText;
            return new MvcHtmlString(actionLink.ToString());
        }

        public static MvcHtmlString UcActionLinkIconButton(this AjaxHelper ajax, 
            string url, AjaxOptions options, string buttonContent,
            string iconStyle,object htmlattributes = null, 
            string buttonsize = "btn-xs")
        {
            HtmlHelper html = null;
            var seeicon = iconStyle;
            var ajaxattributes = options.ToUnobtrusiveHtmlAttributes();
            if (htmlattributes != null)
            {
                ajaxattributes.AddRange(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlattributes));
            }
            return UcActionLinkIconButton(html, buttonContent, seeicon, url, ajaxattributes, buttonsize);
        }


       /* public static MvcHtmlString GetScripSnippet(this HtmlHelper helper, Gritter gritter)
        {
            return helper.Partial("_gritterScript", gritter);
        }*/

        public static MvcHtmlString Tooltip(this HtmlHelper helper, MvcHtmlString content, string htmlTooltip,
            object htmlattributes = null)
        {
            return Tooltip(helper, content.ToHtmlString(), htmlTooltip, htmlattributes);
        }

        public static MvcHtmlString Tooltip(this HtmlHelper helper, string content, string htmlTooltip,object htmlattributes = null)
        {            
            var p = new TagBuilder("p"){InnerHtml = content};
            p.Attributes.Add("data-toggle","tooltip");
            p.Attributes.Add("title",htmlTooltip);
            p.Attributes.Add("data-placement", "top");
            if (htmlattributes != null)
            {
                p.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlattributes));
            }
            return new MvcHtmlString(p.ToString());
        
        }

        /*public static SelectList SelectListForTableDropDown(Dictionary<int,string> dic, int selectedValue)
        {
            var selectList = new SelectList(dic,);

        }*/
    }
}