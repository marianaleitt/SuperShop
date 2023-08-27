﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Core.Imaging;
using SuperShop.Data;
using SuperShop.Data.Entities;
using SuperShop.Helpers;
using SuperShop.Models;


namespace SuperShop.Controllers
{

    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IUserHelper _userHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;

        public ProductsController(
            IProductRepository productRepository,
            IUserHelper userHelper,
            IBlobHelper blobHelper,
            IConverterHelper converterHelper)

        {
            _productRepository = productRepository;
            _userHelper = userHelper;
            _blobHelper = blobHelper;
            _converterHelper = converterHelper;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(_productRepository.GetAll().OrderBy(p => p.Name));
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ProductNotFound");
            }

            var product = await _productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return new NotFoundViewResult("ProductNotFound");
            }

            return View(product);
        }

        // GET: Products/Create
        [Authorize(Roles ="Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel model)
        {

            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile,"products");
                }

                var product = _converterHelper.ToProduct(model, imageId, false);

                product.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                await _productRepository.UpdateAsync(product);
                return RedirectToAction(nameof(Index));
            }
            return View(model);

        }
        private Product ToProduct(ProductViewModel model, string path)
        {
            return new Product
            {
                Id = model.Id,
                ImageId = model.ImageId,
                IsAvailable = model.IsAvailable,
                LastPurchase = model.LastPurchase,
                Name = model.Name,
                Price = model.Price,
                Stock = model.Stock,
                User = model.User
            };
        }

        // GET: Products/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ProductNotFound");
            }

            var product = await _productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return new NotFoundViewResult("ProductNotFound");
            }

            var model = _converterHelper.ToProductViewModel(product);
            return View(model);
        }

        private  ProductViewModel ToProductViewModel(Product product)
        {
            return new ProductViewModel
            {
                Id = product.Id,
                ImageId = product.ImageId,
                IsAvailable = product.IsAvailable,
                LastPurchase = product.LastPurchase,
                LastSale = product.LastSale,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                User = product.User,
            };
        }


        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var path = model.ImageId;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        path = await _blobHelper.UploadImageAsync(model.ImageFile, "products");
                    }

                    var product = _converterHelper.ToProduct(model,path, false);

                    product.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                    await _productRepository.UpdateAsync(product);

                                    }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await _productRepository.ExistAsync(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ProductNotFound");
            }

            var product = await _productRepository.GetByIdAsync(id.Value);
                
            if (product == null)
            {
                return new NotFoundViewResult("ProductNotFound");
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            try
            {
                await _productRepository.DeleteAsync(product);
                return RedirectToAction(nameof(Index));
            }

            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("DELETE"))
                {
                    ViewBag.ErrorTitle = $"{product.Name} provavelmente está a ser usado!";
                    ViewBag.ErrorMessage = $"{product.Name} não pode ser usado visto haverem encomendas que o usam.</br></br>"+
                                       $"Experimente apagar todas as encomendas que o estão a usar,";
                }

                return View("Error");
            }
         
        }

        public IActionResult ProductNotFound()
        {
            return View();
        }
    }
}
