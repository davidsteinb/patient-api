namespace PatientApi.Infrastructure.Security;

public interface IEncryptionHelper
{
    string Encrypt(string plainText);
    string Decrypt(string encryptedText);
}