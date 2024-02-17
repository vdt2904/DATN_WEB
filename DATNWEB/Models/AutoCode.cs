using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace DATNWEB.Models
{
    public class AutoCode
    {
        public string GenerateMa(string id)
        {
            string firstThreeCharacters = id.Substring(0, Math.Min(3, id.Length));
            string lastThreeCharacters = id.Substring(Math.Max(0, id.Length - 3));
            int code = int.Parse(lastThreeCharacters) + 1;
            string newid = firstThreeCharacters + code.ToString("D3");
            return newid;
        }
        public string HashPassword(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Chuyển đổi chuỗi thành mảng byte
                byte[] bytes = Encoding.UTF8.GetBytes(input);

                // Mã hóa mảng byte và nhận được mảng byte kết quả
                byte[] hashBytes = sha256.ComputeHash(bytes);

                // Chuyển đổi mảng byte kết quả thành chuỗi hex
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
        public string ExtractPathFromUrl(string url)
        {
            int startIndex = url.IndexOf("DATN");
            if (startIndex != -1)
            {
                string extractedPath = url.Substring(startIndex);

                // Remove file extension (assuming it ends with ".xxx")
                int dotIndex = extractedPath.LastIndexOf(".");
                if (dotIndex != -1)
                {
                    extractedPath = extractedPath.Substring(0, dotIndex);
                }

                return extractedPath;
            }

            return url;
        }

    }
}
