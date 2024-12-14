﻿namespace UniqloTasks.ViewModels.Basket
{
	public class BasketItemVM
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string ImgUrl { get; set; }
		public decimal SellPrice { get; set; }
		public int Discount { get; set; }
		public int Count { get; set; }
	}
}