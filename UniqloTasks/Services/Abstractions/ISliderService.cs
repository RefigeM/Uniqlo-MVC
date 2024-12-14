using UniqloTasks.Models;

namespace UniqloTasks.Services.Abstractions
{
	public interface ISliderService
	{

		Slider GetSliderById(int id);
		Task<List<Slider>> GetAllSliders();
		Task CreateAsync(Slider slider);
		Task DeleteAsync(int id);
		Task UpdateAsync(int id, Slider slider);
	}
}
