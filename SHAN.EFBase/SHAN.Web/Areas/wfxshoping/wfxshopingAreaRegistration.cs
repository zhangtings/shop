using System.Web.Mvc;

namespace CZ.Areas.wfxshoping
{
    public class wfxshopingAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "wfxshoping";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "wfxshoping_default",
                "wfxshoping/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}