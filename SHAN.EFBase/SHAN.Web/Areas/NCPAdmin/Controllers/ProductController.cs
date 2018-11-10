using SHAN.Service;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Nong.Areas.NCPAdmin.Controllers
{
    public class ProductController : BaseController
    {
        public IProductService _iProSer { get; set; }
       

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Product()
        {
            return View();
        }

        public ActionResult List(商品DTO dTO)
        {
            var list = _iProSer.List(dTO);
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
        public ActionResult Edit(商品DTO dto)
        {
            商品DTO model = new 商品DTO();
            ViewBag.Categories = _iProSer.List(model);
            if (dto.Id > 0)
            {
                model = _iProSer.GetDTO(dto.Id);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Save(商品DTO dto)
        {
            //if(dto.Id==0) dto.Addtime= (int)((DateTime.Now.Ticks - 621356256000000000) / 10000000);
            var result = _iProSer.Save(dto);
            return Json(result);
        }

        public JsonResult Delete(商品DTO dto)
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
                _iProSer.Del(dto);
            }
            return Json(json);
        }
    }
}