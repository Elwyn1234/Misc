using System.Runtime.Caching;
using Shop.WebUI.Models;

namespace Shop.WebUI.InMemory {
    public class ProductCategoryRepository {
        ObjectCache cache = MemoryCache.Default;
        List<ProductCategory> categories;

        public ProductCategoryRepository() {
            categories = cache?["categories"] as List<ProductCategory>;
            if (categories == null) {
                categories = new List<ProductCategory>();
            }
        }
        public void Commit() {
            cache["categories"] = categories;
        }
        public ProductCategory Find(string id) {
            ProductCategory? category = categories.Find(category => category.Id == id);
            if (category != null)
                return category;
            throw new System.Exception("Category not found");
        }
        public IQueryable<ProductCategory> Collection() {
            return categories.AsQueryable();
        }

        public void Insert(ProductCategory category) {
            categories.Add(category);
        }
        public void Update(ProductCategory category) {
            ProductCategory? toUpdate = categories.Find(category => category.Id == category.Id);
            if (toUpdate != null) {
                toUpdate.Category = category.Category;
            }
            else {
                throw new Exception("Category not found");
            }
        }
        public void Delete(string id) {
            ProductCategory? toDelete = categories.Find(category => category.Id == id);
            if (toDelete != null)
                categories.Remove(toDelete);
            throw new System.Exception("Category not found");
        }
    }
}
