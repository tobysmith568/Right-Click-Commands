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

        #region Convert

        [TestCase(0, WindowState.Normal)]
        [TestCase(1, WindowState.Minimized)]
        [TestCase(2, WindowState.Maximized)]
        public void Test_Convert_ValueInRange(int value, WindowState expectedResult)
        {
            object result = subject.Convert(value, null, null, null);

            Assert.IsInstanceOf(typeof(WindowState), result);

            WindowState windowState = (WindowState)result;

            Assert.AreEqual(expectedResult, windowState);
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

        #endregion
        #region ConvertBack

        [TestCase(WindowState.Normal, 0)]
        [TestCase(WindowState.Minimized, 1)]
        [TestCase(WindowState.Maximized, 2)]
        public void Test_ConvertBack_ValidValue(WindowState value, int expectedResult)
        {
            object result = subject.ConvertBack(value, null, null, null);

            Assert.IsInstanceOf(typeof(int), result);

            int integer = (int)result;

            Assert.AreEqual(expectedResult, integer);
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

        #endregion
    }
}