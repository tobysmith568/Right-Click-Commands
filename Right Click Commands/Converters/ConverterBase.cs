using System;
using System.Windows.Markup;

namespace Right_Click_Commands.Converters
{
    public abstract class ConverterBase<T> : MarkupExtension where T : class, new()
    {
        private static T converter = null;

        public ConverterBase() { }

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