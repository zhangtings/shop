using CZ.Application.Weixin;
using DDb.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CZ.Areas.wfxshoping.Controllers
{
    public class apiController : Controller
    {
        private Application.IUserWeixinService _iUserWeixinService;
        private Application.IOrderService _iOrderService;
        public apiController(
            Application.IUserWeixinService iUserWeixinService,
            Application.IOrderService iOrderService
            )
        {
            _iUserWeixinService = iUserWeixinService;
            _iOrderService = iOrderService;
        }

        // GET: village/api
        public ActionResult Index()
        {
            return View();
        }

        private string GetPostData()
        {
            byte[] buf = new byte[Request.ContentLength];
            Request.InputStream.Read(buf, 0, Request.ContentLength);
            string data = Request.ContentEncoding.GetString(buf);
            return data;
        }


        public string  wxNotify()
        {
            String post = this.GetPostData();

            LogHelper.Info<apiController>("wxNotify接受到支付通知结果:");


            System.Web.HttpContext.Current.Application.Lock();

            try
            {

                PayResultNotify myPayResultNotify = new PayResultNotify(post);

                Models.OrderDTO payrec = new Models.OrderDTO();
                payrec = _iOrderService.Get(myPayResultNotify.out_trade_no);

                string sign = myPayResultNotify.CountSign(2);

                LogHelper.Info<apiController>("wxNotify对比myPayResultNotify.sign:" + myPayResultNotify.sign);
                LogHelper.Info<apiController>("wxNotify对比myPayResultNotify.CountSign():" + sign);

                if (myPayResultNotify.sign.ToLower() == sign.ToLower())
                {
                    if (myPayResultNotify.return_code.ToUpper() == "SUCCESS".ToUpper() && myPayResultNotify.result_code.ToUpper() == "SUCCESS".ToUpper())
                    { 
                        try
                        {
                            //payrec.p = DateTime.Now;
                            payrec.Price_h = Decimal.Parse(myPayResultNotify.total_fee) / 100;
                            //payrec.Status = 20;
                            _iOrderService.ChangeStatus(payrec,"付款成功");
                        }
                        catch(Exception er)
                        {
                            LogHelper.Error<apiController>(er,"wxNotify改变支付状态:"+ er.ToString());
                        }
                        LogHelper.Info<apiController>("wxNotify改变支付状态:");

                        //处理业务逻辑

                    }

                    Response.Write("<xml><return_code><![CDATA[SUCCESS]]></return_code><return_msg><![CDATA[OK]]></return_msg></xml>");
                }
                else
                {


                    LogHelper.Info<MainController>("wxNotify接受到支付通知结果:" + post);
                    LogHelper.Info<MainController>("wxNotify传入的加密参数:" + myPayResultNotify.sign);
                    LogHelper.Info<MainController>("wxNotify计算的加密参数:" + sign);
                    Response.Write("<xml><return_code><![CDATA[FAIL]]></return_code><return_msg><![CDATA[sign error!]]></return_msg></xml>");
                }
            }
            catch (Exception ex)
            {
                LogHelper.Info<MainController>("wxNotify接收支付结果通知失败:" + ex);
            }
            System.Web.HttpContext.Current.Application.UnLock();
            return "";
        }

        public string api(int? wid)
        {
            if (wid==null) { wid = 2; }
            var weixin = _iUserWeixinService.GetUserWeixin(wid.Value);
            string Token = weixin.wxToken;
            string signature = Request["signature"];
            string timestamp = Request["timestamp"];
            string nonce = Request["nonce"];
            string echostr = Request["echostr"];

            DDb.Common.LogHelper.Info<apiController>(Request.RawUrl);

            if (Request.HttpMethod == "GET")
            {

                //get method - 仅在微信后台填写URL验证时触发
                if (CheckSignature(signature, timestamp, nonce, Token))
                {
                    return (echostr); //返回随机字符串则表示验证通过
                }
                else
                {
                    return ("failed:" + signature + ",token:" + Token + " "  + "。" +
                                "如果你在浏览器中看到这句话，说明此地址可以被作为微信公众账号后台的Url，请注意保持Token一致。");
                } 
            }
            return (echostr); 
        }

        private bool CheckSignature(string signature,string timestamp, string nonce,string Token)
        {
            string[] ArrTmp = { Token, timestamp, nonce };
            Array.Sort(ArrTmp);     //字典排序 
            string tmpStr = string.Join("", ArrTmp);
            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            tmpStr = tmpStr.ToLower();
            if (tmpStr == signature)
            {
                return true;
            }
            else
            {
                return false;
            }

        }


    }
}