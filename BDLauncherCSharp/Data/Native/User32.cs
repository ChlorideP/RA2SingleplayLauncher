using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using BDLauncherCSharp.Extensions;

namespace BDLauncherCSharp.Data.Native
{
    public class User32
    {

        [DllImport("user32.dll")]
        public static extern bool EnumDisplaySettings(string deviceName, int modeNum, out DEVMODE devMode);
    }
}
