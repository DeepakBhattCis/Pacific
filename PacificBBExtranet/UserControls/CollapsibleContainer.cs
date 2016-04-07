using PacificBBExtranet.Web.Helpers;
using System;
using System.IO;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace PacificBBExtranet.Web.UserControls
{
    public class Wrapper : IDisposable
    {
        private bool disposed;
        private readonly TextWriter writer;

        public Action<TextWriter> DisposeAction { get; set; }

        public Wrapper(HtmlHelper html, Action<TextWriter> disposeAction)
        {
            this.writer = html.ViewContext.Writer;
            DisposeAction = disposeAction;
        }

        public Wrapper(HtmlHelper html)
            : this(html, (w) =>
            {
                w.WriteLine("   </div>");
                w.WriteLine("</div>");
            })
        {
        }

        public void Dispose()
        {
            if (disposed) return;

            disposed = true;
            DisposeAction(writer);
        }
    }
    public static class CollapsibleContainer
    {

        public static IDisposable BeginCollapsibleRawTitleContainer(this HtmlHelper helper,
    string titlehtml,
    bool isCollapsable = true,
    bool isRemovable = false,
    bool startCollapsed = false,
    bool isSubContainer = false,
    string boxAdditionalClasses = "",
    string boxIconCss = "fa-align-justify",
    string contentId = null,
    MvcHtmlString additionalheaderIcons = null,
    string topRightIconID = "",
    MvcHtmlString headerButton = null,
            string boxHeaderStyle = "",
            string boxId = "", string boxcontentClasses = "")
        {


            TextWriter writer = helper.ViewContext.Writer;

            writer.WriteLine(
                "<div class='box small clearfix "+ boxAdditionalClasses + "' id='" + boxId + "'>");
            writer.WriteLine(String.Format("    <div class='box-header' style='{0}'>", boxHeaderStyle));
            writer.WriteLine("        <h2>");

            if (!string.IsNullOrEmpty(boxIconCss))
            {
                writer.WriteLine(
                    string.Format("           <i class='fa {0} dark'></i>", boxIconCss));
                writer.WriteLine("            <span class='break'></span>");
            }

            writer.WriteLine("            " + titlehtml);
            writer.WriteLine("        </h2>");


            //Button
            if (headerButton != null)
            {
                writer.WriteLine(headerButton);
            }

            writer.WriteLine("        <div class='box-icon'>");

            //Insert Additional buttons
            if (additionalheaderIcons != null)
                writer.WriteLine(additionalheaderIcons.ToHtmlString());

            writer.WriteLine(
                string.Format("            <a id='{2}' href='#' class='btn-minimize' {0}><i class='fa {1}'></i></a>",
                isCollapsable ? string.Empty : "style='display:none'",
                startCollapsed ? "fa-chevron-down" : "fa-chevron-up", topRightIconID));

            if (isRemovable)
                writer.WriteLine("            <a href='#' class='btn-close'><i class='fa fa-times'></i></a>");

            writer.WriteLine("        </div>");
            writer.WriteLine("    </div>");
            writer.WriteLine(string.Format("    <div class='box-content clearfix {2}' {0} {1}>",
                startCollapsed ? "style='display:none'" : string.Empty,
                string.IsNullOrWhiteSpace(contentId) ? "" : string.Format("id='{0}'", contentId), boxcontentClasses));

            return new Wrapper(helper);
        }



        public static IDisposable BeginCollapsibleContainer(this HtmlHelper helper,
            string title,
            bool isCollapsable = true,
            bool isRemovable = false,
            bool startCollapsed = false,
            bool isSubContainer = false,
            string boxAdditionalClasses = "",
            string boxIconCss = null,
            string contentId = null,
            MvcHtmlString additionalheaderIcons = null,
            string topRightIconID = "",
            MvcHtmlString headerButton = null, string boxcontentClasses = "")
        {

            return BeginCollapsibleRawTitleContainer(helper, helper.Encode(title), isCollapsable, isRemovable, startCollapsed,
                isSubContainer, boxAdditionalClasses, boxIconCss, contentId, additionalheaderIcons, topRightIconID, headerButton, boxcontentClasses: boxcontentClasses);

        }

        public static IDisposable BeginCollapsibleBoldContainer(this HtmlHelper helper,
            string title,
            bool isCollapsable = true,
            bool isRemovable = false,
            bool startCollapsed = false,
            bool isSubContainer = false,
            string boxAdditionalClasses = "",
            string boxIconCss = null,
            string contentId = null,
            MvcHtmlString additionalheaderIcons = null,
            string topRightIconID = "",
            MvcHtmlString headerButton = null, string boxcontentClasses = "")
        {
            var titlehtml = new TagBuilder("span") { InnerHtml = title };
            titlehtml.Attributes.Add("style", "font-weight:bold;");

            return BeginCollapsibleRawTitleContainer(helper, titlehtml.ToString(), isCollapsable, isRemovable, startCollapsed,
                isSubContainer, boxAdditionalClasses, boxIconCss, contentId, additionalheaderIcons, topRightIconID, headerButton, boxcontentClasses: boxcontentClasses);

        }

        public static IDisposable BeginCollapsibleBlockedContainer(this HtmlHelper helper,
            string title,
            bool isRemovable = false,
            bool isBlocked = true,
            bool isSubContainer = false,
            string boxAdditionalClasses = "",
            string boxIconCss = null,
            string contentId = null,
            MvcHtmlString additiontalIcons = null)
        {
            return BeginCollapsibleContainer(helper, title,
                !isBlocked, isRemovable,
                isBlocked, isSubContainer,
                boxAdditionalClasses, boxIconCss, contentId, additionalheaderIcons: additiontalIcons);
        }
    }
}