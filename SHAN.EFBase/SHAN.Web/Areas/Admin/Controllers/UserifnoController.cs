using CZ.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZ.Areas.Admin.Controllers
{
    public class UserInfoController : BaseController
    {
        // GET: Admin/UserInfo
        private IUserInfoService _iUserInfoService;
        public UserInfoController(IUserInfoService iUserInfoService)
        {
            _iUserInfoService = iUserInfoService;
        }


        public ActionResult UserInfo()
        {
            return View();
        }

        public ActionResult UserInfoQuery(Models.UserInfoQuery query)
        {
            var result = _iUserInfoService.GetPage(query);
            return View("_UserInfoList", result);
        }

        public ActionResult UserInfoEdit(int? Id)
        {
            Models.UserInfoDTO model = new Models.UserInfoDTO();
            if (Id > 0)
            {
                model = _iUserInfoService.GetUserInfo(Id.Value);
            }
            return View(model);
        }

        public JsonResult UserInfoSave(Models.UserInfoDTO dto)
        { 
            var result = _iUserInfoService.SaveOrUpdateUserInfo(dto);
            return Json(result);
        }

        public JsonResult UserInfoDelete(int id)
        {
            var result = _iUserInfoService.DeleteUserInfo(id);
            return Json(result);
        }
    }
}