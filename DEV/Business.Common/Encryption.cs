using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace Business.Common
{
    public class Encryption
    {
        //#region Member Variables
        //private static byte[] m_seed = new byte[] { 0x4d, 0x61, 0x72, 0x73, 0x68, 0x61, 0x43, 0x68, 0x61, 0x69, 0x43, 0x68, 0x69 };
        //#endregion

        //#region Public Methods
        //public static string EncryptLong(long data, string encrptKey)
        //{
        //    byte[] encryptedData = TripleDESEncrypt(data.ToString(), encrptKey);
        //    return Convert.ToBase64String(encryptedData, 0, encryptedData.Length);
        //}

        //public static long DecriptLong(string data, string encrtyKey)
        //{
        //    string decryptedData = TripleDESDecrypt(Convert.FromBase64String(data), encrtyKey);
        //    long result;
        //    long.TryParse(decryptedData, out result);
        //    return result;
        //}

        //public static string EncryptString(string data, string encrptKey)
        //{
        //    byte[] encryptedData = TripleDESEncrypt(data, encrptKey);
        //    return Convert.ToBase64String(encryptedData, 0, encryptedData.Length);
        //}

        //public static string DecriptString(string data, string encrtyKey)
        //{
        //    string decryptedData = TripleDESDecrypt(Convert.FromBase64String(data), encrtyKey);
        //    return decryptedData;
        //}

        ///// <summary>Encrypt the string with SHA512 algorithm</summary>
        ///// <param name="strToHash">The string to be encrypted</param>
        ///// <returns>The encrypted string</return>
        //public static string SHA512Encrypt(string strToEncrypt)
        //{
        //    SHA512 sha = new SHA512Managed();
        //    byte[] data = sha.ComputeHash(Encoding.ASCII.GetBytes(strToEncrypt));
        //    string ret = BitConverter.ToString(data).Replace("-", "").ToLower();
        //    return ret;
        //}
        //#endregion
        
        //#region Private Methods
        //private static byte[] TripleDESEncrypt(string strToEncrypt, string encryptKey)
        //{
        //    MemoryStream ms = new MemoryStream();

        //    byte[] dataToEncrypr = Encoding.Unicode.GetBytes(strToEncrypt);

        //    PasswordDeriveBytes pdb = new PasswordDeriveBytes(encryptKey, m_seed);

        //    TripleDES tripleDes = TripleDES.Create();
        //    tripleDes.Key = pdb.GetBytes(24);
        //    tripleDes.IV = pdb.GetBytes(8);

        //    CryptoStream cs = new CryptoStream(ms, tripleDes.CreateEncryptor(), CryptoStreamMode.Write);
        //    cs.Write(dataToEncrypr, 0, dataToEncrypr.Length);
        //    cs.Close();
        //    tripleDes.Clear();

        //    return ms.ToArray();
        //}

        //private static string TripleDESDecrypt(byte[] encryptedData, string encryptKey)
        //{
        //    try
        //    {
        //        MemoryStream ms = new MemoryStream();

        //        PasswordDeriveBytes pdb = new PasswordDeriveBytes(encryptKey, m_seed);

        //        TripleDES tripleDes = TripleDES.Create();
        //        tripleDes.Key = pdb.GetBytes(24);
        //        tripleDes.IV = pdb.GetBytes(8);

        //        CryptoStream cs = new CryptoStream(ms, tripleDes.CreateDecryptor(), CryptoStreamMode.Write);
        //        cs.Write(encryptedData, 0, encryptedData.Length);
        //        cs.Close();
        //        tripleDes.Clear();

        //        string decryptdText = System.Text.Encoding.Unicode.GetString(ms.ToArray());
        //        return decryptdText;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}
        //#endregion
        
    }
}
