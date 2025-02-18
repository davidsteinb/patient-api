using System;
using System.Security.Cryptography;
using System.Text;

namespace PatientApi.Infrastructure.Security;

public class EncryptionHelper : IEncryptionHelper
{
    private readonly byte[] _key;

    public EncryptionHelper()
    {
        string? keyFromEnv = Environment.GetEnvironmentVariable("ENCRYPTION_KEY");

        if (string.IsNullOrEmpty(keyFromEnv) || keyFromEnv.Length != 32)
            throw new InvalidOperationException("Invalid or missing encryption key. It must be a 32-byte key.");

        _key = Encoding.UTF8.GetBytes(keyFromEnv);
    }

    public string Encrypt(string plainText)
    {
        using var aes = Aes.Create();
        aes.Key = _key;
        aes.GenerateIV();

        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        var bytes = Encoding.UTF8.GetBytes(plainText);
        var encrypted = encryptor.TransformFinalBlock(bytes, 0, bytes.Length);

        return Convert.ToBase64String(aes.IV) + ":" + Convert.ToBase64String(encrypted);
    }

    public string Decrypt(string encryptedText)
    {
        var parts = encryptedText.Split(':');
        if (parts.Length != 2)
            throw new InvalidOperationException("Invalid encrypted text format.");

        var iv = Convert.FromBase64String(parts[0]);
        var cipherText = Convert.FromBase64String(parts[1]);

        using var aes = Aes.Create();
        aes.Key = _key;
        aes.IV = iv;
        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

        var decrypted = decryptor.TransformFinalBlock(cipherText, 0, cipherText.Length);
        return Encoding.UTF8.GetString(decrypted);
    }
}