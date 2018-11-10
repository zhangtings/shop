using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hotel.Areas.Admin.Controllers
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
        public JsonResult CheckLogin(LogUser model)
        {
            string uName = Application.Setting.Instance.GetValue("LogName");
            string pwd = Application.Setting.Instance.GetValue("Pwd");
            if (model.LoginName == uName && model.Password == pwd)
            {
                LogUser userData = new LogUser()
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