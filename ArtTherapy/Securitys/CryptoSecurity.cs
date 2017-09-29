using PCLCrypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtTherapy.Securitys
{
    public static class CryptoSecurity
    {
        /// <summary>
        /// Перевод строки мд5
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GetHashMd5(string data)
        {
            var hasher = WinRTCrypto.HashAlgorithmProvider.OpenAlgorithm(HashAlgorithm.Md5);
            var inputBytes = Encoding.UTF8.GetBytes(data);
            var hash = hasher.HashData(inputBytes);
            var sb = new StringBuilder();

            foreach (var t in hash)
                sb.Append(t.ToString("X2"));

            return sb.ToString().ToLower();
        }
    }
}
