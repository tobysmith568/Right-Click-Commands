using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Right_Click_Commands.Converters
{
    public abstract class ConverterBase<T> : MarkupExtension, IValueConverter where T : class, new()
    {
        //  Variables
        //  =========

        private static T converter = null;

        //  Constructors
        //  ============

        public ConverterBase()
        {

        }

        //  Abstract Methods
        //  ================

        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);
        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);

        //  Methods
        //  =======

        /// <exception cref="System.Reflection.TargetInvocationException">Ignore.</exception>
        /// <exception cref="MethodAccessException">Ignore.</exception>
        /// <exception cref="MemberAccessException">Ignore.</exception>
        /// <exception cref="System.Runtime.InteropServices.InvalidComObjectException">Ignore.</exception>
        /// <exception cref="MissingMethodException">Ignore.</exception>
        /// <exception cref="System.Runtime.InteropServices.COMException">Ignore.</exception>
        /// <exception cref="TypeLoadException">Ignore.</exception>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return converter ?? (converter = (T)Activator.CreateInstance(typeof(T), null));
        }
    }
}