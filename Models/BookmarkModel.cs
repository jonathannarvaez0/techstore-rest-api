namespace Application.Models
{
    public class BookmarkModel
    {
        public int id { get; set; }
        public int bookmarkerId { get; set; }
        public int itemBookmarkedId { get; set; }
        public ProductModel? product { get; set; }
    }
}
