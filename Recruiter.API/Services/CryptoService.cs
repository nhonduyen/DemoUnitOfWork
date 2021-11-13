using System.Security.Cryptography;
using System.Text;

namespace Recruiter.API.Services
{
    public class CryptoService : ICryptoService
    {
        public string ComputeSha256Hash(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                var byteArray = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                var result = ConvertByteArrayToString(byteArray);
                return result;
            }
        }

        public string ConvertByteArrayToString(byte[] arr)
        {
            var stringBuilder = new StringBuilder();
            foreach (var item in arr)
            {
                stringBuilder.Append(item.ToString("x2"));
            }
            return stringBuilder.ToString();
        }
    }
}
