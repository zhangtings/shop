using SHAN.Service;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace LW.Areas.SKLAdmin.Controllers
{
    public class NewsController : BaseController
    {
        // GET: Admin/Category
        public IRosterService _iNewsSer { get; set; }
       

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult News()
        {
            return View();
        }

        public ActionResult List(企业用户DTO dTO)
        {
            var list = _iNewsSer.List(dTO);
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
        public ActionResult Edit(企业用户DTO dto)
        {
            企业用户DTO model = new 企业用户DTO();
            ViewBag.Categories = _iNewsSer.List(model);
            if (dto.Id > 0)
            {
                model = _iNewsSer.GetDTO(dto.Id);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Save(企业用户DTO dto)
        {
            //if(dto.Id==0) dto.Addtime= (int)((DateTime.Now.Ticks - 621356256000000000) / 10000000);
            var result = _iNewsSer.Save(dto);
            return Json(result);
        }

        public JsonResult Delete(企业用户DTO dto)
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
                _iNewsSer.Del(dto);
            }
            return Json(json);
        }
    }
}