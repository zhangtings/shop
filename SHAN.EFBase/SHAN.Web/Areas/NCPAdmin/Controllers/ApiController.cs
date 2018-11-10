using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace Nong.Areas.NCPAdmin.Controllers
{ 

    public class ApiController : Controller
    { 
       

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Upload()
        {
            //Message message = null;
            string newFileName = new Random().Next(10000, 99999).ToString();
            string path = Server.MapPath("~") + "/upload/" + DateTime.Now.ToString("yyyyMMdd")+"/";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }


            HttpPostedFileBase file = Request.Files[0];
            file.SaveAs(path + newFileName + Path.GetExtension(file.FileName));
            string result = "/upload/" + DateTime.Now.ToString("yyyyMMdd") + "/" + newFileName + Path.GetExtension(file.FileName);
            //message = Message.Success(result, result);
            var message = new {
                Key = result
            };
            return Json(message);
        }
    }
}