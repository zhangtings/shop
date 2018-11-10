using SHAN.Service;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Nong.Areas.NCPAdmin.Controllers
{
    public class BusinessController : BaseController
    {
        // GET: Admin/Category
        public IBusineService _iBusineSer { get; set; }
       

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Business()
        {
            return View();
        }

        public ActionResult List(商家DTO dTO)
        {
            var list = _iBusineSer.List(dTO);
            var json = new
            {
                code = 0,
                msg = "",
                count = 0,
                data = list.ToArray()
            };
            return Json(json);
        }

        public ActionResult Edit(商家DTO dto)
        {
            商家DTO model = new 商家DTO();
            ViewBag.Categories = _iBusineSer.List(model);
            if (dto.Id > 0)
            {
                model = _iBusineSer.GetDTO(dto.Id);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Save(商家DTO dto)
        {
            //if(dto.Id==0) dto.Addtime= (int)((DateTime.Now.Ticks - 621356256000000000) / 10000000);
            var result = _iBusineSer.Save(dto);
            return Json(result);
        }

        public JsonResult Delete(商家DTO dto)
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
                _iBusineSer.Del(dto);
            }
            return Json(json,JsonRequestBehavior.AllowGet);
        }
    }
}