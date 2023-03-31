using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shop.WebUI.Models {
    public class ProductCategory {
        public ProductCategory() {
            Id = Guid.NewGuid().ToString();
        }
        public string Id {get; set;}
        [Required]
        [StringLength(30)]
        public string? Category {get; set;}
    }
}