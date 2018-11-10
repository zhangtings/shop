using SHAN.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nong.Areas.Nong.Controllers
{

    public class NongController : Controller
    {
        public IColumnService _iColSer { get; set; }
        public IProductService _iProSer { get; set; }
        public IBusineService _iBusineSer { get; set; }
        public INewsService _iNewsSer { get; set; }

        public ActionResult Index()
        {
            var dto = _iColSer.GetDTO(1);
            var list公告 = _iNewsSer.List(new 新闻DTO()).Where(r => r.CName == "协会公告").ToList();
            ViewBag.公告list = list公告;
            return View(dto);
        }

        public ActionResult MIndex()
        {
            var list = _iColSer.List(new 栏目DTO());
            ViewBag.商家分类 = list;
            var plist = _iProSer.List(new 商品DTO() { Is_hot = 1 });
            ViewBag.商品 = plist;
            return View();
        }

        public ActionResult BusiList(商家DTO dTO)
        {
            return View(dTO);
        }

        public JsonResult GetBusiList(商家DTO dTO)
        {
            var list = _iBusineSer.List(dTO);
            if (!string.IsNullOrEmpty(dTO.Tags))
            {
                list = list.Where(r => r.Tags.Split(',').Contains(dTO.Tags))
                //.Select(r => new { r.Id, r.Title, r.Address, r.Zhekou, r.Photox })
                .ToList();
            }

            var json = new
            {
                code = 0,
                msg = "",
                count = 0,
                data = list.ToArray()
            };
            return Json(json, JsonRequestBehavior.AllowGet);
            //ViewBag.blist = list;
            //return View();
        }

        public ActionResult Detail(商家DTO dTO)
        {
            dTO = _iBusineSer.GetDTO(dTO.Id);
            return View(dTO);
        }

        public ActionResult News(新闻DTO dTO)
        {
            dTO = _iNewsSer.GetDTO(dTO.Id);
            if (dTO == null)
            {
                return RedirectToAction("Index");
            }
            return View(dTO);
        }

        public ActionResult ColInfo(栏目DTO dTO)
        {
            if (dTO.Id > 0)
            {
                var vm = _iColSer.GetDTO(dTO.Id);
                if (!string.IsNullOrEmpty(vm.Concent)) return View(vm);
            }
            if (!string.IsNullOrEmpty(dTO.Name))
            {
                var pcdto = _iColSer.List(new 栏目DTO()).FirstOrDefault(r => r.Name == dTO.Name);
                return View(pcdto);
            }
            return RedirectToAction("MIndex");

        }

        public ActionResult NewsList(新闻DTO dTO)
        {
            var cdto = _iColSer.List(new 栏目DTO()).FirstOrDefault(r => r.Name == dTO.CName);
            ViewBag.list新闻 = _iNewsSer.List(new 新闻DTO() { Cid = cdto.Id }).OrderByDescending(r=>r.Addtime).ToList();
            return View(cdto);
        }
    }
}
