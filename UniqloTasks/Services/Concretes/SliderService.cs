using Microsoft.EntityFrameworkCore;
using UniqloTasks.DataAccess;
using UniqloTasks.Models;
using UniqloTasks.Services.Abstractions;

namespace UniqloTasks.Services.Concretes
{
	public class SliderService : ISliderService
	{
		private readonly UniqloDbContext _context;

		public SliderService(UniqloDbContext context)
		{
			_context = context;
		}

		public async Task CreateAsync(Slider slider)
		{
			await _context.Sliders.AddAsync(slider);
			await _context.SaveChangesAsync();

		}

		public async Task DeleteAsync(int id)
		{
			var slider = await _context.Sliders.FirstOrDefaultAsync(x => x.Id == id);
			if (slider is not null)
			{
				_context.Sliders.Remove(slider);
				slider.IsDeleted = true;
				await _context.SaveChangesAsync();
			}
		}

		public async Task<List<Slider>> GetAllSliders()
		{
			return await _context.Sliders.ToListAsync();
		}

		public async Task<Slider> GetSliderById(int id)
		{
			var slider = await _context.Sliders.FindAsync(id);
			return slider;
		}

		public async Task UpdateAsync(int id, Slider slider)
		{
			var sliderSelected = await _context.Sliders.FindAsync(id);
			if (sliderSelected != null)
			{
				sliderSelected.Title = slider.Title;
				sliderSelected.Subtitle = slider.Subtitle;
				sliderSelected.Link = slider.Link;
				//sliderSelected.ImageUrl = slider.ImageUrl;
				await _context.SaveChangesAsync();
			}
		}

		Slider ISliderService.GetSliderById(int id)
		{
			throw new NotImplementedException();
		}
	}
}
