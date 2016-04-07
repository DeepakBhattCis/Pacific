using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PacificBBExtranet.Utils.ObjectExtensions;

namespace PacificBBExtranet.Web.UserControls
{
    public class DialogScriptTargetModel
    {
        public IHtmlString KeyControlID { get; set; }
        public IHtmlString ValueControlID { get; set; }
    }

    public class DialogTargetModel
    {
        public string ActionName { get; private set; }
        public string ControllerName { get; private set; }
        public object RouteValues { get; private set; }
        public string UpdateTargetId { get; private set; }

        public DialogTargetModel(string actionName, string controllerName, object routeValues)
        {
            ActionName = actionName;
            ControllerName = controllerName;
            RouteValues = routeValues;
        }

        public DialogTargetModel(string actionName, string controllerName, object routeValues, string updateTargetId)
            : this(actionName, controllerName, routeValues)
        {
            UpdateTargetId = updateTargetId;
        }

        public bool HasTarget()
        {
            return !UpdateTargetId.IsEmpty();
        }
    }
}