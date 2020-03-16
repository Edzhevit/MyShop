using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MyShop.Core.Contracts;
using MyShop.Core.Models;

namespace MyShop.WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductCategoryController : Controller
    {
        private IRepository<ProductCategory> repository;

        public ProductCategoryController()
        {
        }

        public ProductCategoryController(IRepository<ProductCategory> repository)
        {
            this.repository = repository;
        }

        // GET: ProductCategoryManager
        [HttpGet]
        public ActionResult Index()
        {
            List<ProductCategory> productCategories = repository.Collection().ToList();
            return View(productCategories);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ProductCategory productCategory = new ProductCategory();
            return View(productCategory);
        }

        [HttpPost]
        public ActionResult Create(ProductCategory productCategory)
        {
            if (!ModelState.IsValid)
            {
                return View(productCategory);
            }
            repository.Insert(productCategory);
            repository.Commit();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            ProductCategory productCategory = repository.Find(id);

            if (productCategory == null)
            {
                return HttpNotFound();
            }
            return View(productCategory);
        }

        [HttpPost]
        public ActionResult Edit(ProductCategory productCategory, string id)
        {
            ProductCategory category = repository.Find(id);

            if (productCategory == null)
            {
                return HttpNotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(productCategory);
            }

            category.Category = productCategory.Category;

            repository.Commit();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            ProductCategory category = repository.Find(id);

            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string id)
        {
            ProductCategory category = repository.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }

            repository.Delete(id);
            repository.Commit();
            return RedirectToAction("Index");
        }
    }
}