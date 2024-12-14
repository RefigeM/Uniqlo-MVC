using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniqloTasks.DataAccess;
using UniqloTasks.Helpers;
using UniqloTasks.Models;
using UniqloTasks.Services.Concretes;
using UniqloTasks.ViewModels.Sliders;

namespace UniqloTasks.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = RoleConstants.Product)]

	public class SliderController(UniqloDbContext _context, IWebHostEnvironment _env) : Controller
	{
		private readonly SliderService _sliderService;




		public async Task<IActionResult> Index()
		{
			return View(await _context.Sliders.ToListAsync());
		}
		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Create(SliderCreateVM vm)
		{
			if (!ModelState.IsValid) return View(vm);
			if (!vm.File.ContentType.StartsWith("image"))
			{
				ModelState.AddModelError("File", "Format type must be image");
				return View(vm);
			}
			if (vm.File.Length > 2 * 1024 * 1024)
			{
				ModelState.AddModelError("File", "File size must be less than 2 mb");
				return View(vm);
			}
			string newFileName = Path.GetRandomFileName() + Path.GetExtension(vm.File.FileName);

			using (Stream stream = System.IO.File.Create(Path.Combine(_env.WebRootPath, "imgs", "sliders", newFileName)))
			{
				await vm.File.CopyToAsync(stream);
			}
			Slider slider = new Slider
			{
				ImageUrl = newFileName,
				Title = vm.Title,
				Subtitle = vm.Subtitle!,
				Link = vm.Link
			};
			await _context.Sliders.AddAsync(slider);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}
		[HttpGet]
		public async Task<IActionResult> Update(int id)
		{
			var slider = await _context.Sliders.FindAsync(id);
			if (slider is null) { return NotFound(); }
			return View(slider);
		}
		[HttpPost]
		public async Task<IActionResult> Update(Slider slider)
		{
			if (!ModelState.IsValid)
			{
				foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
				{
					Console.WriteLine(error.ErrorMessage);
				}
				return View(slider);
			}
			var sliderSelected = await _context.Sliders.FindAsync(slider.Id);
			if (sliderSelected != null)
			{
				sliderSelected.Title = slider.Title;
				sliderSelected.Subtitle = slider.Subtitle;
				sliderSelected.Link = slider.Link;
				sliderSelected.ImageUrl = slider.ImageUrl;
				await _context.SaveChangesAsync();
				return View(sliderSelected);

			}
			return View(slider);

		}
		public async Task<IActionResult> Delete(int id)
		{
			var slider = await _context.Sliders.FirstOrDefaultAsync(x => x.Id == id);
			if (slider is not null)
			{
				//_context.Sliders.Remove(slider);
				slider.IsDeleted = true;
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return NotFound();
		}
		public async Task<IActionResult> Hide(int id)
		{
			var deletedSlider = await _context.Sliders.FirstOrDefaultAsync(x => x.IsDeleted);
			if (deletedSlider is null) NotFound();
			else
			{
				_context.Sliders.Remove(deletedSlider);
				await _context.SaveChangesAsync();
			}
			

			return RedirectToAction(nameof(Index));

		}




	}
}

//        var options = new DbContextOptionsBuilder<UniqloDbContext>()
//.UseSqlServer("Server=DESKTOP-9OJ3NSG\\SQLEXPRESS;Database=UniqloProject;Trusted_Connection=True;TrustServerCertificate=True;")
//.Options;
//        using (UniqloDbContext _context1 = new(options))
//        {
//            _context1.Sliders.ToList();
//            _context1.Sliders.Add(new Models.Slider { });
//            _context1.SaveChanges();