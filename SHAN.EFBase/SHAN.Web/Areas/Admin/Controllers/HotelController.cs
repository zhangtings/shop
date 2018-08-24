using CZ.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZ.Areas.Admin.Controllers
{
    public class HotelController : BaseController
    {
        // GET: Admin/Hotel
        private IHotelService _iHotelService;
        public HotelController(IHotelService iHotelService)
        {
            _iHotelService = iHotelService;
        }


        public ActionResult Hotel()
        {
            return View();
        }

        public ActionResult HotelQuery(Models.HotelQuery query)
        {
            var result = _iHotelService.GetPage(query);
            return View("_HotelList", result);
        }

        public ActionResult HotelEdit(int? Id)
        {
            Models.HotelDTO model = new Models.HotelDTO();
            if (Id > 0)
            {
                model = _iHotelService.GetHotel(Id.Value);
            }
            return View(model);
        }

        public JsonResult HotelSave(Models.HotelDTO dto)
        { 
            var result = _iHotelService.SaveOrUpdateHotel(dto);
            return Json(result);
        }

        public JsonResult HotelDelete(int id)
        {
            var result = _iHotelService.DeleteHotel(id);
            return Json(result);
        }
    }
}