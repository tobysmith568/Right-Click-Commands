using Microsoft.VisualStudio.TestTools.UnitTesting;
using Right_Click_Commands.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Right_Click_Commands.Converters.Tests
{
    [TestClass]
    public class IntegerToWindowStateConverterTests
    {
        IntegerToWindowStateConverter subject;

        [TestInitialize]
        public void TestInitialize()
        {
            subject = new IntegerToWindowStateConverter();
        }

        [TestMethod]
        public void Test_Convert_ValueInRange()
        {
            int value = 2;

            object result = subject.Convert(value, null, null, null);

            Assert.IsInstanceOfType(result, typeof(WindowState));

            WindowState windowState = (WindowState)result;

            Assert.AreEqual(WindowState.Maximized, windowState);
        }

        [TestMethod]
        public void Test_Convert_ValueOutOfRange()
        {
            int value = 5;
            
            object result = subject.Convert(value, null, null, null);

            Assert.IsInstanceOfType(result, typeof(WindowState));

            WindowState windowState = (WindowState)result;

            Assert.AreEqual(WindowState.Normal, windowState);
        }

        [TestMethod]
        public void Test_Convert_Null()
        {
            object result = subject.Convert(null, null, null, null);

            Assert.IsInstanceOfType(result, typeof(WindowState));

            WindowState windowState = (WindowState)result;

            Assert.AreEqual(WindowState.Normal, windowState);
        }

        [TestMethod]
        public void Test_ConvertBack_ValidValue()
        {
            WindowState value = WindowState.Maximized;

            object result = subject.ConvertBack(value, null, null, null);

            Assert.IsInstanceOfType(result, typeof(int));

            int integer = (int)result;

            Assert.AreEqual(2, integer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "The given value must be a WindowState")]
        public void Test_ConvertBack_Null()
        {
            object result = subject.ConvertBack(null, null, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "The given value must be a WindowState")]
        public void Test_ConvertBack_UnexpectedObject()
        {
            object result = subject.ConvertBack(new Style(), null, null, null);
        }
    }
}