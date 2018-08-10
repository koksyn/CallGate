using System.Security.Cryptography;
using System.Text;

namespace CallGate.Services.Authentication
{
    public class Md5HashService : IHashService
    {
        public string GenerateHash(string data)
        {
            var md5 = MD5.Create();
            var inputBytes = Encoding.ASCII.GetBytes(data);
            var hash = md5.ComputeHash(inputBytes);

            var sb = new StringBuilder();
            for (var i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            return sb.ToString();
        }
    }
}
