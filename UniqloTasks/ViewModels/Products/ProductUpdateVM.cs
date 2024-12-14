using System.ComponentModel.DataAnnotations;

namespace UniqloTasks.ViewModels.Products
{
    public class ProductUpdateVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SellPrice { get; set; }
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }
        [Range(0, 100)]
        public int Discount { get; set; }
        public int BrandId { get; set; }
        public string? FileUrl { get; set; }
        public IFormFile File { get; set; }
        public IEnumerable<string>? OtherFilesUrls { get; set; }
        public ICollection<IFormFile>? OtherFiles { get; set; }
    }
}
