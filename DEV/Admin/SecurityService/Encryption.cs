using System;
using System.Security.Cryptography;
using System.Text;

namespace Admin.SecurityService
{
    public static class Encryption
    {
        public static string GetHash(string text, string passwordFormat = "SHA256")
        {
            HashAlgorithm _algorithm;

            switch (passwordFormat.ToUpper())
            {
                case "MD5":
                    _algorithm = MD5.Create();
                    break;
                case "SHA1":
                    _algorithm = SHA1.Create();
                    break;
                case "SHA256":
                    _algorithm = SHA256.Create();
                    break;
                case "SHA512":
                    _algorithm = SHA512.Create();
                    break;
                default:
                    throw new ArgumentException("Invalid password format.", "passwordFormat");
            }
            if (!string.IsNullOrEmpty(text))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(text);
                byte[] hash = _algorithm.ComputeHash(bytes);
                string hashString = string.Empty;
                foreach (byte x in hash)
                {
                    hashString += String.Format("{0:x2}", x);
                }
                return hashString.ToUpperInvariant();
            }
            return string.Empty;
        }

        /// <summary>Encrypt the string with SHA512 algorithm</summary>
        /// <param name="strToHash">The string to be encrypted</param>
        /// <returns>The encrypted string</return>
        public static string HashSHA512(string strToHash)
        {
            SHA512 sha = new SHA512Managed();
            byte[] data = sha.ComputeHash(Encoding.ASCII.GetBytes(strToHash));
            string ret = BitConverter.ToString(data).Replace("-", "").ToLower();
            return ret;
        }
    }
}