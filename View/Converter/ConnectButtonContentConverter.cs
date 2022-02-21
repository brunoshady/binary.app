using System;
using System.Globalization;
using System.Windows.Data;

namespace BinaryApp.View.Converter;

public class ConnectButtonContentConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is true)
            return "Disconnect";

        return "Connect";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}