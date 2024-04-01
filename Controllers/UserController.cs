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
using Application.Functions;
using static Application.Queries.UserQueries;
using Application.Database;

namespace Application.Controllers
{
    [Route("user/")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly DatabaseConnection _connection;
        public UserController(DatabaseConnection databaseConnection)
        {
            this._connection = databaseConnection;
        }

        public string Index()
        {
            return "User Route";
        }

        [HttpPost("signup")]
        public ActionResult<Object> SignUp(UserModel body)
        {
            string auth = HttpContext.Request.Headers["Authorization"];

            if (auth != "Bearer @Sarmen20")
            {
                return new StatusCode { code = 401, message = "Not authorized" };
            }

            try {

                using (SqlConnection conn = new SqlConnection(_connection.ConnectionString))
                {
                    conn.Open();

                    DbCommand usernameCmd = new SqlCommand("SELECT username FROM dbo.userinfo WHERE username = @username", conn);
                    SqlParameter usernameParam = new SqlParameter();
                    usernameParam.ParameterName = "@username";
                    usernameParam.Value = body.username;
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
                    emailParam.Value = body.email;
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
                    usernameInsertParam.Value = body.username;
                    insertCmd.Parameters.Add(usernameInsertParam);

                    SqlParameter passwordInsertParam = new SqlParameter();
                    passwordInsertParam.ParameterName = "@password";
                    passwordInsertParam.Value = PasswordEncryption.Encrypt(body.password);
                    insertCmd.Parameters.Add(passwordInsertParam);

                    SqlParameter emailInsertParam = new SqlParameter();
                    emailInsertParam.ParameterName = "@email";
                    emailInsertParam.Value = body.email;
                    insertCmd.Parameters.Add(@emailInsertParam);

                    SqlParameter firstnameInsertParam = new SqlParameter();
                    firstnameInsertParam.ParameterName = "@firstname";
                    firstnameInsertParam.Value = body.firstname;
                    insertCmd.Parameters.Add(firstnameInsertParam);

                    SqlParameter lastnameInsertParam = new SqlParameter();
                    lastnameInsertParam.ParameterName = "@lastname";
                    lastnameInsertParam.Value = body.lastname;
                    insertCmd.Parameters.Add(lastnameInsertParam);

                    SqlParameter contactInsertParam = new SqlParameter();
                    contactInsertParam.ParameterName = "@contact";
                    contactInsertParam.Value = body.contact;
                    insertCmd.Parameters.Add(contactInsertParam);

                    var insertReader = insertCmd.ExecuteReader();

                    if (insertReader != null)
                    {
                        return new StatusCode { code = 200, message = "Success!" };
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
                return new StatusCode { code = 500, message = error.Message };
            }


        }


        [HttpPost("/[controller]/signin")]
        public ActionResult<Object> SignIn(UserModel body)
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
                    DbCommand cmd = new SqlCommand(LoginAccount, conn);

                    SqlParameter usernameParam = new SqlParameter();
                    usernameParam.ParameterName = "username";
                    usernameParam.Value = body.username;

                    SqlParameter passwordParam = new SqlParameter();
                    passwordParam.ParameterName = "password";
                    passwordParam.Value = PasswordEncryption.Encrypt(body.password);

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
                            string sessionId = Session.GenerateSessionId(DateTime.Now.ToShortDateString() + username);

                            loggedinAccount.id = recordId;
                            loggedinAccount.firstname = firstName;
                            loggedinAccount.lastname = lastName;
                            loggedinAccount.email = email;
                            loggedinAccount.username = username;
                            loggedinAccount.contact = contactNo;
                            loggedinAccount.sessionId = sessionId;


                        }
                        conn.Close();

                        try
                        {
                            conn.Open();
                            DbCommand sessionCmd = new SqlCommand(CreateSessionId, conn);

                            SqlParameter userIdParam = new SqlParameter();
                            userIdParam.ParameterName = "@userId";
                            userIdParam.Value = loggedinAccount.id;

                            SqlParameter sessionIdParam = new SqlParameter();
                            sessionIdParam.ParameterName = "@sessionId";
                            sessionIdParam.Value = loggedinAccount.sessionId;

                            sessionCmd.Parameters.Add(userIdParam);
                            sessionCmd.Parameters.Add(sessionIdParam);

                            sessionCmd.ExecuteReader();

                            conn.Close();
                        }
                        catch (Exception error)
                        {
                            return new StatusCode { code = 500, message = error.Message };
                        }

                        return loggedinAccount;
                    }
                    else
                    {
                        return new StatusCode { code = 404, message = "Username or password does not exist in our records" };
                    }
                }
            }
            catch (Exception error)
            {
                return new StatusCode { code = 500, message = error.Message };
            }

        }

        [HttpPost("/[controller]/relogin")]
        public ActionResult<Object> Relogin([FromBody]string body)
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

                    DbCommand cmd = new SqlCommand(LoginUsingSession, conn);

                    SqlParameter sessionIdParam = new SqlParameter();
                    sessionIdParam.ParameterName = "@sessionId";
                    sessionIdParam.Value = body;

                    cmd.Parameters.Add(sessionIdParam);

                    var reader = cmd.ExecuteReader();

                    UserModel user = new UserModel();
                    while (reader.Read())
                    {
                        user.id = reader.GetInt32(0);
                        user.username = reader.GetString(1);
                        user.email = reader.GetString(2);
                        user.firstname = reader.GetString(3);
                        user.lastname = reader.GetString(4);
                        user.contact = reader.GetString(5);
                        user.sessionId = reader.GetString(6);
                    }

                    conn.Close();

                    return user;
                }
            }
            catch (Exception error)
            {
                return new StatusCode { code = 500, message = error.Message };
            }
        }  
    }
}
