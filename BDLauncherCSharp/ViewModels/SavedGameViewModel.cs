using System;
using System.IO;

namespace BDLauncherCSharp.ViewModels
{
    public class SavedGameViewModel
    {
        public string Name { get; set; }
        public DateTime Time { get; set; }
        public FileInfo RealFile { get; set; }
    }
}