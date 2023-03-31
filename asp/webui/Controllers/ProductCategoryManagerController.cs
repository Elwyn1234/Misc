using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Dapper;
using System.Data;
using MySql.Data.MySqlClient;
using webui.Models;
using Shop.WebUI.Models;

namespace WebUI.Controllers;

public class ProductCategoryManagerController : Controller
{    
    IDbConnection connection;

    public ProductCategoryManagerController() {
        connection = new MySqlConnection("Server=127.0.0.1;Port=3307;database=webui;user id=ejoh;password=YEVT4w2^N4uv2q48TnA9#k&ep");
    }

    public IActionResult Index()
    {
        List<ProductCategory> categories = connection.Query<ProductCategory>("SELECT * FROM categories").ToList();
        return View(categories);
    }

    public IActionResult Create() {
        ProductCategory category = new ProductCategory();
        return View(category);
    }
    [HttpPost]
    public IActionResult Create(ProductCategory productCategory) {
        if (!ModelState.IsValid) {
            return View(productCategory);
        }
        connection.Execute("INSERT INTO categories (id, category) VALUES (@Id, @Category)", productCategory);
        return RedirectToAction("Index");
    }

    public IActionResult Edit(ProductCategory productCategory) {
        productCategory = connection.QuerySingle<ProductCategory>("SELECT * FROM categories WHERE id=@Id", productCategory);
        if (productCategory == null)
            return NotFound(); 
        return View(productCategory);
    }
    [HttpPost]
    [ActionName("Edit")]
    public IActionResult EditPost(ProductCategory productCategory) {
        if (!ModelState.IsValid)
            return View(productCategory);
        connection.Execute("UPDATE categories SET category=@Category", productCategory);
        return RedirectToAction("Index");
    }

    public IActionResult Delete(ProductCategory productCategory) {
        ProductCategory toDelete = connection.QuerySingle<ProductCategory>("SELECT * FROM categories WHERE id=@Id", productCategory);
        if (toDelete == null)
            return NotFound();
        return View(toDelete);
    }
    [HttpPost]
    [ActionName("Delete")]
    public IActionResult ConfirmDelete(ProductCategory productCategory) {
        connection.Execute("DELETE FROM categories WHERE id=@Id", productCategory);
        return RedirectToAction("Index");
    }
}
