using PacificBBExtranet.Services.Models.ResortModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PacificBBExtranet.Web.Helpers
{
    public static class ModelExtensions
    {

        public static MvcHtmlString RoomListActions<TModel>(this HtmlHelper<TModel> helper, ResortRoomModel model)
            where TModel : List<ResortRoomModel>
        {

            var actionsmarkup = string.Format("{0}&nbsp{1}&nbsp{2}",
                helper.UcIconActionLink(
                     text: "", action: "RoomEdit", controller: "PropertyDetails", routeValues:
                            new { roomType = model.Type },
                            updateTargetID: "id_roomFormContainer", iconCSS: "fa-pencil", anchorStyle: ""),
                helper.UcIconActionLink(
                     text: "", action: "RoomImages", controller: "PropertyDetails", routeValues:
                            new { roomType = model.Type },
                            updateTargetID: "id_roomFormContainer", iconCSS: "fa-image", anchorStyle: ""),
                helper.UcIconActionLink(
                     text: "", action: "RoomEdit", controller: "PropertyDetails", routeValues:
                            new { roomType = model.Type },
                            updateTargetID: "id_roomFormContainer", iconCSS: "fa-trash-o", anchorStyle: "")
                );

            return MvcHtmlString.Create(actionsmarkup);
        }

        public static MvcHtmlString AddonListActions<TModel>(this HtmlHelper<TModel> helper, AddOnModel model)
           where TModel : List<AddOnModel>
        {

            var actionsmarkup = string.Format("{0}&nbsp{1}",
                helper.UcIconActionLink(
                     text: "", action: "AddonEdit", controller: "PropertyDetails", routeValues:
                            new { addonID = model.AddonID },
                            updateTargetID: "id_addonFormContainer", iconCSS: "fa-pencil", anchorStyle: ""),
                helper.UcIconActionLink(
                     text: "", action: "RoomEdit", controller: "PropertyDetails", routeValues:
                            new { addonID = model.AddonID },
                            updateTargetID: "id_addonFormContainer", iconCSS: "fa-trash-o", anchorStyle: "")
                );

            return MvcHtmlString.Create(actionsmarkup);
        }
    }
}
