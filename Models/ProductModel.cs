using Microsoft.Identity.Client;

namespace Application.Models
{
    public class ProductModel
    {
        public int? id { get; set; }
        public string productName { get; set; }
        public string price { get; set; }
        public string? details { get; set; }
        public string location { get; set; }
        public string? datePosted { get; set; }
        public int categoryId { get; set; }
        public string? categoryName { get; set; }
        public int conditionId { get; set; }
        public string? conditionName { get; set; }
        public int warrantyId { get; set; }
        public string? warrantyName { get; set;}
        public int sellerId { get; set; }
        public string? sellerUsername { get; set; }
        public string? sellerEmail { get; set;}
        public string? sellerContact { get; set;}
        public bool? isBookmarked { get; set; }
    }
}
