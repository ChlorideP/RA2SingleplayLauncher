using System.Runtime.InteropServices;

namespace BDLauncherCSharp.Data.Native
{
    public class User32
    {

        [DllImport("user32.dll")]
        public static extern bool EnumDisplaySettings(string deviceName, int modeNum, out DEVMODE devMode);
    }
}
