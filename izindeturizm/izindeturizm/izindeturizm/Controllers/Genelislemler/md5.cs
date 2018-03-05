using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace izindeturizm.Controllers.Genelislemler
{
    public static class md5
    {
        public static string MD5eDonustur(string metin)
        {
            MD5CryptoServiceProvider pwd = new MD5CryptoServiceProvider();
            return Sifrele(metin, pwd);
        }

        private static string Sifrele(string metin, HashAlgorithm alg)
        {
            byte[] byteDegeri = System.Text.Encoding.UTF8.GetBytes(metin);
            byte[] sifreliByte = alg.ComputeHash(byteDegeri);
            return Convert.ToBase64String(sifreliByte);
        }

        public static string SHA1_sİFRELE(string parola)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] veri = Encoding.UTF8.GetBytes(parola);
            byte[] sifreli=sha1.ComputeHash(veri);
            return Convert.ToBase64String(sifreli);
        }
    }
}