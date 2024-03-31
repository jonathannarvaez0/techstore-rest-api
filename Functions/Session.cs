namespace Application.Functions
{
    public class Session
    {
        public static string GenerateSessionId(string session)
        {
            return PasswordEncryption.Encrypt(session);
        }
    }
}
