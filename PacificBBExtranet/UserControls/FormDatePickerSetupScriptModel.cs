
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PacificBBExtranet.Web.UserControls
{
    public class FormDatePickerSetupScriptModel
    {
        public IHtmlString ControlId { get; set; }
        public string Position { get; set; }
        public bool IsMultiDay { get; set; }
        public string dataSeparator { get; set; }
    }
}