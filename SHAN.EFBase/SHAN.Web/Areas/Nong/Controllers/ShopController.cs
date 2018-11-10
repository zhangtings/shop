using SHAN.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nong.Areas.Nong.Controllers
{

    public class ShopController : BaseController
    {
        public IProductService _iProSer { get; set; }
        public ICategoryService _iCateSer { get; set; }

        public ActionResult Index()
        {
            return View();
        }



        public ActionResult List(商家DTO dTO)
        {
            return View(dTO);
        }

        public JsonResult GetProList(商品DTO dTO)
        {
            var list = _iProSer.List(dTO);
                        
            var json = new
            {
                code = 0,
                msg = "",
                count = 0,
                data = list.ToArray()
            };
            return Json(json,JsonRequestBehavior.AllowGet);
            //ViewBag.blist = list;
            //return View();
        }

        public ActionResult Pinfo(商品DTO dTO)
        {
            dTO = _iProSer.GetDTO(dTO.Id);
            return View(dTO);
        }

        public ActionResult Plist(商品DTO dTO)
        {
            ViewBag.list商品 = _iProSer.List(dTO);
            return View(dTO);
        }

        public ActionResult Category()
        {
            var list = _iCateSer.List(new 分类DTO());
            ViewBag.Categores = list;
            return View();
        }

        public ActionResult Cart(商品DTO dTO)
        {
            
            return View();
        }

        public ActionResult Myshop(商品DTO dTO)
        {
            
            return View();
        }

        public ActionResult AddOrder(int? Cid)
        {

            return View();
        }



       


    }
}
