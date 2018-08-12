using System.Web;
using System.Web.Optimization;

namespace SHAN.Web
{
    public class BundleConfig
    {
        // 有关捆绑的详细信息，请访问 https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //jquery核心库
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery.min.js"));
            //easyui核心库
            bundles.Add(new ScriptBundle("~/bundles/easyui").Include(
                        "~/Scripts/jquery.easyui.min.js"));
            //easyui核心UI文件 css
            bundles.Add(new StyleBundle("~/Content/themes/default/css").Include("~/Content/themes/default/easyui.css"));

            //easyui图标
            bundles.Add(new StyleBundle("~/Content/theme/css").Include("~/Content/themes/icon.css"));

            //easyui国际化文件
            bundles.Add(new ScriptBundle("~/bundles/locale").Include(
                        "~/Scripts/easyui-lang-zh_CN.js"));
            
            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备就绪，请使用 https://modernizr.com 上的生成工具仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));
            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css"));
        }
    }
}
