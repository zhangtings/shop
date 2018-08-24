using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CZ.Application;

namespace CZ.Areas.Admin.Controllers
{
    public class ProjectController : BaseController
    {
        private IProjectService _iProjectService;
        public ProjectController(IProjectService iProjectService)
        {
            _iProjectService = iProjectService;
        }

        public ActionResult Project(int? VillageId)
        {
            ViewBag.VillageId = VillageId;
            return View();
        }


        public ActionResult ProjectEdit(int? Id, int? VillageId)
        {
            Models.ProjectDTO model = new Models.ProjectDTO();
            if (Id > 0)
            {
                model = _iProjectService.GetProject(Id.Value);
            }
            else if (VillageId > 0)
            {
                model.VillageId = VillageId;
            }
            return View(model);
        }

        public ActionResult ProjectQuery(Models.ProjectQuery query)
        {
            var result = _iProjectService.GetPage(query);
            return View("_ProjectList", result);
        }

        [ValidateInput(false)]
        public JsonResult SaveOrUpate(Models.ProjectDTO dto)
        {
            var result = _iProjectService.SaveOrUpdateProject(dto);
            return Json(result);
        }

        public JsonResult ProjectDelete(int id)
        {
            var result = _iProjectService.DeleteProject(id);
            return Json(result);
        }
    }
}