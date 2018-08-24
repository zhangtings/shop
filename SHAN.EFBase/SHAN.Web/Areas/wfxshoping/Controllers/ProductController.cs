using CZ.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZ.Areas.wfxshoping.Controllers
{
    public class ProductController : BaseController
    {
        //private IProductService _iProductService;
        private IEFProductService _iProSer;
        public ProductController(//IProductService iProductService, 
            IEFProductService iProSer)
        {
            //_iProductService = iProductService;
            _iProSer = iProSer;
        }


        //显示商品列表，商店商品列表PlistinShop
        public ActionResult ShowList(int? cid)
        {
            if (cid == null) cid = 0;
            var list = _iProSer.products(new Models.ProductDTO()
            {
                Cid = cid.Value,
                Is_show = 1
            });
            ViewBag.Products = list;
            return View();
        }

        public JsonResult JsonShowList(int? cid)
        {
            if (cid == null) cid = 0;
            var result = _iProSer.products(new Models.ProductDTO()
            {
                Cid = cid.Value,
                Is_show = 1
            });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Pinfo(int? Pid)
        {
            Models.ProductDTO model = new Models.ProductDTO();
            if (Pid > 0)
            {
                model = _iProSer.Get(Pid.Value);
            }
            return View(model);
        }

        public JsonResult JsonPinfo(int? Pid)
        {
            Models.ProductDTO model = new Models.ProductDTO();
            if (Pid > 0)
            {
                model = _iProSer.Get(Pid.Value);
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}