using System.IO;
using System.Text;

using BDLauncherCSharp.Data.Model;

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

        public static SavedGameInfo GetSavedGameInfo(this FileInfo file) => new SavedGameInfo
        {
            Name = file.GetGameSavedName(),
            Time = file.LastWriteTime,
            RealFile = file
        };
    }
}