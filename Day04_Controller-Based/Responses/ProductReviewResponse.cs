using Day04_Controller_Based.Models;

namespace Day04_Controller_Based.Responses
{
    public class ProductReviewResponse
    {
        public Guid ReviewId { get; set; }
        public Guid ProductId { get; set; }
        public string? Reviewer { get; set; }
        public decimal Stars { get; set; }
        private ProductReviewResponse() { }

        public static ProductReviewResponse FromModel(ProductReviewer? review)
        {
            if (review == null) throw new ArgumentNullException(nameof(review), "Cannot create a response from anull review");

            return new ProductReviewResponse
            {
                ReviewId = review.Id,
                ProductId = review.ProductId,
                Reviewer = review.Reviewer,
                Stars = review.Stars,
            };

        }



        public static IEnumerable<ProductReviewResponse> FromModels(IEnumerable<ProductReviewer> reviews)
        {
            if (reviews == null)
                throw new ArgumentNullException(nameof(reviews));
            return reviews.Select(FromModel);

        }
    }

}
