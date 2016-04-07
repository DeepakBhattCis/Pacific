
using System;

namespace PacificBBExtranet.Web.UserControls
{
    public class SearchFilterModel
    {
        public SearchFilterModel()
        {
            Hint = string.Empty;
        }

        public string UpdateTargetId { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }

        public string Hint { get; set; }

        public string SearchPlaceholder { get; set; }

        public object RouteValues { get; set; }
    }
}