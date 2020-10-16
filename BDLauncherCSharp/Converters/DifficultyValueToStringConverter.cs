using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace BDLauncherCSharp.Converters
{
    public class DifficultyValueToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double i)
            {
                switch (i)
                {
                    case 0: return "Easy";
                    case 1: return "Normal";
                    case 2: return "Hard";
                    default: return null;
                }
                //C井 8.0写法
                //因目标框架降级所以不再采用。
                /* return i switch
                {
                    0 => "Easy",
                    1 => "Normal",
                    2 => "Hard",
                    _ => null,
                }; */
            }
            else return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}
