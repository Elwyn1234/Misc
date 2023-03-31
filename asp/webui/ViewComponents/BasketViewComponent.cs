using Microsoft.AspNetCore.Mvc;
using Shop.WebUI.Models;

namespace Shop.WebUI.ViewComponents;
public class BasketViewComponent : ViewComponent {
    public IViewComponentResult Invoke() {
        List<BasketItem> basketItems = HttpContext.Session.GetJson<List<BasketItem>>("Basket");


        BasketModel basket = new BasketModel() {
            BasketItems = basketItems,
            GrandTotal = basketItems?.Sum(item => item.Quantity * item.Price)
        };
        return View(basket);
    }
}