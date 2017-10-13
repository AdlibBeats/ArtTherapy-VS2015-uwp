using System.Text;
using PCLCrypto;

namespace ArtTherapyCore.Extensions
{
    public static class StringExtension
    {
        public static string ToSecurityString(this string value, HashAlgorithm hashAlgorithm = HashAlgorithm.Md5)
        {
            var hasher = WinRTCrypto.HashAlgorithmProvider.OpenAlgorithm(hashAlgorithm);
            var inputBytes = Encoding.UTF8.GetBytes(value);
            var hash = hasher.HashData(inputBytes);
            var sb = new StringBuilder();

            foreach (var t in hash)
                sb.Append(t.ToString("X2"));

            return sb.ToString().ToLower();
        }
    }
}
