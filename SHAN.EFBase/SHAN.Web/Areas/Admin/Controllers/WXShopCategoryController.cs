using CZ.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZ.Areas.Admin.Controllers
{
    public class WXShopCategoryController : BaseController
    {
        // GET: Admin/WXShopCategory
        private IWXShopCategoryService _iWXShopCategoryService;
        public WXShopCategoryController(IWXShopCategoryService iWXShopCategoryService)
        {
            _iWXShopCategoryService = iWXShopCategoryService;
        }


        public ActionResult WXShopCategory()
        {
            return View();
        }
        
        public ActionResult WXShopCategoryQuery(Models.WXShopCategoryQuery query)
        {
            var result = _iWXShopCategoryService.GetPage(query);
            return View("_WXShopCategoryList", result);
        }

        public ActionResult WXShopCategoryEdit(int? Id)
        {
            ViewBag.Categories = _iWXShopCategoryService.getCategoryList();
            Models.WXShopCategoryDTO model = new Models.WXShopCategoryDTO();
            if (Id > 0)
            {
                model = _iWXShopCategoryService.GetWXShopCategory(Id.Value);
            }
            return View(model);
        }

        public JsonResult WXShopCategorySave(Models.WXShopCategoryDTO dto)
        { 
            var result = _iWXShopCategoryService.SaveOrUpdateWXShopCategory(dto);
            return Json(result);
        }

        public JsonResult WXShopCategoryDelete(int id)
        {
            var result = _iWXShopCategoryService.DeleteWXShopCategory(id);
            return Json(result);
        }
    }
}