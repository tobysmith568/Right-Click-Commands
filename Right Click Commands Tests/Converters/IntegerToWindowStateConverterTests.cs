using NUnit.Framework;
using Right_Click_Commands.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Right_Click_Commands.Converters.Tests
{
    [TestFixture]
    public class IntegerToWindowStateConverterTests
    {
        IntegerToWindowStateConverter subject;

        [SetUp]
        public void SetUp()
        {
            subject = new IntegerToWindowStateConverter();
        }

        [Test]
        public void Test_Convert_ValueInRange()
        {
            int value = 2;

            object result = subject.Convert(value, null, null, null);

            Assert.IsInstanceOf(typeof(WindowState), result);

            WindowState windowState = (WindowState)result;

            Assert.AreEqual(WindowState.Maximized, windowState);
        }

        [Test]
        public void Test_Convert_ValueOutOfRange()
        {
            int value = 5;
            
            object result = subject.Convert(value, null, null, null);

            Assert.IsInstanceOf(typeof(WindowState), result);

            WindowState windowState = (WindowState)result;

            Assert.AreEqual(WindowState.Normal, windowState);
        }

        [Test]
        public void Test_Convert_Null()
        {
            object result = subject.Convert(null, null, null, null);

            Assert.IsInstanceOf(typeof(WindowState), result);

            WindowState windowState = (WindowState)result;

            Assert.AreEqual(WindowState.Normal, windowState);
        }

        [Test]
        public void Test_ConvertBack_ValidValue()
        {
            WindowState value = WindowState.Maximized;

            object result = subject.ConvertBack(value, null, null, null);

            Assert.IsInstanceOf(typeof(int), result);

            int integer = (int)result;

            Assert.AreEqual(2, integer);
        }

        [Test]
        public void Test_ConvertBack_Null()
        {
            Exception e = Assert.Throws<ArgumentException>(() => subject.ConvertBack(null, null, null, null));
            Assert.AreEqual("The given value must be a WindowState", e.Message);
        }

        [Test]
        public void Test_ConvertBack_UnexpectedObject()
        {
            Exception e = Assert.Throws<ArgumentException>(() => subject.ConvertBack(new Style(), null, null, null));
            Assert.AreEqual("The given value must be a WindowState", e.Message);
        }
    }
}