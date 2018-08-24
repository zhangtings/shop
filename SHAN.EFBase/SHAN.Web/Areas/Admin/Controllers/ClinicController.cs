using CZ.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZ.Areas.Admin.Controllers
{
    public class ClinicController : BaseController
    {
        // GET: Admin/Clinic
        private IClinicService _iClinicService;
        public ClinicController(IClinicService iClinicService)
        {
            _iClinicService = iClinicService;
        }


        public ActionResult Clinic()
        {
            return View();
        }

        public ActionResult ClinicQuery(Models.ClinicQuery query)
        {
            var result = _iClinicService.GetPage(query);
            return View("_ClinicList", result);
        }

        public ActionResult ClinicEdit(int? Id)
        {
            Models.ClinicDTO model = new Models.ClinicDTO();
            if (Id > 0)
            {
                model = _iClinicService.GetClinic(Id.Value);
            }
            return View(model);
        }

        public JsonResult ClinicSave(Models.ClinicDTO dto)
        { 
            var result = _iClinicService.SaveOrUpdateClinic(dto);
            return Json(result);
        }

        public JsonResult ClinicDelete(int id)
        {
            var result = _iClinicService.DeleteClinic(id);
            return Json(result);
        }
    }
}