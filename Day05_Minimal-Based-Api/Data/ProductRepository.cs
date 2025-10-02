using Day04_Controller_Based.Models;

namespace Day04_Controller_Based.Data
{
   
        public class ProductRepository
        {
            private List<Product> _products = new List<Product>
        {
            new Product { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Laptop", Price = 3500.00m },
            new Product { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "Smartphone", Price = 2200.00m },
            new Product { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Name = "Tablet", Price = 1500.00m },
            new Product { Id = Guid.Parse("44444444-4444-4444-4444-444444444444"), Name = "Smartwatch", Price = 800.00m },
            new Product { Id = Guid.Parse("55555555-5555-5555-5555-555555555555"), Name = "Headphones", Price = 450.00m }
            };
            private List<ProductReviewer> _reviews = new List<ProductReviewer>
            {
            new ProductReviewer { Id = Guid.NewGuid(), ProductId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Reviewer = "Ahmed", Stars = 4.5m },
            new ProductReviewer { Id = Guid.NewGuid(), ProductId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Reviewer = "Sara", Stars = 5.0m },
            new ProductReviewer { Id = Guid.NewGuid(), ProductId = Guid.Parse("22222222-2222-2222-2222-222222222222"), Reviewer = "Khalid", Stars = 3.8m },
            new ProductReviewer { Id = Guid.NewGuid(), ProductId = Guid.Parse("22222222-2222-2222-2222-222222222222"), Reviewer = "Mona", Stars = 4.2m },
            new ProductReviewer { Id = Guid.NewGuid(), ProductId = Guid.Parse("33333333-3333-3333-3333-333333333333"), Reviewer = "Omar", Stars = 2.9m },
            new ProductReviewer { Id = Guid.NewGuid(), ProductId = Guid.Parse("33333333-3333-3333-3333-333333333333"), Reviewer = "Laila", Stars = 4.0m },
            new ProductReviewer { Id = Guid.NewGuid(), ProductId = Guid.Parse("44444444-4444-4444-4444-444444444444"), Reviewer = "Noura", Stars = 3.5m },
            new ProductReviewer { Id = Guid.NewGuid(), ProductId = Guid.Parse("44444444-4444-4444-4444-444444444444"), Reviewer = "Fahad", Stars = 4.7m },
            new ProductReviewer { Id = Guid.NewGuid(), ProductId = Guid.Parse("55555555-5555-5555-5555-555555555555"), Reviewer = "Huda", Stars = 5.0m },
            new ProductReviewer { Id = Guid.NewGuid(), ProductId = Guid.Parse("55555555-5555-5555-5555-555555555555"), Reviewer = "Yasser", Stars = 4.3m }
        };
            public int GetProductsCount() => _products.Count();
            public List<Product> GetAllProducts => _products;

            public List<Product> GetProductPage(int page = 1, int pageSize = 10)
            {
                return _products.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }

            public Product? GetProductById(Guid productId)
            {
                return _products.FirstOrDefault(p => p.Id == productId);
            }

            public List<ProductReviewer> GetProductReviewers(Guid productId)
            {
                return _reviews.Where(r => r.ProductId == productId).ToList();
            }

            public ProductReviewer? GetReview(Guid productId, Guid reviewId)
            {
                return _reviews.FirstOrDefault(r => r.ProductId == productId && r.Id == reviewId);
            }

            public bool AddProduct(Product product)
            {
                _products.Add(product);
                return true;
            }

            public bool AddProductReview(ProductReviewer productReviewer)
            {
                if (_products.Any(p => p.Id == productReviewer.ProductId))
                {
                    _reviews.Add(productReviewer);
                    return true;
                }
                return false;
            }

            public bool UpdateProduct(Product updateProduct)
            {
                var existingProduct = _products.FirstOrDefault(p => p.Id == updateProduct.Id);
                if (existingProduct == null)
                    return false;

                existingProduct.Name = updateProduct.Name;
                existingProduct.Price = updateProduct.Price;
                return true;
            }

            public bool DeleteProduct(Guid id)
            {
                var product = _products.FirstOrDefault(p => p.Id == id);
                if (product == null) return false;

                _products.Remove(product);
                _reviews.RemoveAll(r => r.ProductId == id);
                return true;
            }

            public bool ExistById(Guid id)
            {
                return _products.Any(p => p.Id == id);
            }

            public bool ExistByName(string name)
            {
                return _products.Any(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));


            }
        }

    }

