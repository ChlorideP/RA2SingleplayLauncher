using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using BDLauncherCSharp.Data;

namespace BDLauncherCSharp.Extensions
{
    class FileHashIdentify
    {
        public static bool SHA512Verify(string filename, string CorCode)
        {
            var file = Path.Combine(OverAll.MainPath, filename);
            if (File.Exists(file))
            {
                using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                {
                    var sha512 = new SHA512CryptoServiceProvider();
                    var buffer = sha512.ComputeHash(fs);
                    sha512.Clear();
                    string sb = BitConverter.ToString(buffer).Replace("-", string.Empty);
                    var result = sb.ToString() == CorCode;
                    return result;
                }
            }
            else return false;
        }
    }
}
