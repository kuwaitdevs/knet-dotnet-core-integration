using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace knet_dotnet_core_integration
{
    public class KnetUtil
    {
        private string TranportalId;
        private string TranportalPwd;
        private string PaymentApiUrl;
        private string TerminalResourceKey;
        private static AesManaged CreateAes(string key)
        {
            var KeyBytes = Encoding.UTF8.GetBytes(key);
            var aes = new AesManaged
            {
                Key = KeyBytes,
                IV = KeyBytes,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7
            };
            return aes;
        }
        private static string Encrypt_AES_BCB_128(string text, string key)
        {
            using (AesManaged aes = CreateAes(key))
            {
                ICryptoTransform encryptor = aes.CreateEncryptor();
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                            sw.Write(text);
                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
        }

        public static string Decrypt_AES_BCB_128(string text, string key)
        {
            using (var aes = CreateAes(key))
            {
                ICryptoTransform decryptor = aes.CreateDecryptor();
                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(text)))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader reader = new StreamReader(cs))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
        }

        private string EncryptUrlParams(string clearTextParams)
        {
            var EncryptedParams = Encrypt_AES_BCB_128(clearTextParams, TerminalResourceKey);
            byte[] binary = Convert.FromBase64String(EncryptedParams);
            var hex = BitConverter.ToString(binary).Replace("-", "");
            var encryptedUrl = WebUtility.UrlEncode(hex);
            return encryptedUrl;
        }

        public string DecryptUrlParams(string transactionData)
        {
            byte[] binary = Convert.FromHexString(transactionData);
            var plainTextEncryptedParams = Convert.ToBase64String(binary);
            var decryptedUrl = Decrypt_AES_BCB_128(plainTextEncryptedParams, TerminalResourceKey);
            return decryptedUrl;
        }

        public KnetUtil()
        {
            TranportalId = Startup.StaticConfig["Knet:TranportalId"];
            TranportalPwd = Startup.StaticConfig["Knet:TranportalPwd"];
            PaymentApiUrl = Startup.StaticConfig["Knet:PaymentApiUrl"];
            TerminalResourceKey = Startup.StaticConfig["Knet:TerminalResourceKey"];
        }

        public string GenerateKnetUrl(
        double amount,
        string trackId,
        string lang,
        string currency,
        string onResponseUrl,
        string onErrorUrl,
        string udf1,
        string udf2,
        string udf3,
        string udf4,
        string udf5)
        {
            var clearTextParams =
                $"id={TranportalId}&" +
                $"password={TranportalPwd}&" +
                $"action=1&" +
                $"langid={lang}&" +
                $"currencycode={414}&" +
                $"amt={amount:0.000}&" +
                $"responseURL={onResponseUrl}&" +
                $"errorURL={onErrorUrl}&" +
                $"trackid={trackId}&" +
                $"udf1={udf1}&" +
                $"udf2={udf2}&" +
                $"udf3={udf3}&" +
                $"udf4={udf4}&" +
                $"udf5={udf5}&";

            var encryptedUrlParams = EncryptUrlParams(clearTextParams);

            var transactionData = $"{encryptedUrlParams}&tranportalId={TranportalId}&responseURL={onResponseUrl}&errorURL={onErrorUrl}";

            string knetPaymentUrl = $"{PaymentApiUrl}&trandata=" + transactionData;

            return knetPaymentUrl;
        }
    }
}