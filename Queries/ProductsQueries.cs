namespace Application.Queries
{
    public class ProductsQueries
    {
        public static string GetAllProductsQuery = "SELECT product_id,product_name,price,date_posted,location,details,category,condition,warranty,category_name,condition_name,warranty_name,seller,username,email,contact_no FROM dbo.products INNER JOIN dbo.product_category ON dbo.products.category = dbo.product_category.category_id INNER JOIN dbo.product_condition ON dbo.products.condition = dbo.product_condition.condition_id INNER JOIN dbo.product_warranty on dbo.products.warranty  = dbo.product_warranty.warranty_id INNER JOIN dbo.userinfo on dbo.products.seller = dbo.userinfo.record_id  ORDER BY date_posted DESC";

        public static string GetAllCategories = "SELECT * FROM dbo.product_category";

        public static string AddProduct = "INSERT INTO dbo.products (product_name, price, date_posted, location, details, category, condition, warranty,seller) VALUES (@productName,@price,@datePosted,@location,@details,@category,@condition,@warranty,@seller)";
    }
}
