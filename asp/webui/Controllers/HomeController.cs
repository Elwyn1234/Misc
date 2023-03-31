using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using System.Data;
using MySql.Data.MySqlClient;
using Shop.WebUI.Models;

namespace webui.Controllers;

public class HomeController : Controller
{
    IDbConnection connection;
    IWebHostEnvironment hostEnvironment;

    public HomeController(IWebHostEnvironment env) {
        connection = new MySqlConnection("Server=127.0.0.1;Port=3307;database=webui;user id=ejoh;password=YEVT4w2^N4uv2q48TnA9#k&ep");
        hostEnvironment = env;
    }

    public IActionResult Index(string category="") {
        List<Product> products = connection.Query<Product>("SELECT * FROM products").ToList();
        List<ProductCategory> categories = connection.Query<ProductCategory>("SELECT * FROM categories").ToList();
        if (category != "")
            products = products.Where(p => {
                return p.Category == category;
            }).ToList();
        ProductListModel vm = new ProductListModel();
        vm.Products = products;
        vm.Categories = categories;
        return View(vm);
    }

    public IActionResult Details(string id) {
        Product product = new Product(id);
        product = connection.QuerySingle<Product>("SELECT * FROM products WHERE id=@Id", product);
        return View(product);
    }
}