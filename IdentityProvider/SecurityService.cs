using System.Security.Cryptography;
using System.Text;

namespace IdentityProvider
{
    public class SecurityService : ISecurityService
    {

        public string GetSha256Hash(string input)
        {
            using (var hashAlgorithm = new SHA256CryptoServiceProvider())
            {
                if (string.IsNullOrEmpty(input))
                {
                    return "";
                }
                var byteValue = Encoding.UTF8.GetBytes(input);
                var byteHash = hashAlgorithm.ComputeHash(byteValue);
                return Convert.ToBase64String(byteHash);
            }
        }
    }
}
