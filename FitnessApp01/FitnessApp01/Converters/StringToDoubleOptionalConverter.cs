using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace FitnessApp01.Converters
{
    public class StringToDoubleOptionalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if ((string)value == "" || value == null)
            {
                return (double)0;
            }
            double.TryParse((string)value, out double num);
            return num;
        }
    }
}
