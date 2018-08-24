using CZ.Application;
using System.Web.Mvc;

namespace CZ.Areas.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        // GET: Admin/WXShopCategory
        public ICategoryService _iCategoryService { get; set; }
        public CategoryController(ICategoryService iCategoryService)
        {
            _iCategoryService = iCategoryService;
        }


        public ActionResult Category()
        {
            return View();
        }
        
        public ActionResult CategoryQuery(Models.CategoryDTO query)
        {
            var result = _iCategoryService.GetPage(query);
            return View("_CategoryList", result);
        }

        
    }
}