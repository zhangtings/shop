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
        public IOrderService _iOrdSer { get; set; }
        public ICartService  _iCartSer { get; set; }

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
            前端用户DTO user = (前端用户DTO) Session["user"];
            ViewBag.list = _iCartSer.List(new 购物车DTO());
            return View();
        }

        public ActionResult Myshop(商品DTO dTO)
        {
            
            return View();
        }

        public ActionResult MyOrder(订单DTO dTO)
        {
            ViewBag.list = _iOrdSer.List(dTO);
            return View();
        }

        public JsonResult AddTOCart(订单DTO dTO)
        {
            //_iCartSer.Save();
            var msg = "添加成功";
            var json = new
            {
                code = 0,
                msg = msg,
                count = 0,
                data = ""
            };
            return Json(json);
        }

        public ActionResult AddOrder(int? Cid)
        {

            return View();
        }



       


    }
}
