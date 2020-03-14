using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;

namespace MyShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        private IRepository<Product> productRepository;
        private IRepository<ProductCategory> categoryRepository;

        public ProductManagerController(IRepository<Product> productRepository, IRepository<ProductCategory> categoryRepository)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
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
        public ActionResult Create(Product product, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            if (file != null)
            {
                product.Image = product.Id + Path.GetExtension(file.FileName);
                file.SaveAs(Server.MapPath("//Content//ProductImages//") + product.Image);
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
        public ActionResult Edit(Product product, string id, HttpPostedFileBase file)
        {
            Product productToEdit = productRepository.Find(id);

            if (product == null)
            {
                return HttpNotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(product);
            }

            if (file != null)
            {
                productToEdit.Image = product.Id + Path.GetExtension(file.FileName);
                file.SaveAs(Server.MapPath("//Content//ProductImages//") + productToEdit.Image);
            }

            productToEdit.Category = product.Category;
            productToEdit.Description = product.Description;
            productToEdit.Name = product.Name;
            productToEdit.Price = product.Price;

            productRepository.Commit();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            Product productToDelete = productRepository.Find(id);

            if (productToDelete == null)
            {
                return HttpNotFound();
            }

            return View(productToDelete);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string id)
        {
            Product productToDelete = productRepository.Find(id);
            if (productToDelete == null)
            {
                return HttpNotFound();
            }

            productRepository.Delete(id);
            productRepository.Commit();
            return RedirectToAction("Index");
        }

    }
}