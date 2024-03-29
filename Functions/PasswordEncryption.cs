using System.Security.Cryptography;
using System.Text;

namespace Application.Functions
{
    public class PasswordEncryption
    {
        public static string Encrypt(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] byteArray = sha.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder encryptedPassword = new StringBuilder();

                for(int i=0; i < byteArray.Length; i++)
                {
                    encryptedPassword.Append(byteArray[i].ToString("x2"));
                }

                return encryptedPassword.ToString();
            }
        }
    }
}
