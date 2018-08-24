using CZ.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZ.Areas.Admin.Controllers
{
    public class CompanyController : BaseController
    {
        // GET: Admin/Company
        private ICompanyService _iCompanyService;
        public CompanyController(ICompanyService iCompanyService)
        {
            _iCompanyService = iCompanyService;
        }


        public ActionResult Company()
        {
            return View();
        }

        public ActionResult CompanyQuery(Models.CompanyQuery query)
        {
            var result = _iCompanyService.GetPage(query);
            return View("_CompanyList", result);
        }

        public ActionResult CompanyEdit(int? Id)
        {
            Models.CompanyDTO model = new Models.CompanyDTO();
            if (Id > 0)
            {
                model = _iCompanyService.GetCompany(Id.Value);
            }
            return View(model);
        }

        public JsonResult CompanySave(Models.CompanyDTO dto)
        { 
            var result = _iCompanyService.SaveOrUpdateCompany(dto);
            return Json(result);
        }

        public JsonResult CompanyDelete(int id)
        {
            var result = _iCompanyService.DeleteCompany(id);
            return Json(result);
        }
    }
}