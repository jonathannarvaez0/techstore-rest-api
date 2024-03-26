using Application.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.Common;
/*using System.Data.Sql;*/
/*using System.Data.SqlClient;*/
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sql;
using Microsoft.Extensions.Configuration;
using System;

namespace Application.Controllers
{
    [Route("user/")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private IConfiguration configuration;

        public UserController(IConfiguration _config)
        {
            this.configuration = _config;
        }
        public string Index()
        {
            return "User Route";
        }
        [HttpGet("/[controller]/k")]
        public string Kian()
        {
            return "Kian Sarmen";
        }

        /*[HttpGet("/[controller]/all")]*/
        /* public List<UserModel> GetUsers()
         {
             List<UserModel> l = new List<UserModel>();

             l.Add(new UserModel {
                 Age = 23, Id = 0, Name = "Kian"
             });

             l.Add(new UserModel { Id = 1, Age = 24, Name = "Jonthan" });

             return l;
         }*/

        [HttpPost("/[controller]/reg")]
        public ActionResult<Object> RegisterUser(UserModel body)
        {
            string auth = HttpContext.Request.Headers["Authorization"];

            if (auth != "Bearer @Sarmen20")
            {
                return new StatusCode { code = 401, message = "Not authorized" };
            }

            try {

                /*string connectionString = "Data Source=jnarvaez; Database=ASPNET;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False";*/
                string connectionString = this.configuration.GetConnectionString("App");
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    DbCommand cmd = new SqlCommand("INSERT INTO dbo.userinfo (firstname,lastname) VALUES ('Reynan','Belendevs');", conn);
                    cmd.ExecuteReader();

                    conn.Close();
                }


            }
            catch (Exception ex)
            {

                Console.Write(ex.StackTrace);
            }
            return body;
        }

        [HttpPost("signup")]
        public ActionResult<Object> SignUp(UserModel body)
        {
            string auth = HttpContext.Request.Headers["Authorization"];

            if (auth != "Bearer @Sarmen20")
            {
                return new StatusCode { code = 401, message = "Not authorized" };
            }

            UserModel user = new UserModel {
                firstname = body.firstname,
                lastname = body.lastname,
                username = body.username,
                password = body.password,
                email = body.email,
                contact = body.contact
            };

            try {
                string connectionString = this.configuration.GetConnectionString("App");

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    DbCommand usernameCmd = new SqlCommand("SELECT username FROM dbo.userinfo WHERE username = @username", conn);
                    SqlParameter usernameParam = new SqlParameter();
                    usernameParam.ParameterName = "@username";
                    usernameParam.Value = user.username;
                    usernameCmd.Parameters.Add(usernameParam);

                    var usernameReader = usernameCmd.ExecuteReader();

                    if (usernameReader.HasRows)
                    {
                        return new StatusCode { code = 201, message = "Username is taken" };
                    }

                    conn.Close();

                    conn.Open();
                    DbCommand emailCmd = new SqlCommand("SELECT email FROM dbo.userinfo WHERE email = @email", conn);
                    SqlParameter emailParam = new SqlParameter();
                    emailParam.ParameterName = "@email";
                    emailParam.Value = user.email;
                    emailCmd.Parameters.Add(emailParam);


                    var emailReader = emailCmd.ExecuteReader();

                    if (emailReader.HasRows)
                    {
                        return new StatusCode { code = 201, message = "Email is taken" };
                    }

                    conn.Close();

                    conn.Open();
                    DbCommand insertCmd = new SqlCommand("INSERT INTO dbo.userinfo (username,password,email,first_name,last_name,contact_no) VALUES (@username,@password,@email,@firstname,@lastname,@contact)", conn);
                    SqlParameter usernameInsertParam = new SqlParameter();
                    usernameInsertParam.ParameterName = "@username";
                    usernameInsertParam.Value = user.username;
                    insertCmd.Parameters.Add(usernameInsertParam);

                    SqlParameter passwordInsertParam = new SqlParameter();
                    passwordInsertParam.ParameterName = "@password";
                    passwordInsertParam.Value = user.password;
                    insertCmd.Parameters.Add(passwordInsertParam);

                    SqlParameter emailInsertParam = new SqlParameter();
                    emailInsertParam.ParameterName = "@email";
                    emailInsertParam.Value = user.email;
                    insertCmd.Parameters.Add(@emailInsertParam);

                    SqlParameter firstnameInsertParam = new SqlParameter();
                    firstnameInsertParam.ParameterName = "@firstname";
                    firstnameInsertParam.Value = user.firstname;
                    insertCmd.Parameters.Add(firstnameInsertParam);

                    SqlParameter lastnameInsertParam = new SqlParameter();
                    lastnameInsertParam.ParameterName = "@lastname";
                    lastnameInsertParam.Value = user.lastname;
                    insertCmd.Parameters.Add(lastnameInsertParam);

                    SqlParameter contactInsertParam = new SqlParameter();
                    contactInsertParam.ParameterName = "@contact";
                    contactInsertParam.Value = user.contact;
                    insertCmd.Parameters.Add(contactInsertParam);

                    var insertReader = insertCmd.ExecuteReader();

                    if (insertReader != null)
                    {
                        return new StatusCode { code =200, message ="Success!"};
                    }
                    else
                    {
                        return new StatusCode { code = 500, message = "Signup Error" };
                    }
                    conn.Close();
                }

            }
            catch (Exception error)
            {
                return new StatusCode { code=500, message=error.Message};
            }

     
        }


        [HttpPost("/[controller]/signin")]
        public ActionResult<Object> SignIn(UserModel body)
        {
            UserModel user = new UserModel
            {
                username = body.username,
                password = body.password,
            };

            string auth = HttpContext.Request.Headers["Authorization"];

            if (auth != "Bearer @Sarmen20")
            {
                return new StatusCode { code = 401, message = "Not authorized" };
            }

            try
            {
                string connectionString = this.configuration.GetConnectionString("App");

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    DbCommand cmd = new SqlCommand("SELECT record_id,username,email,first_name,last_name,contact_no FROM dbo.userinfo WHERE username = @username AND password = @password", conn);

                    SqlParameter usernameParam = new SqlParameter();
                    usernameParam.ParameterName = "username";
                    usernameParam.Value = user.username;

                    SqlParameter passwordParam = new SqlParameter();
                    passwordParam.ParameterName = "password";
                    passwordParam.Value = user.password;

                    cmd.Parameters.Add(usernameParam);
                    cmd.Parameters.Add(passwordParam);

                    var loginReader = cmd.ExecuteReader();

                    if (loginReader.HasRows)
                    {
                        UserModel loggedinAccount = new UserModel();
                        while (loginReader.Read())
                        {
                            int recordId = loginReader.GetInt32(0);
                            string username = loginReader.GetString(1);
                            string email = loginReader.GetString(2);
                            string firstName = loginReader.GetString(3); 
                            string lastName = loginReader.GetString(4); 
                            string contactNo = loginReader.GetString(5);

                            loggedinAccount.id= recordId;
                            loggedinAccount.firstname = firstName;
                            loggedinAccount.lastname = lastName;
                            loggedinAccount.email = email;
                            loggedinAccount.username = username;
                            loggedinAccount.contact= contactNo;
                      
                        }
                        return loggedinAccount;
                    }
                    else
                    {
                        return new StatusCode { code = 404, message = "Username or password does not exist in our records" };
                    }

                    conn.Close();
                }

            }
            catch (Exception error)
            {
                return new StatusCode { code=500, message=error.Message};
            }

        }    
    }
}
