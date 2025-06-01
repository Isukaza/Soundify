using System.Security.Cryptography;
using System.Text;

namespace Helpers;

public static class CryptoHelper
{
    public static string Aes256Encrypt(string plainText, string key)
    {
        using var aesAlg = Aes.Create();
        aesAlg.Key = Encoding.UTF8.GetBytes(key);
        aesAlg.Mode = CipherMode.CBC;
        aesAlg.Padding = PaddingMode.PKCS7;

        aesAlg.GenerateIV();
        var iv = aesAlg.IV;

        var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, iv);

        using var msEncrypt = new MemoryStream();
        using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
        using (var swEncrypt = new StreamWriter(csEncrypt))
            swEncrypt.Write(plainText);

        var cipherText = msEncrypt.ToArray();

        var result = new byte[iv.Length + cipherText.Length];
        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
        Buffer.BlockCopy(cipherText, 0, result, iv.Length, cipherText.Length);

        return Convert.ToBase64String(result);
    }
}