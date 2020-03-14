using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using MyShop.DataAccess.InMemory.Repositories;

namespace MyShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        InMemoryRepository<Product> productRepository;
        InMemoryRepository<ProductCategory> categoryRepository;

        public ProductManagerController()
        {
            productRepository = new InMemoryRepository<Product>();
            categoryRepository = new InMemoryRepository<ProductCategory>();
        }

        // GET: ProductManager
        [HttpGet]
        public ActionResult Index()
        {
            List<Product> products = productRepository.Collection().ToList();
            return View(products);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ProductViewModel viewModel = new ProductViewModel();
            viewModel.Product = new Product();
            viewModel.ProductCategories = categoryRepository.Collection();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            productRepository.Insert(product);
            productRepository.Commit();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(string Id)
        {
            Product product = productRepository.Find(Id);

            if (product == null)
            {
                return HttpNotFound();
            }
            ProductViewModel viewModel = new ProductViewModel();
            viewModel.Product = product;
            viewModel.ProductCategories = categoryRepository.Collection();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(Product product, string Id)
        {
            Product productToEdit = productRepository.Find(Id);

            if (product == null)
            {
                return HttpNotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(product);
            }

            productToEdit.Category = product.Category;
            productToEdit.Description = product.Description;
            productToEdit.Image = product.Image;
            productToEdit.Name = product.Name;
            productToEdit.Price = product.Price;

            productRepository.Commit();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(string Id)
        {
            Product productToDelete = productRepository.Find(Id);

            if (productToDelete == null)
            {
                return HttpNotFound();
            }

            return View(productToDelete);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string Id)
        {
            Product productToDelete = productRepository.Find(Id);
            if (productToDelete == null)
            {
                return HttpNotFound();
            }

            productRepository.Delete(Id);
            productRepository.Commit();
            return RedirectToAction("Index");
        }

    }
}