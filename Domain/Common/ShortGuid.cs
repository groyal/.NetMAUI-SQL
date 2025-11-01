using System;

namespace BeebopNoteApp.Domain.Common
{
    public class ShortGuid(Guid guid)
    {
        public readonly string Content = ToShortGuid(guid);

        public static string ToShortGuid(Guid guid)
        {
            return Convert.ToBase64String(guid.ToByteArray())
                          .Replace("+", "-")
                          .Replace("/", "_")
                          [..22];
        }

        public static Guid FromShortGuid(string shortGuid)
        {
            string base64 = shortGuid.Replace("-", "+")
                                     .Replace("_", "/") + "==";

            byte[] bytes = Convert.FromBase64String(base64);

            return new Guid(bytes);
        }
    }
}