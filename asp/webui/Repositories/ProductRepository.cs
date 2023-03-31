using System.Runtime.Caching;
using Shop.WebUI.Models;

namespace Shop.WebUI.InMemory {
    public class ProductRepository {
        ObjectCache cache = MemoryCache.Default;
        List<Product> products;

        public ProductRepository() {
            products = cache["products"] as List<Product>;
            if (products == null) {
                products = new List<Product>();
            }
        }
        public void Commit() {
            cache["products"] = products;
        }
        public void Insert(Product product) {
            products.Add(product);
        }
        public void Update(Product product) {
            Product? toUpdate = products.Find(p => p.Id == product.Id);
            if (toUpdate != null) {
                toUpdate = product;
            }
            else {
                throw new Exception("Product not found");
            }
        }
        public Product Find(string id) {
            Product? product = products.Find(product => product.Id == id);
            if (product != null)
                return product;
            throw new System.Exception("Product not found");
        }

        public IQueryable<Product> Collection() {
            return products.AsQueryable();
        }
        public void Delete(string id) {
            Product? toDelete = products.Find(product => product.Id == id);
            if (toDelete != null)
                products.Remove(toDelete);
            throw new System.Exception("Product not found");
        }
    }
}
