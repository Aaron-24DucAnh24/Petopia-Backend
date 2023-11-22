using System.Security.Cryptography;
using System.Text;

namespace Petopia.Business.Utils
{
  public static class HashUtils
  {
    public static string HashPassword(string password)
    {
      return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public static bool VerifyHashedPassword(string hash, string text)
    {
      return BCrypt.Net.BCrypt.Verify(text, hash);
    }

    public static string HashString(string text)
    {
      if(string.IsNullOrEmpty(text))
      {
        return string.Empty;
      }
      SHA256 sha = SHA256.Create();
      byte[] textData = Encoding.UTF8.GetBytes(text);
      byte[] hash = sha.ComputeHash(textData);
      return BitConverter.ToString(hash).Replace("-", string.Empty);
    }
  }
}