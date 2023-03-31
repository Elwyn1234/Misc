namespace Shop.WebUI.Models {
    public class BasketItem {
        public string ProductId {get; set;}
        public string Name {get; set;}
        public int Quantity {get; set;}
        public decimal Price {get; set;}
        public string ImageFileName {get; set;}
    }
}