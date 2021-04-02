using Akorin.Models;
using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akorin.Converters
{
    public class AudioToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (value is AudioFile)
            {
                if (((AudioFile)value).Data.Length > 0)
                {
                    //The Color here is the color of the highlight of recorded lines.
                    return new SolidColorBrush(Color.FromRgb(200, 255, 180));
                }
                else
                {
                    //The Color here is the color of the highlight of unrecorded lines.
                    return new SolidColorBrush(Color.FromRgb(255, 255, 255));
                }
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (value is AudioFile)
            {
                if (((AudioFile)value).Data.Length > 0)
                {
                    return new SolidColorBrush(Color.FromRgb(200, 255, 180));
                }
                else
                {
                    return new SolidColorBrush(Color.FromRgb(255, 255, 255));
                }
            }

            return value;
        }
    }
}
