using CZ.Application;
using System.Web.Mvc;

namespace CZ.Areas.wfxshoping.Controllers
{
    public class CategoryController : BaseController
    {
        public ICategoryService _iCategoryService { get; set; }
        //public CategoryController(ICategoryService iCategoryService)
        //{
        //    _iCategoryService = iCategoryService;
        //}

        public ActionResult Index()
        {
            /// <summary>
            /// 构造查询分页条件获得分类
            /// </summary>
            var list = _iCategoryService.GetPage(new Models.CategoryDTO()
            {
                Pager = new Models.Common.Pagination()
                {
                    CurrentPage = 1,
                    PageSize = 100
                }
            });
            ViewBag.Categores = list;
            return View();
        }
    }
}