using System.Security.Cryptography;
using System.Text;

namespace Petopia.Business.Utils
{
  public static class HashUtils
  {
    private static string KEY_STRING = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public static string HashPassword(string password)
    {
      return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public static bool VerifyHashedPassword(string hash, string text)
    {
      return BCrypt.Net.BCrypt.Verify(text, hash);
    }

    public static string EnryptString(string text)
    {
      if (string.IsNullOrEmpty(text))
      {
        return string.Empty;
      }
      byte[] clearBytes = Encoding.Unicode.GetBytes(text);
      Aes encryptor = Aes.Create();

      Rfc2898DeriveBytes pdb = new(KEY_STRING, new byte[]
      {
        0x49,
        0x76,
        0x61,
        0x6e,
        0x20,
        0x4d,
        0x65,
        0x64,
        0x76,
        0x65,
        0x64,
        0x65,
        0x76
      }, 5, HashAlgorithmName.SHA1);
      encryptor.Key = pdb.GetBytes(32);
      encryptor.IV = pdb.GetBytes(16);
      MemoryStream ms = new MemoryStream();
      CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write);
      cs.Write(clearBytes, 0, clearBytes.Length);
      cs.Close();
      return Convert.ToBase64String(ms.ToArray());
    }

    public static string DecryptString(string text)
    {
      if (string.IsNullOrEmpty(text))
      {
        return string.Empty;
      }
      string cipherText = text.Replace(" ", "+");
      byte[] cipherBytes = Convert.FromBase64String(cipherText);
      Aes encryptor = Aes.Create();
      Rfc2898DeriveBytes pdb = new(KEY_STRING, new byte[]
      {
        0x49,
        0x76,
        0x61,
        0x6e,
        0x20,
        0x4d,
        0x65,
        0x64,
        0x76,
        0x65,
        0x64,
        0x65,
        0x76
      }, 5, HashAlgorithmName.SHA1);
      encryptor.Key = pdb.GetBytes(32);
      encryptor.IV = pdb.GetBytes(16);
      MemoryStream ms = new MemoryStream();
      CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write);
      cs.Write(cipherBytes, 0, cipherBytes.Length);
      cs.Close();
      return Encoding.Unicode.GetString(ms.ToArray());
    }
  }
}