

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Policy;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Mvc.Ajax;
// Application
using System.Web.Routing;
using System.Web.Script.Serialization;
using PacificBBExtranet.Web.UserControls;
using Newtonsoft.Json;
using WebGrease.Css.Extensions;
using PacificBBExtranet.Utils.ObjectExtensions;
using PacificBBExtranet.Services.Models;
using PacificBBExtranet.Web.Helpers;

namespace PacificBBExtranet.Web.Helpers
{
    public static class UcExtensions
    {
        #region Search Filters

        public static MvcHtmlString SearchFilterSingleField(this HtmlHelper helper,
            string actionName, string controllerName, 
            string updateTargetId, string searchPlaceholder,
            string hint,object routeValues = null)
        {
            var model = new SearchFilterModel()
            {
                UpdateTargetId = updateTargetId,
                ActionName = actionName,
                SearchPlaceholder = searchPlaceholder,
                ControllerName = controllerName,
                Hint = hint,
                RouteValues = routeValues
            };

            return helper.UserControlPartialInternal("_SearchFilterSingleField", model);
        }

        public static MvcHtmlString UcSearchFilterSingleField(this HtmlHelper helper,
            string actionName, string controllerName,
            string updateTargetId, string searchPlaceholder,
            string hint, object routeValues = null)
        {
            var model = new SearchFilterModel()
            {
                UpdateTargetId = updateTargetId,
                ActionName = actionName,
                SearchPlaceholder = searchPlaceholder,
                ControllerName = controllerName,
                Hint = hint,
                RouteValues = routeValues
            };

            return helper.UserControlPartialInternal("_UcSearchFilterSingleField", model);
        }

        #endregion Search filters

        #region Form Controls - File Upload

        //public static MvcHtmlString UcFileUploader(this HtmlHelper helper,
        //    bool allowMultiple,
        //    string mimeAcceptTypes,
        //    string returnControl,
        //    string displayControl,
        //    bool autoSubmitForm)
        //{
        //    var model = new FormFileUploadModel()
        //    {
        //        AllowMultiple = allowMultiple,
        //        MimeAcceptTypes = mimeAcceptTypes,
        //        ReturnControlId = returnControl,
        //        DisplayControlId = displayControl,
        //        AutoSubmit = autoSubmitForm
        //    };
        //    return helper.UserControlPartialInternal("_FormFileUpload", model);
        //}

        //public static MvcHtmlString UcFileUploaderPPT(this HtmlHelper helper,
        //    bool allowMultiple,
        //    string returnControl,
        //    string displayControl,
        //    bool autoSubmitForm)
        //{
        //    var model = new FormFileUploadModel()
        //    {
        //        AllowMultiple = allowMultiple,
        //        MimeAcceptTypes = /*"application/vnd.ms-powerpoint," +*/
        //                          "application/vnd.openxmlformats-officedocument.presentationml.presentation",
        //        ReturnControlId = returnControl,
        //        DisplayControlId = displayControl,
        //        OnlyPPTX = true,
        //        AutoSubmit = autoSubmitForm
        //    };

        //    return helper.UserControlPartialInternal("_FormFileUpload", model);
        //        // "_FormPPTFileUpload", model);
        //}

        #endregion //Form Controls - File Upload

        #region Form Controls - Display

        public static MvcHtmlString UcDisplayText<TModel>(
            this HtmlHelper<TModel> helper,
            string value,
            string label,
            int labelColDimension)
        {
            var htmlAttributes = GetFormHtmlAttributes(false);
            htmlAttributes["class"] += " form-control-static";

            var span = new TagBuilder("span");
            span.SetInnerText(value);

            var model = new FormTextBoxModel()
            {
                Label = label,
                LabelColDimension = labelColDimension,
                ControlFor = MvcHtmlString.Create(span.ToString(TagRenderMode.Normal))
            };

            return helper.FormControlGenericInternal(model);
        }

        public static MvcHtmlString UcDisplayTextFor<TModel, TProperty>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression,
            string label,
            int labelColDimension)
        {
            var htmlAttributes = GetFormHtmlAttributes();
            htmlAttributes["class"] += " form-control-static";

            var model = new FormTextBoxModel()
            {
                Label = label,
                LabelColDimension = labelColDimension,
                ControlName = FormExtensions.ControlNameFor(expression),
                ControlFor = helper.DisplayTextFor(expression)
            };

            return helper.FormControlGenericInternal(model);
        }

        #endregion // Form Controls - Display

        #region Form Controls - TextBox

        public static MvcHtmlString UcNumericInputFor<TModel, TProperty>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression,
            string label,
            int labelColDimension)
        {
            var htmlAttributes = GetFormHtmlAttributes(false, label, 0);

            htmlAttributes.Add("type", "number");
            htmlAttributes.Add("step", "any");

            var model = new FormTextBoxModel()
            {
                Label = label,
                LabelColDimension = labelColDimension,
                ControlName = FormExtensions.ControlNameFor(expression),
                ControlFor = helper.TextBoxFor(expression, htmlAttributes),
                ValidationMessageFor = helper.ValidationMessageFor(expression)
            };

            return helper.UserControlPartialInternal("_FormTextBox", model);
        }

        public static MvcHtmlString UcPasswordFor<TModel, TProperty>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression,
            string label,
            int labelColDimension,
            bool showValidationMessageBelow=true)
        {
            var model = new FormTextBoxModel()
            {
                Label = label,
                LabelColDimension = labelColDimension,
                ControlName = FormExtensions.ControlNameFor(expression),
                ControlFor = helper.PasswordFor(expression, GetFormHtmlAttributes(false, label, 0)),
                ValidationMessageFor = helper.ValidationMessageFor(expression),
                ShowValidationMessageBelow=showValidationMessageBelow
            };

            return helper.UserControlPartialInternal("_FormTextBox", model);
        }

        public static MvcHtmlString UcTextBoxFor<TModel, TProperty>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression,
            string label,
            int labelColDimension,
            string placeHolder = "",
            string inputGroupPrepend = null,
            string inputGroupAppend = null,
            bool isPassword = false,
            string format = null,
            object additionalHtmlattributes = null,
            bool isReadOnly = false)
        {
            var addtional = HtmlHelper.AnonymousObjectToHtmlAttributes(additionalHtmlattributes)
                .Select(x=>new KeyValuePair<string,object>(x.Key,x.Value)).ToList();

            var html = GetFormHtmlAttributes(false, label, 0, isReadOnly: isReadOnly);
            if (!String.IsNullOrEmpty(placeHolder)) html["placeholder"] = placeHolder;
            addtional.ForEach(html.Add);
            var model = new FormTextBoxModel()
            {
                Label = label,                
                LabelColDimension = labelColDimension,
                InputGroupPrepend = inputGroupPrepend,
                InputGroupAppend = inputGroupAppend,
                ControlName = FormExtensions.ControlNameFor(expression),
                ControlFor = helper.TextBoxFor(expression, 
                    htmlAttributes: html,format: format),
                ValidationMessageFor = helper.ValidationMessageFor(expression)
            };

            return helper.UserControlPartialInternal("_FormTextBox", model);
        }

        public static MvcHtmlString UcStandaloneTextBoxFor<TModel, TProperty>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression,
            bool includeValidationMessage = true,
            string placeHolder = "")
        {
            var textBox = helper.TextBoxFor(expression, htmlAttributes:
                new { @class = "form-control input-sm", placeholder = placeHolder });

            if (includeValidationMessage)
            {
                var validationMessage = helper.ValidationMessageFor(expression);

                return MvcHtmlString.Create(textBox.ToString() + validationMessage.ToString());
            }

            return textBox;
        }

        #endregion // Form Controls - TextBox

        #region Form Controls - File Upload

       /* public static MvcHtmlString UcFileUpload(this HtmlHelper helper,            
            bool allowMultiple,
            string fileTypes,
            string returnControl)
        {
            var model = new FormFileUploadModel()
            {
               AllowMultiple = allowMultiple,
               UploadFileTypes = fileTypes,
               ReturnControlId = returnControl
            };
            return helper.UserControlPartialInternal("_FormFileUpload", model);
        }*/

        #endregion //Form Controls - File Upload

        #region Form Controls - Date Picker

        public static MvcHtmlString UcDatePickerFor<TModel, TProperty>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression,
            string label,
            int labelColDimension,
            DateTime? defaultValue,
            string position,
            bool isReadOnly = false)
        {
            var htmlAttributes = GetFormHtmlAttributes(false, label, 0,isReadOnly:true);

            htmlAttributes["class"] += " " + helper.StyleDatePicker();
            htmlAttributes.Add("data-date",defaultValue.DefaultShortFormat());

            var model = new FormControlGenericModel()
            {
                Label = label,
                LabelColDimension = labelColDimension,
                ControlName = FormExtensions.ControlNameFor(expression),
                ControlFor = helper.TextBoxFor(expression, htmlAttributes: htmlAttributes, 
                format: "{0:" + DateTimeExtensions.DateFormatShort + "}"),
                ValidationMessageFor = helper.ValidationMessageFor(expression)
            };

            var control = helper.FormControlGenericInternal(model);

            var datePickerScript = "";

            //date picker field is always read only , but if isReadOnly parameter is passed as true then 
            //it wont work as date picker
            if (!isReadOnly) {
                datePickerScript = helper.UserControlPartialInternal(
                            "_DatePickerSetupScript", new FormDatePickerSetupScriptModel()
                            {
                                ControlId = helper.IdFor(expression),
                                Position = position
                            }).ToString();
            }
    

            return MvcHtmlString.Create(control.ToString() + datePickerScript);
        }

        #endregion // Form Controls - Date Picker

        #region Form Controls - Text Area

        public static MvcHtmlString UcTextAreaFor<TModel, TProperty>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression,
            string label,
            int labelColDimension,
            int rows = 3, 
            object additionalHtmlattributes = null,
            bool isReadOnly = false)
        {
            var addtional = HtmlHelper.AnonymousObjectToHtmlAttributes(additionalHtmlattributes)
               .Select(x => new KeyValuePair<string, object>(x.Key, x.Value)).ToList();

            var html = GetFormHtmlAttributes(false, null, rows, inputSmall: true,isReadOnly : isReadOnly);
            addtional.ForEach(html.Add);
            var model = new FormControlGenericModel()
            {
                Label = label,
                ControlName = FormExtensions.ControlNameFor(expression),
                LabelColDimension = labelColDimension,
                ControlFor = helper.TextAreaFor(expression,
                    htmlAttributes: html),
                ValidationMessageFor = helper.ValidationMessageFor(expression)
            };

            return helper.FormControlGenericInternal(model);
        }

        public static MvcHtmlString UcTextAreaStandaloneFor<TModel, TProperty>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression,
            int rows = 3,
            string placeHolder = "",
            bool includeValidation = true)
        {
            var html = GetFormHtmlAttributes(false, null, rows, inputSmall: true);

            var textBox = helper.TextAreaFor(expression,
                htmlAttributes: new { 
                    @class = "form-control input-sm", 
                    placeholder = placeHolder, 
                    rows = rows });

            if (includeValidation)
            {
                var validationMessage = helper.ValidationMessageFor(expression);

                return MvcHtmlString.Create(textBox.ToString() + validationMessage.ToString());
            }

            return textBox;
        }

       /* public static MvcHtmlString UcTextAreaCounterFor<TModel, TProperty>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression)
        {
            var attr = expression.PropertyAttribute(typeof(MaxLengthAttribute));

            if(attr == null)
                throw new ApplicationException(
                    string.Format("Property {0} does not have a MaxLength Attribute", expression.MemberName()));

            var model = new TextAreaCounterModel()
            {
                TextAreaId = helper.IdFor(expression),
                TextAreaMaxLength = ((MaxLengthAttribute)attr).Length
            };

            return helper.UserControlPartialInternal("_TextAreaCounter", model);
        }*/

        #endregion // Form Controls - Text Area

        #region Form Controls - List/Dropdown

        public static MvcHtmlString UcListBoxFor<TModel, TProperty>(
           this HtmlHelper<TModel> helper,
           Expression<Func<TModel, TProperty>> expression,
           string label,
           int labelColDimension,
           MultiSelectList listItems,
           int rows = 3)
        {
            var model = new FormControlGenericModel()
            {
                Label = label,
                ControlName = FormExtensions.ControlNameFor(expression),
                LabelColDimension = labelColDimension,
                ControlFor = helper.ListBoxFor(expression, listItems,
                    htmlAttributes: GetFormHtmlAttributes(rows: rows, inputSmall: false)),
                ValidationMessageFor = helper.ValidationMessageFor(expression)
            };

            return helper.FormControlGenericInternal(model);
        }

        public static MvcHtmlString UcDropDownListFor<TModel, TProperty>(
           this HtmlHelper<TModel> helper,
           Expression<Func<TModel, TProperty>> expression,
           string label,
           int labelColDimension,
           IEnumerable<SelectListItem> dataSource)
        {
            return helper.UcDropDownListFor(
                expression, label, labelColDimension,
                new SelectList(dataSource, "Value", "Text"));
        }


        public static MvcHtmlString UcDropDownListFor<TModel, TProperty>(
           this HtmlHelper<TModel> helper,
           Expression<Func<TModel, TProperty>> expression,
           string label,
           int labelColDimension,
           IDictionary<int, string> dataSource,
           string optionLabel = "",
           int? selectedValue = null ,
           object htmlAttrs = null)
        {
            return helper.UcDropDownListFor(
                expression, label, labelColDimension,
                new SelectList(dataSource, "Key", "Value",selectedValue),
                optionLabel: optionLabel, 
                addtionalHtmlAttributes: htmlAttrs);
        }

        public static MvcHtmlString UcDropDownListFor<TModel, TProperty>(
           this HtmlHelper<TModel> helper,
           Expression<Func<TModel, TProperty>> expression,
           string label,
           int labelColDimension,
           IDictionary<string, string> dataSource,
           string optionLabel = "")
        {
            return helper.UcDropDownListFor(
                expression, label, labelColDimension,
                new SelectList(dataSource, "Key", "Value"),
                optionLabel: optionLabel);
        }
        /*
        public static MvcHtmlString UcMultiSelectDropDownListFor<TModel, TProperty>(
           this HtmlHelper<TModel> helper,
           Expression<Func<TModel, TProperty>> expression,string label,
            IDictionary<string, string> dataSource = null,int labelColDimension= 6,object htmlattributes=null)
        {

            var attributes = GetFormHtmlAttributes();
            attributes.Add("multiple", "multiple");
                attributes.AddRange(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlattributes));
            
            var model = new FormControlGenericModel()
            {
                Label = label,
                ControlName = FormExtensions.ControlNameFor(expression),
                LabelColDimension = labelColDimension,
                ControlFor = helper.DropDownListFor(expression, 
                dataSource.Select(d=>new SelectListItem(){ Value=d.Key.ToString(),Text=d.Value.ToString() }),attributes),
                ValidationMessageFor = helper.ValidationMessageFor(expression)
            };

            return helper.FormControlGenericInternal(model);
        }*/

        //public static MvcHtmlString UcMultiSelectDropDownList(
        //    this HtmlHelper helper,string fieldName, string label,string controlID, IDictionary<int, string> dataSource, IEnumerable<string> selectedValues, 
        //    int labelColDimension = 6)
        //{
        //    var model = new FormControlGenericModel()
        //    {
        //        Label = label,
        //        ControlName = fieldName,
        //        LabelColDimension = labelColDimension,
        //        ControlFor = helper.Partial("~/Views/UserControls/_MultiSelectDropDown.cshtml", new MultiSelectDropDownModel()
        //        {
        //          ControlID=controlID,
        //          Source  = dataSource.Select(d => new SelectListItem()
        //          {
        //              Value = d.Key.ToString(), Text = d.Value.ToString(),Selected = selectedValues.Any(v=>v == d.Key.ToString())
        //          }),FieldName = fieldName
        //        })
        //    };

        //    return helper.FormControlGenericInternal(model);
        //}

        public static MvcHtmlString UcAjaxDropDownListFor<TModel, TProperty>(
                   this HtmlHelper<TModel> helper,
                   Expression<Func<TModel, TProperty>> expression,
                   string label,
                   int labelColDimension,
                   SelectList dataSource,
                    string targetDrop = "",
                    string targetUrl = "",
                   string optionLabel = "")
        {

            var htmlattributes = GetFormHtmlAttributes();
            var attribu = htmlattributes["class"] ?? String.Empty;
            attribu = attribu + " ajaxdrop";
            htmlattributes["class"] = attribu;

            htmlattributes.Add("data-target-dropdown", targetDrop);
            htmlattributes.Add("data-target-url", targetUrl);

            var model = new FormControlGenericModel()
            {
                Label = label,
                ControlName = FormExtensions.ControlNameFor(expression),
                LabelColDimension = labelColDimension,
                ControlFor = helper.DropDownListFor(expression,
                   selectList: dataSource,
                   htmlAttributes: htmlattributes,
                   optionLabel: optionLabel),
                ValidationMessageFor = helper.ValidationMessageFor(expression)
            };

            return helper.FormControlGenericInternal(model);
        }


        public static MvcHtmlString UcDropDownListFor<TModel, TProperty>(
           this HtmlHelper<TModel> helper,
           Expression<Func<TModel, TProperty>> expression,
           string label,
           int labelColDimension,
           string[] optionList)
        {
            return helper.UcDropDownListFor(
                expression, label, labelColDimension,
                new SelectList(optionList));
        }

      /*  public static MvcHtmlString UcDropDownListForNewOnly<TModel, TPropertyValue, TPropertyText>(
           this HtmlHelper<TModel> helper,
           Expression<Func<TModel, TPropertyValue>> expressionId,
           Expression<Func<TModel, TPropertyText>> expressionText,
           string label,
           int labelColDimension,
           IDictionary<int, string> dataSource,
           string optionLabel = "")
           where TModel : IViewModel
        {
            if (helper.ViewData.Model.IsEmpty())
                return helper.UcDropDownListFor(expressionId, label, labelColDimension,
                    new SelectList(dataSource, "Key", "Value"), optionLabel);
            else
            {
                var text = helper.UcDisplayTextFor(expressionText, label, labelColDimension);
                var hiddenId = helper.HiddenFor(expressionId);
                var hiddenText = helper.HiddenFor(expressionText);
                return MvcHtmlString.Create(text.ToString() + hiddenId.ToString() + hiddenText.ToString());
            }
        }*/

        public static MvcHtmlString UcStandaloneDropDownListFor<TModel, TProperty>(
           this HtmlHelper<TModel> helper,
           Expression<Func<TModel, TProperty>> expression,
           SelectList dataSource,
           string optionLabel = "")
        {
            return helper.DropDownListFor(expression,
                   selectList: dataSource,
                   htmlAttributes: GetFormHtmlAttributes(),
                   optionLabel: optionLabel);
        }

        public static MvcHtmlString UcDropDownListFor<TModel, TProperty>(
           this HtmlHelper<TModel> helper,
           Expression<Func<TModel, TProperty>> expression,
           string label,
           int labelColDimension,
           SelectList dataSource,
           string optionLabel = null,
           object addtionalHtmlAttributes=null)
        {
            var html = GetFormHtmlAttributes();

            HtmlHelper.AnonymousObjectToHtmlAttributes(addtionalHtmlAttributes)
                .ForEach(html.Add);

            var model = new FormControlGenericModel()
            {
                Label = label,
                ControlName = FormExtensions.ControlNameFor(expression),
                LabelColDimension = labelColDimension,
                ControlFor = helper.DropDownListFor(expression,
                   selectList: dataSource,
                   htmlAttributes: html,
                   optionLabel: optionLabel),
                ValidationMessageFor = helper.ValidationMessageFor(expression)
            };

            return helper.FormControlGenericInternal(model);
        }

        public static MvcHtmlString UcDropDownListForEnum<TModel, TEnum>(
            this HtmlHelper<TModel> helper, 
            Expression<Func<TModel, TEnum>> expression,
            string label,
            int labelColDimension)
            //where TEnum : struct
        {
            return helper.UcDropDownListFor(
                expression,
                label,
                labelColDimension,
                dataSource: helper.GetSelectListForEnumInternal(expression));
        }

        public static MvcHtmlString UcStandaloneDropDownListForEnum<TModel, TEnum>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TEnum>> expression,
            string optionLabel = "")
            where TEnum : struct
        {
            return helper.UcStandaloneDropDownListFor(
                expression,
                dataSource: helper.GetSelectListForEnumInternal(expression),
                optionLabel: optionLabel);
        }

        #endregion // Form Controls - List/Dropdown

        #region Form Controls - Checkbox

        public static MvcHtmlString UcCheckBoxFor<TModel>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, bool>> expression,
            string label,
            int labelColDimension,
            object additionalHtmlattributes = null)
        {
            var attrs = GetFormHtmlAttributes(omitFormControlStyle: true);
            attrs.AddRange(HtmlHelper.AnonymousObjectToHtmlAttributes(additionalHtmlattributes));

            var model = new FormControlGenericModel()
            {
                Label = label,
                ControlName = FormExtensions.ControlNameFor(expression),
                LabelColDimension = labelColDimension,
                ControlFor = helper.CheckBoxFor(expression,
                    htmlAttributes: attrs),
                ValidationMessageFor = helper.ValidationMessageFor(expression)
            };

            return helper.FormControlGenericInternal(model);
        }

        #endregion // Form Controls - CheckBox

        #region Buttons

        /*public static MvcHtmlString UcFormSubmitButtonsForTabContent<TModel>(this HtmlHelper<TModel> helper,
            string actionNameCancel,
            string controllerNameCancel,
            bool excludeSaveButton = false,
            bool excludeApplyButton = false,
            object CancelRouteValues = null)
            where TModel : IViewModel
        {
            MvcHtmlString cancelButton;

                cancelButton = helper.TabTriggerActionLink("backTo" + actionNameCancel, "Cancel",
                    helper.ViewContext.RequestContext.ActionUrl(actionNameCancel, controllerNameCancel, routeValues:CancelRouteValues),
                        helper.StyleButton(false),
                        null);

            return helper.UcFormSubmitButtonsInternal(
                cancelButton: cancelButton,
                excludeSaveButton: excludeSaveButton,
                excludeApplyButton: excludeApplyButton);
        }*/



        public static MvcHtmlString UcFormSubmitButtons<TModel>(
           this HtmlHelper<TModel> helper,
           bool excludeSaveButton = false,
           bool excludeApplyButton = true,
           string submitButtonText = "Save") where TModel : IViewModel
        {
            var model = new FormSubmitButton()
            {
                SaveButtonText = submitButtonText
            };

            var ajaxHelper = ViewExtensions.CurrentAjaxHelper;

            model.ExcludeSaveButton = excludeSaveButton;
            model.ExludeApplyButton = excludeApplyButton;

           // var cancelButton = null;
            return helper.UserControlPartialInternal("_FormSubmitButton", model);
        }


        public static MvcHtmlString UcFormSubmitButtonsForModal<TModel>(
           this HtmlHelper<TModel> helper,
           bool excludeSaveButton = false,
           bool excludeApplyButton = true,
           string submitButtonText = "Save") where TModel : IViewModel
        {
            var model = new FormSubmitButton()
            {
                SaveButtonText = submitButtonText
            };

            var ajaxHelper = ViewExtensions.CurrentAjaxHelper;

            model.ExcludeSaveButton = excludeSaveButton;
            model.ExludeApplyButton = excludeApplyButton;

            var cancelButton = new TagBuilder("a");
            cancelButton.InnerHtml = "Cancel";
            cancelButton.AddCssClass(helper.StyleButton(small: false));
            cancelButton.Attributes.Add("data-dismiss", "modal");
            model.CancelButton = MvcHtmlString.Create(cancelButton.ToString());
            return helper.UserControlPartialInternal("_FormSubmitButton", model);
        }
        
        public static MvcHtmlString UcFormSubmitButtonsForTarget<TModel>(
           this HtmlHelper<TModel> helper,
           string cancelActionName,
           string cancelControllerName,
           string cancelTargetId=null,
           object cancelRouteValues = null,
           bool excludeSaveButton = false,
           bool excludeApplyButton =false,
            bool excludeCancelButton=false) where TModel : IViewModel
        {
            var ajaxHelper = ViewExtensions.CurrentAjaxHelper;

            var model = new FormSubmitButton()
            {
            };

            model.ExcludeSaveButton = excludeSaveButton;
            model.ExludeApplyButton = excludeApplyButton;
            if (!excludeCancelButton)
            {

                model.CancelButton = ajaxHelper.ActionLink(
                    linkText: "Cancel",
                    actionName: cancelActionName,
                    controllerName: cancelControllerName,
                    routeValues: cancelRouteValues,
                    ajaxOptions: new AjaxOptions()
                    {
                        InsertionMode = InsertionMode.Replace,
                        HttpMethod = "GET",
                        UpdateTargetId = cancelTargetId,
                        LoadingElementId = ScriptHelper.MarkupIDs.AjaxLoadingOverlay
                    },
                    htmlAttributes: new { @class = helper.StyleButton(small: false) });
            }
            return helper.UserControlPartialInternal("_FormSubmitButton", model);
        }

        //mal hecho, tiene que usar el ucaction q usa el loading
        public static MvcHtmlString UcButtonInfo(AjaxHelper helper,string text,string action,string controller,
            string updateTargetID,object routeValues=null)
        {
          return helper.UcActionLink(text, action, controller,routeValues, new AjaxOptions { InsertionMode = InsertionMode.Replace, UpdateTargetId = updateTargetID ,HttpMethod ="GET"}, htmlAttributes: new { @class = "btn btn-info" });
        }


        public static MvcHtmlString UcDropdownButton(
            this HtmlHelper helper, 
            string text,
            MvcHtmlString leftHtml = null,
            string iconCss = "fa-caret-down",
            params IButtonDropdownItemModel[] items)
        {
            return helper.UcDropdownButton(text, "btn-default", "btn-xs", false, 0, leftHtml,iconCss:iconCss,items:items );
        }


        public static MvcHtmlString UcDropdownButton(this HtmlHelper helper, string text,
             string buttonCssStyle, 
            string buttonCssSize,
            bool pullLeft = false,
            int maxWidth = 0, 
            MvcHtmlString leftHtml = null,
            string iconCss = "fa-caret-down",
            params IButtonDropdownItemModel[] items)

        {
            var model = new ButtonDropdownModel()
            {
                Items = items,
                Text = text,
                ButtonCssSize = buttonCssSize,
                ButtonCssStyle = buttonCssStyle,
                PullLeft = pullLeft,
                LeftHtml = leftHtml,
                MaxWidth = maxWidth, // Needed for longer than 300px text  which is set in the .dropdown-menu class.
                IconCss = iconCss
            };

            return helper.UserControlPartialInternal("_ButtonDropdown", model);
        }

        /*public static MvcHtmlString UcDropdownButton(this HtmlHelper helper,
            string text, 
            string buttonCssSize ="btn-xs",
            string buttonCssStyle = "btn-default",
            params IButtonDropdownItemModel[] items)
        {
            var model = new ButtonDropdownModel();
            model.Items = items;
            model.Text = text;
            model.ButtonCssSize = buttonCssSize;
            model.ButtonCssStyle = buttonCssStyle;
            // wip

            return helper.UserControlPartialInternal("_ButtonDropdown", model);
        }*/

        public static MvcHtmlString UcButton(this HtmlHelper helper, string content,ButtonType type,ButtonSize? size =null, object additionalHtmlAttributes = null)
        {
            var htmlattributes = HtmlHelper.AnonymousObjectToHtmlAttributes(additionalHtmlAttributes);
            var button = new TagBuilder("button");
            button.InnerHtml = content;
            button.AddCssClass("btn");
            button.AddCssClass(GetButtonClass(type));
            if(size!=null)button.AddCssClass(GetButtonSize(size.Value));
            button.MergeAttributes(htmlattributes);
            return new MvcHtmlString(button.ToString());
        }

        public static MvcHtmlString UcButtonNewItem(this HtmlHelper helper,
            string text, string actionName, string controllerName, string updateTargetId, object routeValues = null,
            string onBegin=null,string onComplete=null,string onFailure=null,string onSuccess=null,
            string additionalClasses="")
        {
            var ajaxHelper = ViewExtensions.CurrentAjaxHelper;

            return ajaxHelper.ActionLink(
                linkText: text,
                actionName: actionName,
                controllerName: controllerName,
                routeValues: routeValues,
                ajaxOptions: new AjaxOptions()
                {
                    InsertionMode = InsertionMode.Replace,
                    HttpMethod = "GET",
                    UpdateTargetId = updateTargetId,
                    LoadingElementId = ScriptHelper.MarkupIDs.AjaxLoadingOverlay,
                    OnBegin = onBegin,
                    OnComplete = onComplete,
                    OnFailure = onFailure,
                    OnSuccess = onSuccess
                },
                htmlAttributes: new { @class = helper.StyleButtonSmallInfo()+" " + additionalClasses });
        }

        #endregion

        #region Tabs Controls

        public static MvcHtmlString TabActionLink(
            this HtmlHelper helper, string anchorId, string text, string url, string updateTargetId)
        {
            return MvcHtmlString.Create(
                string.Format("<a id='{0}' data-url='{1}' data-target='#{2}' data-tab-persist='true'>{3}</a>",
                    anchorId, url, updateTargetId, text));
        }

        public static MvcHtmlString TabSubActionLink(
            this HtmlHelper helper, string anchorId, string text, string url, string updateTargetId,
            string iconCss)
        {
            return MvcHtmlString.Create(
                string.Format(
                    "<a id='{0}' data-url='{1}' data-target='#{2}' data-tab-persist='false'>{3}{4}</a>",
                    anchorId, 
                    url, 
                    updateTargetId, 
                    !string.IsNullOrWhiteSpace(iconCss) ? "<i class='" + iconCss + "'></i> " : "",
                    text));
        }

        /*public static MvcHtmlString TabTriggerActionLink(this HtmlHelper helper,
            string anchorId, string text, string url, string anchorStyle, string iconStyle)
        {
            string anchorHtml = string.Format("<a id='{0}' data-url='{1}' class='{2}'><i class='{3}'></i> {4}</a>",
                anchorId, url, anchorStyle, iconStyle, text);
            string script = string.Format("<script type='text/javascript'>AjaxTabs.SetupTabTriggerLink('{0}');</script>",
                anchorId);

            return MvcHtmlString.Create(anchorHtml + script);
        }*/




        #endregion Tab Controls

        #region Links

        //public static MvcHtmlString UcActionLinkDeleteItem(this HtmlHelper helper,
        //   //string[] authorizedRoles,
        //   string linkCss, string linkText, bool dismissDialog,
        //   string confirmMessage, string deleteActionPostUri, 
        //    string refreshContainerActionUri, 
        //    string refreshContainerTargetId,
        //    string onSuccessCallback="",
        //    string iconCss ="fa fa-trash-o")
        //    {
        //    /*if (helper.ViewData.Model.IsEmpty())
        //        return MvcHtmlString.Empty;*/

        //   // var isInRoles = authorizedRoles == null || helper.Auth.IsInRoles(authorizedRoles);

        //    //if (!isInRoles)
        //     //   return MvcHtmlString.Empty;

        //    var model = new ActionLinkDeleteEntityModel()
        //    {
        //        LinkCss = linkCss,
        //        LinkText = linkText,
        //        IconCss = iconCss,
        //        //IconSrc = "Trash-Icon.png",
        //        DismissDialog = dismissDialog,
        //        ConfirmMessage = confirmMessage,
        //        DeleteActionUri = MvcHtmlString.Create(deleteActionPostUri),
        //        RefreshContainerActionUri = MvcHtmlString.Create(refreshContainerActionUri),
        //        RefreshContainerTargetId = refreshContainerTargetId,
        //        OnSucessCallback = onSuccessCallback
        //    };

        //    return helper.UserControlPartialInternal("_ActionLinkDeleteItem", model);
        //}



        public static MvcHtmlString UcActionLink(
            this AjaxHelper helper,
            string innerHtml, 
            string action, 
            string controller, 
            object routeValues, 
            AjaxOptions ajaxOptions, 
            object htmlAttributes)
        {
            return UcActionLink(
                helper, 
                innerHtml, 
                action, 
                controller, 
                routeValues, 
                ajaxOptions,
                HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString UcActionLinkButton(this HtmlHelper helper,
            string innerHtml,
            string action,
            string controller,
            object routeValues,
            string updateTargetId,
            bool showLoadingInTarget)
        {
            return helper.UcActionLink(innerHtml, action, controller,
                routeValues: routeValues,
                htmlAttributes: new { @class="btn btn-default btn-xs"},
                updateTargetId: updateTargetId,
                showLoadingInTarget: showLoadingInTarget
                );
        }

        public static MvcHtmlString UcActionLink(this HtmlHelper helper,
            string innerHtml, 
            string action, 
            string controller,
            object routeValues,
            object htmlAttributes, 
            string updateTargetId, 
            bool showLoadingInTarget)
        {
            var ajaxHelper = ViewExtensions.CurrentAjaxHelper;

            var ajaxOptions = ActionLinkAjaxOptions(showLoadingInTarget, updateTargetId);

            return ajaxHelper.UcActionLink(
                innerHtml, 
                action, 
                controller,
                routeValues: routeValues,
                htmlAttributes: htmlAttributes,
                ajaxOptions: ajaxOptions);
        }

        public static MvcHtmlString UcActionLink(this AjaxHelper helper,
            string innerHtml, 
            string action, 
            string controller, 
            object routeValues, 
            AjaxOptions ajaxOptions, 
            RouteValueDictionary htmlAttributes)
        {
            string holder = Guid.NewGuid().ToString(); // again, why is this ?
            var routesdic = HtmlHelper.AnonymousObjectToHtmlAttributes(routeValues);
            var anchor =helper.ActionLink(holder, action, controller, routesdic, ajaxOptions, htmlAttributes).ToString();
            return MvcHtmlString.Create(anchor.Replace(holder, innerHtml));
        }

        public static MvcHtmlString UcActionLink(this HtmlHelper helper,
            string innerHtml, string action, string controller, object routeValues, object htmlAttributes)
        {
            string holder = Guid.NewGuid().ToString();// why a random id? - DPS-REVIEW , do we even need this helper?
            string anchor = helper.ActionLink(holder, action, controller, routeValues, htmlAttributes).ToString();
            return MvcHtmlString.Create(anchor.Replace(holder, innerHtml));
        }

        /*
        public static MvcHtmlString UcActionLinkOnTab(this HtmlHelper helper,string innerHtml, string url,string tabParentId,string tabContentId,string contentTargetId,string tabId,string tabTitle ,object htmlAttributes=null)
        {
            var anchor = new TagBuilder("a") {InnerHtml = innerHtml};
            var adds = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes ?? new {});
            anchor.Attributes.Add("data-tab-link","true");
            anchor.Attributes.Add("data-tab-parentId", tabParentId);
            anchor.Attributes.Add("data-tab-contentId", tabContentId);
            anchor.Attributes.Add("data-tab-contentTargetId", contentTargetId);
            anchor.Attributes.Add("data-tab-url", url);
            anchor.Attributes.Add("data-tab-tabId",tabId);
            anchor.Attributes.Add("data-tab-title", tabTitle);
            anchor.MergeAttributes(adds);
            return MvcHtmlString.Create(anchor.ToString(TagRenderMode.Normal));
        }*/

        public static MvcHtmlString UcActionLink(
            this HtmlHelper helper,
            string text, 
            string action, 
            string controller, 
            object routeValues, 
            string iconStyle,
            string anchorStyle="",
            object htmlAttributes = null)
        {
            object htmlAtrtrs = new { @class = anchorStyle };
            if (htmlAttributes != null)
            {
                htmlAtrtrs = new { htmlAttributes , @class = anchorStyle};
            }

            string innerHtml = string.Format("<i class='{0}'></i> {1}", iconStyle, text);

            return helper.UcActionLink(innerHtml, action, controller, routeValues, htmlAtrtrs);
        }

        public static MvcHtmlString UcActionLink(
            this AjaxHelper helper,
            string text, 
            string action, 
            string controller, 
            object routeValues, 
            string anchorStyle, 
            string iconStyle,
            AjaxOptions options,
            RouteValueDictionary htmlAttributes)
        {
            string innerHtml = string.Format("<i class='{0}'></i> {1}", iconStyle, text);

            return helper.UcActionLink(innerHtml, action, controller, routeValues, options, new { @class = anchorStyle });
        }

        /*public static MvcHtmlString UcActionLinkImageIcon(this AjaxHelper helper, string action, string controller, object routeValues, string anchorStyle,string imgIconName, AjaxOptions options)
        {
            string innerHtml = string.Format("<img src='"+StyleExtensions.IconsSrcPath()+"{0}' alt='icon'/>",imgIconName);
            //string innerHtml = string.Format("<div class='{0}' style='width:13px;height:8px;'/>", iconClass);
            return helper.UcActionLink(innerHtml, action, controller, routeValues, options, new { @class = anchorStyle });// + iconClass
        }*/

        private static AjaxOptions ActionLinkAjaxOptions(bool showLoadingInTarget, string updateTargetId)
        {
            var ajaxOptions = new AjaxOptions()
            {
                InsertionMode = InsertionMode.Replace,
                UpdateTargetId = updateTargetId,
                HttpMethod = "GET"
            };

            if (showLoadingInTarget)
                ajaxOptions.OnBegin = "AjaxHelper.SetLoadingOnTarget('#" + updateTargetId + "')";
            else
                ajaxOptions.LoadingElementId = ScriptHelper.MarkupIDs.AjaxLoadingOverlay;

            return ajaxOptions;
        }

        #endregion

        #region Messages
        /*
        private const string MessagesKey = "ViewMessages";

        public static void AddMessage(this Controllers.ControllerBase controller, ViewMessageModel model)
        {
            var messages = (controller.ViewData[MessagesKey] ?? new List<ViewMessageModel>())
                as IList<ViewMessageModel>;

            messages.Add(model);

            controller.ViewData[MessagesKey] = messages;
        }

        public static bool UcHasMessages(this HtmlHelper helper)
        {
            var messages = helper.ViewData[MessagesKey] as IList<ViewMessageModel>;

            return messages != null && messages.Count > 0;
        }

        public static MvcHtmlString UcMessages(this HtmlHelper helper)
        {
            var messages = helper.ViewData[MessagesKey] as IList<ViewMessageModel>;

            helper.ViewData[MessagesKey] = null;

            if(messages != null && messages.Count > 0)
                return helper.UserControlPartialInternal("_ViewMessages",messages);
            return MvcHtmlString.Empty;
        }*/

        public static MvcHtmlString UcMessageInfo(this HtmlHelper helper, IHtmlString content)
        {
            return helper.UserControlPartialInternal("_MessageInfo", content);
        }

        public static MvcHtmlString UcMessageInfo(this HtmlHelper helper, string content)
        {
            return helper.UserControlPartialInternal("_MessageInfo", helper.Raw(content));
        }

        public static MvcHtmlString UcMessageDanger(this HtmlHelper helper, string content)
        {
            return helper.UserControlPartialInternal("_MessageDanger", helper.Raw(content));
        }

        #endregion

        #region containers

        public static IDisposable BeginFullColContainer(this HtmlHelper helper,int xs=12,int sm=12,int md=12,int lg=12,string id="",string additionalclasses="")
        {
            TextWriter writer = helper.ViewContext.Writer;
            var layout = helper.GetCol(xs, sm, md, lg);
            writer.WriteLine(
                "<div class='{0} {1}' id='{2}'>",layout,additionalclasses,id);
            return new Wrapper(helper,w=> w.WriteLine("</div>"));
        }

        public static IDisposable BeginColContainer(this HtmlHelper helper, int col=12, string id = "", string additionalclasses = "",string style=null)
        {
            TextWriter writer = helper.ViewContext.Writer;
            var layout = helper.GetCol(col,col,col,col);
            string varStyle = null;
            if (style != null)
                varStyle = string.Format("style='{0}'", style);

            writer.WriteLine(
                "<div class='{0} {1}' id='{2}' {3}>", layout, additionalclasses, id, varStyle);
            return new Wrapper(helper, w => w.WriteLine("</div>"));
        }

        #endregion

        #region User Control Internal Helper Methods



        private static MvcHtmlString UcFormSubmitButtonsInternal<TModel>(
           this HtmlHelper<TModel> helper,
           MvcHtmlString cancelButton,
           bool excludeApplyButton = false,
           bool excludeSaveButton = false
            ) where TModel : IViewModel
        {
            var model = new FormSubmitButton()
            {
                //IsAuthorized = AuthenticationHelper.UserCanWrite(),
                CancelButton = cancelButton,
                ExludeApplyButton = excludeApplyButton,
                ExcludeSaveButton = excludeSaveButton
            };

            return helper.UserControlPartialInternal("_FormSubmitButton", model);
        }


        private static SelectListItem[] DropDownListSingleItem
        {
            get 
            {
                return new[] { new SelectListItem { Text = "", Value = "" } };
            }
        }

        private static Type GetNonNullableModelType(ModelMetadata modelMetadata)
        {
            Type realModelType = modelMetadata.ModelType;

            Type underlyingType = Nullable.GetUnderlyingType(realModelType);
            if (underlyingType != null)
            {
                realModelType = underlyingType;
            }
            return realModelType;
        }

        internal static SelectList GetSelectListForEnumInternal<TModel, TEnum>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TEnum>> expression)
            //where TEnum : struct
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            Type enumType = GetNonNullableModelType(metadata);
            IEnumerable<TEnum> values = Enum.GetValues(enumType).Cast<TEnum>();

            IEnumerable<SelectListItem> items =
                values.Select(value => new SelectListItem
                {
                    Text = value.ToString(),
                    Value = value.ToString(),
                    Selected = value.Equals(metadata.Model)
                });

            if (metadata.IsNullableValueType)
            {
                items = DropDownListSingleItem.Concat(items);
            }

            return new SelectList(items,
                    selectedValue: metadata.Model,
                    dataTextField: "Text",
                    dataValueField: "Value");
        }

        private static MvcHtmlString FormControlGenericInternal(this HtmlHelper helper, FormControlGenericModel model)
        {
            return helper.UserControlPartialInternal("_FormControlGeneric", model);
        }

        internal static IDictionary<string, object> GetFormHtmlAttributes(
            bool omitFormControlStyle = false,
            string placeHolder = null,
            int rows = 0,
            bool inputSmall = true,
            bool isReadOnly = false)
        {
            var dictionary = new Dictionary<string, object> 
            { 
                { "class", 
                    (omitFormControlStyle == false ? "form-control" : string.Empty) + 
                    (inputSmall == true ? " input-sm" : "")
                }
            };

            if (!string.IsNullOrWhiteSpace(placeHolder))
                dictionary.Add("placeholder", placeHolder);

            if (rows > 0)
                dictionary.Add("rows", rows);

            if(isReadOnly)
                dictionary.Add("readonly" , "readonly");

            return dictionary;
        }

        internal static MvcHtmlString UserControlPartialInternal(this HtmlHelper helper, string viewName, object model)
        {
            return helper.Partial(UserControlPartialPathInternal(viewName), model);
        }

        internal static MvcHtmlString UserControlPartialInternal<TModel>(this HtmlHelper<TModel> helper, string viewName, object model)
        {
            return helper.Partial(UserControlPartialPathInternal(viewName), model);
        }

        private static string UserControlPartialPathInternal(string viewName)
        {
            return string.Format("~/Views/UserControls/{0}.cshtml", viewName);
        }


        #endregion

        #region Ko Helpers

       
        #endregion

        private static string GetButtonClass(ButtonType type)
        {
            return String.Format("btn-{0}", (type.ToString()).ToLowerInvariant());
        }
        private static string GetButtonSize(ButtonSize size)
        {
            return String.Format("btn-{0}", (size.ToString()).ToLowerInvariant());
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /*uc action link for custom icons from the Icons/ folder*/
        public static MvcHtmlString UcImageIconActionLink(this HtmlHelper helper, 
            string text,
            string action,
            string controller,
            object routeValues,
            string anchorStyle, 
            string iconimagesrc, 
            string updateTargetID)
        {
            var url = new UrlHelper(helper.ViewContext.RequestContext);
            string innerHtml = string.Format("<img src='{0}' alt='icon'/> {1}", url.Content(iconimagesrc), text);
            return helper.UcActionLink(innerHtml, action, controller,routeValues,null,updateTargetId: updateTargetID, showLoadingInTarget:true);
        }

        public static MvcHtmlString UcIconActionLink(this HtmlHelper helper, string text, string action,
            string controller, object routeValues, string anchorStyle, 
            string iconCSS, string updateTargetID)
        {
           
            /*var data = new RouteValueDictionary {{"class", anchorStyle}};
            if (addtionalAttributes != null)
            {
                data.AddRange(HtmlHelper.AnonymousObjectToHtmlAttributes(addtionalAttributes));
            }*/
            string innerHtml = string.Format("<i class='fa {0}'></i>", iconCSS);
            return helper.UcActionLink(innerHtml, action, controller, routeValues, null, updateTargetId: updateTargetID, showLoadingInTarget: true);

        }

        public static MvcHtmlString UcImageIconActionLink(this HtmlHelper helper, string text, string action, string controller, object routeValues, string anchorStyle, string iconImageName, string iconImageClasses = null,object htmlAttrs = null)
        {
            var iconImageClassesVar = iconImageClasses == null ? string.Empty : "class=" + iconImageClasses;
            var url = new UrlHelper(helper.ViewContext.RequestContext);
            string innerHtml = string.Format("<img src='" + StyleExtensions.IconsSrcPath() + "{0}' {2} alt='icon'/> {1}", url.Content(iconImageName), text, iconImageClassesVar);
             
            var atrs = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttrs);

            return helper.UcActionLink(innerHtml, action, controller, routeValues, new { @class = anchorStyle, atrs });
        }

        public static MvcHtmlString UcControlsListInline(this HtmlHelper helper, IEnumerable<MvcHtmlString> controls)
        {
            var content = new StringBuilder();
            content.AppendLine("<div class='action-buttons'>");
            controls.ToList().ForEach(c=>content.Append(c));
            content.AppendLine("</div>");
            return new MvcHtmlString(content.ToString());
        }

        public static MvcHtmlString UcImage(this HtmlHelper helper, string src,string title="",string alt="",object addtionalattributes=null)
        {
            var Url = new UrlHelper(helper.ViewContext.RequestContext);
            var img = new TagBuilder("img");
            img.Attributes["src"] = Url.Content(src);
            img.Attributes["alt"] = alt;
            img.Attributes["title"] = title;
            if (addtionalattributes != null)
            {
                img.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(addtionalattributes));
            }
            return new MvcHtmlString(img.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString UcIconStatisticsImage(this HtmlHelper helper,string name)
        {
            return helper.UcImage(String.Format("~/Content/images/Icons/{0}.jpg",name));
        }
        public static MvcHtmlString UcIconStatisticsImagePNG(this HtmlHelper helper,string name)
        {
            return helper.UcImage(String.Format("~/Content/images/Icons/{0}.png",name));
        }

        public static MvcHtmlString UcReportStatisticsImagePNG(this HtmlHelper helper, string name)
        {
            return helper.UcImage(String.Format("~/Content/images/Modules/Reports/{0}.png", name));
        }

        public static IDisposable BeginPanel(this HtmlHelper helper, MvcHtmlString title,
            MvcHtmlString topRightContent = null, MvcHtmlString searchFilter = null, string id = "", string additionalTitleClasses = "", string additionalclasses = "", bool addPadTop = true)
        {
            //dev note : pad-top-10 includes mar-bot-10 , so pad-bot is always added but pad-top depends on parameter
            TextWriter writer = helper.ViewContext.Writer;
            writer.WriteLine(
                "<div class='well content-panel {0}' id='{1}'>", additionalclasses, id);
            writer.WriteLine("<div class='row pad-lr-none "+(addPadTop?"pad-top-10":"marg-bot-10")+"'>");
            writer.WriteLine(string.Format("<span class='{1}'>{0}</span>", title.ToHtmlString(),additionalTitleClasses));            
                writer.WriteLine("<div class='float-right' style='display:inline-flex'>");
                    if (searchFilter != null)
                    {
                        writer.WriteLine("  <div class='float-left'>");
                        writer.WriteLine(searchFilter.ToHtmlString());
                        writer.WriteLine("   </div>");
                    }
                    if (topRightContent != null)
                    {
                        writer.Write(topRightContent.ToHtmlString());
                    }
                writer.WriteLine("</div>");
            writer.WriteLine("</div>");
            return new Wrapper(helper, w => w.WriteLine("</div>"));
        }

        public static IDisposable BeginPanelWithLeftBoldLabel(this HtmlHelper helper, string title,
            MvcHtmlString topRightContent = null, MvcHtmlString searchFilter = null, string id = "", 
            string additionalTitleClasses = "", string additionalclasses = "", bool addPadTop = true)
        {
            return helper.BeginPanel(MvcHtmlString.Create(title), topRightContent, searchFilter, id,
                additionalTitleClasses: "containerBoldTitle",
                additionalclasses: additionalclasses, 
                addPadTop:addPadTop);
        }


        //It creates a single image upload field + image preview (after upload)
        //using jquery.fileupload
        public static MvcHtmlString UcSingleImageUpload(this HtmlHelper helper,
        string actionUrl,
        string currentPhotoUrl,
        string additionalClasses = null,
        bool hideInput = false,
        string defaultPhotoUrl = null)
        {
            if (defaultPhotoUrl == null)
                defaultPhotoUrl = WebConstants.DefaultPhotoUrl;


            var model = new SingleImageUploadModel()
            {
                UploadActionUrl = actionUrl,
                CurrentPhotoUrl = currentPhotoUrl,
                DefaultImageUrl = defaultPhotoUrl,
                ContainerAddClasses = additionalClasses,
                HideInput = hideInput
            };

            return helper.UserControlPartialInternal("_SingleImageUpload", model);
        }
    }

    public enum ButtonType
    {
        Success,
        Info,
        Danger,
        Default,
        Warning,
        Primary
    }

    public enum ButtonSize
    {
        xs,
        sm,
        lg
    }

}