using System.Windows.Input;

namespace BattleLauncher.Commands
{
    public static class MainWindowRoutedCommands
    {
        public static RoutedCommand ExitCommand = new RoutedCommand();
        public static RoutedCommand RunGameCommand = new RoutedCommand();
        public static RoutedCommand ClearCommandLineCommand = new RoutedCommand();
        public static RoutedCommand OpenGameSettingsCommand = new RoutedCommand();
        public static RoutedCommand OpenArchiveLoaderCommand = new RoutedCommand();
    }
}
