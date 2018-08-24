using CZ.Application;
using CZ.Application.Weixin;
using CZ.Models;
using CZ.Models.Common;
using DDb.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CZ.Areas.wfxshoping.Controllers
{
    public class UserController : BaseController
    {
        //private IUserService _iUserService;
        //private IProductService _iProductService;
        public IEFProductService _iProSer { get; set; }
        public IOrderService _iOrderService { get; set; }
        //public UserController(IUserService iUserService, 
        //    IOrderService iOrderService, 
        //    IEFProductService iProSer)
        //{
        //    _iUserService = iUserService;
        //    _iOrderService =iOrderService;
        //    _iProSer = iProSer;

        // }


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult def()
        {
            return View();
        }

        // 相同的商品只能一个人只能有一条记录，重复加入将提示。
        public JsonResult AddToCar(int? pid)
        {
            //从session（"openid"）或者session（"userid"）
            var result = new Models.Common.Message();
            var dto = new ProductDTO() { Id = pid.Value };
            if (pid == null) return Json(result.Text = "没有这个商品");
            if (_iUserService.GetInCar(UserData, dto))
            {
                result = _iUserService.AddToCar(UserData, dto);
            }
            else {
                result.Text = "已经在购物车里了，亲！";
            }
            return Json(result,JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShopCar() {
            ViewBag.Carlist=_iUserService.GetCarList(UserData.Id);
            return View();
        }

        public ActionResult MyShop()
        {
            UserDTO model = _iUserService.Get(UserData.Id);
            OrderDTO dTO = new OrderDTO
            {
                Uid = UserData.Id,
                Pager = new Pagination()
                {
                    CurrentPage = 1,
                    PageSize = 100
                }
            };
            List<OrderDTO> dTOs = _iOrderService.GetPage(dTO);
            ViewBag.status10 = (from s in dTOs where s.Status == 10 select s).Count();
            ViewBag.status20 = (from s in dTOs where s.Status == 20 select s).Count();
            ViewBag.status30 = (from s in dTOs where s.Status == 30 select s).Count();
            ViewBag.status40 = (from s in dTOs where s.Status == 40 select s).Count();
            return View(model);
        }

        /// <summary>
        /// 生成订单记录，返回订单id,---这个方法需要优化。
        /// </summary>
        /// <param name="id">购物车记录id,生成订单后根据id删除购物车记录</param>
        /// <param name="pid">商品id</param>
        /// <param name="num">购买几件该记录的商品</param>
        /// <param name="totalmomey">总价</param>
        public JsonResult AddOrder(List<int> id,List<int> pid, List<int> num , decimal totalmomey)
        {
            GetUserInfo(UserData, "地址");
            if (UserData.DefUaddress == null) { return Json(Message.Success("Address")); }
            //减库存应该是生成订单后还是付款成功后？
            //这里减库存的话，取消订单还要加回去。
            totalmomey = 0;
            OrderDTO dTO = new OrderDTO
            {
                Products = _iProSer.products(pid),
                Addtime = (int)GetTimestamp(DateTime.Now),
                Uid = UserData.Id,
                Status = 10,
                Address_xq = UserData.DefUaddress.Address_xq,
                Receiver = UserData.DefUaddress.Name,
                Tel = UserData.DefUaddress.Tel,
                Order_sn = DateTime.Now.ToString("yyyyMMddHHmmss") + DDb.Common.Rand.Number(6)
            };

            int i = 0;
            foreach (var x in pid)
            {
                var p=(from m in dTO.Products where m.Id == x select m).First();
                p.Num = num[i];
                totalmomey += p.Price_yh>0? p.Price_yh * p.Num:p.Price * p.Num;
                i++;
            }
            dTO.Price = totalmomey;
            dTO.Post_remark = "";
            dTO.Code = 0;
            var result = _iOrderService.SaveOrUpdate(dTO);
            _iUserService.Delete(id);//生成订单后清理购物车
            return Json(result);
        }

        /// <summary>
        /// 从购物车中删除记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult CartDel(List<int> id)
        {
            var result= _iUserService.Delete(id);
            return Json(result);
        }
        /// <summary>
        /// 直接购买商品，跳转到Pay页面
        /// </summary>
        /// <param name="Id">商品id</param>
        public ActionResult PayNow(int? Id,int? num)
        {
            //WechatJSConfig wechatJS = new WechatJSConfig(wid);
            WechatJSConfig wechatJS = new WechatJSConfig("['chooseWXPay']", wid);
            ViewData["wxjs"] = wechatJS.ToString();
            GetUserInfo(UserData, "地址");
            if (UserData.DefUaddress == null) { return RedirectToAction("Address");  }

            if (Id == null) return View("index");
            if (num == null) num = 1;
            OrderDTO dTO = new OrderDTO();
            //var uadto = _iUserService.GetAddress(UserData);
            ProductDTO pdTO = _iProSer.Get(Id.Value);
            pdTO.Num = num.Value;
            dTO.Products = new List<ProductDTO>
            {
                pdTO
            };
            dTO.Price = pdTO.Price_yh>0? pdTO.Price_yh * num.Value: pdTO.Price*num.Value;
            dTO.Addtime = (int)GetTimestamp(DateTime.Now);
            dTO.Uid = UserData.Id;
            dTO.Address_xq = UserData.DefUaddress.Address_xq;
            dTO.Receiver = UserData.DefUaddress.Name;
            dTO.Tel = UserData.DefUaddress.Tel;
            dTO.Status = 10;
            dTO.Order_sn = DateTime.Now.ToString("yyyyMMddHHmmss") + Rand.Number(6);
            dTO.Post_remark = "";
            dTO.Post = 0;
            var result = _iOrderService.SaveOrUpdate(dTO);
            dTO = _iOrderService.Get(int.Parse(result.Key.ToString()));
            dTO.OrderProducts = _iOrderService.GetProducts(dTO.Id);
            
            return View("Pay", dTO);
        }

        /// <summary>
        /// 根据订单id显示详细的订单信息，要验证用户。
        /// </summary>
        /// <param name="id">订单id</param>
        /// <returns>返回用户DTO</returns>
        public ActionResult OrderInfo(int? id)
        {
            //WechatJSConfig wechatJS = new WechatJSConfig(wid);
            WechatJSConfig wechatJS = new WechatJSConfig("['chooseWXPay']", wid);
            ViewData["wxjs"] = wechatJS.ToString();
            OrderDTO dTO = _iOrderService.Get(id.Value);
            dTO.OrderProducts = _iOrderService.GetProducts(dTO.Id);
            return View(dTO);
        }

        /// <summary>
        /// 根据订单id显示付款信息，带上用户地址信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Pay(int? id) {
            //WechatJSConfig wechatJS = new WechatJSConfig(wid);
            WechatJSConfig wechatJS = new WechatJSConfig("['chooseWXPay']", wid);
            //jsApiList = 
            ViewData["wxjs"] = wechatJS.ToString();

            OrderDTO model = new OrderDTO();
            if (id > 0)
            {
                model = _iOrderService.Get(id.Value);
                model.OrderProducts = _iOrderService.GetProducts(id.Value);
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult wxCreatePay(int oid,string remark=null)
        {
            OrderDTO model = _iOrderService.Get(oid);
            model.Shop_id = Session["fxid"].ToInt32();
            if (remark != null) {

                model.Remark = remark;//输入验证？
                _iOrderService.SaveOrUpdate(model);
            }
             //先用商店id当分销id;
            _iOrderService.ChangeStatus(model, "付款成功");
            //string teststr="{\"success\": null }";
            //return Json(Message.Success(teststr));//测试用
            WechatHelper wechatHelper = new WechatHelper(this.wid);
            string notifyUrl = "http://ms.571400yb.com/wfxShoping/api/wxNotify";

            if (this.OpenId == "oEeLDwVCN9aA66-ghojAh8ptupNQ")
            {
                model.Price = 0.01M;
            }
            //第一个参数需要修改。
            UnifiedOrderResponse uoRes = wechatHelper.CreateUnifiedOrder(title+" 购买商品", notifyUrl, this.OpenId, model.Order_sn, Convert.ToInt32((model.Price) * 100));

            if (uoRes.return_code.ToLower() == "SUCCESS".ToLower())
            {
                LogHelper.Info<UserController>("创建订单成功:" + wid);

                string timeSpan = DateTime.Now.ToFileTime().ToString().Substring(0, 9);
                string nonceStr = Guid.NewGuid().ToString().Replace("-", "");

                model.Pay_sn = uoRes.prepay_id;

                string package = "prepay_id=" + uoRes.prepay_id;
                Dictionary<String, String> dict = new Dictionary<string, string>();
                dict.Add("appId", wechatHelper.AppId);
                dict.Add("nonceStr", nonceStr);
                dict.Add("package", package);
                dict.Add("signType", "MD5");
                dict.Add("timeStamp", timeSpan);

                string sign = wechatHelper.WechatPaySign(dict);

                String cfg = "{\"timestamp\": " + timeSpan + ", \"nonceStr\": \"" + nonceStr + "\", \"package\": \"" + package + "\", \"signType\":\"MD5\", \"paySign\": \"" + sign + "\", \"success\": null }";
                
                return Json(Message.Success(cfg));
            }
            else
            {
                return Json(Message.Error(uoRes.return_msg));
            }
            
        }

        /// <summary>
        /// 用户的送货地址列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Address()
        {
            List<UAddressDTO> dTOs=_iUserService.GetAddressList(UserData);
            ViewBag.Uaddress = dTOs;
            UAddressDTO uAddressDTO = dTOs.Find(r => r.Is_default == 1);
            if (uAddressDTO == null) { 
            uAddressDTO = new UAddressDTO()
            {
                Name = UserData.Name,
            };
            }
            return View(uAddressDTO);
        }

        /// <summary>
        /// 用户地址编辑，修改或者增加默认地址。
        /// </summary>
        /// <param name="dTO"></param>
        /// <returns></returns>
        public ActionResult AddressEdit(UAddressDTO dTO)
        {
            dTO.Uid = UserData.Id;
            dTO.Is_default = 1;
            dTO.Address = "";
            Message dTOs = _iUserService.SaveOrUpdate(dTO);
            return RedirectToAction("MyShop");
        }

        public ActionResult MyInfo()
        {
            return View();
        }

        /// <summary>
        /// 根据订单状态返回用户的订单列表
        /// </summary>
        /// <param name="status">订单状态，空则是查询用户所有订单</param>
        /// <returns></returns>
        public ActionResult MyOrders(int? status)
        {
            OrderDTO dTO = new OrderDTO();
            if(status!=null) dTO.Status = status.Value;
            dTO.Uid = UserData.Id;
            dTO.IsInfo = true;
            dTO.Pager = new Pagination()
            {
                CurrentPage = 1,
                PageSize = 10
            };
            List<OrderDTO> dTOs= _iOrderService.GetPage(dTO);
            ViewBag.Orders = dTOs;
            return View();
        }

        /// <summary>
        /// 通过按钮修改订单状态。
        /// 先在控制器里验证吧，以后优化。
        /// </summary>
        /// <param name="id">订单id</param>
        /// <param name="status">下一个状态代码</param>
        /// <returns>回到个人中心</returns>
        public ActionResult OrderEdit(int? id, int? status)
        {
            var yanzheng = false;
            if (id == null || status == null) return RedirectToAction("MyOrders");//任意参数为空跳到订单列表
            OrderDTO dTO = _iOrderService.Get(id.Value);
            if (status==0&&dTO.Status==10) yanzheng = true;//0是取消订单，只有10等待付款才能取消
            if (status ==40&& dTO.Status == 30) yanzheng = true;//30待收货才能40待评价
            if (status == 50 && dTO.Status == 40) yanzheng = true;//40待评价才能40交易完成
            if (yanzheng)//10待付款到20待发货不在次方法实现，退货逻辑看情况以后再说
            {
                dTO.Status = status.Value;
                _iOrderService.SaveOrUpdate(dTO);
            }
            
            return RedirectToAction("MyShop");
        }

        /// <summary>
        /// infoStr用中文："地址"或"购物车"等对象的中文
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="infoStr">"地址"就更新dto的地址</param>
        /// <returns></returns>
        public UserDTO GetUserInfo(UserDTO dto,string infoStr) {
            if (infoStr == null || infoStr.Length < 1) return dto;//没有指定类型就直接返回dto
            if (dto.DefUaddress == null&&infoStr.Equals("地址"))//默认地址的对象非null就是已经获得过了
            {
                dto = _iUserService.GetAddress(dto);
            }
            return dto;
        }
    }
}