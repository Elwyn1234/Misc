namespace Shop.WebUI.Models {
    public class BasketModel {
        public List<BasketItem>? BasketItems { get; set; }
        public decimal? GrandTotal { get; set; }
    }
}