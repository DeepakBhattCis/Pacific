

using System;
using System.Web.Mvc;

namespace PacificBBExtranet.Web.UserControls
{
    public class FormControlGenericModel
    {
        public bool ShowValidationMessageBelow { get; set; }

        public MvcHtmlString ValidationMessageFor { get; set; }

        public MvcHtmlString ControlFor { get; set; }

        public string ControlName { get; set; }

        public string Label { get; set; }

        public int LabelColDimension { get; set; }

        public FormControlGenericModel()
        {
            ShowValidationMessageBelow = true;
        }
    }
}