using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZ.Areas.Admin.Controllers
{
    public class MainController : BaseController
    {
        // GET: Admin/Main
        public ActionResult Index()
        {
            return View("Index");
        }
        public ActionResult Default()
        {
            return View();
        }

        
    }
}