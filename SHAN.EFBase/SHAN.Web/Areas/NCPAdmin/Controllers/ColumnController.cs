using SHAN.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Nong.Areas.NCPAdmin.Controllers
{
    public class ColumnController : BaseController
    {
        // GET: Admin/Category
        public IColumnService _iColSer { get; set; }
       

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Column()
        {
            return View();
        }

        public ActionResult SubColumn(栏目DTO dTO)
        {
            var list = _iColSer.List(dTO);
            dTO = list.Where(r=>r.Name==dTO.Name).FirstOrDefault();
            return View(dTO);
        }

        public ActionResult List(栏目DTO dTO)
        {
            var list = _iColSer.List(dTO);
            var json = new
            {
                code = 0,
                msg = "",
                count = 0,
                data = list.ToArray()
            };
            return Json(json,JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CEdit(栏目DTO dto)
        {
            栏目DTO model = new 栏目DTO();
            var list = _iColSer.List(model);
            if (!string.IsNullOrEmpty(dto.Name))
            {
                model = list.FirstOrDefault(r=>r.Name==dto.Name);
            }
            else
            {
                model = dto;
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(栏目DTO dto)
        {
            栏目DTO model = new 栏目DTO();
            ViewBag.Categories = _iColSer.List(model);
            if (dto.Id > 0)
            {
                model = _iColSer.GetDTO(dto.Id);
            }
            else {
                model = dto;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Save(栏目DTO dto)
        {
            //if(dto.Id==0) dto.Addtime= (int)((DateTime.Now.Ticks - 621356256000000000) / 10000000);
            var result = _iColSer.Save(dto);
            return Json(result);
        }

        public JsonResult Delete(栏目DTO dto)
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