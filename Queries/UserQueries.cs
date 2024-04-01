namespace Application.Queries
{
    public class UserQueries
    {
        public static string LoginAccount = "SELECT record_id,username,email,first_name,last_name,contact_no FROM dbo.userinfo WHERE username = @username AND password = @password";
        public static string CreateSessionId = "INSERT INTO dbo.sessions (user_id, session_id) VALUES (@userId, @sessionId)";
        public static string LoginUsingSession = "SELECT user_id, username, email,first_name,last_name, contact_no, session_id from dbo.sessions inner join dbo.userinfo on dbo.userinfo.record_id = dbo.sessions.user_id WHERE session_id = @sessionId";
        public static string RemoveSessionById = "DELETE FROM dbo.sessions WHERE session_id = @sessionId";
    }
}
