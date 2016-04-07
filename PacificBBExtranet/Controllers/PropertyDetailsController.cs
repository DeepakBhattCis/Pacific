using PacificBBExtranet.Services.Models;
using PacificBBExtranet.Services.Models.ResortModels;
using PacificBBExtranet.Services.PropertyDetails;
using PacificBBExtranet.Web.Helpers;
using PacificBBExtranet.Web.Helpers.Azure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PacificBBExtranet.Web.Controllers
{
    [Authorize]
    public class PropertyDetailsController : Controller
    {
        private readonly PropertyDetailsService _propertyDetailsService;
        public PropertyDetailsController(PropertyDetailsService propService)
        {
            _propertyDetailsService = propService;
        }
        public PropertyDetailsController() : this(ApplicationServices.PropertyDetailsService)
        {
        }

        // GET: PropertyDetails
        public ActionResult Index()
        {
            ApplicationServices.PropertyDetailsService.CheckAndFixAccount();
            var resortModel = _propertyDetailsService.GetUserResort();

            return View(resortModel);
        }



        [HttpPost]
        public ActionResult ResortEdit(ResortModel resort)
        {
            if (ModelState.IsValid)
            {
                _propertyDetailsService.Update(resort);

                return RedirectToAction("ResortEdit", new { success = true });
            }

            return PartialView("~/Views/PropertyDetails/_ResortEdit.cshtml", resort);
        }


        [HttpGet]
        public ActionResult ResortEdit(bool success = false)
        {
            var resort = _propertyDetailsService.GetUserResort();

            ViewBag.Success = success;
            return PartialView("~/Views/PropertyDetails/_ResortEdit.cshtml", resort);
        }

        public JsonResult UploadResortLogo()
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                var ResortModel = _propertyDetailsService.GetUserResort();
                var logoFileName = ResortModel.ResortID + "/logo";

                MemoryStream target = new MemoryStream();
                file.InputStream.CopyTo(target);
                byte[] fileData = target.ToArray();

                var url = new AzureHelper(WebConstants.Azure.connectionString)
                    .SaveToBlob(fileData, WebConstants.Azure.ResortsContainer, logoFileName, "image/jpg");

                _propertyDetailsService.UpdateResortLogo(resortID: ResortModel.ResortID, logoUrl: url);

                return Json(url, JsonRequestBehavior.AllowGet);
            }

            return null;
        }

        public JsonResult UploadResortImage(int imageID)
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                var ResortModel = _propertyDetailsService.GetUserResort();
                var logoFileName = ResortModel.ResortID + "/img-" + imageID;

                MemoryStream target = new MemoryStream();
                file.InputStream.CopyTo(target);
                byte[] fileData = target.ToArray();

                var url = new AzureHelper(WebConstants.Azure.connectionString)
                    .SaveToBlob(fileData, WebConstants.Azure.ResortsContainer, logoFileName, "image/jpg");

                _propertyDetailsService.UpdateResortImage(imageID: imageID, imageURL: url);

                return Json(url, JsonRequestBehavior.AllowGet);
            }

            return null;
        }

        public PartialViewResult ResortImages()
        {
            var images = _propertyDetailsService.GetResortImages();
            return PartialView("~/Views/PropertyDetails/_ResortImagesTab.cshtml", images);
        }



        #region Rooms


        [HttpPost]
        public ActionResult RoomNew(ResortRoomModel model)
        {
            if (ModelState.IsValid)
            {
                _propertyDetailsService.AddNewRoom(model);
                return RedirectToAction("RoomNew", new { success = true });
            }

            return PartialView("~/Views/PropertyDetails/_RoomForm.cshtml", model);
        }

        [HttpPost]
        public ActionResult RoomEdit(ResortRoomModel model)
        {
            if (ModelState.IsValid)
            {
                _propertyDetailsService.UpdateRoom(model);

                return RedirectToAction("RoomEdit", new { roomType = model.Type, success = true });
            }

            return PartialView("~/Views/PropertyDetails/_RoomForm.cshtml", model);
        }

        [HttpGet]
        public ActionResult RoomEdit(string roomType, bool success = false)
        {
            var roomModel = _propertyDetailsService.GetRoomModel(roomType);

            ViewBag.Success = success;
            return PartialView("~/Views/PropertyDetails/_RoomForm.cshtml", roomModel);
        }

        [HttpGet]
        public ActionResult RoomNew(bool success = false)
        {
            var roomModel = new ResortRoomModel();
            ViewBag.Success = success;
            return PartialView("~/Views/PropertyDetails/_RoomForm.cshtml", roomModel);
        }

        [HttpGet]
        public ActionResult RoomList()
        {
            var roomslist = _propertyDetailsService.GetRoomList();
            return PartialView("~/Views/PropertyDetails/_RoomList.cshtml", roomslist);
        }

        [HttpGet]
        public PartialViewResult RoomImages(string roomType)
        {
            var roomImages = _propertyDetailsService.GetRoomImages(roomType);
            ViewBag.RoomName = roomType;
            return PartialView("~/Views/PropertyDetails/_RoomImages.cshtml", roomImages);
        }

        public JsonResult UploadRoomImage(int imageID)
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                var ResortModel = _propertyDetailsService.GetUserResort();
                var logoFileName = ResortModel.ResortID + "/room/img-" + imageID;

                MemoryStream target = new MemoryStream();
                file.InputStream.CopyTo(target);
                byte[] fileData = target.ToArray();

                var url = new AzureHelper(WebConstants.Azure.connectionString)
                    .SaveToBlob(fileData, WebConstants.Azure.ResortsContainer, logoFileName, "image/jpg");

                _propertyDetailsService.UpdateRoomImage(imageID: imageID, imageURL: url);

                return Json(url, JsonRequestBehavior.AllowGet);
            }

            return null;
        }
        #endregion Rooms

        #region AvailabilityANDRates

        public PartialViewResult AvailabilityAndRatesTab()
        {
            AvailabilityAndRatesTabModel model = new AvailabilityAndRatesTabModel();
            model.RoomTypeList = _propertyDetailsService.GetRoomTypeList();
            return PartialView("~/Views/PropertyDetails/_AvailabilityAndRatesTabs.cshtml", model);
        }

        /*public PartialViewResult AvailabilityTab(string selectedRoomType)
        {
            AvailabilityAndRatesTabModel model = new AvailabilityAndRatesTabModel();
            model.selectedRoomType = selectedRoomType;
            model.RoomTypeList = _propertyDetailsService.GetRoomTypeList();
            return PartialView("~/Views/PropertyDetails/_AvailabilityAndRatesTab_Inner.cshtml", model);
        }*/

        public PartialViewResult AvailabilityGrid(string roomType)
        {
            //AvailabilityAndRatesTabModel model = new AvailabilityAndRatesTabModel();

            var DateValueList = _propertyDetailsService.GetAvailability(roomType);
            return PartialView("~/Views/Shared/_GridComponent.cshtml", DateValueList);
        }

        /* public PartialViewResult RatesTab(string selectedRoomType)
         {
             AvailabilityAndRatesTabModel model = new AvailabilityAndRatesTabModel();
             model.selectedRoomType = selectedRoomType;
             model.RoomTypeList = _propertyDetailsService.GetRoomTypeList();
             return PartialView("~/Views/PropertyDetails/_AvailabilityAndRatesTab_Inner.cshtml", model);
         }*/

        public PartialViewResult RatesGrid(string roomType)
        {
            var DateValueList = _propertyDetailsService.GetRates(roomType);
            return PartialView("~/Views/Shared/_GridComponent.cshtml", DateValueList);
        }

        public void UpdateRoomDateValues(
            List<CalendarDateValueModel> datesAvailability,
            string roomType, string availabilityORRates)
        {

           
            if (datesAvailability != null)
            {
                var standardRateID = _propertyDetailsService.UpdateRoomDateValues(datesAvailability, roomType, availabilityORRates);

                if (_propertyDetailsService.InstantUpdates())
                {
                    new SynchController().BookingSynchRate(standardRateID, true, datesAvailability);
                }
            }

           // ApplicationServices.XMLMessagesService.GetBookingXMLAvailabilityMessageToSend(roomType,fromDate:datesAvailability.First().date,toDate:datesAvailability.Last().date);

        }
        #endregion


        #region Addon

        [HttpPost]
        public ActionResult AddonNew(AddOnModel model)
        {
            if (ModelState.IsValid)
            {
                _propertyDetailsService.Create(model);
                return RedirectToAction("AddonNew", new { success = true });
            }

            return PartialView("~/Views/PropertyDetails/_AddonForm.cshtml", model);
        }

        [HttpPost]
        public ActionResult AddonEdit(AddOnModel model)
        {
            if (ModelState.IsValid)
            {
                _propertyDetailsService.Update(model);

                return RedirectToAction("AddonEdit", new { addonID = model.AddonID , success = true });
            }

            return PartialView("~/Views/PropertyDetails/_AddonForm.cshtml", model);
        }

        [HttpGet]
        public ActionResult AddonEdit(int addonID , bool success = false)
        {
            var addonModel = _propertyDetailsService.GetAddonModel(addonID);
            ViewBag.Success = success;
            return PartialView("~/Views/PropertyDetails/_AddonForm.cshtml", addonModel);
        }

        [HttpGet]
        public ActionResult AddonNew(bool success = false)
        {
            var addonModel = new AddOnModel();
            ViewBag.Success = success;
            return PartialView("~/Views/PropertyDetails/_AddonForm.cshtml", addonModel);
        }

        [HttpGet]
        public ActionResult AddonList()
        {
            var roomslist = _propertyDetailsService.GetAddonList();
            return PartialView("~/Views/PropertyDetails/_AddonList.cshtml", roomslist);
        }



        #endregion Addon


    }
}