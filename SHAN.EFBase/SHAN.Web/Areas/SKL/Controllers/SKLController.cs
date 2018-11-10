using SHAN.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LW.Areas.SKL.Controllers
{

    public class SKLController : BaseController
    {
        public IRosterService _iRosSer { get; set; }

        // GET: Hotle/Hotel
        public ActionResult Index(int? Cid)
        {
            var 名录 = _iRosSer.List(new 企业用户DTO() {
                Cid=Cid.Value
            } );
            ViewBag.名录 = 名录;
            return View();
        }

        public ActionResult Search(string word)
        {
            var 名录 = _iRosSer.List(new 企业用户DTO()
            {
                CompanyName = word
            });
            ViewBag.名录 = 名录;
            return View();
        }

        public ActionResult Detail(int? Cid)
        {

            return View();
        }

        public ActionResult Company()
        {
            //var model = new 企业用户DTO();
            return View();
        }

        public ActionResult Register()
        {
            var model = new 企业用户DTO();
            return View(model);
        }

        public ActionResult Sign()
        {
            if (Session["user"] != null) { 
            var model =(企业用户DTO) Session["user"];
            return View(model);
            }
            else
            {
                return Redirect("/demo/index.html");
            }
            
        }

        public ActionResult UserLogin() {
            if (Session["user"] != null)
            {
                return RedirectToAction("Sign");
            }
                return View();
        }

        public ActionResult Login(string LogName,string Pwd)
        {

            var model = new 企业用户DTO();
            var isLog = _iRosSer.List(model);
            if (!string.IsNullOrEmpty(LogName) &&!string.IsNullOrEmpty(Pwd))
            {
                var user = isLog.Where(r => r.LogName == LogName && r.Pwd == Pwd).ToList();
                if (user.Count() == 1)
                {
                    Session["user"] = user.FirstOrDefault();
                    var json = new
                    {
                        code = 200,
                        msg = "Sign",
                        count = 0,
                        data = ""
                    };
                    return Json(json);
                }
            }
            
            var result = new
                {
                    code = 0,
                    msg = "账号或密码错误",
                    count = 0,
                    data = ""
                };
                return Json(result);
            
        }
        public ActionResult UpReg(企业用户DTO dto)
        {
            if (Session["user"] != null || dto.Id > 0)
            {
                var info = _iRosSer.GetDTO(dto.Id);
                if (info == null)
                {
                    info = _iRosSer.GetDTO(((企业用户DTO)Session["user"]).Id);
                }
                info.CompanyName = dto.CompanyName;
                info.Address = dto.Address;
                info.WebSite = dto.WebSite;
                info.Service = dto.Service;
                _iRosSer.Save(info);
                
                Session["user"] = info;
            }
            var json = new
            {
                code = 0,
                msg = "用户信息更新成功",
                count = 0,
                data = ""
            };
            return Json(json);
        }

        public ActionResult DoReg(企业用户DTO dto)
        {

            var model = new 企业用户DTO();
            var isreg = _iRosSer.List(model);
            if (isreg.Where(r => r.LogName == dto.LogName).Count() < 1) {
                _iRosSer.Save(dto);
            }
            var result = new
            {
                code = 0,
                msg = "注册成功",
                count = 0,
                data = ""
            };
            return Json(result);
        }
        // GET: Hotle/Hotel/Details/5
        public ActionResult Details(int Id)
        {
            var model = _iRosSer.GetDTO(Id);
            return View(model);
        }

        // GET: Hotle/Hotel/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Hotle/Hotel/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Hotle/Hotel/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Hotle/Hotel/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Hotle/Hotel/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Hotle/Hotel/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
