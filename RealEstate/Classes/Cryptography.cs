using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Configuration;

namespace RealEstate.Classes
{
    class Cryptography
    {
        /// <summary>
        /// Encrypts a string
        /// </summary>
        /// <param name="toEnctypt">String to encrypt</param>
        /// <returns>Encrypted string</returns>
        public string EncryptString(string toEncrypt)
        {
            try
            {
                byte[] keyArray;
                byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

                System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
                string key = (string)settingsReader.GetValue("SecurityKey", typeof(String));

                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                tdes.Clear();
                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine("EncryptString Exception : " + e.ToString());
                return null;
            }
        }

        /// <summary>
        /// Decrypts a cipher sting
        /// </summary>
        /// <param name="cipherString">String to decrypt</param>
        /// <returns>Decrypted string</returns>
        public string DecryptString(string cipherString)
        {
            try
            {
                byte[] keyArray;
                byte[] toEncryptArray = Convert.FromBase64String(cipherString);

                System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
                string key = (string)settingsReader.GetValue("SecurityKey", typeof(String));

                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                tdes.Clear();
                return UTF8Encoding.UTF8.GetString(resultArray);
            }
            catch (Exception e)
            {
                Console.WriteLine("DecryptString exception : " + e.ToString());
                return null;
            }
        }
    }
}
