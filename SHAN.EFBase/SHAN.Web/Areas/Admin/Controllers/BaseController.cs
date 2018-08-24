using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZ.Areas.Admin.Controllers
{

    // GET: Admin/Base
    public class BaseController : Controller
    {
        // GET: village/Base

        public string uid { get; set; }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            string requestUrl = System.Web.HttpContext.Current.Request.Url.ToString().ToLower();
            if(requestUrl.Contains("localhost:"))
            {
                Models.Users userData = new Models.Users()
                {
                    LoginName = "shengda"
                };
                filterContext.HttpContext.Session["admin"] = userData; 
            }



            if (filterContext.HttpContext.Session["admin"] == null)
            {
                Response.Redirect("/Admin/Login/Index");
                Response.End();
            }
        }
    }
}