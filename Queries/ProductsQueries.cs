namespace Application.Queries
{
    public class ProductsQueries
    {
        public static string GetAllProducts = "SELECT product_id,product_name,price,date_posted,location,details,category,condition,warranty,category_name,condition_name,warranty_name,seller,username,email,contact_no,bookmark_id FROM dbo.products INNER JOIN dbo.product_category ON dbo.products.category = dbo.product_category.category_id INNER JOIN dbo.product_condition ON dbo.products.condition = dbo.product_condition.condition_id INNER JOIN dbo.product_warranty ON dbo.products.warranty = dbo.product_warranty.warranty_id INNER JOIN dbo.userinfo ON dbo.products.seller = dbo.userinfo.record_id LEFT JOIN dbo.bookmarks ON dbo.bookmarks.item_bookmarked_id = dbo.products.product_id AND dbo.bookmarks.bookmarker_id = @userLoggedIn ORDER BY date_posted DESC";

        public static string GetAllCategories = "SELECT * FROM dbo.product_category";

        public static string AddProduct = "INSERT INTO dbo.products (product_name, price, date_posted, location, details, category, condition, warranty,seller) VALUES (@productName,@price,@datePosted,@location,@details,@category,@condition,@warranty,@seller)";

        public static string ModifyProduct = "UPDATE dbo.products SET product_name=@productName,price=@price,location=@location,details=@details,category=@categoryId,condition=@conditionId,warranty=@warrantyId WHERE product_id=@productId";

        public static string GetAllConditions = "SELECT * FROM dbo.product_condition";

        public static string GetAllWarranty = "SELECT * FROM dbo.product_warranty";

        public static string RemoveProduct = "DELETE FROM dbo.products WHERE product_id = @productId";

        public static string GetAllBookmarks = "SELECT bookmark_id,product_id,product_name, price, date_posted, location, details,category, category_name,condition,condition_name,warranty,warranty_name,seller,username,email,contact_no FROM dbo.bookmarks INNER JOIN dbo.products ON dbo.products.product_id = dbo.bookmarks.item_bookmarked_id INNER JOIN dbo.userinfo ON dbo.userinfo.record_id = dbo.products.seller INNER JOIN dbo.product_category ON dbo.products.category = dbo.product_category.category_id INNER JOIN dbo.product_condition ON dbo.products.condition = dbo.product_condition.condition_id INNER JOIN dbo.product_warranty ON dbo.products.warranty = dbo.product_warranty.warranty_id WHERE bookmarker_id = @bookmarkerId";

        public static string AddBookmark = "INSERT INTO dbo.bookmarks (bookmarker_id, item_bookmarked_id) VALUES (@bookmarkerId, @itemBookmarkedId)";

        public static string RemoveBookmarkFromMyBookmarks = "DELETE FROM dbo.bookmarks WHERE bookmark_id = @bookmarkId";

        public static string RemoveBookmark = "DELETE FROM dbo.bookmarks WHERE bookmarker_id = @bookmarkerId AND item_bookmarked_id = @itemBookmarkedId";
    }
}
