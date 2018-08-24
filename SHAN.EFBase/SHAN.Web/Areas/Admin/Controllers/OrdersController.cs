using CZ.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZ.Areas.Admin.Controllers
{
    public class OrdersController : BaseController
    {
        // GET: Admin/Orders
        private IOrdersService _iOrdersService;
        public OrdersController(IOrdersService iOrdersService)
        {
            _iOrdersService = iOrdersService;
        }


        public ActionResult Orders()
        {
            return View();
        }

        public ActionResult OrdersQuery(Models.OrdersQuery query)
        {
            var result = _iOrdersService.GetPage(query);
            return View("_OrdersList", result);
        }

        public ActionResult OrdersEdit(int? Id)
        {
            Models.OrdersDTO model = new Models.OrdersDTO();
            if (Id > 0)
            {
                model = _iOrdersService.GetOrders(Id.Value);
            }
            return View(model);
        }

        public JsonResult OrdersSave(Models.OrdersDTO dto)
        { 
            var result = _iOrdersService.SaveOrUpdateOrders(dto);
            return Json(result);
        }

        public JsonResult OrdersDelete(int id)
        {
            var result = _iOrdersService.DeleteOrders(id);
            return Json(result);
        }
    }
}