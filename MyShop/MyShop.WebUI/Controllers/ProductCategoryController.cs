using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MyShop.Core.Models;
using MyShop.DataAccess.InMemory.Repositories;

namespace MyShop.WebUI.Controllers
{
    public class ProductCategoryController : Controller
    {
        ProductCategoryRepository repository;

        public ProductCategoryController()
        {
            repository = new ProductCategoryRepository();
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
        public ActionResult Edit(string Id)
        {
            ProductCategory productCategory = repository.Find(Id);

            if (productCategory == null)
            {
                return HttpNotFound();
            }
            return View(productCategory);
        }

        [HttpPost]
        public ActionResult Edit(ProductCategory productCategory, string Id)
        {
            ProductCategory category = repository.Find(Id);

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
        public ActionResult Delete(string Id)
        {
            ProductCategory category = repository.Find(Id);

            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string Id)
        {
            ProductCategory category = repository.Find(Id);
            if (category == null)
            {
                return HttpNotFound();
            }

            repository.Delete(Id);
            repository.Commit();
            return RedirectToAction("Index");
        }
    }
}