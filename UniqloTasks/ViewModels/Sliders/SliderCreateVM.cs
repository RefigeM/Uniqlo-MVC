using Microsoft.Build.Framework;
using Xunit.Sdk;

namespace UniqloTasks.ViewModels.Sliders
{
    public class SliderCreateVM
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string? Subtitle { get; set; }
        public string? Link { get; set; }
        public IFormFile File { get; set; }
    }
    
}

