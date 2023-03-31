using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shop.WebUI.Models {
    public class Product {
        public Product() {
            Id = Guid.NewGuid().ToString();
        }
        public Product(string id) {
            Id = id;
        }
        public string Id {get; set;}
        [Required]
        [StringLength(32)]
        [DisplayName("Product Name")]
        public string? Name { get; set; }
        [Required]
        [StringLength(200)]
        public string? Description {get; set;}
        [Required]
        [StringLength(32)]
        public string? Price {get; set;}
        [Required]
        [StringLength(64)]
        public string? Category {get; set;}
        [Required]
        [DisplayName("Image File")] public IFormFile? ImageFile {get; set;}
        public string? ImageFileName {get; set;}
    }
}