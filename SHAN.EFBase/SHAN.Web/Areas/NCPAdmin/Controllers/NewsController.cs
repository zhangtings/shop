using SHAN.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Nong.Areas.NCPAdmin.Controllers
{
    public class NewsController : BaseController
    {
        // GET: Admin/Category
        public INewsService _iNewsSer { get; set; }
        public IColumnService _iColSer { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult News(新闻DTO dTO)
        {
            return View(dTO);
        }

        public ActionResult SPNews(新闻DTO dTO)
        {
            var list = _iNewsSer.List(dTO);
            dTO = list.FirstOrDefault(r => r.Name == dTO.Name);
            return View(dTO);
        }

        public ActionResult List(新闻DTO dTO)
        {

            var list = _iNewsSer.List(dTO);
            if (!string.IsNullOrEmpty(dTO.CName))
            {
                list = list.Where(r => r.CName == dTO.CName).ToList();
            }
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
        public ActionResult Edit(新闻DTO dto)
        {
            新闻DTO model = new 新闻DTO();

            if (dto.Id > 0)
            {
                model = _iNewsSer.GetDTO(dto.Id);
            }
            if (!string.IsNullOrEmpty(dto.CName))
            {
                var cdto = _iColSer.List(new 栏目DTO()).FirstOrDefault(r => r.Name == dto.CName);
                model.Cid = cdto.Id;
            }
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Save(新闻DTO dto)
        {
            if (dto.Id<1) dto.Addtime= DateTime.Now;
            dto.Updatetime= DateTime.Now;
            var result = _iNewsSer.Save(dto);
            return Json(result);
        }

        public JsonResult Delete(新闻DTO dto)
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