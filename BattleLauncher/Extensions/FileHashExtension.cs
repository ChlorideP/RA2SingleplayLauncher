using System;
using System.IO;
using System.Security.Cryptography;

namespace BattleLauncher.Extensions
{
    public static class FileHashExtension
    {
        public static bool SHA512Verify(this FileInfo file, string CorCode)
        {
            file.Refresh();
            if (file.Exists)
            {
                using (var sha512 = SHA512.Create())
                using (var fs = file.OpenRead())
                {
                    var buffer = sha512.ComputeHash(fs);
                    return BitConverter.ToString(buffer).Replace("-", string.Empty) == CorCode;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
