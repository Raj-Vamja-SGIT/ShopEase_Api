using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ShopEase.Common.Helpers
{
    public class EncryptionDecryption
    {
        #region Variable Declaration

        /// <summary>
        /// key String
        /// </summary>
        //private static string keyString = "09TSITUS-AARH-JMBM-2BOB-26OVN1983BYE";
        private static string keyString = "EA34FF3E-6DF2-4551-B59E-BB81D9564426";

        #endregion
         
        #region Encrypt/Decrypt

        /// <summary>
        /// Get Encrypted Value of Passed value
        /// </summary>
        /// <param name="value">value to Encrypted</param>
        /// <returns>encrypted string</returns>
        public static string GetEncrypt(string value)
        {
            return Encrypt(keyString, value);
        }

        /// <summary>
        /// Encrypt value
        /// </summary>
        /// <param name="strKey">Passphrase for Encrypt</param>
        /// <param name="strData">Message to Encrypt</param>
        /// <returns>encrypted string</returns>
        public static string Encrypt(string plainText, string key)
        {
            byte[] keyBytes = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(key));
            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.GenerateIV(); // Generate random IV for encryption

                using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                    byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

                    // Prepend the IV to the encrypted bytes
                    byte[] result = new byte[aes.IV.Length + encryptedBytes.Length];
                    Buffer.BlockCopy(aes.IV, 0, result, 0, aes.IV.Length);
                    Buffer.BlockCopy(encryptedBytes, 0, result, aes.IV.Length, encryptedBytes.Length);

                    return Convert.ToBase64String(result);
                }
            }
        }


        /// <summary>
        /// Get Decrypted value of passed encrypted string
        /// </summary>
        /// <param name="value">value to Decrypted</param>
        /// <returns>Decrypted string</returns>
        public static string GetDecrypt(string value)
        {
            return Decrypt(keyString, value);
        }

        /// <summary>
        /// decrypt value
        /// </summary>
        /// <param name="strKey">Passphrase for Decrypt</param>
        /// <param name="strData">Message to Decrypt</param>
        /// <returns>Decrypted string</returns>
        public static string Decrypt(string cipherText, string key)
        {
            byte[] keyBytes = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(key));
            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                // Extract IV from the cipherBytes (first part is the IV)
                byte[] iv = new byte[aes.BlockSize / 8];
                Buffer.BlockCopy(cipherBytes, 0, iv, 0, iv.Length);
                aes.IV = iv;

                // Decrypt the remaining cipher data
                using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    byte[] encryptedData = new byte[cipherBytes.Length - iv.Length];
                    Buffer.BlockCopy(cipherBytes, iv.Length, encryptedData, 0, encryptedData.Length);
                    byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedData, 0, encryptedData.Length);

                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }
        }


        #endregion
    }
}