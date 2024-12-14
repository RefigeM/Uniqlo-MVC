using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using UniqloTasks.DataAccess;
using UniqloTasks.Extentions;
using UniqloTasks.Helpers;
using UniqloTasks.Models;
using UniqloTasks.ViewModels.Products;

namespace UniqloTasks.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = RoleConstants.Product)]

	public class ProductController(IWebHostEnvironment _env, UniqloDbContext _context) : Controller
	{
		public IActionResult Index()
		{
			var products = _context.Products.Include(p => p.Brand).Where(p => !p.IsDeleted).ToList();

			return View(products);
			//return RedirectToAction(nameof(Create));
		}
		public async Task<IActionResult> Create()
		{
			ViewBag.Categories = await _context.Brands.Where(x => !x.IsDeleted).ToListAsync();
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Create(ProductCreateVM vm)
		{
			if (vm.File != null)
			{
				if (!vm.File.IsValidType("image"))
					ModelState.AddModelError("File", "File must be an image");
				if (!vm.File.IsValidSize(400))
					ModelState.AddModelError("File", "File must be less than 400kb");
			}
			if (vm.OtherFiles.Any())
			{
				if (!vm.OtherFiles.All(x => x.IsValidType("image")))
				{
					string fileNames = string.Join(',', vm.OtherFiles.Where(x => !x.IsValidType("image")).Select(x => x.FileName));
					ModelState.AddModelError("OtherFiles", fileNames + " is (are) not an image");
				}
				if (!vm.OtherFiles.All(x => x.IsValidSize(400)))
				{
					string fileNames = string.Join(',', vm.OtherFiles.Where(x => !x.IsValidSize(400)).Select(x => x.FileName));
					ModelState.AddModelError("OtherFiles", fileNames + " is (are) bigger than 400kb");
				}
			}
			if (!ModelState.IsValid)
			{
				ViewBag.Categories = await _context.Brands.Where(x => !x.IsDeleted).ToListAsync();
				return View(vm);
			}
			if (!await _context.Brands.AnyAsync(x => x.Id == vm.BrandId))
			{
				ViewBag.Categories = await _context.Brands.Where(x => !x.IsDeleted).ToListAsync();
				ModelState.AddModelError("BrandId", "Brand not found");
				return View();
			}
			Product product = vm;
			product.CoverImage = await vm.File!.UploadAsync(_env.WebRootPath, "imgs", "products");
			product.Images = vm.OtherFiles.Select(x => new ProductImage
			{
				ImageUrl = x.UploadAsync(_env.WebRootPath, "imgs", "products").Result
			}).ToList();
			await _context.Products.AddAsync(product);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}
		[HttpGet]
		public async Task<IActionResult> Update(int? id)
		{
			if (id is null) return BadRequest();
			var data = await _context.Products
				.Where(x => x.Id == id)
				.Select(x => new ProductUpdateVM
				{
					Id = x.Id,
					BrandId = x.BrandId ?? 0,
					CostPrice = x.CostPrice,
					Description = x.Description,
					Discount = x.Discount,
					FileUrl = x.CoverImage,
					Name = x.Name,
					Quantity = x.Quantity,
					SellPrice = x.SellPrice,
					OtherFilesUrls = x.Images.Select(y => y.ImageUrl)
				})
				.FirstOrDefaultAsync();
			if (data is null) return NotFound();
			ViewBag.Categories = await _context.Brands.Where(x => !x.IsDeleted)
				.ToListAsync();
			return View(data);
		}
		[HttpPost]
		public async Task<IActionResult> Update(int? id, ProductUpdateVM vm)
		{
			var data = await _context.Products.Include(x => x.Images)
				.Where(x => x.Id == id)
				.FirstOrDefaultAsync();
			data.Images.AddRange(vm.OtherFiles.Select(x => new ProductImage
			{
				ImageUrl = x.UploadAsync(_env.WebRootPath, "imgs", "products").Result
			}).ToList());
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}
		[HttpPost]
		public async Task<IActionResult> DeleteImgs(int id, IEnumerable<string> imgNames)
		{
			int result = await _context.Images.Where(x => imgNames.Contains(x.ImageUrl)).ExecuteDeleteAsync();
			if (result > 0)
			{
			}
			return RedirectToAction(nameof(Update), new { id });
		}
		public async Task<IActionResult> Delete(int id)
		{
			// Məhsulu əlaqəli məlumatlarla birlikdə yükləyirik
			var product = await _context.Products
				.Include(p => p.ProductRatings) // ProductRatings əlaqəli cədvəli yüklə
				.FirstOrDefaultAsync(p => p.Id == id);

			if (product == null)
			{
				return NotFound();
			}

			// Əlaqəli qiymətləndirmələri sil
			_context.ProductRatings.RemoveRange(product.ProductRatings);

			// Məhsulu sil
			_context.Products.Remove(product);

			// Dəyişiklikləri tətbiq et
			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}



	}
}
