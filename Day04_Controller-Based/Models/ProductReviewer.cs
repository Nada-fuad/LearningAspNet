namespace Day04_Controller_Based.Models
{
    public class ProductReviewer
    {
        public  Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string? Reviewer { get; set; }
        public decimal Stars { get; set; }
    }
}
