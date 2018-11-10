using SHAN.Service;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Hotel.Areas.Admin.Controllers
{
    public class GuestController : BaseController
    {
        public IGuestService _iGuestService { get; set; }
        public IGuestTypeService _iGuestTypeService { get; set; }

        public ActionResult Guest()
        {
            return View();
        }
        

        public ActionResult GuestList(客人信息DTO dTO)
        {
            var 客人 = _iGuestService.List(dTO);
            var json = new
            {
                code = 0,
                msg = "",
                count = 0,
                data = 客人.ToArray()
            };
             return Json(json);
        }

        [HttpGet]
        public ActionResult GuestEdit(客人信息DTO dto) {
            客人信息DTO model = new 客人信息DTO();
            if (dto.Id > 0) {
                model = _iGuestService.GetDTO(dto.Id);
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult GuestSave(客人信息DTO dto)
        {
            var json = new
            {
                code = 0,
                msg = "保存成功",
                count = 0,
                data = ""
            };
            _iGuestService.Save(dto);
            return Json(json);
        }

        public JsonResult GuestDelete(客人信息DTO dto)
        {
            var json = new
            {
                code = 0,
                msg = "删除成功",
                count = 0,
                data = ""
            };
            
            if (dto.Id > 0) {
                _iGuestService.Del(dto);
            }

            return Json(json); 
        }

        //
        public ActionResult GuestType()
        {
            return View();
        }

        public ActionResult GTList(客人类型DTO dTO)
        {

            var 客人 = _iGuestTypeService.List(dTO);
            if (客人.Count < 1)
            {
                dTO.CompanyId = 0;
                客人 = _iGuestTypeService.List(dTO);
            }
            var json = new
            {
                code = 0,
                msg = "",
                count = 0,
                data = 客人.ToArray()
            };
            return Json(json);
        }
        [HttpGet]
        public ActionResult GTEdit(客人信息DTO dto)
        {
            客人类型DTO model = new 客人类型DTO();
            if (dto.Id > 0)
            {
                model = _iGuestTypeService.GetDTO(dto.Id);
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult GTSave(客人类型DTO dto)
        {
            var json = new
            {
                code = 0,
                msg = "保存成功",
                count = 0,
                data = ""
            };
            //dto.CompanyId = 1;
            _iGuestTypeService.Save(dto);
            return Json(json);
        }

        public JsonResult GTDelete(客人类型DTO dto)
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
                _iGuestTypeService.Del(dto);
            }

            return Json(json);
        }

    }
}