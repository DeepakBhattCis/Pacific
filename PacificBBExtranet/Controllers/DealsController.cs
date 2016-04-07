using PacificBBExtranet.Services.Models;
using PacificBBExtranet.Services.Models.Deals;
using PacificBBExtranet.Services.Services.Deals;
using PacificBBExtranet.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PacificBBExtranet.Web.Controllers
{
    [Authorize]
    public class DealsController : Controller
    {
        public readonly DealsService _dealsService;
        public DealsController(DealsService dealsService) {
            _dealsService = dealsService;
        }

        public DealsController():this(ApplicationServices.DealsServices) { }
        // GET: Deals
        public ActionResult Index()
        {
            DealsIndexModel dealsModel = new DealsIndexModel();
            dealsModel.SpecialsList = _dealsService.GetSpecialsList();

            return View(dealsModel);
        }

        [HttpGet]
        public PartialViewResult SpecialNew(bool success = false)
        {
            var specialNewModel = new SpecialModel();
            specialNewModel.RoomList = ApplicationServices.PropertyDetailsService.GetRoomTypeList();
            ViewBag.Success = success;
            return PartialView("~/Views/Deals/_SpecialNew.cshtml", specialNewModel);
        }
        [HttpPost]
        public ActionResult SpecialNew(SpecialModel model)
        {
            if (ModelState.IsValid)
            {
                _dealsService.Create(model);
                return RedirectToAction("SpecialNew", new { success = true});
            }
            model.RoomList = ApplicationServices.PropertyDetailsService.GetRoomTypeList();
            return PartialView("~/Views/Deals/_SpecialNew.cshtml", model);
        }

        public PartialViewResult SpecialsDropDown(string selectedSpecialID = null)
        {
            DealsIndexModel dealsModel = new DealsIndexModel();
            dealsModel.SpecialsList = _dealsService.GetSpecialsList();
            dealsModel.SelectedSpecial = selectedSpecialID;

            return PartialView("~/Views/Deals/_SpecialsDropDown.cshtml", dealsModel);
        }

        //get availability given a special
        public PartialViewResult AvailabilityGrid(string specialID)
        {
            Dictionary<DateTime, decimal> DateValueList = new Dictionary<DateTime, decimal>();
            int parsed = 0;
            if (int.TryParse(specialID, out parsed))
            {
                DateValueList = _dealsService.GetAvailability(Convert.ToInt32(specialID));
            }
            return PartialView("~/Views/Shared/_GridComponent.cshtml", DateValueList);
        }

        public PartialViewResult RatesGrid(int specialID)
        {
            Dictionary<DateTime, decimal> DateValueList = new Dictionary<DateTime, decimal>();

            DateValueList = _dealsService.GetSpecialCalendarInfo(specialID, "rates");
            
            return PartialView("~/Views/Shared/_GridComponent.cshtml", DateValueList);
        }

        public PartialViewResult PersonRateGrid(int specialID)
        {
         
                Dictionary<DateTime, decimal> DateValueList = new Dictionary<DateTime, decimal>();
                DateValueList = _dealsService.GetSpecialCalendarInfo(specialID, "price1");
                return PartialView("~/Views/Shared/_GridComponent.cshtml", DateValueList);
            
        }

        public PartialViewResult MinNightsGrid(int specialID)
        {
            Dictionary<DateTime, decimal> DateValueList = new Dictionary<DateTime, decimal>();
            DateValueList = _dealsService.GetSpecialCalendarInfo(specialID, "minnights");
         

           
            return PartialView("~/Views/Shared/_GridComponent.cshtml", DateValueList);
        }

        public PartialViewResult MaxNightsGrid(int specialID)
        {
            Dictionary<DateTime, decimal> DateValueList = new Dictionary<DateTime, decimal>();
            DateValueList = _dealsService.GetSpecialCalendarInfo(specialID, "maxnights");

            return PartialView("~/Views/Shared/_GridComponent.cshtml", DateValueList);
        }

        public PartialViewResult StopSellGrid(int specialID)
        {
            Dictionary<DateTime, decimal> DateValueList = new Dictionary<DateTime, decimal>();
            DateValueList = _dealsService.GetSpecialCalendarInfo(Convert.ToInt32(specialID), "stopsell");

            return PartialView("~/Views/Shared/_GridComponent.cshtml", DateValueList);
        }

        public void UpdateSpecialRates(
            List<CalendarDateValueModel> datesAvailability,
            int specialID)
        {
            if (datesAvailability != null)
                _dealsService.UpdateSpecialRates(datesAvailability,specialID);

            if (_dealsService.InstantUpdates())
            {
                new SynchController().BookingSynchRate(specialID, false, datesAvailability);
            }

        }

        public void UpdateSpecialPersonRate(
            List<CalendarDateValueModel> datesAvailability,
            int specialID)
        {
            if (datesAvailability != null)
                _dealsService.UpdateSpecialPersonRate(datesAvailability, specialID);

            if(_dealsService.InstantUpdates())
            {
                new SynchController().BookingSynchRate(specialID, false, datesAvailability);
            }
        }

        public void UpdateSpecialMinNights(
            List<CalendarDateValueModel> datesAvailability,
            int specialID)
        {
            if (datesAvailability != null)
                _dealsService.UpdateSpecialMinNights(datesAvailability, specialID);

            if (_dealsService .InstantUpdates())
            {
                new SynchController().BookingSynchRate(specialID, false, datesAvailability);
            }
        }

        public void UpdateSpecialMaxNights(
            List<CalendarDateValueModel> datesAvailability,
            int specialID)
        {
            if (datesAvailability != null)
                _dealsService.UpdateSpecialMaxNights(datesAvailability, specialID);

            if (_dealsService.InstantUpdates())
            {
                new SynchController().BookingSynchRate(specialID, false, datesAvailability);
            }
        }

        public void UpdateSpecialStopSell(
            List<CalendarDateValueModel> datesAvailability,
            int specialID)
        {
            if (datesAvailability != null)
                _dealsService.UpdateSpecialStopSell(datesAvailability, specialID);

            if (_dealsService.InstantUpdates())
            {
                new SynchController().BookingSynchRate(specialID, false, datesAvailability);
            }
        }


        //this updates the availability for the room of the selected special, this does the same as the update availabiliy of propertydetails controlelr
        public void UpdateSpecialRoomAvailability(
            List<CalendarDateValueModel> datesAvailability,
            int specialID)
        {
            var roomType = _dealsService.GetSpecialRoomType(specialID);

            new PropertyDetailsController().UpdateRoomDateValues(datesAvailability,roomType,"availability");
            /*
            if (datesAvailability != null)
            {
                ApplicationServices.PropertyDetailsService.UpdateRoomDateValues(datesAvailability, roomType, "availability");
            }*/
        }

        [HttpGet]
        public ActionResult SpecialInclusions(int specialID)
        {
            var model = _dealsService.GetSpecialInclusions(specialID);

            return PartialView("~/Views/Deals/_InclusionsForm.cshtml", model);
        }

        [HttpPost]
        public ActionResult SpecialInclusions(SpecialInclusionsModel model)
        {
            if (ModelState.IsValid)
            {
                _dealsService.UpdateSpecialInclusions(model);
            }
            return PartialView("~/Views/Deals/_InclusionsForm.cshtml", model);
        }

       


    }
}