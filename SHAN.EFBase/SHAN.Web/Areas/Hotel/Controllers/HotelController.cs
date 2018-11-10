using SHAN.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hotel.Areas.Hotel.Controllers
{

    public class HotelController : BaseController
    {
        public IHotelUserService _iHUService { get; set; }
        public IGuestTypeService _iGTService { get; set; }
        public IRoomTypeService _iRTService { get; set; }
        public IRoomOrderService _iROService { get; set; }
        // GET: Hotle/Hotel
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Detail(int? Cid)
        {
            酒店用户DTO dto = new 酒店用户DTO();
            if (Cid == null)
                Cid = 1;

            dto = _iHUService.GetDTO(Cid.Value);
            return View(dto);
        }

        public ActionResult AddOrder(int? Cid)
        {

            return View();
        }

        public ActionResult List()
        {

            return View();
        }

        public JsonResult GetHotelList()
        {

            var hulist = _iHUService.List(new 酒店用户DTO());
            
            foreach (var hu in hulist) {
                var zdrt=_iRTService.List(new 房间类型DTO() { CompanyId = hu.Id }).Min(r=>r.Price);
                if (zdrt == null||zdrt == 0) {
                    zdrt = _iRTService.List(new 房间类型DTO() { CompanyId = 0 }).Min(r => r.Price);
                }
                var zdgt= _iGTService.List(new 客人类型DTO() { CompanyId = hu.Id }).Min(r => r.Trate);
                if (zdgt==null||zdgt==0)
                {
                    zdgt= _iGTService.List(new 客人类型DTO() { CompanyId = 0 }).Min(r => r.Trate);
                }
                hu.Pwd = (zdrt.Value * zdgt.Value).ToString();
            }
            var json = new
            {
                list = hulist.Select(r=>new { r.Id,r.Pic2,r.Address,r.CompanyName,r.Pwd}).ToArray(),

            };
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetRTList(int? cid)
        {
            
            if (cid == null) cid = 0;
            var dto = new 房间类型DTO() {
                CompanyId = cid
            };
            var rtlist = _iRTService.List(dto);
            if (rtlist.Count < 1) {
                dto.CompanyId = 0;
                rtlist = _iRTService.List(dto);
            }
            var gdto = new 客人类型DTO()
            {
                CompanyId = cid
            };
            var gtlist = _iGTService.List(gdto);
            if (gtlist.Count < 1) {
                gdto.CompanyId = 0;
                gtlist = _iGTService.List(gdto);
            }
            var json = new
            {
                rtlist = rtlist.ToArray(),
                gtlist = gtlist.ToArray(),
                info = gdto
            };
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public JsonResult OrderSave(预定DTO dto)
        {
            _iROService.Save(dto);
            var json = new
            {
                msg="预定成功",
            };
            return Json(json);
        }
            // GET: Hotle/Hotel/Details/5
        public ActionResult Details(int id)
        {
            return View();
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
