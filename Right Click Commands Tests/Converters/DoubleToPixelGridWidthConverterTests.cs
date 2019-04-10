using Microsoft.VisualStudio.TestTools.UnitTesting;
using Right_Click_Commands.Converters;
using System.Windows;
using System;

namespace Right_Click_Commands.Converters.Tests
{
    [TestClass]
    public class DoubleToPixelGridWidthConverterTests
    {
        DoubleToPixelGridWidthConverter subject;

        [TestInitialize]
        public void TestInitialize()
        {
            subject = new DoubleToPixelGridWidthConverter();
        }

        [TestMethod]
        public void Test_Convert_ValidValue()
        {
            double value = 31;

            object result = subject.Convert(value, null, null, null);

            Assert.IsInstanceOfType(result, typeof(GridLength));

            GridLength gridLength = (GridLength)result;

            Assert.AreEqual(GridUnitType.Pixel, gridLength.GridUnitType);
            Assert.AreEqual(value, gridLength.Value);
        }

        [TestMethod]
        public void Test_Convert_ValidNegativeValue()
        {
            double value = -67;

            object result = subject.Convert(value, null, null, null);

            Assert.IsInstanceOfType(result, typeof(GridLength));

            GridLength gridLength = (GridLength)result;

            Assert.AreEqual(GridUnitType.Pixel, gridLength.GridUnitType);
            Assert.AreEqual(value, gridLength.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "The given value must be a double")]
        public void Test_Convert_Null()
        {
            object result = subject.Convert(null, null, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "The given value must be a double")]
        public void Test_Convert_UnexpectedObject()
        {
            object result = subject.Convert(new Style(), null, null, null);
        }

        [TestMethod]
        public void Test_ConvertBack_ValidValue()
        {
            GridLength value = new GridLength(54, GridUnitType.Pixel);

            object result = subject.ConvertBack(value, null, null, null);

            Assert.IsInstanceOfType(result, typeof(double));
            Assert.AreEqual(value.Value, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "The given GridLength must be a pixel value")]
        public void Test_ConvertBack_WrongGridUnitType()
        {
            GridLength value = new GridLength(54, GridUnitType.Auto);

            object result = subject.ConvertBack(value, null, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "The given value must be a GridLength")]
        public void Test_ConvertBack_Null()
        {
            object result = subject.ConvertBack(null, null, null, null);
        }
    }
}
