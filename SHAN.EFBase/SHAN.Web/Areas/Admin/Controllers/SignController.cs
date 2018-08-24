using CZ.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZ.Areas.Admin.Controllers
{
    public class SignController : BaseController
    {
        // GET: Admin/Sign
        private ISignService _iSignService;
        private IVillageService _iVillageService;
        public SignController(ISignService iSignService, IVillageService iVillageService)
        {
            _iSignService = iSignService;
            _iVillageService = iVillageService;
        }


        public ActionResult Sign()
        {
            var list = _iVillageService.GetPage(new Models.VillageQuery()
            {
                Pager = new Models.Common.Pagination()
                {
                    CurrentPage = 1,
                    PageSize = 20
                }
            });
            ViewBag.List = list.Data;
            return View();
        }

        public ActionResult SignQuery(Models.SignQuery query)
        {
            var result = _iSignService.GetPage(query);
            return View("_SignList", result);
        }

        public ActionResult SignEdit(int? Id)
        {
            Models.SignDTO model = new Models.SignDTO();
            if (Id > 0)
            {
                model = _iSignService.GetSign(Id.Value);
            }
            return View(model);
        }

        public JsonResult SignSave(Models.SignDTO dto)
        { 
            var result = _iSignService.SaveOrUpdateSign(dto);
            return Json(result);
        }

        public JsonResult SignDelete(int id)
        {
            var result = _iSignService.DeleteSign(id);
            return Json(result);
        }
    }
}