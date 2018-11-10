using SHAN.Service;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Hotel.Areas.Admin.Controllers
{
    public class HotelUserController : BaseController
    {
        public IHotelUserService  _iHUService { get; set; }


        public ActionResult HotelUser() {

            return View();
        }

        public ActionResult HUList(酒店用户DTO dTO)
        {
            var 房间 = _iHUService.List(dTO);
            var json = new
            {
                code = 0,
                msg = "",
                count = 0,
                data = 房间.ToArray()
            };
             return Json(json,JsonRequestBehavior.AllowGet);
        }

        public ActionResult HUEdit(酒店用户DTO dto) {
            酒店用户DTO model = new 酒店用户DTO();
            if (dto.Id > 0) {
                model = _iHUService.GetDTO(dto.Id);
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult HUSave(酒店用户DTO dto)
        {
            if (dto.Id > 0) { }
            else
                dto.Pwd = "123456";//以后要加密
            _iHUService.Save(dto);
            var json = new
            {
                code = 0,
                msg = "保存成功",
                count = 0,
                data = dto.Id.ToString()
            };
            
            //json.data = dto.Id.ToString();
            return Json(json);
        }

        public JsonResult HUDelete(酒店用户DTO dto)
        {
            var json = new
            {
                code = 0,
                msg = "删除成功",
                count = 0,
                data = ""
            };
            
            if (dto.Id > 0) {
                _iHUService.Del(dto);
            }

            return Json(json); 
        }



    }
}