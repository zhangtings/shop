using CZ.Application;
using DDb.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZ.Areas.wfxshoping.Controllers
{
    public class BaseController : Controller
    {
        public IUserService _iUserService { get; set; }
        public ICategoryService _iCategoryService { get; set; }
        public int wid = 2;
        public string fxid;
        public string bjid;
        public string title = "业主超市";
        //public BaseController(IUserService iEFUserSer)
        //{
        //    _iUserService = iEFUserSer;
        //}

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        { 
            string requestUrl = System.Web.HttpContext.Current.Request.Url.ToString().ToLower();
            Application.IUserInfoService _iUserInfoService = Application.Factory.Instance().iUserInfoService;
            object fxid="0"; object bjid="1";



            //开发库用固定session
            if (requestUrl.IndexOf("localhost") >= 0)
            {
                //System.Web.HttpContext.Current.Session["Openid"] = "oEeLDweJAHwIAG9T2XwDiEd2tbcU";
                System.Web.HttpContext.Current.Session["Openid"] = "oQukL0fa013osRbWSdIPaUt88JZE";

            }
            //System.Web.HttpContext.Current.Session["Openid"] = "oEeLDweJAHwIAG9T2XwDiEd2tbcU";


            



            LogHelper.Info<BaseController>("当前url："+ requestUrl);
            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["state"]))
            {
                Models.UserWeixinDTO weixin = Application.Factory.Instance().iUserWeixinService.GetUserWeixin(wid);

                Application.Weixin.OAuthAccessToken result = null;
                string code = System.Web.HttpContext.Current.Request["code"].Trim();
                if (string.IsNullOrEmpty(code))
                {
                    LogHelper.Info<BaseController>("你拒绝了");
                    Response.End();
                }
                else
                {
                    //通过，用code换取access_token
                    //result = OAuth.GetAccessToken(weixin.AppId, weixin.AppSecret, code);
                    //LogHelper.Info<oauth>("获取result:" + result.openid);

                    string url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + weixin.AppId + "&secret=" + weixin.AppSecret + "&code=" + code + "&grant_type=authorization_code";

                    string source = DDb.Common.HttpSend.getSend(url);

                    //表示Code已被使用需要重新授权
                    if (source.Contains("40163"))
                    {
                        Application.Weixin.WechatHelper wechatHelper = new Application.Weixin.WechatHelper();

                        string oaurl = wechatHelper.CreateOAuth2Url();
                        LogHelper.Info<BaseController>("oaurl" + oaurl);
                        filterContext.Result = new RedirectResult(oaurl);
                    }

                    LogHelper.Info<BaseController>("获取source:" + source);
                    result = source.ToObejct<Application.Weixin.OAuthAccessToken>();

                    //下面2个数据也可以自己封装成一个类，储存在数据库中（建议结合缓存）
                    //如果可以确保安全，可以将access_token存入用户的cookie中，每一个人的access_token是不一样的
                    Session["OAuthAccessTokenStartTime"] = DateTime.Now;
                    Session["OAuthAccessToken"] = result;
                    Session["openid"] = result.openid;

                    //有了openid后，通过openid获取用户信息，判断是否是分销账号，修改fxid标记
                    if (string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["fxid"]))
                    {
                        if (System.Web.HttpContext.Current.Session["fxid"] == null)
                        {
                            if (UserData != null && UserData.Fxzt == "fx")
                            {
                                fxid = UserData.Id;
                                Session["fxid"] = UserData.Id;
                                bjid = _iCategoryService.GetBJid(UserData.Fxzt).ToString();
                                Session["bjid"] = bjid;
                            }
                        }
                    }
                    else
                    {
                        Session["fxid"] = System.Web.HttpContext.Current.Request.QueryString["fxid"];
                    }

                    LogHelper.Info<BaseController>(Session["fxid"] + "---" + Session["bjid"]);

                    Models.UserInfoDTO user = new Models.UserInfoDTO();
                    
                    //先取用户
                    user = _iUserInfoService.GetUserInfo(result.openid);

                    string returl = Request.QueryString["state"];
                    returl = System.Web.HttpUtility.UrlDecode(returl);
                    

                    if (!returl.Contains("http"))
                    {
                        returl = MyCommFun.getWebSite() + "/wfxShoping/main/index?wid=" + wid + "&fxid=" + fxid + " & openid=" + result.openid;
                    }

                    LogHelper.Info<BaseController>("微信登陆成功回调returl:" + returl);
                    if (user == null)
                    {
                        //LogHelper.Info<BaseController>("增加用户openid:" + result.openid);
                        user = new Models.UserInfoDTO();
                        user.wid = wid;
                        user.openid = result.openid;
                        user.subscribe = 0;
                        var newUser = _iUserInfoService.SaveOrUpdateUserInfo(user);

                        if (newUser.Code == "success")
                        {
                            user.id = newUser.Key.ToInt32();
                            Session["WeixinUser"] = user;
                            //LogHelper.Info<BaseController>("增加用户uid:" + user.id);
                        }
                        else
                        {
                            LogHelper.Info<BaseController>("增加用户error:" + newUser.Text);
                        }
                        //filterContext.Result = new RedirectResult(returl); 

                    }
                    else
                    {
                        if (user.subscribe == 0)
                        {
                            Application.Weixin.WechatHelper wechat = new Application.Weixin.WechatHelper(wid);

                            Application.Weixin.WechatUserInfo UserInfo = wechat.GetUserInfo(user.openid);
                            user.nickname = UserInfo.nickname;
                            user.headimgul = UserInfo.headimgurl;
                            user.subscribe = int.Parse(UserInfo.subscribe);

                            user.sex = int.Parse(UserInfo.sex);
                            _iUserInfoService.SaveOrUpdateUserInfo(user);
                        }
                        //LogHelper.Info<BaseController>("登陆成功uid" + user.id);
                        Session["WeixinUser"] = user;
                    }



                    Models.UserDTO userDTO = _iUserService.Get(result.openid);
                    if (userDTO == null)
                    {
                        userDTO = new Models.UserDTO()
                        {
                            Openid = result.openid,
                            Uname = user.nickname,
                            Photo = user.headimgul,
                            Sex = (user.sex.Value),
                            Addtime = (int)GetTimestamp(DateTime.Now),
                            Source ="wx"
                        };
                        var xx = _iUserService.SaveOrUpdate(userDTO);
                        userDTO.Id = int.Parse((string)xx.Key);
                    }
                    
                    Session["UserDTO"] = userDTO;

                }
            }


            if (System.Web.HttpContext.Current.Session["Openid"] == null)
            {
                Application.Weixin.WechatHelper wechatHelper = new Application.Weixin.WechatHelper(wid);

                string oaurl = wechatHelper.CreateOAuth2Url();
                LogHelper.Info<BaseController>("oaurl" + oaurl);
                filterContext.Result = new RedirectResult(oaurl);
            }
            ViewBag.fxid = Session["fxid"];
            ViewBag.bjid = Session["bjid"]??bjid;
            ViewBag.Title = Session["bjid"] == null ? title : "物业采购";
        }
        
        private String _OpenId = null;
        public String OpenId
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["openid"] != null)
                {
                    _OpenId = System.Web.HttpContext.Current.Session["openid"].ToString();
                }
                return _OpenId;
            }
        }

        private Models.UserInfoDTO _OldUserData = null;
        private Models.UserDTO _UserData = null;

        /// <summary>
        /// 获取用户当前的openid下的用户信息 未存session
        /// </summary>
        public Models.UserDTO UserData
        {
            get
            {
                if (_UserData == null)
                {
                    if (Session["UserDTO"] != null) { _UserData = (Models.UserDTO)Session["UserDTO"];
                    }
                    else if (System.Web.HttpContext.Current.Session["Openid"] != null)
                    {
                        //_UserData = Application.Factory.Instance().iUserInfoService.GetUserInfo(System.Web.HttpContext.Current.Session["Openid"].ToString());
                        var ud=_iUserService.Get(System.Web.HttpContext.Current.Session["Openid"].ToString());
                        _UserData = ud;
                        Session["UserDTO"] = ud;
                    }
                }
                return _UserData;
            }
        }

        public long GetTimestamp(DateTime dateTime)
        {
            return (dateTime.Ticks - 621356256000000000) / 10000000;
        }

        public DateTime NewDate(long timestamp)
        {
            long tt = 621356256000000000 + timestamp * 10000000;
            return new DateTime(tt);
        }
    }
}