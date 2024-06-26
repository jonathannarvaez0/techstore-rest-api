﻿using Application.Database;
using Application.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data.Common;
using static Application.Queries.ProductsQueries;
namespace Application.Controllers
{
    [Route("product/")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly DatabaseConnection _connection;
        public ProductController(DatabaseConnection databaseConnection) {
            this._connection = databaseConnection;
        }
        public string Index()
        {
            return "Products";
        }

        [HttpGet("all/{userLoggedIn}")]
        public ActionResult<Object> GetProducts(string userLoggedIn)
        {
            List<ProductModel> products = new List<ProductModel>();

            string auth = HttpContext.Request.Headers["Authorization"];

            if (auth != "Bearer @Sarmen20")
            {
                return new StatusCode { code = 401, message = "Not authorized" };
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(_connection.ConnectionString))
                {
                    conn.Open();
                    DbCommand cmd = new SqlCommand(GetAllProducts, conn);

                    SqlParameter userLoggedInParam = new SqlParameter();
                    userLoggedInParam.ParameterName = "@userLoggedIn";
                    userLoggedInParam.Value = userLoggedIn;

                    cmd.Parameters.Add(userLoggedInParam);

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        products.Add(new ProductModel
                        {
                            id = reader.GetInt32(0),
                            productName = reader.GetString(1),
                            price = reader.GetString(2),
                            datePosted = reader.GetDateTime(3).ToString().Split(" ")[0],
                            location = reader.GetString(4),
                            details = reader.GetString(5),
                            categoryId = reader.GetInt32(6),
                            conditionId = reader.GetInt32(7),
                            warrantyId = reader.GetInt32(8),
                            categoryName = reader.GetString(9),
                            conditionName = reader.GetString(10),
                            warrantyName = reader.GetString(11),
                            sellerId = reader.GetInt32(12),
                            sellerUsername = reader.GetString(13),
                            sellerEmail = reader.GetString(14),
                            sellerContact = reader.GetString(15),
                            isBookmarked = reader.IsDBNull(16) ? false : true,
                        });
                    }
                    conn.Close();

                    return products;

                }
            }
            catch (Exception error)
            {
                return new StatusCode { code = 500, message = error.Message };
            }
        }

        [HttpPost("add")]
        public ActionResult<Object> CreateProduct(ProductModel body)
        {

            string auth = HttpContext.Request.Headers["Authorization"];

            if (auth != "Bearer @Sarmen20")
            {
                return new StatusCode { code = 401, message = "Not authorized" };
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(_connection.ConnectionString))
                {
                    conn.Open();

                    DbCommand cmd = new SqlCommand(AddProduct, conn);

                    SqlParameter nameParam = new SqlParameter();
                    nameParam.ParameterName = "@productName";
                    nameParam.Value = body.productName;
                    cmd.Parameters.Add(nameParam);

                    SqlParameter priceParam = new SqlParameter();
                    priceParam.ParameterName = "@price";
                    priceParam.Value = body.price;
                    cmd.Parameters.Add(priceParam);

                    SqlParameter datePostedParam = new SqlParameter();
                    datePostedParam.ParameterName = "@datePosted";
                    datePostedParam.Value = DateTime.Now;
                    cmd.Parameters.Add(datePostedParam);

                    SqlParameter locationParam = new SqlParameter();
                    locationParam.ParameterName = "@location";
                    locationParam.Value = body.location;
                    cmd.Parameters.Add(locationParam);

                    SqlParameter detailsParam = new SqlParameter();
                    detailsParam.ParameterName = "@details";
                    detailsParam.Value = body.details;
                    cmd.Parameters.Add(detailsParam);

                    SqlParameter categoryParam = new SqlParameter();
                    categoryParam.ParameterName = "@category";
                    categoryParam.Value = body.categoryId;
                    cmd.Parameters.Add(categoryParam);

                    SqlParameter conditionParam = new SqlParameter();
                    conditionParam.ParameterName = "@condition";
                    conditionParam.Value = body.conditionId;
                    cmd.Parameters.Add(conditionParam);

                    SqlParameter warrantyParam = new SqlParameter();
                    warrantyParam.ParameterName = "@warranty";
                    warrantyParam.Value = body.warrantyId;
                    cmd.Parameters.Add(warrantyParam);

                    SqlParameter sellerParam = new SqlParameter();
                    sellerParam.ParameterName = "@seller";
                    sellerParam.Value = body.sellerId;
                    cmd.Parameters.Add(sellerParam);

                    var reader = cmd.ExecuteReader();

                    conn.Close();
                    return new StatusCode { code = 200, message = "Success" };
                }
            }
            catch (Exception error)
            {
                return new StatusCode { code = 500, message = error.Message };
            }
        }

        [HttpPost("modify")]
        public ActionResult<Object> ModifyExistingProduct([FromBody] ProductModel body)
        {
            string auth = HttpContext.Request.Headers["Authorization"];

            if (auth != "Bearer @Sarmen20")
            {
                return new StatusCode { code = 401, message = "Not authorized" };
            }

            try
            {

                using (SqlConnection conn = new SqlConnection(_connection.ConnectionString))
                {
                    conn.Open();

                    DbCommand cmd = new SqlCommand(ModifyProduct, conn);

                    SqlParameter idParam = new SqlParameter();
                    idParam.ParameterName = "@productId";
                    idParam.Value = body.id;
                    cmd.Parameters.Add(idParam);

                    SqlParameter productNameParam = new SqlParameter();
                    productNameParam.ParameterName = "@productName";
                    productNameParam.Value = body.productName;
                    cmd.Parameters.Add(productNameParam);

                    SqlParameter priceParam = new SqlParameter();
                    priceParam.ParameterName = "@price";
                    priceParam.Value = body.price;
                    cmd.Parameters.Add(priceParam);

                    SqlParameter locationParam = new SqlParameter();
                    locationParam.ParameterName = "@location";
                    locationParam.Value = body.location;
                    cmd.Parameters.Add(locationParam);

                    SqlParameter details = new SqlParameter();
                    details.ParameterName = "@details";
                    details.Value = body.details;
                    cmd.Parameters.Add(details);

                    SqlParameter categoryParam = new SqlParameter();
                    categoryParam.ParameterName = "@categoryId";
                    categoryParam.Value = body.categoryId;
                    cmd.Parameters.Add(categoryParam);

                    SqlParameter conditionParam = new SqlParameter();
                    conditionParam.ParameterName = "@conditionId";
                    conditionParam.Value = body.conditionId;
                    cmd.Parameters.Add(conditionParam);

                    SqlParameter warrantyParam = new SqlParameter();
                    warrantyParam.ParameterName = "@warrantyId";
                    warrantyParam.Value = body.warrantyId;
                    cmd.Parameters.Add(warrantyParam);

                    var reader = cmd.ExecuteReader();

                    conn.Close();

                    return new StatusCode { code = 200, message = "Edit Success" };
                }

            }
            catch (Exception error)
            {
                return new StatusCode { code = 500, message = error.Message };
            }
        }

        [HttpGet("delete/{productId}")]
        public ActionResult<Object> DeleteProduct(string productId)
        {

            string auth = HttpContext.Request.Headers["Authorization"];

            if (auth != "Bearer @Sarmen20")
            {
                return new StatusCode { code = 401, message = "Not authorized" };
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(_connection.ConnectionString))
                {
                    conn.Open();

                    DbCommand cmd = new SqlCommand(RemoveProduct, conn);

                    SqlParameter productIdParam = new SqlParameter();
                    productIdParam.ParameterName = "@productId";
                    productIdParam.Value = productId;

                    cmd.Parameters.Add(productIdParam);

                    cmd.ExecuteReader();

                    conn.Close();

                    return new StatusCode { code = 200, message = "Sucessfully Deleted Product" };
                }

            } catch (Exception error)
            {
                return new StatusCode { code = 500, message = error.Message };
            }
        }

        [HttpGet("category")]
        public ActionResult<Object> GetCategory()
        {
            List<CategoryModel> category = new List<CategoryModel>();

            try
            {

                using (SqlConnection conn = new SqlConnection(_connection.ConnectionString))
                {
                    conn.Open();
                    DbCommand cmd = new SqlCommand(GetAllCategories, conn);
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        category.Add(new CategoryModel
                        {
                            id = reader.GetInt32(0),
                            categoryName = reader.GetString(1)
                        });
                    }
                    conn.Close();
                    return category;
                }
            }
            catch (Exception error)
            {
                return new StatusCode { code = 500, message = error.Message };
            }
        }

        [HttpGet("condition")]
        public ActionResult<Object> GetCondition()
        {
            List<ConditionModel> condition = new List<ConditionModel>();

            try
            {

                using (SqlConnection conn = new SqlConnection(_connection.ConnectionString))
                {
                    conn.Open();
                    DbCommand cmd = new SqlCommand(GetAllConditions, conn);
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        condition.Add(new ConditionModel
                        {
                            id = reader.GetInt32(0),
                            conditionName = reader.GetString(1)
                        });
                    }
                    conn.Close();
                    return condition;
                }
            }
            catch (Exception error)
            {
                return new StatusCode { code = 500, message = error.Message };
            }
        }

        [HttpGet("warranty")]
        public ActionResult<Object> GetWarranty()
        {
            List<WarrantyModel> warranty = new List<WarrantyModel>();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connection.ConnectionString))
                {
                    conn.Open();

                    DbCommand cmd = new SqlCommand(GetAllWarranty, conn);
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        warranty.Add(new WarrantyModel { id = reader.GetInt32(0), warrantyName = reader.GetString(1) });
                    }
                    conn.Close();
                    return warranty;
                }
            }
            catch (Exception error)
            {
                return new StatusCode { code = 500, message = error.Message };
            }
        }

        [HttpGet("bookmark/all/{bookmarkerId}")]
        public ActionResult<Object> GetBookmarks(string bookmarkerId)
        {
            string auth = HttpContext.Request.Headers["Authorization"];

            if (auth != "Bearer @Sarmen20")
            {
                return new StatusCode { code = 401, message = "Not authorized" };
            }

            List<BookmarkModel> bookmarks = new List<BookmarkModel>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connection.ConnectionString))
                {
                    conn.Open();

                    DbCommand cmd = new SqlCommand(GetAllBookmarks, conn);

                    SqlParameter bookmarkIdParam = new SqlParameter();
                    bookmarkIdParam.ParameterName = "@bookmarkerId";
                    bookmarkIdParam.Value = bookmarkerId;

                    cmd.Parameters.Add(bookmarkIdParam);

                    var reader = cmd.ExecuteReader();

                    while(reader.Read())
                    {
                        bookmarks.Add(new BookmarkModel
                        {
                            id = reader.GetInt32(0),
                            itemBookmarkedId = reader.GetInt32(1),
                            product = new ProductModel
                            {
                                id =reader.GetInt32(1),
                                productName = reader.GetString(2),
                                price = reader.GetString(3),
                                datePosted = reader.GetDateTime(4).ToString(),
                                location = reader.GetString(5),
                                details = reader.GetString(6),
                                categoryId= reader.GetInt32(7),
                                categoryName = reader.GetString(8),
                                conditionId = reader.GetInt32(9),
                                conditionName = reader.GetString(10),
                                warrantyId = reader.GetInt32(11),
                                warrantyName = reader.GetString(12),
                                sellerId = reader.GetInt32(13),
                                sellerUsername = reader.GetString(14),
                                sellerEmail = reader.GetString(15),
                                sellerContact = reader.GetString(15)
                            }
                        });
                    }

                    conn.Close();

                    return bookmarks;
                }
            }
            catch (Exception error)
            {
                return new StatusCode { code = 500, message = error.Message };
            }
        }
        
        [HttpPost("bookmark/add")]
        public ActionResult<Object> CreateBookmark(BookmarkModel body)
        {
            string auth = HttpContext.Request.Headers["Authorization"];

            if (auth != "Bearer @Sarmen20")
            {
                return new StatusCode { code = 401, message = "Not authorized" };
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(_connection.ConnectionString))
                {
                    conn.Open();
                    
                    DbCommand cmd = new SqlCommand(AddBookmark,conn);

                    SqlParameter bookmarkerIdParam = new SqlParameter();
                    bookmarkerIdParam.ParameterName = "@bookmarkerId";
                    bookmarkerIdParam.Value = body.bookmarkerId;
                    cmd.Parameters.Add(bookmarkerIdParam);

                    SqlParameter itemBookmarkedIdParam = new SqlParameter();
                    itemBookmarkedIdParam.ParameterName = "@itemBookmarkedId";
                    itemBookmarkedIdParam.Value = body.itemBookmarkedId;
                    cmd.Parameters.Add(itemBookmarkedIdParam);

                    cmd.ExecuteReader();

                    conn.Close();

                    return new StatusCode { code = 200, message = "Insert Success" };
                }

            }catch(Exception error) 
            { 
                return new StatusCode { code = 500, message = error.Message}; 
            }
        }

        [HttpGet("bookmark/delete/{bookmarkId}")]
        public ActionResult<Object> DeleteBookmarkFromMyBookmarks(string bookmarkId)
        {
            string auth = HttpContext.Request.Headers["Authorization"];

            if (auth != "Bearer @Sarmen20")
            {
                return new StatusCode { code = 401, message = "Not authorized" };
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(_connection.ConnectionString))
                {
                    conn.Open();

                    DbCommand cmd = new SqlCommand(RemoveBookmarkFromMyBookmarks, conn);

                    SqlParameter bookmarkIdParam = new SqlParameter();
                    bookmarkIdParam.ParameterName = "@bookmarkId";
                    bookmarkIdParam.Value = bookmarkId;

                    cmd.Parameters.Add(bookmarkIdParam);

                    cmd.ExecuteReader();

                    conn.Close();

                    return new StatusCode { code = 200, message = "Success deletion" };
                }

            }catch(Exception error) 
            {
                return new StatusCode { code = 500, message = error.Message };
            }
        }

        [HttpPost("bookmark/delete")]
        public ActionResult<Object> RemoveBookmarkFromList(BookmarkModel body)
        {
            string auth = HttpContext.Request.Headers["Authorization"];

            if (auth != "Bearer @Sarmen20")
            {
                return new StatusCode { code = 401, message = "Not authorized" };
            }

            try
            {
                using(SqlConnection conn = new SqlConnection(_connection.ConnectionString))
                {
                    conn.Open();

                    DbCommand cmd = new SqlCommand(RemoveBookmark, conn);

                    SqlParameter bookmarkerIdParam = new SqlParameter();
                    bookmarkerIdParam.ParameterName = "@bookmarkerId";
                    bookmarkerIdParam.Value = body.bookmarkerId;
                    cmd.Parameters.Add(bookmarkerIdParam);

                    SqlParameter itemBookmarkedId = new SqlParameter();
                    itemBookmarkedId.ParameterName = "@itemBookmarkedId";
                    itemBookmarkedId.Value = body.itemBookmarkedId;
                    cmd.Parameters.Add(itemBookmarkedId);

                    cmd.ExecuteReader();

                    return new StatusCode { code = 200, message = "Success deletion" };
                    
                    conn.Close();
                }
            } catch (Exception error)
            {
                return new StatusCode { code = 500, message = error.Message };
            }
        }
    }
}
