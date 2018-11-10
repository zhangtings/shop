using CZ.Application;
using CZ.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZ.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        // GET: Admin/WXShopProduct
        private IEFProductService _iEFProductService { get; set; }




        public ActionResult WXShopProduct()
        {
            return View();
        }
        
        public ActionResult WXShopProductQuery(int? PageSize, int? CurrentPage)
        {
            Models.ProductDTO query = new Models.ProductDTO() { Pager = new Pagination() { CurrentPage = CurrentPage.Value, PageSize = PageSize.Value } };
            Message<List<Models.ProductDTO>> result = new Message<List<Models.ProductDTO>>();
            result.Data = _iEFProductService.products(query);
            result.Pager = query.Pager;
            return View("_ProductList", result);
        }

        public ActionResult WXShopProductEdit(int? Id)
        {
            //ViewBag.Categories = _iEFProductService.getCategoryList();
            Models.ProductDTO model = new Models.ProductDTO();
            if (Id > 0)
            {
                model = _iEFProductService.Get(Id.Value);
            }
            return View(model);
        }


    }
}