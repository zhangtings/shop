using System;
using System.Collections.Generic;
using System.Linq;
using SHAN.Service;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace SHAN.Web.Controllers
{
    public class HomeController : Controller
    {
        public IEFProductService _ProductService { get; set; }
        public 接口分类Service _分类业务 { get; set; }

        //public HomeController(IEFProductService ProductService) {
        //    _ProductService = ProductService;
        //}


        public ActionResult Index()
        {
            var plist=_ProductService.products;
            ViewBag.plist = plist;
            string jlist = JsonConvert.SerializeObject(plist.Select(r=>new {r.Id,r.Name,r.Cid }));
            ViewBag.jlist = jlist;
            return View();
        }

        [HttpPost]
        public JsonResult GetList()
        {
            List<ProductDTO> list = _ProductService.products;
            var json = new
            {
                total = list.Count,
                rows = list.ToArray()
            };
            var xx = Json(json, JsonRequestBehavior.AllowGet);
            return xx;
        }
        [HttpPost]
        public JsonResult GetCateList(int? tid)
        {
            if (tid == null) tid = 1;
            List<分类传输> list = _分类业务.分类集合fljh(tid.Value);
            var json = new
            {
                total = list.Count,
                rows = list.ToArray()
            };
            var xx = Json(json, JsonRequestBehavior.AllowGet);
            return xx;
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}