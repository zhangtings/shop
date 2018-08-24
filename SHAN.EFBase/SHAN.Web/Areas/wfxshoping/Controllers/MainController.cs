using CZ.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZ.Areas.wfxshoping.Controllers
{
    public class MainController : BaseController
    {
        //private IProductService _iProductService;
        public IEFProductService _iEFproSer { get; set; }
        //public MainController(IEFProductService iEFproSer
        //    //IProductService iProductService
        //    )
        //{
        //    _iEFproSer = iEFproSer;
        //   // _iProductService = iProductService;
        //}

        // GET: wfxshoping/Main
        public ActionResult Index()
        {
            
            var udata = this.UserData;

            Models.ProductDTO query = new Models.ProductDTO()
            {
                Is_hot = 1,
                Pager = new Models.Common.Pagination()
                {
                    CurrentPage = 1,
                    PageSize = 10
                }
            };
            //var result = _iProductService.GetPage(query);
            //ViewBag.hotProduct = result.Data;
            ViewBag.efProduct = _iEFproSer.products(query);
            return View();
        }


    }
}