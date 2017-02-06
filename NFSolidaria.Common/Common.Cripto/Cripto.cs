using Common.Domain.Enums;
using Common.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common.Cripto
{
   
    public class Cripto : ICripto
    {
        public string Salt { get; set; }

        public string Encrypt(string value, TypeCripto type)
        {
            if(type == TypeCripto.Hash128)
                return ComputeHash128(value);

            if (type == TypeCripto.Hash512)
                return ComputeHash512(value);

            return string.Empty;
        }

        private string ComputeHash128(string value)
        {
            var encrypt = true;
            byte[] toEncryptorDecryptArray;
            ICryptoTransform cTransform;
            MD5CryptoServiceProvider md5Hasing = new MD5CryptoServiceProvider();
            byte[] keyArrays = md5Hasing.ComputeHash(UTF8Encoding.UTF8.GetBytes(Salt));
            md5Hasing.Clear();
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider() { Key = keyArrays, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 };
            if (encrypt == true)
            {
                toEncryptorDecryptArray = UTF8Encoding.UTF8.GetBytes(value);
                cTransform = tdes.CreateEncryptor();
            }
            else
            {
                toEncryptorDecryptArray = Convert.FromBase64String(value.Replace(' ', '+'));
                cTransform = tdes.CreateDecryptor();
            }
            byte[] resultsArray = cTransform.TransformFinalBlock(toEncryptorDecryptArray, 0, toEncryptorDecryptArray.Length);
            tdes.Clear();
            if (encrypt == true)
                return Convert.ToBase64String(resultsArray, 0, resultsArray.Length);

            return UTF8Encoding.UTF8.GetString(resultsArray);
        }

        private string ComputeHash512(string value)
        {
            System.Security.Cryptography.SHA512Managed sha512 = new System.Security.Cryptography.SHA512Managed();
            Byte[] EncryptedSHA512 = sha512.ComputeHash(System.Text.Encoding.UTF8.GetBytes(string.Concat(value, this.Salt)));
            sha512.Clear();
            return Convert.ToBase64String(EncryptedSHA512);
        }
    }
}
