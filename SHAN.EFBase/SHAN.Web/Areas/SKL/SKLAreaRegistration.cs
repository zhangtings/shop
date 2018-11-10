using System.Web.Mvc;

namespace LW.Areas.SKL
{
    public class SKLAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "SKL";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "SKL_default",
                "SKL/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}