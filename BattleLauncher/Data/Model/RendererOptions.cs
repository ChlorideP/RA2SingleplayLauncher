using System;
using System.Collections.Generic;
using System.IO;

using BattleLauncher.Data.Native;
using BattleLauncher.Extensions;

namespace BattleLauncher.Data.Model
{
    public class RendererOptions
    {
        private string _name;

        public string Name { get => _name; set => _name = value.ToUpper(); }
        public string FriendlyName { get; set; }
        public DirectoryInfo Directory { get; set; }

        public override string ToString() => FriendlyName;
    }
}
