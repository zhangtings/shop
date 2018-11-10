using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nong.Areas.NCPAdmin.Controllers
{


    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            string requestUrl = System.Web.HttpContext.Current.Request.Url.ToString().ToLower();
            if (requestUrl.Contains("Nong:"))
            {
                if (filterContext.HttpContext.Session["admin"] == null)
                {
                    Response.Redirect("/Admin/Login/Nong");
                    Response.End();
                }
                //filterContext.HttpContext.Session["admin"] = "LW";
            }
            if (requestUrl.Contains("localhost:"))
            {
                filterContext.HttpContext.Session["admin"] = "LW"; 
            }
            filterContext.HttpContext.Session["admin"] = "LW";
            if (filterContext.HttpContext.Session["admin"] == null)
            {
                Response.Redirect("/NCPAdmin/Login/Index");
                Response.End();
            }
        }
    }
}