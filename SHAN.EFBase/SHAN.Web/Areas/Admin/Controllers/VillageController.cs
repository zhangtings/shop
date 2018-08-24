using CZ.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZ.Areas.Admin.Controllers
{
    public class VillageController : BaseController
    {
        // GET: Admin/Village
        private IVillageService _iVillageService;
        public VillageController(IVillageService iVillageService)
        {
            _iVillageService = iVillageService;
        }


        public ActionResult Village()
        {
            return View();
        }
        public ActionResult CreateCode(int id)
        {
            ViewData["id"] = id;
            return View();
        }

        
        public ActionResult VillageQuery(Models.VillageQuery query)
        {
            var result = _iVillageService.GetPage(query);
            return View("_VillageList", result);
        }

        public ActionResult VillageEdit(int? Id)
        {
            Models.VillageDTO model = new Models.VillageDTO();
            if (Id > 0)
            {
                model = _iVillageService.GetVillage(Id.Value);
            }
            return View(model);
        }

        public JsonResult VillageSave(Models.VillageDTO dto)
        { 
            var result = _iVillageService.SaveOrUpdateVillage(dto);
            return Json(result);
        }

        public JsonResult VillageDelete(int id)
        {
            var result = _iVillageService.DeleteVillage(id);
            return Json(result);
        }
    }
}