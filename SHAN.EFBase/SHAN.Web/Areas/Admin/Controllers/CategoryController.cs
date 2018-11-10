using SHAN.Service;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Hotel.Areas.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        // GET: Admin/Category
        public ICategoryService _iCatSer { get; set; }
       

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Category()
        {
            return View();
        }

        public ActionResult List(分类DTO dTO)
        {
            var list = _iCatSer.List(dTO);
            var json = new
            {
                code = 0,
                msg = "",
                count = 0,
                data = list.ToArray()
            };
            return Json(json);
        }


       
        [HttpGet]
        public ActionResult Edit(int Id) {
            分类DTO model = new 分类DTO();
            ViewBag.Categories = _iCatSer.List(model);
            if (Id > 0) {
                model = _iCatSer.GetDTO(Id);
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(分类DTO dto)
        {
            //if(dto.Id==0) dto.Addtime= (int)((DateTime.Now.Ticks - 621356256000000000) / 10000000);
            var result = _iCatSer.Save(dto);
            return Json(result);
        }

        public JsonResult Delete(分类DTO dto)
        {
            var json = new
            {
                code = 0,
                msg = "删除成功",
                count = 0,
                data = ""
            };
            if (dto.Id > 0) {
                _iCatSer.Del(dto);
            }
            return Json(json); 
        }
    }
}