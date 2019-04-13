using NUnit.Framework;
using System.Windows;
using System;

namespace Right_Click_Commands.Converters.Tests
{
    [TestFixture]
    public class DoubleToPixelGridWidthConverterTests
    {
        DoubleToPixelGridWidthConverter subject;

        [SetUp]
        public void SetUp()
        {
            subject = new DoubleToPixelGridWidthConverter();
        }

        [Test]
        public void Test_Convert_ValidValue()
        {
            double value = 31;

            object result = subject.Convert(value, null, null, null);

            Assert.IsInstanceOf(typeof(GridLength), result);

            GridLength gridLength = (GridLength)result;

            Assert.AreEqual(GridUnitType.Pixel, gridLength.GridUnitType);
            Assert.AreEqual(value, gridLength.Value);
        }

        [Test]
        public void Test_Convert_ValidNegativeValue()
        {
            double value = -67;

            object result = subject.Convert(value, null, null, null);

            Assert.IsInstanceOf(typeof(GridLength), result);

            GridLength gridLength = (GridLength)result;

            Assert.AreEqual(GridUnitType.Pixel, gridLength.GridUnitType);
            Assert.AreEqual(value, gridLength.Value);
        }

        [Test]
        public void Test_Convert_Null()
        {
            Exception e = Assert.Throws<ArgumentException>(() => subject.Convert(null, null, null, null));
            Assert.AreEqual("The given value must be a double", e.Message);
        }

        [Test]
        public void Test_Convert_UnexpectedObject()
        {
            Exception e = Assert.Throws<ArgumentException>(() => subject.Convert(new Style(), null, null, null));
            Assert.AreEqual("The given value must be a double", e.Message);
        }

        [Test]
        public void Test_ConvertBack_ValidValue()
        {
            GridLength value = new GridLength(54, GridUnitType.Pixel);

            object result = subject.ConvertBack(value, null, null, null);

            Assert.IsInstanceOf(typeof(double), result);
            Assert.AreEqual(value.Value, result);
        }

        [Test]
        public void Test_ConvertBack_WrongGridUnitType()
        {
            GridLength value = new GridLength(54, GridUnitType.Auto);

            Exception e = Assert.Throws<ArgumentException>(() => subject.ConvertBack(value, null, null, null));
            Assert.AreEqual("The given GridLength must be a pixel value", e.Message);
        }

        [Test]
        public void Test_ConvertBack_Null()
        {
            Exception e = Assert.Throws<ArgumentException>(() => subject.ConvertBack(null, null, null, null));
            Assert.AreEqual("The given value must be a GridLength", e.Message);
        }
    }
}
