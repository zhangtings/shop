using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CZ.Application;

namespace CZ.Areas.Admin.Controllers
{
    public class ArticleController : BaseController
    {
        private IArticleService _iArticleService;
        public ArticleController(IArticleService iArticleService)
        {
            _iArticleService = iArticleService;
        }

        public ActionResult List(int? Category)
        {
            ViewBag.Category = Category;
            return View();
        }


        public ActionResult ArticleQuery(Models.ArticleQuery query)
        {
            var result = _iArticleService.GetPage(query);
            return View("_ArticleList", result);
        }

        /// <summary>
        /// 单个图片编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult OneEdit(string artKey)
        {
            var model = _iArticleService.GetArticle(artKey);
            if (model == null)
            {
                model = new Models.ArticleDTO();
                model.ArtKey = artKey;
            }
            return View(model);
        }

        public ActionResult Edit(int? Id, int? Category)
        {
            Models.ArticleDTO model = new Models.ArticleDTO();
            if (Id > 0)
            {
                model = _iArticleService.GetArticle(Id.Value);
            }
            else
            {
                model.Category = Category;
            }
            return View(model);
        }

        [ValidateInput(false)]
        public JsonResult SaveOrUpate(Models.ArticleDTO dto)
        {
            var result = _iArticleService.SaveOrUpdateArticle(dto);
            return Json(result);
        }
    }
}