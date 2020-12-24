using System;
using System.Collections.Generic;
using System.IO;

using BattleLauncher.Data.Model;
using BattleLauncher.Data.Native;
using BattleLauncher.Extensions;

namespace BattleLauncher.ViewModels
{
    public class RendererViewModel
    {
        private string _name;

        public string Name { get => _name; set => _name = value.ToUpper(); }
        public string FriendlyName { get; set; }
        public DirectoryInfo Directory { get; set; }

        public override string ToString() => FriendlyName;
    }
}
