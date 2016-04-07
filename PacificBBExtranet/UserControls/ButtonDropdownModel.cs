
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PacificBBExtranet.Web.UserControls
{
    public interface IButtonDropdownItemModel
    {
        MvcHtmlString Html();
    }

    public class ButtonDropdownItemHtmlModel : IButtonDropdownItemModel
    {
        protected readonly MvcHtmlString _html;

        public ButtonDropdownItemHtmlModel(MvcHtmlString html)
        {
            _html = html;
        }

        public MvcHtmlString Html()
        {
            return _html;
        }
    }

    public class ButtonDropdownItemDisabledModel : ButtonDropdownItemHtmlModel
    {
        public ButtonDropdownItemDisabledModel(string text, string iconcss)
            : base(getHtml(text, iconcss)) { }

        private static MvcHtmlString getHtml(string text, string iconCss)
        {
            return MvcHtmlString.Create("<a onclick=\"javascript:bootbox.dialog({message:'Not Implemented'});\" >" +
                "<i class='" + iconCss + "'></i> " +
                "<span class='text-muted'>" + text + "</span></a>");
        }
    }

    public class ButtonDropdownModel
    {
        public string Text { get; set; }
        public string ButtonCssStyle { get; set; }
        public string ButtonCssSize { get; set; }
        public bool PullLeft { get; set; }
        public int MaxWidth { get; set; }
        public ButtonDropdownModel() { }
        public IList<IButtonDropdownItemModel> Items { get; set; }
        public MvcHtmlString LeftHtml { get; set; }

        public string IconCss { get; set; }


    }
}