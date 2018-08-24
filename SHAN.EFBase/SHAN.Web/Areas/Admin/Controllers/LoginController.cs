using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZ.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        public ActionResult Index()
        {
            Session["admin"] = null;
            return View();
        }

        [HttpPost]
        /// <summary>
        /// 登录代码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult CheckLogin(CZ.Models.Users model)
        {
            string uName = Application.Setting.Instance.GetValue("LoginName");
            string pwd = Application.Setting.Instance.GetValue("Password");
            if (model.LoginName == uName && model.Password == pwd)
            {
                Models.Users userData = new Models.Users()
                {
                    LoginName = uName
                };
                Session["admin"] = userData;
                return Json(Models.Common.Message.Success("登录成功"));

            }
            else
            {
                return Json(Models.Common.Message.Error("用户名或者密码失败"));
            }

        }
    }
}