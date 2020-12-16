using System.Runtime.InteropServices;

namespace BattleLauncher.Data.Native
{
    public class User32
    {

        [DllImport("user32.dll")]
        public static extern bool EnumDisplaySettings(string deviceName, int modeNum, out DEVMODE devMode);
    }
}
