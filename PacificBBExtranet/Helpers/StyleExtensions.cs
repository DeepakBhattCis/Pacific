
using PacificBBExtranet.Utils.ObjectExtensions;
using System;
using System.Security.Policy;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace PacificBBExtranet.Web.Helpers
{
    public static class StyleExtensions
    {
        public static MvcHtmlString StyleCol(this HtmlHelper helper, int coli)
        {
            return MvcHtmlString.Create(string.Format("col-sm-{0} col-md-{0} col-lg-{0} col-xs-{0}", coli));
        }

        public static MvcHtmlString StyleCol(this HtmlHelper helper, int small , int mid , int large)
        {
            return MvcHtmlString.Create(string.Format("col-xs-{0} col-sm-{0} col-md-{1} col-lg-{2} ", small,mid,large));
        }

        public static string GetCol(this HtmlHelper helper, int xs=12,int sm=12,int md=12,int lg=12)
        {
            return string.Format("col-xs-{0} col-sm-{1} col-md-{2} col-lg-{3}",xs,sm,md,lg);
        }

        public static MvcHtmlString StyleColOffset(this HtmlHelper helper, int coli)
        {
            return MvcHtmlString.Create(string.Format("col-sm-offset-{0} col-md-offset-{0} col-lg-offset-{0}", coli));
        }

        public static MvcHtmlString ClassIfTrue(this HtmlHelper helper, string classes, bool expression)
        {
            return new MvcHtmlString(classes.IfTrue(expression));
        }

        #region Tables

        public static string StyleTable(bool condensed = true, bool extraCondensed=false, bool fixedHeader=false)
        {
            return "table table-striped" + (
                condensed ? " table-condensed table-condensed-sm" : string.Empty ) +
                (fixedHeader ? " fixed-header" : string.Empty)
            ;
        }

        public static MvcHtmlString StyleTable(this HtmlHelper helper,
            bool condensed = true, bool extraCondensed = false,bool fixedHeader=false)
        {
            return MvcHtmlString.Create(StyleTable(condensed, extraCondensed,fixedHeader));
        }

        #endregion Tables

        #region Form Styles

        public static string StyleDatePicker(this HtmlHelper helper)
        {
            return "form-datepicker";
        }

        public static MvcHtmlString StyleInput(this HtmlHelper helper)
        {
            return MvcHtmlString.Create("form-control input-sm");
        }

        public static string StyleFormHorizontal(this HtmlHelper helper, bool extraCondensed = false)
        {
            return "form-horizontal condensed" + (extraCondensed ? " xs" : "");
        }


        #endregion

        #region Buttons

        public static string StyleButtonLargeInfo(this HtmlHelper helper)
        {
            return "btn btn-large btn-info";
        }

        public static string StyleButtonSmallInfo(this HtmlHelper helper, bool xs=false)
        {
            return string.Format("btn {0} btn-info", xs ? "btn-xs" : "btn-sm");
        }

        public static string StyleButtonPrint(this HtmlHelper helper, bool sm = false)
        {
            return string.Format("btn {0} btn-default", sm ? "btn-sm" : "");
        }

        public static string StyleButton(this HtmlHelper helper, bool small, bool xs = false)
        {
            return "btn btn-default" + (small ? " btn-sm" : string.Empty) + (xs ? " btn-xs" : string.Empty);
        }

        #endregion

        
        #region Icons

        public static string StyleIconBoolean(bool isTrue)
        {
            return isTrue ? "fa fa-check text-success" : "fa fa-times text-danger";
        }

        public static string StyleIconSlideLib()
        {
            return "fa fa-file-powerpoint-o";
        }

        public static string StyleIconProjectView(this HtmlHelper helper)
        {
            return "fa fa-barcode";
        }

        public static string StyleIconPlus(this HtmlHelper helper)
        {
            return "fa fa-plus";
        }

        public static string StyleIconTimes(this HtmlHelper helper)
        {
            return "fa fa-times";
        }

        public static string StyleIconReports(this HtmlHelper helper)
        {
            return "fa fa-bar-chart-o";
        }

        public static string StyleIconDocuments(this HtmlHelper helper)
        {
            return "fa fa-folder-open";
        }

        public static string StyleIconDownload(this HtmlHelper helper)
        {
            return "fa fa-download";
        }

        /*public static string StyleIconCheckInBox(this HtmlHelper helper)
        {
            return "fa fa-check-square-o";
        }*/

        public static string StyleIconEdit(this HtmlHelper helper)
        {
            return "icon edit-icon";
        }

        public static string StyleIconHistory(this HtmlHelper helper)
        {
            return "fa fa-list";
        }

        public static string StyleIconNewFolder(this HtmlHelper helper)
        {
            return "fa fa-folder-open-o";
        }

        public static string StyleIconNewFile(this HtmlHelper helper)
        {
            return "fa fa-file-text-o";
        }

        public static string StyleIconCheckout(this HtmlHelper helper)
        {
            return "fa fa-check-square-o";
        }

        public static string StyleIconCheckin(this HtmlHelper helper)
        {
            return "fa fa-check-circle-o";
        }

        public static string StyleIconDelete(this HtmlHelper helper)
        {
            return "fa fa-times";
        }
        public static string StyleIconUndo(this HtmlHelper helper)
        {
            return "fa fa-undo";
        }
        public static string StyleIconMove(this HtmlHelper helper)
        {
            return "fa fa-archive";
        }

        public static string DeleteIcon(this HtmlHelper helper)
        {
            return "Trash-Icon.png";
        }

        public static string ImageEditIcon(this HtmlHelper helper)
        {
            return "Edit-Icon.png";
        }

        public static string ViewEyeIcon(this HtmlHelper helper)
        {
            return "View-Icon.png";
        }

        public static string NewIcon(this HtmlHelper helper)
        {
            return "newIcon.png";
        }

        public static string FullScreenIcon(this HtmlHelper helper)
        {
            return "view-full-screen.png";
        }
        

        public static string IconsSrcPath()
        {
            return "/Content/images/Icons/";
        }

        public static string LabelsSrcPath()
        {
            return "~/Content/images/Labels/";
        }

        public static string ProjectModuleContentSrcPath()
        {
            return "~/Content/images/Modules/Project/";
        }
        

        #endregion

        #region "Mime Icons"

        public static string MimeFolder(this HtmlHelper helper)
        {
            return "<img style=\"margin-top:-4px;\" src=\"" + UrlHelper.GenerateContentUrl("~/images/folder.png", helper.ViewContext.HttpContext)  + "\"/>";
        }

        public static string MimeIcon(this HtmlHelper helper, string extension)
        {
            switch (extension.ToLower())
            {
                //Images
                case ".png":
                case ".jpg":
                case ".bmp":
                case ".gif":
                case ".jpeg":
                case ".tif":
                case ".tiff":
                    return "<img src=\"" + UrlHelper.GenerateContentUrl("~/images/imagefile.png", helper.ViewContext.HttpContext) + "\"/>";
                //Office
                case ".txt":
                case ".rtf":
                case ".csv":
                case ".doc":
                case ".dot":
                case ".docx":
                case ".dotx":
                case ".docm":
                case ".dotm":
                case ".xls":
                case ".xlt":
                case ".xla":
                case ".xlsx":
                case ".xltx":
                case ".xlsm":
                case ".xltm":
                case ".xlam":
                case ".xlsb":
                case ".ppt":
                case ".pot":
                case ".pps":
                case ".ppa":
                case ".pptx":
                case ".potx":
                case ".ppsx":
                case ".ppam":
                case ".pptm":
                case ".potm":
                case ".ppsm":
                    return "<img src=\"" + UrlHelper.GenerateContentUrl("~/images/officefile.png", helper.ViewContext.HttpContext) + "\"/>";
                //PDF
                case ".pdf":
                    return "<img src=\"" + UrlHelper.GenerateContentUrl("~/images/pdffile.png", helper.ViewContext.HttpContext) + "\"/>";
                //Video
                case ".3g2":
                case ".3gp":
                case ".asf":
                case ".asx":
                case ".avi":
                case ".flv":
                case ".m4v":
                case ".mov":
                case ".mp4":
                case ".mpg":
                case ".rm":
                case ".srt":
                case ".swf":
                case ".vob":
                case ".wmv":
                    return "<img src=\"" + UrlHelper.GenerateContentUrl("~/images/videofile.png", helper.ViewContext.HttpContext) + "\"/>";
                //Audio
                case ".aif":
                case ".iff":
                case ".m3u":
                case ".m4a":
                case ".mid":
                case ".mp3":
                case ".mpa":
                case ".ra":
                case ".wav":
                case ".wma":
                    return "<img src=\"" + UrlHelper.GenerateContentUrl("~/images/audiofile.png", helper.ViewContext.HttpContext) + "\"/>";
                default:
                    return "<img src=\"" + UrlHelper.GenerateContentUrl("~/images/genericfile.png", helper.ViewContext.HttpContext) + "\"/>";
            }
        }

        #endregion

    }
}