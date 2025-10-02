using Day04_Controller_Based.Models;

namespace Day04_Controller_Based.Responses
{
    public class ProductResponse
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public List<ProductReviewResponse> Reviews { get; set; }
        private ProductResponse() { }

        public static ProductResponse FromModel(Product product, IEnumerable<ProductReviewer>? reviews = null)
        {
            if (product == null) throw new ArgumentNullException(nameof(product), "Cannot create aresponse null from a null");

            var response = new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            };

            if (reviews != null)
            {
                response.Reviews = ProductReviewResponse.FromModels(reviews).ToList();
            }

            return response;
        }

        public static IEnumerable<ProductResponse> FromModels(IEnumerable<Product> products)
        {
            if (products == null) throw new ArgumentNullException(nameof(products), "cannot");

            return products.Select(p => FromModel(p));
        }
    }
}
