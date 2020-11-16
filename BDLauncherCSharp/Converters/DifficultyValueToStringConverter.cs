using System;
using System.Globalization;
using System.Windows.Data;

using BDLauncherCSharp.Extensions;

namespace BDLauncherCSharp.Converters
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
                    case 0: return "Easy".I18N();
                    case 1: return "Normal".I18N();
                    case 2: return "Hard".I18N();
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
            else return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}
