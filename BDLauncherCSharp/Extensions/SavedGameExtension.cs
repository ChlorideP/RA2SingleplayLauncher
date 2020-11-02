using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDLauncherCSharp
{
    public static class SavedGameExtension// ¹æ·¶ÃüÃû
    {
        public static string GetGameSavedName(this FileInfo file) => GetGameSavedName(file.OpenRead());

        public static string GetGameSavedName(Stream stream)
        {
            var sb = new StringBuilder(0x40);
            stream.Seek(0x09C0, SeekOrigin.Begin);
            using (var sr = new StreamReader(stream, Encoding.Unicode, true, 0x40, false))
                for (int i = 0; sr.Peek() > 0; i++) sb.Append((char)sr.Read());
            return sb.ToString();
        }

        public static Data.SavedGameInfo GetSavedGameInfo(this FileInfo file) => new Data.SavedGameInfo
        {
            Name = file.GetGameSavedName(),
            Time = file.LastWriteTime
        };
    }
}