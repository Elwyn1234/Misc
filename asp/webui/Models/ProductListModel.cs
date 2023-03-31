namespace Shop.WebUI.Models {
    public class ProductListModel {
        public IEnumerable<Product> Products {get; set;}
        public IEnumerable<ProductCategory> Categories {get; set;}
    }
}