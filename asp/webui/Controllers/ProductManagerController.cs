using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Dapper;
using System.Data;
using MySql.Data.MySqlClient;
using webui.Models;
using Shop.WebUI.Models;

namespace WebUI.Controllers;

public class ProductManagerController : Controller
{    
    IDbConnection connection;
    IWebHostEnvironment hostEnvironment;

    public ProductManagerController(IWebHostEnvironment env) {
        connection = new MySqlConnection("Server=127.0.0.1;Port=3307;database=webui;user id=ejoh;password=YEVT4w2^N4uv2q48TnA9#k&ep");
        hostEnvironment = env;
    }

    public IActionResult Index()
    {
        List<Product> products = connection.Query<Product>("SELECT * FROM products").ToList();
        foreach (var product in products) {
            product.Category = connection.QuerySingle<ProductCategory>("SELECT * FROM categories WHERE Id=@Category", product).Category;
        }
        return View(products);
    }

    public IActionResult Create() {
        Product product = new Product();
        ViewBag.Items = new List<SelectListItem>();
        List<ProductCategory> categories = connection.Query<ProductCategory>("SELECT * FROM categories").ToList();
        foreach (ProductCategory c in categories) {
            SelectListItem item = new SelectListItem() { Text = c.Category?.ToString(), Value = c.Id?.ToString() };
            ViewBag.Items.Add(item);
        }
        return View(product);
    }
    [HttpPost]
    public IActionResult Create(Product product) {
        if (!ModelState.IsValid) {
            foreach (var key in ModelState.Keys) {
                foreach (var error in ModelState[key].Errors) {
                    System.Console.WriteLine(key + ": " + error.ErrorMessage);
                }
            }
            return View(product);
        }

        UploadFile(product);
        product.ImageFileName = product.ImageFile?.FileName;
        System.Console.WriteLine(product.ImageFileName);
        connection.Execute("INSERT INTO products (id, name, description, price, category, imageFileName) VALUES (@Id, @Name, @Description, @Price, @Category, @ImageFileName)", product);
        return RedirectToAction("Index");
    }

    public IActionResult Edit(Product product) {
        product = connection.QuerySingle<Product>("SELECT * FROM products WHERE id=@Id", product);
        if (product == null)
            return NotFound();

        ViewBag.Items = new List<SelectListItem>();
        List<ProductCategory> categories = connection.Query<ProductCategory>("SELECT * FROM categories").ToList();
        foreach (ProductCategory c in categories) {
            SelectListItem item = new SelectListItem() { Text = c.Category?.ToString(), Value = c.Id?.ToString() };
            ViewBag.Items.Add(item);
        }
        return View(product);
    }
    [HttpPost]
    [ActionName("Edit")]
    public IActionResult EditPost(Product product) {
        if (!ModelState.IsValid)
            return View(product);

        UploadFile(product);

        product.ImageFileName = product.ImageFile?.FileName;
        connection.Execute("UPDATE products SET name=@Name, description=@Description, price=@Price, category=@Category, imageFileName=@ImageFileName WHERE id=@Id", product);
        return RedirectToAction("Index");
    }

    public IActionResult Delete(Product product) {
        Product toDelete = connection.QuerySingle<Product>("SELECT * FROM products WHERE id=@Id", product);
        if (toDelete == null)
            return NotFound();
        toDelete.Category = connection.QuerySingle<ProductCategory>("SELECT * FROM categories WHERE Id=@Category", toDelete).Category;
        return View(toDelete);
    }
    [HttpPost]
    [ActionName("Delete")]
    public IActionResult ConfirmDelete(Product product) {
        connection.Execute("DELETE FROM products WHERE id=@Id", product);
        return RedirectToAction("Index");
    }

    private void UploadFile(Product product) {
        if (product.ImageFile == null)
            throw new System.Exception("File not found");
        
        string dir = Path.Combine(hostEnvironment.WebRootPath, "ProductImages");
        string filePath = Path.Combine(dir, product.ImageFile.FileName);

        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        using FileStream fileStream = new FileStream(filePath, FileMode.Create);
        product.ImageFile.CopyTo(fileStream);
    }
}
