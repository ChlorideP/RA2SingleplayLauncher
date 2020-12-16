using System;
using System.Globalization;
using System.Windows.Data;

using BattleLauncher.Extensions;

namespace BattleLauncher.Converters
{
    public class DifficultyValueToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double i)
            {
#if NET48
                switch (i)
                {
                    case 0: return "txtEasy".I18N();
                    case 1: return "txtNormal".I18N();
                    case 2: return "txtHard".I18N();
                    default: return null;
                }
#elif NETCOREAPP3_1
                return i switch
                {
                    0 => "Easy".I18N(),
                    1 => "Normal".I18N(),
                    2 => "Hard".I18N(),
                    _ => null,
                };
#endif
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}
