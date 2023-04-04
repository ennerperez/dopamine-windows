using System.Security.Cryptography;
using System.Text;

namespace Amphetamine.Core.Utils
{
    public static class CryptographyUtils
    {
        public static string MD5Hash(string str)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
                StringBuilder stringBuilder = new StringBuilder();
                for (int index = 0; index <= hash.Length - 1; ++index)
                    stringBuilder.Append(hash[index].ToString("x2"));
                return stringBuilder.ToString();
            }
        }
    }
}