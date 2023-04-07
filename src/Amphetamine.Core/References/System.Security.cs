using System.Text;

namespace System.Security
{
	namespace Cryptography
	{
		public static class CryptographyExtensions
		{
			public static string Md5Hash(this string str)
			{
				using (var md5 = MD5.Create())
				{
					var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
					var stringBuilder = new StringBuilder();
					for (var index = 0; index <= hash.Length - 1; ++index)
						stringBuilder.Append(hash[index].ToString("x2"));
					return stringBuilder.ToString();
				}
			}
		}
	}
}
