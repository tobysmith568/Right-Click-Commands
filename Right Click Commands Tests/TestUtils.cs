using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Right_Click_Commands.Tests
{
    public static class TestUtils
    {
        /// <exception cref="FieldAccessException">Ignore.</exception>
        /// <exception cref="TargetException">Ignore.</exception>
        public static void SetPrivateField(this object subject, string field, object value)
        {
            subject.GetType()
                .GetField(field, BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(subject, value);
        }
    }
}
