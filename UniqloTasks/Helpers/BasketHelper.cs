using System.Text.Json;
using UniqloTasks.ViewModels.Basket;

namespace UniqloTasks.Helpers
{
	public class BasketHelper
	{

	public static	List<BasketCokiesItemVM> GetBasket(HttpRequest request)
		{
		
				string value =request.Cookies["basket"];
				if (value is null)
				{
					return new();
				}
				return JsonSerializer.Deserialize<List<BasketCokiesItemVM>>(value) ?? new();
			
			
		}
	}
}
