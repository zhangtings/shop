using SHAN.Service;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace LW.Areas.SKLAdmin.Controllers
{
    public class ColumnController : BaseController
    {
        // GET: Admin/Category
        public IDimService _iColSer { get; set; }
       

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Column()
        {
            return View();
        }

        public ActionResult List(维度DTO dTO)
        {
            var list = _iColSer.List(dTO);
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
        public ActionResult Edit(维度DTO dto)
        {
            维度DTO model = new 维度DTO();
            ViewBag.Categories = _iColSer.List(model);
            if (dto.Id > 0)
            {
                model = _iColSer.GetDTO(dto.Id);
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Save(维度DTO dto)
        {
            //if(dto.Id==0) dto.Addtime= (int)((DateTime.Now.Ticks - 621356256000000000) / 10000000);
            var result = _iColSer.Save(dto);
            return Json(result);
        }

        public JsonResult Delete(维度DTO dto)
        {
            var json = new
            {
                code = 0,
                msg = "删除成功",
                count = 0,
                data = ""
            };
            if (dto.Id > 0)
            {
                _iColSer.Del(dto);
            }
            return Json(json);
        }
    }
}