using CZ.Application;
using CZ.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZ.Areas.Admin.Controllers
{
    public class ShopUserController : BaseController
    {
        // GET: Admin/ShopUser
        public IUserService _iUserService { get; set; }



        public ActionResult ShopUser()
        {
            return View();
        }
        
        public ActionResult ShopUserQuery(int? PageSize,int? CurrentPage)
        {
            Models.UserDTO query = new Models.UserDTO() { Pager = new Pagination() { CurrentPage = CurrentPage.Value, PageSize = PageSize.Value } };
            Message<List<Models.UserDTO>> result = new Message<List<Models.UserDTO>>();
            result.Data = _iUserService.GetList(query);
            result.Pager = query.Pager;
            return View("_ShopUserList", result);
        }

        public ActionResult ShopUserEdit(int? id,string fxzt)
        {
            if (id.Value > 0)
            {
                var model = _iUserService.Get(id.Value);
                model.Fxzt = fxzt;
                _iUserService.SaveOrUpdate(model);
            }
            return RedirectToAction("ShopUser"); ;
        }

        //
        public ActionResult ShopUserEditin(Models.UserDTO query)
        {
            
            var result = new Models.Common.Message();
            result.Text = "亲！";
            if (query.Id > 0)
            {
                result.Text = "亲！";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}