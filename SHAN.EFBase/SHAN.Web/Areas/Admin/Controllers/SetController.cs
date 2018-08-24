using CZ.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CZ.Areas.Admin.Controllers
{
    public class SetController : BaseController
    {

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Save(CZ.Models.SetConfig model)
        {
            Application.Setting.Instance.SetValue("Cunhaohuo", model.Cunhaohuo);
            Application.Setting.Instance.SetValue("Cunzhibo", model.Cunzhibo);
            Application.Setting.Instance.SetValue("Weifangchan", model.Weifangchan);
            Application.Setting.Instance.SetValue("Weitour", model.Weitour);
            Application.Setting.Instance.SetValue("Workurl", model.Workurl);
            Application.Setting.Instance.SetValue("Weinews", model.Weinews);
            Application.Setting.Instance.SetValue("Zhibo", model.Zhibo);
            var result = Message.Success("成功");
            return Json(result);
        }

        public string wxMenuCreate(int wid)
        {
            StringBuilder str = new StringBuilder();
            str.Append("{");
            str.Append("\"button\":[");
            str.Append("{");
            str.Append("\"type\":\"view\",");
            str.Append("\"name\":\"公司官网\",");
            str.Append("\"url\":\"http://cz.571400yb.com/village/village/Index?wid=1\"");
            str.Append("},{");
            str.Append("\"type\":\"view\",");
            str.Append("\"name\":\"共享村庄\",");
            str.Append("\"url\":\"http://cz.571400yb.com/village/main/index?wid=1\"");
            str.Append("},{");
            str.Append("\"type\":\"miniprogram\",");
            str.Append("\"name\":\"村里点餐\",");
            str.Append("\"appid\":\"wxafb193acbad6cc75\",");
            str.Append("\"pagepath\":\"zh_dianc/pages/home/home\",");
            str.Append("\"url\":\"http://cz.571400yb.com/village/main/index?wid=1\"");
            str.Append("}]");
            str.Append("}");

            DDb.Common.LogHelper.Info<Application.Weixin.WechatHelper>("菜单"+str.ToString());

            //string strSB = " {\"button\":[ { \"type\":\"view\",\"name\":\"琼海天气\",\"url\":\"http://https//so.m.sm.cn/\"},{ \"type\":\"view\",\"name\":\"美客社区\" , \"url\":\"http://wsq.qq.com/182326565/\"   },{ \"type\":\"view\",\"name\":\" 美发图库\",\"url\":\"http://wap.meike8.com/show/index.html/\"}]}";

            Application.Weixin.WechatHelper wechatHelper = new Application.Weixin.WechatHelper(wid);
            var result = wechatHelper.CreateMenu(str.ToString());
            return result.ToJSONSString();
        }
    }
}