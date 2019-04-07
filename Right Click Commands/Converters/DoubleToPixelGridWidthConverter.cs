using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Right_Click_Commands.Converters
{
    public class DoubleToPixelGridWidthConverter : ConverterBase<DoubleToPixelGridWidthConverter>
    {
        //  Methods
        //  =======

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is double))
                throw new ArgumentException("The given value must be a double");

            return new GridLength((double)value, GridUnitType.Pixel);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is GridLength))
                throw new ArgumentException("The given value must be a GridLength");

            GridLength gridLength = (GridLength)value;

            if (!gridLength.IsAbsolute)
                throw new ArgumentException("The given GridLength must be a pixel value");

            return gridLength.Value;
        }
    }
}
