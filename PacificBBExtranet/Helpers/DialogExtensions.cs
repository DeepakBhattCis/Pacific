
using PacificBBExtranet.Services.Models;
using PacificBBExtranet.Web.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace PacificBBExtranet.Web.Helpers
{
    public static class DialogExtensions
    {

        private static readonly Lazy<JavaScriptSerializer> _javascriptSerializer =
            new Lazy<JavaScriptSerializer>(() => new JavaScriptSerializer());

        private static JavaScriptSerializer javaScriptSerializer { get { return _javascriptSerializer.Value; } }

        public static MvcHtmlString UcSpanTitle(this HtmlHelper helper,MvcHtmlString image,string text)
        {
            const string textformat = "{0} <span>{1}</span>";
            return new MvcHtmlString(String.Format(textformat, image.ToHtmlString(), text));
        }

        public static MvcHtmlString UcSpanTitle(this HtmlHelper helper,string boldtext,string text)
        {
            const string textformat = "<span><strong>{0}</strong><span>{1}</span></span>";
            return new MvcHtmlString(String.Format(textformat,boldtext,text));
        }

        public static MvcHtmlString DialogLinkButton(this HtmlHelper helper,
            string actionUrl, string linkText, string dialogTitle, string iconCss,
            DialogScriptTargetModel targetModel, byte? widthPercent = null, string btnCssSize = "btn-xs", string controlID = null)
        {
            return helper.DialogLink(
                AppendDialogTargetInternal(actionUrl, targetModel),
                linkText, dialogTitle, iconCss, widthPercent, "btn btn-default " + btnCssSize, controlID: controlID);
        }

        public static MvcHtmlString DialogLinkButton(this HtmlHelper helper,
            string actionUrl, string linkText, string dialogTitle, string iconCss,
            DialogTargetModel targetModel, byte? widthPercent = null, string btnCssSize = "btn-xs", string btnClasses = null)
        {
            return helper.DialogLink(
                AppendDialogTargetInternal(actionUrl, targetModel),
                linkText, dialogTitle, iconCss, widthPercent, btnClasses == null ? ("btn btn-default " + btnCssSize) : btnClasses);
        }

        public static MvcHtmlString DialogLinkButton(
            this HtmlHelper helper,
            string actionUrl, 
            string linkText, 
            string dialogTitle, 
            string iconCss, 
            byte? widthPercent = null,
            string btnCss = "btn btn-default ",
            string btnCssSize = "btn-xs",
            string linkAdditionalClasses=null,
            string linkTitle = null)
        {
            return helper.DialogLink(
                actionUrl, 
                linkText, 
                dialogTitle, 
                iconCss, 
                widthPercent,
                btnCss + btnCssSize + " " + linkAdditionalClasses,
                title: linkTitle);
        }

        public static MvcHtmlString DialogLink(this HtmlHelper helper,
            string actionUrl, string linkText, string dialogTitle, string iconCss=null, byte? widthPercent = null, string linkCss = null,
            bool dismissCurrentDialog = false, string controlID = null,bool disabled = false, string title= null)
        {
            return MvcHtmlString.Create(string.Format(
                "<a {7} data-dialog-url='{0}' data-dialog-title='{1}' data-dialog-width-percent='{2}' class='{5}' {6} {8} {9} {10}>{3}{4}</a>",
                actionUrl,
                dialogTitle,
                widthPercent ?? 0,
                string.IsNullOrWhiteSpace(iconCss) ? "" : string.Format("<i class='{0}'></i> ", iconCss),
                linkText,
                linkCss,
                dismissCurrentDialog ? "data-dismiss='modal'" : "",
                controlID != null ? "id='" + controlID + "'" : "",
                disabled ? "disabled" : "",
                string.IsNullOrWhiteSpace(title) ? string.Empty : "title='" + title + "'", // 9
                string.IsNullOrWhiteSpace(title) ? string.Empty : "data-toggle='tooltip'" // 10
             ));
        }

        public static MvcHtmlString DialogLinkInnerHtml(this HtmlHelper helper,
            string actionUrl, MvcHtmlString innerHtml, string dialogTitle, byte? widthPercent = null,
            bool dismissCurrentDialog = false, string controlID = null,string title = null)
        {

            return MvcHtmlString.Create(string.Format(
                "<a {7} data-dialog-url='{0}' data-dialog-title='{1}' data-dialog-width-percent='{2}' {5} {4} {6}>{3}</a>",
                actionUrl,
                dialogTitle,
                widthPercent ?? 0,
                innerHtml,
                dismissCurrentDialog ? "data-dismiss='modal'" : "",
                controlID != null ? "id='" + controlID + "'" : "",
                string.IsNullOrWhiteSpace(title) ? string.Empty : "title='" + title + "'", // 9
                string.IsNullOrWhiteSpace(title) ? string.Empty : "data-toggle='tooltip'" // 10
             ));
        }


        public static MvcHtmlString DialogLinkImageIcon(this HtmlHelper helper,
            string actionUrl, string dialogTitle, string imageIconName, byte? widthPercent = null, string linkCssClass = null,
            bool dismissCurrentDialog = false, string controlID = null)
        {
            return MvcHtmlString.Create(string.Format(
                "<a {3} class='{6}' data-dialog-url='{0}' data-dialog-title='{1}' data-dialog-width-percent='{2}' {5}><span>{4}</span></a>",
                actionUrl,
                dialogTitle,
                widthPercent ?? 0,
                dismissCurrentDialog ? "data-dismiss='modal'" : "",
                string.IsNullOrWhiteSpace(imageIconName) ? "" : string.Format("<img src='"+StyleExtensions.IconsSrcPath()+"{0}'></i> ", imageIconName),
                controlID != null ? "id='" + controlID + "'" : "",linkCssClass
             ));
        }

        public static MvcHtmlString DialogLinkImage(this HtmlHelper helper,
            string actionUrl, string dialogTitle, string imageSrc,string imageClasses ="", byte? widthPercent = null, string linkCssClass = null,
            bool dismissCurrentDialog = false, string controlID = null,string imageID= "")
        {
            return MvcHtmlString.Create(string.Format(
                "<a {3} class='{6}' data-dialog-url='{0}' data-dialog-title='{1}' data-dialog-width-percent='{2}' {5}>{4}</a>",
                actionUrl,
                dialogTitle,
                widthPercent ?? 0,
                dismissCurrentDialog ? "data-dismiss='modal'" : "",
                //string.Format("<img src={0} class={1} />", helper.UcImage(imageSrc), imageClasses),
                helper.UcImage(imageSrc,addtionalattributes:new {@class = imageClasses, @id=imageID}),
                controlID != null ? "id='" + controlID + "'" : "", linkCssClass
             ));
        }

        /*this is for the small icon actions like edit, delete etc we have on trees*/
        /*public static MvcHtmlString DialogLinkIconImage(this HtmlHelper helper,
        string actionUrl, string linkText, string dialogTitle, string iconsrc = null, byte? widthPercent = null, string linkCss = null,
        bool dismissCurrentDialog = false, string controlID = null)
        {
            var url = new UrlHelper(helper.ViewContext.RequestContext);
            return MvcHtmlString.Create(string.Format(
                "<a {7} data-dialog-url='{0}' data-dialog-title='{1}' data-dialog-width-percent='{2}' class='{5}' {6}>{3}{4}</a>",
                actionUrl,
                dialogTitle,
                widthPercent ?? 0,
                string.IsNullOrWhiteSpace(iconsrc) ? "" : string.Format("<img src='{0}' alt='icon' /> ", url.Content(iconsrc)),
                linkText,
                linkCss,
                dismissCurrentDialog ? "data-dismiss='modal'" : "",
                controlID != null ? "id='" + controlID + "'" : ""
             ));
        }*/

        //revisar esto 
        public static MvcHtmlString UcNewDialogLink(this HtmlHelper helper,string actionUrl, string dialogTitle,string iconPNGName=null, byte? widthPercent = null)
        {
            var iconImg = (iconPNGName != null) ? iconPNGName + ".png" : helper.NewIcon();
            return helper.DialogLinkImageIcon(actionUrl, dialogTitle, iconImg , widthPercent: widthPercent);
        }

        public static MvcHtmlString Dialog(this HtmlHelper helper,
            string actionUrl, string dialogTitle, DialogTargetModel targetModel, byte? widthPercent = null)
        {
            return helper.Dialog(AppendDialogTargetInternal(actionUrl, targetModel),
                dialogTitle, widthPercent);
        }

        public static MvcHtmlString Dialog(this HtmlHelper helper,
            string actionUrl, string dialogTitle, byte? widthPercent = null)
        {
            var uniqueId = Guid.NewGuid();

            return MvcHtmlString.Create(string.Format(
                "<a id='{0}' data-dialog-url='{1}' data-dialog-title='{2}' data-dialog-width-percent='{3}' style='display:none'></a>{4}",
                uniqueId,
                actionUrl,
                dialogTitle,
                widthPercent ?? 0,
                "<script type='text/javascript'>$(function() { DialogHelper.Load($('#" + uniqueId + "')); });</script>"
             ));
        }

        /*public static MvcHtmlString DialogFormSubmitButtons<TModel>(
           this HtmlHelper<TModel> helper, string saveButtonText = "Save", string applyButtonTest = "Apply", bool excludeApplyButton = true) where TModel : IViewModel
        {
            var model = new FormSubmitButton()
            {
                IsAuthorized = AuthExtensions.UserCanWrite(),
            };

            model.ExludeApplyButton = excludeApplyButton;
            model.SaveButtonText = saveButtonText;
            model.ApplyButtonText = applyButtonTest;
            model.CancelButton = MvcHtmlString.Create(
                string.Format("<a data-dismiss='modal' class='{0}'>Cancel</a>", helper.StyleButton(small: false)));

            return helper.UserControlPartialInternal("_FormSubmitButton", model);
        }*/

        public static MvcHtmlString DialogFormSubmitButtons<TModel>(
            this HtmlHelper<TModel> helper,  
            string saveButtonText = "Save", 
            string applyButtonText = "Apply",
            bool excludeApplyButton = true, 
            MvcHtmlString additionalActionButton = null,
            bool excludeCancelButton = false) where TModel : IViewModel
        {
            var model = new FormSubmitButton()
            {
              //  IsAuthorized = AuthenticationHelper.UserCanWrite()
            };

            model.ExludeApplyButton = excludeApplyButton;
            model.SaveButtonText = saveButtonText;
            model.ApplyButtonText = applyButtonText;
            if (!excludeCancelButton)
            {
                model.CancelButton = MvcHtmlString.Create(
                    string.Format("<a data-dismiss='modal' class='{0}'>Cancel</a>", helper.StyleButton(small: false)));
            }
            //model.AdditionalActionButton = additionalActionButton;

            return helper.UserControlPartialInternal("_FormSubmitButton", model);
        }

        public static MvcHtmlString DialogSelectLinkForScriptTarget(this HtmlHelper helper,
            string idPropertyValue, string textPropertyValue)
        {
            var model = helper.GetDailogScriptTargetModel();

            var json = javaScriptSerializer.Serialize(new
            {
                KeyControlID = model.KeyControlID,
                ValueControlID = model.ValueControlID,
                Key = idPropertyValue,
                Value = textPropertyValue
            });

            return MvcHtmlString.Create(
                string.Format("<a data-dismiss='modal' class='{0}' data-dialog-select-target='{1}'>Select</a>",
                    helper.StyleButton(small: true, xs: true),
                    json));
        }

        internal static void RememberDialogTarget(HtmlHelper helper, System.Web.Routing.RouteValueDictionary roueValues)
        {
            var request = helper.ViewContext.RequestContext.HttpContext.Request;

            if (!string.IsNullOrWhiteSpace(request["dialogScriptTarget"]))
                roueValues["dialogScriptTarget"] = request["dialogScriptTarget"];

            if (!string.IsNullOrWhiteSpace(request["dialogTarget"]))
                roueValues["dialogTarget"] = request["dialogTarget"];
        }

        internal static void RememberDialogTarget(HtmlHelper helper, ref string actionUri)
        {
            string key = null, value = null;

            var request = helper.ViewContext.RequestContext.HttpContext.Request;

            if (!string.IsNullOrWhiteSpace(request["dialogScriptTarget"]))
            {
                key = "dialogScriptTarget";
                value = request["dialogScriptTarget"];
            }
            else if (!string.IsNullOrWhiteSpace(request["dialogTarget"]))
            {
                key = "dialogTarget";
                value = request["dialogTarget"];
            }

            if (!string.IsNullOrWhiteSpace(key))
            {
                actionUri = actionUri.Contains("?") ?
                    actionUri + "&" + key + "=" + value :
                    actionUri + "?" + key + "=" + value;
                /*
                string[] fragments = actionUri.Split('?');

                // 
                if (fragments.Length > 1)
                {
                    var queryParams = fragments[1].Split('&')
                            .Select(x => x.Split('='))
                            .ToDictionary(x => x[0], x => x[1]);

                    if (queryParams.ContainsKey(key))
                    {
                        queryParams[key] = value;

                        actionUri = fragments[0]
                    }
                }
                 */
            }
        }

        public static DialogScriptTargetModel GetDailogScriptTargetModel(this HtmlHelper helper)
        {
            return javaScriptSerializer.Deserialize<DialogScriptTargetModel>(
                helper.ViewContext.RequestContext.HttpContext.Request["dialogScriptTarget"]);
        }

        public static DialogTargetModel GetDailogTargetModel(this HtmlHelper helper)
        {
            return javaScriptSerializer.Deserialize<DialogTargetModel>(
                helper.ViewContext.RequestContext.HttpContext.Request["dialogTarget"]);
        }

        public static string AppendDialogTargetInternal(string actionUrl, DialogScriptTargetModel scriptTargetModel)
        {
            if (scriptTargetModel == null)
                return actionUrl;

            string param = "dialogScriptTarget=" + javaScriptSerializer.Serialize(scriptTargetModel);

            return actionUrl.Contains("?") ? actionUrl + "&" + param : actionUrl + "?" + param;
        }

        public static string AppendDialogTargetInternal(string actionUrl, DialogTargetModel targetModel)
        {
            string param = "dialogTarget=" + javaScriptSerializer.Serialize(targetModel);

            return actionUrl.Contains("?") ? actionUrl + "&" + param : actionUrl + "?" + param;
        }

        

        //OLD HELPERS - LOOK FORWARD - i dont want to break anything right now so i copied these
        /*
        public static MvcHtmlString DialogLinkButtonInfo(this HtmlHelper helper,
            string actionUrl, string linkText, string dialogTitle, string iconCss = "", string btnID = null, byte? widthPercent = null, string btnCssSize = "btn-sm", string style = null)
        {
            return helper.DialogLinkOld(actionUrl, linkText, dialogTitle, iconCss, widthPercent, "btn btn-info " + btnCssSize, btnID: btnID, style: style);
        }

        public static MvcHtmlString DialogLinkOld(this HtmlHelper helper,
            string actionUrl, string linkText, string dialogTitle, string iconCss = "", byte? widthPercent = null, string linkCss = null,
            bool dismissCurrentDialog = false, string btnID = null, string style = null)
        {
            return MvcHtmlString.Create(string.Format(
                "<a data-dialog='true' {7} data-dialog-url='{0}' data-dialog-title='{1}' data-dialog-width-percent='{2}' class='{5}' {8} {6}>{3}{4}</a>",
                actionUrl,
                dialogTitle,
                widthPercent ?? 0,
                string.IsNullOrWhiteSpace(iconCss) ? "" : string.Format("<i class='{0}'></i> ", iconCss),
                linkText,
                linkCss,
                dismissCurrentDialog ? "data-dismiss='modal'" : "",
                btnID != null ? "id='" + btnID + "'" : "",
                style != null ? "style='" + style + "'" : ""
             ));
        }



        /*
        public static MvcHtmlString DialogLinkButton(this HtmlHelper helper,
            string actionUrl, string linkText, string dialogTitle, string iconCss, byte? widthPercent = null, string btnCssSize = "btn-xs")
        {
            return helper.DialogLink(actionUrl, linkText, dialogTitle, iconCss, widthPercent, "btn btn-default " + btnCssSize);
        }

        public static MvcHtmlString DialogLinkButtonInfo(this HtmlHelper helper,
         string actionUrl, string linkText, string dialogTitle, string iconCss="", string btnID= null,byte? widthPercent = null, string btnCssSize = "btn-sm",string style=null)
        {
            return helper.DialogLink(actionUrl, linkText, dialogTitle, iconCss, widthPercent, "btn btn-info " + btnCssSize, btnID:btnID,style:style);
        }

        public static MvcHtmlString DialogLink(this HtmlHelper helper,
            string actionUrl, string linkText, string dialogTitle, string iconCss="", byte? widthPercent = null, string linkCss = null,
            bool dismissCurrentDialog = false, string btnID = null,string style = null)
        {
            return MvcHtmlString.Create(string.Format(
                "<a data-dialog='true' {7} data-dialog-url='{0}' data-dialog-title='{1}' data-dialog-width-percent='{2}' class='{5}' {8} {6}>{3}{4}</a>",
                actionUrl,
                dialogTitle,
                widthPercent ?? 0,
                string.IsNullOrWhiteSpace(iconCss) ? "" : string.Format("<i class='{0}'></i> ", iconCss),
                linkText,
                linkCss,
                dismissCurrentDialog ? "data-dismiss='modal'" : "",
                btnID != null ? "id='"+btnID+"'":"",
                style != null ? "style='"+style+"'":""
             ));
        }

        public static MvcHtmlString Dialog(this HtmlHelper helper,
            string actionUrl, string dialogTitle, byte? widthPercent = null)
        {
            var uniqueId = Guid.NewGuid();

            return MvcHtmlString.Create(string.Format(
                "<a id='{0}' data-dialog='true' data-dialog-url='{1}' data-dialog-title='{2}' data-dialog-width-percent='{3}' style='display:none'></a>{4}",
                uniqueId,
                actionUrl,
                dialogTitle,
                widthPercent ?? 0,
                "<script type='text/javascript'>$(function() { DialogHelper.Load($('#" + uniqueId + "')); });</script>"
             ));
        }

        public static MvcHtmlString DialogFormSubmitButtons<TModel>(
           this HtmlHelper<TModel> helper, bool excludeApplyButton = true) where TModel : IViewModel
        {
            var model = new FormSubmitButton()
            {
                IsAuthorized = AuthExtensions.UserCanWrite(),
            };

            model.ExludeApplyButton = excludeApplyButton;

            model.CancelButton = MvcHtmlString.Create(
                string.Format("<a data-dismiss='modal' class='{0}'>Cancel</a>", helper.StyleButton(small: false)));

            return helper.UserControlPartialInternal("_FormSubmitButton", model);
        }

        public static MvcHtmlString DialogSelectLinkForScriptTarget(this HtmlHelper helper,
            string sessionKeyForModel, string idPropertyValue, string textPropertyValue)
        {
            var model = helper.GetSessionValue<DialogScriptTargetModel>(sessionKeyForModel, true);

            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var json = serializer.Serialize(new
            {
                KeyControlID = model.KeyControlID.ToString(),
                ValueControlID = model.ValueControlID.ToString(),
                Key = idPropertyValue,
                Value = textPropertyValue
            });

            return MvcHtmlString.Create(
                string.Format("<a data-dismiss='modal' class='{0}' data-dialog-select-target='{1}'>Select</a>",
                    helper.StyleButton(small: true),
                    json));
        }*/
    }
}