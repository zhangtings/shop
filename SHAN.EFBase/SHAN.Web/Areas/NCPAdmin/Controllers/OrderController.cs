using SHAN.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Nong.Areas.NCPAdmin.Controllers
{
    public class OrderController : BaseController
    {
        public IOrderService  _iOrderSer { get; set; }
       



        public ActionResult Order()
        {
            return View();
        }


        public ActionResult List(订单DTO dTO)
        {
            var list = _iOrderSer.List(dTO);
            var json = new
            {
                code = 0,
                msg = "",
                count = 0,
                data = list.ToArray()
            };
            return Json(json);
        }


        public ActionResult Edit(订单DTO dto)
        {
            订单DTO model = new 订单DTO();
            
            if (dto.Id > 0)
            {
                model = _iOrderSer.GetDTO(dto.Id);
            }

            return View(model);
        }

        public ActionResult Save(订单DTO dto)
        {
            //if(dto.Id==0) dto.Addtime= (int)((DateTime.Now.Ticks - 621356256000000000) / 10000000);
            var result = _iOrderSer.Save(dto);
            return Json(result);
        }

        public JsonResult Delete(订单DTO dto)
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
                _iOrderSer.Del(dto);
            }
            return Json(json);
        }
    }
}