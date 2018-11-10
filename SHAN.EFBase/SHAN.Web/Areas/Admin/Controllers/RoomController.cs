using SHAN.Service;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Hotel.Areas.Admin.Controllers
{
    public class RoomController : BaseController
    {
        public IRoomService _iRoomService { get; set; }
        public IRoomTypeService _iRoomTypeService { get; set; }
        public IRoomOrderService _iROService { get; set; }

        public ActionResult Room()
        {
            return View();
        }

        public ActionResult Room1()
        {
            return View();
        }

        public ActionResult RoomList(房间信息DTO dTO)
        {
            var 房间 = _iRoomService.List(dTO);
            var json = new
            {
                code = 0,
                msg = "",
                count = 0,
                data = 房间.ToArray()
            };
             return Json(json);
        }
        [HttpGet]
        public ActionResult RoomEdit(房间信息DTO dto) {
            房间信息DTO model = new 房间信息DTO();
            if (dto.Id > 0) {
                model = _iRoomService.GetDTO(dto.Id);
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult RoomSave(房间信息DTO dto)
        {
            var json = new
            {
                code = 0,
                msg = "保存成功",
                count = 0,
                data = ""
            };
            //if
            //dto.
            //dto.ForEach(r => r.CompanyId = 1);
            //dto.ForEach(r => _iRoomService.Save(r));
            //dto.CompanyId = HotelUser.CompanyId;
            //dto.CompanyId = 1;//
            _iRoomService.Save(dto);
            return Json(json);
        }

        public JsonResult RoomDelete(房间信息DTO dto)
        {
            var json = new
            {
                code = 0,
                msg = "删除成功",
                count = 0,
                data = ""
            };
            
            if (dto.Id > 0) {
                _iRoomService.Del(dto);
            }

            return Json(json); 
        }

        //
        public ActionResult RoomType()
        {
            return View();
        }

        public JsonResult RTList(房间类型DTO dTO)
        {
            var 房间 = _iRoomTypeService.List(dTO);
            if (房间.Count<1) {
                dTO.CompanyId = 0;
                房间 = _iRoomTypeService.List(dTO);
            }
            var json = new
            {
                code = 0,
                msg = "",
                count = 0,
                data = 房间.ToArray()
            };
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult RTEdit(房间类型DTO dto)
        {
            房间类型DTO model = new 房间类型DTO();
            if (dto.Id > 0)
            {
                model = _iRoomTypeService.GetDTO(dto.Id);
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult RTSave(房间类型DTO dto)
        {
            var json = new
            {
                code = 0,
                msg = "保存成功",
                count = 0,
                data = ""
            };
            _iRoomTypeService.Save(dto);
            return Json(json);
        }

        public JsonResult RTDelete(房间类型DTO dto)
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
                _iRoomTypeService.Del(dto);
            }

            return Json(json);
        }

        //
        
        public ActionResult RoomOrder()
        {
            return View();
        }

        public JsonResult ROList(预定DTO dTO)
        {
            var 预定 = _iROService.List(dTO);
            var json = new
            {
                code = 0,
                msg = "",
                count = 0,
                data = 预定.ToArray()
            };
            return Json(json);
        }
        [HttpGet]
        public ActionResult ROEdit(预定DTO dto)
        {
            预定DTO model = new 预定DTO();
            if (dto.Id > 0)
            {
                model = _iROService.GetDTO(dto.Id);
            }

            return View(model);
        }

        
        public JsonResult RODelete(预定DTO dto)
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
                _iROService.Del(dto);
            }

            return Json(json);
        }

    }
}