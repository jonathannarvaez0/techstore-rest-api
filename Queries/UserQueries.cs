namespace Application.Queries
{
    public class UserQueries
    {
        public static string LoginAccount = "SELECT record_id,username,email,first_name,last_name,contact_no FROM dbo.userinfo WHERE username = @username AND password = @password";
        public static string CreateSessionId = "INSERT INTO dbo.sessions (user_id, session_id) VALUES (@userId, @sessionId)";
    }
}
