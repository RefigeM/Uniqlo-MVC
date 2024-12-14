namespace UniqloTasks.ViewModels.Products
{
    public class ProductListItemVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal SellPrice { get; set; }
        public int Discount { get; set; }
        public bool IsInStock { get; set; }
        public string CoverImage { get; set; }
		public int BrandId { get; set; }

	}
}
