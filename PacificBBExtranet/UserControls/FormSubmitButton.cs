using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PacificBBExtranet.Web.UserControls
{
    public class FormSubmitButton
    {
        public bool IsAuthorized { get; set; }

        public MvcHtmlString CancelButton { get; set; }

        public bool ExludeApplyButton { get; set; }

        public bool ExcludeSaveButton { get; set; }

        public object CancelRouteValues { get; set; }

        public string ApplyButtonText { get; set; }

        public string SaveButtonText { get; set; }

        public FormSubmitButton() {
            SaveButtonText = "Submit";
            ApplyButtonText = "Apply";
            IsAuthorized = true;
        }
    }
}