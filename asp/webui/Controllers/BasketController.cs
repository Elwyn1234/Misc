using Microsoft.AspNetCore.Mvc;
using Dapper;
using System.Data;
using MySql.Data.MySqlClient;
using Shop.WebUI.Models;

namespace Shop.WebUI.Controllers;
public class BasketController : Controller {
    IDbConnection connection;
    IWebHostEnvironment hostEnvironment;

    public BasketController(IWebHostEnvironment env) {
        connection = new MySqlConnection("Server=127.0.0.1;Port=3307;database=webui;user id=ejoh;password=YEVT4w2^N4uv2q48TnA9#k&ep");
        hostEnvironment = env;
    }

    public IActionResult Index() {
        List<BasketItem> basketItems = HttpContext.Session.GetJson<List<BasketItem>>("Basket") ?? new List<BasketItem>();
        BasketModel basket = new() {
            BasketItems = basketItems,
            GrandTotal = basketItems.Sum(x => x.Quantity * x.Price)
        };
        return View(basket);
    }
    public IActionResult Add(Product product) {
        product = connection.QuerySingle<Product>("SELECT * FROM products WHERE id=@Id", product);
        List<BasketItem> basket = HttpContext.Session.GetJson<List<BasketItem>>("Basket") ?? new List<BasketItem>();
        BasketItem item = basket.Where(i => i.ProductId == product.Id).FirstOrDefault();
        if (item == null) {
            item = new() {
                ProductId = product.Id,
                Quantity = 1,
                Name = product.Name,
                Price = System.Convert.ToDecimal(product.Price),
                ImageFileName = product.ImageFileName
            };
            basket.Add(item);
        }
        else {
            item.Quantity++;
        }
        HttpContext.Session.SetJson("Basket", basket);
        return Redirect(Request.Headers["Referer"].ToString());
    }
    public IActionResult Remove(Product product) {
        List<BasketItem> basket = HttpContext.Session.GetJson<List<BasketItem>>("Basket");
        BasketItem item = basket.Where(i => i.ProductId == product.Id).FirstOrDefault();
        if (item.Quantity > 1)
            item.Quantity--;
        else
            basket.RemoveAll(item => item.ProductId == product.Id);
        HttpContext.Session.SetJson("Basket", basket);
        return Redirect(Request.Headers["Referer"].ToString());
    }
    public IActionResult Clear() {
        HttpContext.Session.Remove("Basket");
        return RedirectToAction("Index");
    }
}