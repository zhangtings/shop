using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using System.Xml;
using Newtonsoft.Json;
using System.Globalization;
using DDb.Common;

namespace CZ.Application.Weixin
{
    public class WechatHelper
    {
        //public static readonly WechatHelper Instance = new WechatHelper();
        public int cropid {get;set;}
        public String AppId = string.Empty;
        public String AppSecret = string.Empty;
        public String Mch_id = string.Empty;
        public String PayKey = string.Empty;

        public WechatHelper()
        {
            int wid = 2;
            var weixin = Factory.Instance().iUserWeixinService.GetUserWeixin(Convert.ToInt32(wid));
            this.AppId = weixin.AppId;
            this.AppSecret = weixin.AppSecret;
            this.Mch_id = weixin.Mch_id;
            this.PayKey = weixin.PayKey;
        }
        public WechatHelper(object wid)
        {
            var weixin = Factory.Instance().iUserWeixinService.GetUserWeixin(Convert.ToInt32(wid));
            this.AppId = weixin.AppId;
            this.AppSecret = weixin.AppSecret;
            this.Mch_id = weixin.Mch_id;
            this.PayKey = weixin.PayKey;
        }

        #region 公开方法

        /// <summary>
        /// 创建二维码
        /// </summary>
        /// <param name="qrvalue"></param>
        /// <returns></returns>
        public String CreateQRCode(int qrvalue)
        {
            String url = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=" + this.AccessToken.Token;
            String postData = "{\"action_name\": \"QR_LIMIT_SCENE\", \"action_info\": {\"scene\": {\"scene_id\": \"" + qrvalue + "\"}}}";
            String res = this.Post(url, postData, Encoding.UTF8, 10000);
            JObject jo = JObject.Parse(res);
            return jo.GetValue("ticket").ToString();
        }

        public String CreateQRCode(String qrvalue)
        {
            String url = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=" + this.AccessToken.Token;
            String postData = "{\"action_name\": \"QR_LIMIT_STR_SCENE\", \"action_info\": {\"scene\": {\"scene_str\": \"" + qrvalue + "\"}}}";
            String res = this.Post(url, postData, Encoding.UTF8, 10000);
            JObject jo = JObject.Parse(res);
            return jo.GetValue("ticket").ToString();
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public WechatUserInfo GetUserInfo(string openId)
        {
            try
            {
                string url = "https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN";
                url = String.Format(url, this.AccessToken.Token, openId);
                string source = this.GetUrl(url);
                //LogHelper.Info<WechatHelper>("获取用户信息返回值：" + source);
                return new WechatUserInfo(source);
            }
            catch (Exception ex)
            {
                //LogHelper.Error<WechatHelper>(ex, "获取微信用户信息出错");
                return new WechatUserInfo();
            }
        }

        /// <summary>
        /// 创建OAuth认证地址
        /// </summary>
        /// <returns></returns>
        public String CreateOAuth2Url()
        {
            string sourceUrl = System.Web.HttpContext.Current.Request.Url.ToString();
            String reqUrl = System.Web.HttpUtility.UrlEncode(sourceUrl);

            LogHelper.Info<WechatHelper>("获取当前页面地址reqUrl：" + reqUrl);
            String state = SHA1((sourceUrl.ToString().Split('?')[0].ToLower() + "O)UHFjd"));
            //微信访问,跳转到OAuth认证地址
            return "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + this.AppId + "&redirect_uri=" + reqUrl + "&response_type=code&scope=snsapi_base&state=" + state + "#wechat_redirect";
        }

        /// <summary>
        /// 校验OAuth认证回调并将openid保存到session
        /// </summary>
        /// <returns></returns>
        public bool CheckOAuthBack()
        {
            System.Web.HttpRequest Request = System.Web.HttpContext.Current.Request;
            System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;
            string countState = SHA1((Request.Url.ToString().Split('?')[0].ToLower() + "O)UHFjd")).ToLower();
            if (!string.IsNullOrEmpty(Request.QueryString["state"]))
            {
                string code = Request.QueryString["code"].Trim();

                string url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + this.AppId + "&secret=" + this.AppSecret + "&code=" + code + "&grant_type=authorization_code";

                string source = this.GetUrl(url);
                OAuthAccessToken at = new OAuthAccessToken();
                try
                {
                    JObject obj = JObject.Parse(source);
                    at.access_token = obj.GetValue("access_token") != null ? obj.GetValue("access_token").ToString() : null;
                    at.expires_in = obj.GetValue("expires_in") != null ? obj.GetValue("expires_in").ToInt32() : -1;
                    at.openid = obj.GetValue("openid") != null ? obj.GetValue("openid").ToString() : null;
                    at.scope = obj.GetValue("scope") != null ? obj.GetValue("scope").ToString() : null;
                }
                catch (Exception ex)
                {
                    //LogHelper.Error<WechatHelper>(ex, "解析OAuth返回json出错.");

                    Response.Write("解析OAuth返回json出错.");
                    Response.End();
                    return false;
                }

                System.Web.HttpContext.Current.Session["WechatOpenId"] = at.openid;
                System.Web.HttpContext.Current.Session["WebAccessToken"] = at.access_token;
                System.Web.HttpContext.Current.Session["WebTokenExpires"] = DateTime.Now.AddSeconds(6000);

                //LogHelper.Info<WechatHelper>("OAuth回调认证完毕，保存WechatOpenId的Session：" + at.openid + "  access_token:" + at.access_token);
                return true;
            }
            else
            {
                LogHelper.Info<WechatHelper>("没有state");
                Response.Write("没有state");
                return false;
            }
        }

        public Result CreateMenu(string jsonData)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}";
            url = String.Format(url, this.AccessToken.Token);
            LogHelper.Info<WechatHelper>("CreateMenu：" + url);
            LogHelper.Info<WechatHelper>("jsonData："+ jsonData);
            string source = this.Post(url, jsonData);
            Result res = source.ToObejct<Result>();
            return res;
        }


        /// <summary>
        /// 创建JSAPI的加密参数
        /// </summary>
        /// <param name="noncestr">任意字符串</param>
        /// <param name="timestamp">时间戳：DateTime.Now.ToFileTime().ToString().Substring(0, 9);</param>
        /// <param name="url">当前网页地址：Request.Url.ToString()</param>
        /// <returns></returns>
        public String CreateJsApiSignature(String noncestr, String timestamp, String url)
        {
            string sign = "";
            string source = String.Format("jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}"
                , this.JsAPITicket.ticket
                , noncestr
                , timestamp
                , url
                );
            sign = WechatHelper.SHA1(source).ToLower();
            //Core.KLoger.Instance.Debug("noncestr:" + noncestr);
            //Core.KLoger.Instance.Debug("timestamp:" + timestamp);
            //Core.KLoger.Instance.Debug("ticket:" + this.JsAPITicket.ticket);
            //Core.KLoger.Instance.Debug("url:" + url);
            //Core.KLoger.Instance.Debug("JsApiSignature:" + sign);
            return sign;
        }


        public String CreateEditAddrSignature(String appId, String noncestr, String timestamp)
        {
            string sign = "";
            string source = "";
            source += "accesstoken=" + System.Web.HttpContext.Current.Session["WebAccessToken"].ToString();
            source += "&appid=" + appId;
            source += "&noncestr=" + noncestr;
            source += "&timestamp=" + timestamp;
            source += "&url=" + System.Web.HttpContext.Current.Request.Url.ToString();
            //Library.Core.KLoger.Instance.Debug("EditAddrSignature-Source：" + source);
            sign = WechatHelper.SHA1(source).ToLower();
            //Library.Core.KLoger.Instance.Debug("EditAddrSignature-SHA1：" + sign);

            return sign;
        }

        /// <summary>
        /// 生成微信支付统一订单
        /// </summary>
        /// <param name="productName">商品名称</param>
        /// <param name="notifyUrl">支付结果通知后台地址</param>
        /// <param name="openId">用户OpenId</param>
        /// <param name="outTradeNo">本地系统交易流水号</param>
        /// <param name="money">支付金额，单位为分</param>
        /// <returns></returns>
        public UnifiedOrderResponse CreateUnifiedOrder(String productName, String notifyUrl, String openId, String outTradeNo, int money)
        {
            UnifiedOrder o = new UnifiedOrder();
            o.appid = this.AppId;
            o.attach = "378310285@qq.com";
            o.body = productName;
            o.mch_id = this.Mch_id;
            o.nonce_str = "1458651245";
            o.notify_url = notifyUrl;
            o.openid = openId;
            o.out_trade_no = outTradeNo;
            o.spbill_create_ip = System.Web.HttpContext.Current.Request.UserHostAddress;
            o.total_fee = money.ToString();
            o.trade_type = "JSAPI";

            o.sign = WechatPaySign(o.CountSign());

            //LogHelper.Info<WechatHelper>("微信支付统一订单请求数据:" + o.ToPostData());
            string url = "https://api.mch.weixin.qq.com/pay/unifiedorder";
            string postRes = this.Post(url, o.ToPostData());
            //LogHelper.Info<WechatHelper>("微信支付统一订单响应数据:" + postRes);
            return new UnifiedOrderResponse(postRes);
        }

        /// <summary>
        /// 订单复核接
        /// </summary>
        /// <param name="out_trade_no">微信本地订单号</param>
        /// <returns></returns>
        public OrderQueryResponse CreateQueryOrder(string out_trade_no)
        {
            OrderQuery o = new OrderQuery()
            {
                appid = this.AppId,
                mch_id = this.Mch_id, 
                out_trade_no = out_trade_no,
                nonce_str = "1458651246"
            }; 

            o.sign = WechatPaySign(o.CountSign());
             
            string url = "https://api.mch.weixin.qq.com/pay/orderquery"; //订单查询地址
            string postRes = this.Post(url, o.ToPostData());
            LogHelper.Info<WechatHelper>("微信支付统支付后查询响应数据:" + postRes);
            return new OrderQueryResponse(postRes);
        }

        /// <summary>
        /// 获得签名
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public string CetSign(UnifiedOrder o)
        {
            System.Reflection.PropertyInfo[] propertys = o.GetType().GetProperties();
            Dictionary<String, String> dict = new Dictionary<string, string>();
            for (int i = 0; i < propertys.Length; i++)
            {
                if (propertys[i].Name != "sign")
                {
                    if (propertys[i].GetValue(this, null) != null)
                    {
                        dict.Add(propertys[i].Name, propertys[i].GetValue(this, null).ToString());
                    }
                }
            }
            return WechatPaySign(dict);

        }


        /// <summary>
        /// 微信支付相关的Sign加密算法
        /// </summary>
        /// <param name="list">需要加密的项目</param>
        /// <param name="payApiKey">支付API的Key</param>
        /// <returns></returns>
        public String WechatPaySign(Dictionary<String, String> list)
        {

            List<String> listKey = new List<string>();
            foreach (KeyValuePair<String, String> kv in list)
            {
                listKey.Add(kv.Key);
            }
            String source = "";
            listKey.Sort();

            for (int i = 0; i < listKey.Count; i++)
            {
                foreach (KeyValuePair<String, String> kv in list)
                {
                    if (kv.Key == listKey[i])
                    {
                        source += kv.Key + "=" + kv.Value + "&";
                        break;
                    }
                }
            }
            source += "key=" + this.PayKey;

            //LogHelper.Info<WechatHelper>("微信支付签名source:" + source);
            return source.Md5().ToUpper(); ;
        }


        /// <summary>
        /// 生成微信JsAPI初始化的json, 默认初始化所有js接口
        /// </summary>
        /// <returns></returns>
        public String InitJsApi() { return this.InitJsApi(null); }
        /// <summary>
        /// 生成微信JsAPI初始化的json
        /// </summary>
        /// <param name="apis">接口方法列表</param>
        /// <returns></returns>
        public String InitJsApi(List<String> apis)
        {
            if (apis == null)
            {
                apis = new List<String>();
                apis.Add("onMenuShareTimeline");
                apis.Add("onMenuShareAppMessage");
                apis.Add("onMenuShareQQ");
                apis.Add("onMenuShareWeibo");
                apis.Add("startRecord");
                apis.Add("stopRecord");
                apis.Add("onVoiceRecordEnd");
                apis.Add("playVoice");
                apis.Add("pauseVoice");
                apis.Add("stopVoice");
                apis.Add("onVoicePlayEnd");
                apis.Add("uploadVoice");
                apis.Add("downloadVoice");
                apis.Add("chooseImage");
                apis.Add("previewImage");
                apis.Add("uploadImage");
                apis.Add("downloadImage");
                apis.Add("translateVoice");
                apis.Add("getNetworkType");
                apis.Add("openLocation");
                apis.Add("getLocation");
                apis.Add("hideOptionMenu");
                apis.Add("showOptionMenu");
                apis.Add("hideMenuItems");
                apis.Add("showMenuItems");
                apis.Add("hideAllNonBaseMenuItem");
                apis.Add("showAllNonBaseMenuItem");
                apis.Add("closeWindow");
                apis.Add("scanQRCode");
                apis.Add("chooseWXPay");
                apis.Add("openProductSpecificView");
                apis.Add("addCard");
                apis.Add("chooseCard");
                apis.Add("openCard");
            }
            String jsonApis = apis.ToJSONSString();
            String AppId = this.AppId;
            String TimeStamp = DateTime.Now.ToFileTime().ToString().Substring(0, 9);
            String NonceStr = Guid.NewGuid().ToString().Replace("-", "");
            String Sign = this.CreateJsApiSignature(NonceStr, TimeStamp, System.Web.HttpContext.Current.Request.Url.ToString());
            return "{debug: false, appId: \"" + AppId + "\", timestamp: " + TimeStamp + ", nonceStr: \"" + NonceStr + "\", signature: \"" + Sign + "\", jsApiList: " + jsonApis + "  }";
        }


        /// <summary>
        /// 发送模板消息
        /// </summary>       
        public Result SendTemplateMsg(TemplateMsg templateMsg)
        {
            //LogHelper.Info<string>("发送模板消息AccessToken：" + this.AccessToken.Token);
            Result ret = new Result();
            string apiUrl = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + this.AccessToken.Token;
            string res = this.Post(apiUrl, templateMsg.ToString());
            //LogHelper.Info<string>("发送模板消息res：" + res);
            try
            {
                JObject jo = JObject.Parse(res);
                ret.errcode = jo.GetValue("errcode") == null ? "" : jo.GetValue("errcode").ToString();
                ret.errmsg = jo.GetValue("errmsg") == null ? "" : jo.GetValue("errmsg").ToString();
            }
            catch (Exception ex)
            {
                ret.errcode = "-9";
                ret.errmsg = ex.Message + res;
                //LogHelper.Info<string>("发送模板消息Message：" + ret.errmsg);
            }
            return ret;
        }

        /// <summary>
        /// 发送模板消息
        /// </summary>       
        public Result SendTemplateMsg(string templateMsg)
        {
            //LogHelper.Info<string>("发送模板消息AccessToken：" + this.AccessToken.Token);
            Result ret = new Result();
            string apiUrl = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + this.AccessToken.Token;

            //LogHelper.Info<string>("发送模板消息templateMsg：" + templateMsg);
            string res = this.Post(apiUrl, templateMsg);
            //LogHelper.Info<string>("发送模板消息res：" + res);
            try
            {
                JObject jo = JObject.Parse(res);
                ret.errcode = jo.GetValue("errcode") == null ? "" : jo.GetValue("errcode").ToString();
                ret.errmsg = jo.GetValue("errmsg") == null ? "" : jo.GetValue("errmsg").ToString();
            }
            catch (Exception ex)
            {
                ret.errcode = "-9";
                ret.errmsg = ex.Message + res;
                LogHelper.Error<string>(ex, "发送模板消息Message：" + ret.errmsg);
            }
            return ret;
        }


        public bool SavePic(string serverId, string savePath)
        {
            string strfile = string.Empty;
            string content = string.Empty;
            string strpath = string.Empty;
            string savepath = string.Empty;
            string url = string.Empty;
            url = "http://file.api.weixin.qq.com/cgi-bin/media/get?access_token=" + this.AccessToken.Token + "&media_id=" + serverId;

            try
            {
                #region 保存接收到的图片文件
                System.Net.WebClient wc = new System.Net.WebClient();
                byte[] bs = wc.DownloadData(url);//下载图片 //



                MemoryStream mspic = new MemoryStream(bs, 0, bs.Length);
                mspic.Write(bs, 0, bs.Length);
                //Response.Write("<javascript>alert(已进入上传方法)</javascript>");
                //return "kkkkk";
                System.Drawing.Image iSource = System.Drawing.Image.FromStream(mspic);//保存图片
                                                                                      //return "保存图片";

                int iw = 800;
                //重新压缩图片设置压缩比例返回流宽度为700
                byte[] oa = DDb.Common.Attchment.SaveSimallImage(iSource, "", 1800, iw, 95);
                MemoryStream ms = new MemoryStream(oa, 0, oa.Length);
                ms.Write(oa, 0, oa.Length);

                System.Drawing.Image CroppedImage = System.Drawing.Image.FromStream(ms, true);//压缩图片



                //设置文件名 

                CroppedImage.Save(savePath);


                mspic.Close();
                ms.Close();
                iSource.Dispose();
                CroppedImage.Dispose();

                #endregion
                return true;
            }
            catch(Exception ex)
            {
                LogHelper.Info<string>("保存图片失败" + ex.StackTrace);
                return false;
            }


        }
        #endregion

        #region 属性设置
        private AccessToken _AccessToken = null;
        public AccessToken AccessToken
        {
            get
            {
                bool flag = false;//是否更新缓存
                object cacheObj = DDb.Common.DataCache.GetCache("token" + this.AppId);
                if (cacheObj != null)
                {
                    //LogHelper.Info<WechatHelper>("row 404 从缓存获取token：" + cacheObj.ToString());
                    this._AccessToken = JsonConvert.DeserializeObject<AccessToken>(cacheObj.ToString(), new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                    if (this._AccessToken.Expires < DateTime.Now)
                    {
                        //LogHelper.Info<WechatHelper>("row 407 从缓存获取token已过期，this._AccessToken.Expires：" + this._AccessToken.Expires);
                        flag = true;
                    }
                }
                else
                {
                    //为空直接更新缓存
                    //LogHelper.Info<WechatHelper>("row 414 从缓存获取token为空：" + DateTime.Now);
                    flag = true;
                }
                if (flag)
                {
                    this._AccessToken = this.GetTokenFromWechat();
                    //LogHelper.Info<string>("从网络获取token：" + DateTime.Now.ToString() + _AccessToken.ToJSONSString());
                    //缓存30分钟
                    DataCache.SetCache("token" + this.AppId, _AccessToken.ToJSONSString(), new TimeSpan(0, 30, 0));
                }
                return this._AccessToken;
            }
        }

        private JsAPITicket _JsAPITicket = null;
        public JsAPITicket JsAPITicket
        {
            get
            {
                if (_JsAPITicket == null || _JsAPITicket.Expires < DateTime.Now)
                {
                    //Loger.Debug("从网络获取JsAPITicket");
                    _JsAPITicket = this.GetJsAPITicketFromWechat();
                }
                else
                {
                    //Loger.Debug("从本地获取JsAPITicket");

                }
                return this._JsAPITicket;
            }
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 从腾讯服务器获取token
        /// </summary>
        /// <returns></returns>
        private AccessToken GetTokenFromWechat()
        {
            AccessToken at = new AccessToken();
            at.CreateTime = DateTime.Now;
            string url = String.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}",
                this.AppId,
                this.AppSecret);
            //LogHelper.Info<string>("从网络获取url：" + url);
            string source = this.GetUrl(url);
            //LogHelper.Info<string>("从网络获取source：" + source);
            JObject jo = JObject.Parse(source);
            if (jo.GetValue("access_token") != null)
            {
                at.Token = jo.GetValue("access_token").ToString();
            }
            at.Expires = DateTime.Now.AddSeconds(3600);
            return at;
        }

        private JsAPITicket GetJsAPITicketFromWechat()
        {
            JsAPITicket jsTicket = new JsAPITicket();

            string url = String.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + this.AccessToken.Token + "&type=jsapi");
            string source = this.GetUrl(url);
            JObject jo = JObject.Parse(source);
            jsTicket.errcode = jo.GetValue("errcode") != null ? jo.GetValue("errcode").ToString() : null;
            jsTicket.errmsg = jo.GetValue("errmsg") != null ? jo.GetValue("errmsg").ToString() : null;
            jsTicket.expires_in = jo.GetValue("expires_in") != null ? jo.GetValue("expires_in").ToString() : null;
            jsTicket.ticket = jo.GetValue("ticket") != null ? jo.GetValue("ticket").ToString() : null;
            jsTicket.Expires = DateTime.Now.AddSeconds(3600);
            return jsTicket;
        }


        private static string SHA1(string text)
        {
            byte[] cleanBytes = Encoding.Default.GetBytes(text);
            byte[] hashedBytes = System.Security.Cryptography.SHA1.Create().ComputeHash(cleanBytes);
            return BitConverter.ToString(hashedBytes).Replace("-", "");
        }
        #endregion

        #region 网络数据交换
        public string Post(string url, string postData)
        {
            return this.Post(url, postData, Encoding.UTF8, 10000);
        }
        public string Post(string url, string postData, System.Text.Encoding encoding, int timeOut)
        {
            //this.Loger.Debug("Post to Url:" + url);
            //LogHelper.Info<string>("从网络获取postData：" + postData);
            string response = "";
            HttpWebRequest myRequest = null;
            WebResponse webResponse = null;
            Stream stream = null;
            StreamReader sr = null;
            try
            {
                myRequest = (HttpWebRequest)WebRequest.Create(url);
                myRequest.Method = "Post";
                myRequest.ContentType = "application/x-www-form-urlencoded";
                myRequest.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.2; zh-CN; rv:1.9.0.6) Gecko/2009011913 Firefox/3.0.6 (.NET CLR 3.5.30729)";
                myRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                myRequest.ContentLength = 0;
                myRequest.AllowAutoRedirect = true;
                if (timeOut > 0)
                {
                    myRequest.Timeout = timeOut;//5秒超时
                }
                myRequest.KeepAlive = false;

                byte[] bufPost = encoding.GetBytes(postData);
                myRequest.ContentLength = bufPost.Length;
                Stream outStream = myRequest.GetRequestStream();
                outStream.Write(bufPost, 0, bufPost.Length);
                outStream.Close();


                webResponse = myRequest.GetResponse();
                //webResponse.ContentLength = 1024 * 1000;

                stream = webResponse.GetResponseStream();
                sr = new StreamReader(stream, encoding);

                response = sr.ReadToEnd();
                sr.Close();
                stream.Close();
                webResponse.Close();
                //this.Loger.Debug("Post res:" + response);

            }
            catch
            {

            }
            finally
            {
                if (webResponse != null) { webResponse.Close(); }
                if (stream != null) { stream.Close(); }
                if (sr != null) { sr.Close(); }
                if (myRequest != null) { myRequest.Abort(); }
            }
            return response;
        }

        public string GetUrl(string url)
        {
            return this.GetUrl(url, Encoding.UTF8, 10000);
        }
        public string GetUrl(string url, System.Text.Encoding encoding, int timeOut)
        {
            //this.Loger.Debug("Get Url:" + url);
            string response = "";
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
            WebResponse webResponse = null;
            Stream stream = null;
            StreamReader sr = null;
            try
            {

                myRequest.Method = "GET";
                myRequest.ContentType = "application/x-www-form-urlencoded";
                myRequest.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.2; zh-CN; rv:1.9.0.6) Gecko/2009011913 Firefox/3.0.6 (.NET CLR 3.5.30729)";
                myRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                myRequest.ContentLength = 0;
                myRequest.AllowAutoRedirect = true;

                if (timeOut > 0)
                {
                    myRequest.Timeout = timeOut;//5秒超时
                }
                myRequest.KeepAlive = false;

                webResponse = myRequest.GetResponse();
                //webResponse.ContentLength = 1024 * 1000;

                stream = webResponse.GetResponseStream();
                sr = new StreamReader(stream, encoding);

                response = sr.ReadToEnd();
                sr.Close();
                stream.Close();
                webResponse.Close();
                //this.Loger.Debug("Get Url Res:" + response);
            }
            catch (Exception ex)
            {
                //Loger.Error("get出错" + url, ex);
            }
            finally
            {
                if (webResponse != null) { webResponse.Close(); }
                if (stream != null) { stream.Close(); }
                if (sr != null) { sr.Close(); }
                if (myRequest != null) { myRequest.Abort(); }
            }
            return response;
        }
        #endregion


        public class Result
        {
            public Result() { }

            public string errcode { get; set; }
            public string errmsg { get; set; }

            public string data { get; set; }

            public string response { get; set; }
        }
    }

    public class TemplateMsg
    {
        public TemplateMsg() { this.Pars = new List<ParItem>(); }

        public string ToUserOpenId { get; set; }
        public string TemplateId { get; set; }
        public string Url { get; set; }

        public string TopColor { get; set; }


        public List<ParItem> Pars { get; set; }





        public override string ToString()
        {
            string templateData = "";
            templateData += "{";
            for (int i = 0; i < this.Pars.Count; i++)
            {
                if (i > 0) { templateData += ","; }
                templateData += "\"" + this.Pars[i].ParName + "\": {\"value\":\"" + this.Pars[i].Value + "\",\"color\":\"" + this.Pars[i].Color + "\"}";
            }
            templateData += "}";
            return "{\"touser\":\"" + this.ToUserOpenId + "\",\"template_id\":\"" + this.TemplateId + "\",\"url\":\"" + this.Url + "\",\"topcolor\":\"" + this.TopColor + "\",\"data\":" + templateData + "}";
        }

        public TemplateMsg Clone()
        {
            TemplateMsg clone = new TemplateMsg()
            {
                Pars = this.Pars,
                TemplateId = this.TemplateId,
                TopColor = this.TopColor,
                ToUserOpenId = this.ToUserOpenId,
                Url = this.Url
            };
            return clone;
        }

        public class ParItem
        {
            public ParItem() { }

            public string ParName { get; set; }

            public string Value { get; set; }
            public string Color { get; set; }
        }
    }

    public class AccessToken
    {
        public AccessToken() { }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// token值
        /// </summary>
        public String Token { get; set; }
        /// <summary>
        /// 有效期
        /// </summary>
        public DateTime Expires { get; set; }
    }

    public class JsAPITicket
    {
        public JsAPITicket() { }

        public String errcode { get; set; }
        public String errmsg { get; set; }
        public String ticket { get; set; }
        public String expires_in { get; set; }

        /// <summary>
        /// 有效期
        /// </summary>
        public DateTime Expires { get; set; }
    }

    public class OAuthAccessToken
    {
        public OAuthAccessToken() { }

        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string refresh_token { get; set; }
        public string openid { get; set; }
        public string scope { get; set; }
    }

    public class WechatUserInfo
    {
        public WechatUserInfo() { }
        public WechatUserInfo(string json) { this.init(json); }

        public void init(string json)
        {
            try
            {
                JObject obj = JObject.Parse(json);
                this.JsonSource = json;
                this.subscribe = obj.GetValue("subscribe") != null ? obj.GetValue("subscribe").ToString() : null;
                this.openid = obj.GetValue("openid") != null ? obj.GetValue("openid").ToString() : null;
                this.nickname = obj.GetValue("nickname") != null ? obj.GetValue("nickname").ToString() : null;
                this.sex = obj.GetValue("sex") != null ? obj.GetValue("sex").ToString() : null;
                this.language = obj.GetValue("language") != null ? obj.GetValue("language").ToString() : null;
                this.city = obj.GetValue("city") != null ? obj.GetValue("city").ToString() : null;
                this.province = obj.GetValue("province") != null ? obj.GetValue("province").ToString() : null;
                this.country = obj.GetValue("country") != null ? obj.GetValue("country").ToString() : null;
                this.headimgurl = obj.GetValue("headimgurl") != null ? obj.GetValue("headimgurl").ToString() : null;
                this.subscribe_time = obj.GetValue("subscribe_time") != null ? obj.GetValue("subscribe_time").ToString() : null;
            }
            catch (Exception ex)
            {
                //LogHelper.Error<WechatHelper>(ex, "解析用户信息出错:{0}", json);
                this.openid = "";
            }
            this.JsonSource = json;

        }

        public string JsonSource { get; set; }
        public string subscribe { get; set; }
        public string openid { get; set; }
        public string nickname { get; set; }
        public string sex { get; set; }
        public string language { get; set; }
        public string city { get; set; }
        public string province { get; set; }
        public string country { get; set; }
        public string headimgurl { get; set; }
        public string subscribe_time { get; set; }

    }

    public class WechatJSConfig
    {
        public WechatJSConfig(int wid)
        {
            this.init(null, wid);
        }
        public WechatJSConfig(String jsApiList, int wid)
        {
            this.init(jsApiList, wid);
        }

        private void init(String jsApiList, int wid)
        {
            if (jsApiList == null)
            {
                jsApiList = "['onMenuShareTimeline','onMenuShareAppMessage','onMenuShareQQ','onMenuShareWeibo','startRecord','stopRecord','onVoiceRecordEnd','playVoice','pauseVoice','stopVoice','onVoicePlayEnd','uploadVoice','downloadVoice','chooseImage','previewImage','uploadImage','downloadImage','translateVoice','getNetworkType','openLocation','getLocation','hideOptionMenu','showOptionMenu','hideMenuItems','showMenuItems','hideAllNonBaseMenuItem','showAllNonBaseMenuItem','closeWindow','scanQRCode','chooseWXPay','openProductSpecificView','addCard','chooseCard','openCard']";
            } 


            var weixin = Factory.Instance().iUserWeixinService.GetUserWeixin(Convert.ToInt32(wid)); 

            this.ApiList = jsApiList;
            this.AppId = weixin.AppId;
            this.Timestamp = DateTime.Now.ToFileTime().ToString().Substring(0, 9);
            this.NonceStr = Guid.NewGuid().ToString().Replace("-", "");
            this.Signature = new WechatHelper(wid).CreateJsApiSignature(this.NonceStr, this.Timestamp, System.Web.HttpContext.Current.Request.Url.ToString());

        }

        public String AppId { get; set; }
        public String Timestamp { get; set; }
        public String NonceStr { get; set; }
        public String Signature { get; set; }
        public String ApiList { get; set; }

        public override string ToString()
        {
            return "{debug: false, appId: '" + this.AppId + "', timestamp: " + this.Timestamp + ", nonceStr: '" + this.NonceStr + "', signature: '" + this.Signature + "', jsApiList:" + this.ApiList + "}";
        }
    }


    public class PayResultNotify
    {
        private String xml = "";
        public PayResultNotify() { }
        public PayResultNotify(string xml)
        {
            this.xml = xml;
            this.InitData(xml);
        }

        public void InitData(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(this.xml);
            XmlNode root = doc.SelectSingleNode("xml");
            this.appid = this.GetChildString(root, "appid");
            this.return_code = this.GetChildString(root, "return_code");
            this.return_msg = this.GetChildString(root, "return_msg");
            this.mch_id = this.GetChildString(root, "mch_id");
            this.device_info = this.GetChildString(root, "device_info");
            this.nonce_str = this.GetChildString(root, "nonce_str");
            this.sign = this.GetChildString(root, "sign");
            this.result_code = this.GetChildString(root, "result_code");
            this.err_code = this.GetChildString(root, "err_code");
            this.err_code_des = this.GetChildString(root, "err_code_des");
            this.openid = this.GetChildString(root, "openid");
            this.is_subscribe = this.GetChildString(root, "is_subscribe");
            this.trade_type = this.GetChildString(root, "trade_type");
            this.bank_type = this.GetChildString(root, "bank_type");
            this.total_fee = this.GetChildString(root, "total_fee");
            this.fee_type = this.GetChildString(root, "fee_type");
            this.cash_fee = this.GetChildString(root, "cash_fee");
            this.cash_fee_type = this.GetChildString(root, "cash_fee_type");
            this.coupon_fee = this.GetChildString(root, "coupon_fee");
            this.coupon_count = this.GetChildString(root, "coupon_count");
            this.transaction_id = this.GetChildString(root, "transaction_id");
            this.out_trade_no = this.GetChildString(root, "out_trade_no");
            this.attach = this.GetChildString(root, "attach");
            this.time_end = this.GetChildString(root, "time_end");
        }
        private String GetChildString(XmlNode note, string name)
        {
            return note.SelectSingleNode(name) != null ? note.SelectSingleNode(name).InnerText : null;
        }

        /// <summary>
        /// 根据传入参数计算出的加密参数
        /// </summary>
        /// <returns></returns>
        public Dictionary<String, String> CountSign()
        {
            System.Reflection.PropertyInfo[] propertys = this.GetType().GetProperties();
            Dictionary<String, String> dict = new Dictionary<string, string>();
            for (int i = 0; i < propertys.Length; i++)
            {
                if (propertys[i].Name != "sign")
                {
                    if (propertys[i].GetValue(this, null) != null)
                    {
                        dict.Add(propertys[i].Name, propertys[i].GetValue(this, null).ToString());
                    }
                }
            }
            return dict;
        }

        /// <summary>
        /// 根据传入参数计算出的加密参数
        /// </summary>
        /// <returns></returns>
        public String CountSign(int wid)
        {
            System.Reflection.PropertyInfo[] propertys = this.GetType().GetProperties();
            Dictionary<String, String> dict = new Dictionary<string, string>();
            for (int i = 0; i < propertys.Length; i++)
            {
                if (propertys[i].Name != "sign")
                {
                    if (propertys[i].GetValue(this, null) != null)
                    {
                        dict.Add(propertys[i].Name, propertys[i].GetValue(this, null).ToString());
                    }
                }
            }
            return new WechatHelper(wid).WechatPaySign(dict);
        }


        /// <summary>
        /// 微信分配的公众账号ID
        /// </summary>
        public String appid { get; set; }
        /// <summary>
        /// 商家数据包
        /// </summary>
        public String attach { get; set; }
        /// <summary>
        /// 银行类型，采用字符串类型的银行标识，银行类型见附表
        /// </summary>
        public String bank_type { get; set; }
        /// <summary>
        /// 现金支付金额订单现金支付金额
        /// </summary>
        public String cash_fee { get; set; }
        /// <summary>
        /// 货币类型，符合ISO 4217标准的三位字母代码，默认人民币：CNY，其他值列表
        /// </summary>
        public String cash_fee_type { get; set; }
        /// <summary>
        /// 微信支付分配的终端设备号
        /// </summary>
        public String device_info { get; set; }
        /// <summary>
        /// 货币类型，符合ISO 4217标准的三位字母代码，默认人民币：CNY，其他值列表详见货币类型
        /// </summary>
        public String fee_type { get; set; }
        /// <summary>
        /// 用户是否关注公众账号，Y-关注，N-未关注，仅在公众账号类型支付有效
        /// </summary>
        public String is_subscribe { get; set; }
        /// <summary>
        /// 微信支付分配的商户号
        /// </summary>
        public String mch_id { get; set; }
        /// <summary>
        /// 随机字符串，不长于32位
        /// </summary>
        public String nonce_str { get; set; }
        /// <summary>
        /// 用户在商户appid下的唯一标识
        /// </summary>
        public String openid { get; set; }
        /// <summary>
        /// 商户订单号
        /// </summary>
        public String out_trade_no { get; set; }
        /// <summary>
        /// 业务结果: SUCCESS/FAIL
        /// </summary>
        public String result_code { get; set; }
        /// <summary>
        /// 返回状态码:SUCCESS/FAIL  SUCCESS表示商户接收通知成功并校验成功
        /// </summary>
        public String return_code { get; set; }
        /// <summary>
        /// 返回信息，如非空，为错误原因：
        /// </summary>
        public String return_msg { get; set; }
        /// <summary>
        /// 加密参数
        /// </summary>
        public String sign { get; set; }
        /// <summary>
        /// 支付完成时间，格式为yyyyMMddHHmmss，如2009年12月25日9点10分10秒表示为20091225091010
        /// </summary>
        public String time_end { get; set; }
        /// <summary>
        /// 订单总金额，单位为分
        /// </summary>
        public String total_fee { get; set; }
        /// <summary>
        /// 交易类型
        /// </summary>
        public String trade_type { get; set; }
        /// <summary>
        /// 微信支付订单号
        /// </summary>
        public String transaction_id { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        public String err_code { get; set; }
        /// <summary>
        /// 错误代码描述
        /// </summary>
        public String err_code_des { get; set; }
        /// <summary>
        /// 代金券或立减优惠金额<=订单总金额，订单总金额-代金券或立减优惠金额=现金支付金额
        /// </summary>

        public String coupon_fee { get; set; }

        /// <summary>
        /// 代金券或立减优惠使用数量
        /// </summary>
        public String coupon_count { get; set; }
    }

    public class UnifiedOrderResponse
    {
        public UnifiedOrderResponse() { }
        public UnifiedOrderResponse(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode root = doc.SelectSingleNode("xml");
            System.Reflection.PropertyInfo[] propertys = this.GetType().GetProperties();
            for (int i = 0; i < propertys.Length; i++)
            {
                XmlNode node = root.SelectSingleNode(propertys[i].Name);
                if (node != null)
                {
                    propertys[i].SetValue(this, node.InnerText, null);
                }
            }
        }



        /// <summary>
        /// SUCCESS/FAIL 此字段是通信标识，非交易标识，交易是否成功需要查看result_code来判断
        /// </summary>
        public String return_code { get; set; }
        /// <summary>
        /// 错误原因
        /// </summary>
        public String return_msg { get; set; }
        /// <summary>
        /// 调用接口提交的公众账号ID
        /// </summary>
        public String appid { get; set; }
        /// <summary>
        /// 调用接口提交的商户号
        /// </summary>
        public String mch_id { get; set; }
        /// <summary>
        /// 调用接口提交的终端设备号
        /// </summary>
        public String device_info { get; set; }
        /// <summary>
        /// 微信返回的随机字符串
        /// </summary>
        public String nonce_str { get; set; }
        /// <summary>
        /// 微信返回的签名
        /// </summary>
        public String sign { get; set; }
        /// <summary>
        /// 业务结果  SUCCESS/FAIL
        /// </summary>
        public String result_code { get; set; }
        /// <summary>
        /// 预支付交易会话标识:微信生成的预支付回话标识，用于后续接口调用中使用，该值有效期为2小时
        /// </summary>
        public String prepay_id { get; set; }
        /// <summary>
        /// 交易类型:trade_type为NATIVE是有返回，可将该参数值生成二维码展示出来进行扫码支付
        /// </summary>
        public String trade_type { get; set; }



    }

    public class UnifiedOrder
    {
        public UnifiedOrder()
        {
            this.fee_type = "CNY";
            this.device_info = "WEB";
        }

        #region 属性设置

        /// <summary>
        /// 公众账号ID
        /// </summary>
        public String appid { get; set; }

        /// <summary>
        /// 商品或支付单简要描述
        /// </summary>
        public String body { get; set; }

        /// <summary>
        /// 微信支付分配的商户号
        /// </summary>
        public String mch_id { get; set; }
        /// <summary>
        /// 随机字符串，不长于32位
        /// </summary>
        public String nonce_str { get; set; }
        /// <summary>
        /// 接收微信支付异步通知回调地址
        /// </summary>
        public String notify_url { get; set; }

        /// <summary>
        /// 商户系统内部的订单号,32个字符内、可包含字母, 
        /// </summary>
        public String out_trade_no { get; set; }
        /// <summary>
        /// APP和网页支付提交用户端ip，Native支付填调用微信支付API的机器IP
        /// </summary>
        public String spbill_create_ip { get; set; }
        /// <summary>
        /// 订单总金额，只能为整数
        /// </summary>
        public String total_fee { get; set; }
        /// <summary>
        /// 取值如下：JSAPI，NATIVE，APP，
        /// </summary>
        public String trade_type { get; set; }

        /// <summary>
        /// trade_type=JSAPI，此参数必传，用户在商户appid下的唯一标识。下单前需要调用【网页授权获取用户信息】接口获取到用户的Openid。
        /// </summary>
        public String openid { get; set; }

        /// <summary>
        /// 签名
        /// </summary>

        public String sign { get; set; }




        //*********以下为非必填项目******************


        /// <summary>
        /// 符合ISO 4217标准的三位字母代码，默认人民币
        /// </summary>
        public String fee_type { get; set; }

        /// <summary>
        /// 附加数据，在查询API和支付通知中原样返回，该字段主要用于商户携带订单的自定义数据
        /// </summary>
        public String attach { get; set; }
        /// <summary>
        /// 订单生成时间，格式为yyyyMMddHHmmss，如2009年12月25日9点10分10秒表示为20091225091010。
        /// </summary>
        public String time_start { get; set; }
        /// <summary>
        /// 订单失效时间，格式为yyyyMMddHHmmss，如2009年12月27日9点10分10秒表示为20091227091010。
        /// </summary>
        public String time_expire { get; set; }
        /// <summary>
        /// 商品标记，代金券或立减优惠功能的参数
        /// </summary>
        public String goods_tag { get; set; }
        /// <summary>
        /// trade_type=NATIVE，此参数必传。此id为二维码中包含的商品ID，商户自行定义。
        /// </summary>
        public String product_id { get; set; }

        /// <summary>
        /// 商品名称明细列表
        /// </summary>
        public String detail { get; set; }

        /// <summary>
        /// 终端设备号(商户的门店号或设备ID)，注意：PC网页或公众号内支付请传"WEB"
        /// </summary>
        public String device_info { get; set; }

        #endregion


        public Dictionary<String, String> CountSign()
        {
            System.Reflection.PropertyInfo[] propertys = this.GetType().GetProperties();
            Dictionary<String, String> dict = new Dictionary<string, string>();
            for (int i = 0; i < propertys.Length; i++)
            {
                if (propertys[i].Name != "sign")
                {
                    if (propertys[i].GetValue(this, null) != null)
                    {
                        dict.Add(propertys[i].Name, propertys[i].GetValue(this, null).ToString());
                    }
                }
            }
            return dict;



            //System.Reflection.PropertyInfo[] propertys = this.GetType().GetProperties();
            //List<String> listProperty = new List<string>();
            //for (int i = 0; i < propertys.Length; i++)
            //{
            //    if (propertys[i].Name != "sign")
            //    {
            //        listProperty.Add(propertys[i].Name);
            //    }
            //}
            //listProperty.Sort();
            //String stringSignTemp = "";
            //for (int i = 0; i < listProperty.Count; i++)
            //{
            //    String val = "";
            //    for (int j = 0; j < propertys.Length; j++)
            //    {
            //        if (propertys[j].Name == listProperty[i])
            //        {
            //            var propertyValue = propertys[j].GetValue(this, null);
            //            if (propertys[j].GetValue(this, null) != null)
            //            {
            //                val = propertys[j].GetValue(this, null).ToString();
            //            }
            //            break;
            //        }
            //    }
            //    if (val != null && val != "")
            //    {
            //        stringSignTemp += listProperty[i] + "=" + val;
            //        stringSignTemp += "&";
            //    }
            //}
            //stringSignTemp += "key=" + this.key;
            //this.sign = stringSignTemp.Md5().ToUpper();
        }

        public string ToPostData()
        {
            string xml = "<xml>";
            System.Reflection.PropertyInfo[] propertys = this.GetType().GetProperties();
            List<String> listProperty = new List<string>();
            for (int i = 0; i < propertys.Length; i++)
            {
                listProperty.Add(propertys[i].Name);
            }
            listProperty.Sort();
            for (int i = 0; i < listProperty.Count; i++)
            {
                String val = "";
                for (int j = 0; j < propertys.Length; j++)
                {
                    if (propertys[j].Name == listProperty[i])
                    {
                        var propertyValue = propertys[j].GetValue(this, null);
                        if (propertys[j].GetValue(this, null) != null)
                        {
                            val = propertys[j].GetValue(this, null).ToString();
                        }
                        break;
                    }
                }
                if (val != null && val != "" && listProperty[i] != "sign")
                {
                    xml += "<" + listProperty[i] + ">" + val + "</" + listProperty[i] + ">\r\n";
                }
            }
            xml += "<sign>" + this.sign + "</sign>\r\n";
            xml += "</xml>";

            return xml;
        }
    }


    public class OrderQueryResponse
    {
        public OrderQueryResponse() { }
        public OrderQueryResponse(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode root = doc.SelectSingleNode("xml");
            System.Reflection.PropertyInfo[] propertys = this.GetType().GetProperties();
            for (int i = 0; i < propertys.Length; i++)
            {
                XmlNode node = root.SelectSingleNode(propertys[i].Name);
                if (node != null)
                {
                    propertys[i].SetValue(this, node.InnerText, null);
                }
            }
        }



        /// <summary>
        /// SUCCESS/FAIL 此字段是通信标识，非交易标识，交易是否成功需要查看result_code来判断
        /// </summary>
        public String return_code { get; set; }
        /// <summary>
        /// 错误原因
        /// </summary>
        public String return_msg { get; set; }
        /// <summary>
        /// 调用接口提交的公众账号ID
        /// </summary>
        public String appid { get; set; }
        /// <summary>
        /// 调用接口提交的商户号
        /// </summary>
        public String mch_id { get; set; }
        /// <summary>
        /// 调用接口提交的终端设备号
        /// </summary>
        public String sub_appid { get; set; }
        /// <summary>
        /// 微信返回的随机字符串
        /// </summary>
        public String sub_mch_id { get; set; }
        /// <summary>
        /// 微信返回的签名
        /// </summary>
        public String nonce_str { get; set; }
        /// <summary>
        /// 业务结果  SUCCESS/FAIL
        /// </summary>
        public String sign { get; set; }
        /// <summary>
        /// 预支付交易会话标识:微信生成的预支付回话标识，用于后续接口调用中使用，该值有效期为2小时
        /// </summary>
        public String err_code { get; set; }
        public String result_code { get; set; }
        /// <summary>
        /// 交易类型:trade_type为NATIVE是有返回，可将该参数值生成二维码展示出来进行扫码支付
        /// </summary>
        public String err_code_des { get; set; }
        /// <summary>
        /// 支付状态
        /// </summary>
        public string trade_state { get; set; }
        public string transaction_id { get; set; }



    }

    /// <summary>
    /// 微信订单查询接口
    /// </summary>
    public class OrderQuery
    { 

        #region 属性设置

        /// <summary>
        /// 公众账号ID
        /// </summary>
        public String appid { get; set; }

        /// <summary>
        /// 微信支付分配的商户号
        /// </summary>
        public String mch_id { get; set; }
        /// <summary>
        /// 子商户号 
        /// </summary>
        public String sub_mch_id { get; set; }
        /// <summary>
        /// 微信订单号
        /// </summary>
        public String transaction_id { get; set; }
        /// <summary>
        /// 商户系统内部的订单号,32个字符内、可包含字母, 
        /// </summary>
        public String out_trade_no { get; set; }
        /// <summary>
        /// 随机字符串，不长于32位
        /// </summary>
        public String nonce_str { get; set; } 
        /// <summary>
        /// 签名
        /// </summary>
        public String sign { get; set; }



 

        #endregion


        public Dictionary<String, String> CountSign()
        {
            System.Reflection.PropertyInfo[] propertys = this.GetType().GetProperties();
            Dictionary<String, String> dict = new Dictionary<string, string>();
            for (int i = 0; i < propertys.Length; i++)
            {
                if (propertys[i].Name != "sign")
                {
                    if (propertys[i].GetValue(this, null) != null)
                    {
                        dict.Add(propertys[i].Name, propertys[i].GetValue(this, null).ToString());
                    }
                }
            }
            return dict; 
        }

        public string ToPostData()
        {
            string xml = "<xml>";
            System.Reflection.PropertyInfo[] propertys = this.GetType().GetProperties();
            List<String> listProperty = new List<string>();
            for (int i = 0; i < propertys.Length; i++)
            {
                listProperty.Add(propertys[i].Name);
            }
            listProperty.Sort();
            for (int i = 0; i < listProperty.Count; i++)
            {
                String val = "";
                for (int j = 0; j < propertys.Length; j++)
                {
                    if (propertys[j].Name == listProperty[i])
                    {
                        var propertyValue = propertys[j].GetValue(this, null);
                        if (propertys[j].GetValue(this, null) != null)
                        {
                            val = propertys[j].GetValue(this, null).ToString();
                        }
                        break;
                    }
                }
                if (val != null && val != "" && listProperty[i] != "sign")
                {
                    xml += "<" + listProperty[i] + ">" + val + "</" + listProperty[i] + ">\r\n";
                }
            }
            xml += "<sign>" + this.sign + "</sign>\r\n";
            xml += "</xml>";

            return xml;
        }
    }

    


}
