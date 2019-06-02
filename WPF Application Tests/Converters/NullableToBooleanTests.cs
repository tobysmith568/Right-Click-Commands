using NUnit.Framework;
using System;
using System.Windows;

namespace Right_Click_Commands.WPF.Converters.Tests
{
    [TestFixture]
    public class NullableToBooleanTests
    {
        NullableToBoolean subject;

        [SetUp]
        public void SetUp()
        {
            subject = new NullableToBoolean();
        }

        #region Convert

        [Test]
        public void Convert_Null()
        {
            object result = subject.Convert(null, null, null, null);

            Assert.IsInstanceOf(typeof(bool), result);

            bool visibility = (bool)result;

            Assert.IsFalse(visibility);
        }

        [Test]
        public void Convert_Object()
        {
            object result = subject.Convert(new object(), null, null, null);

            Assert.IsInstanceOf(typeof(bool), result);

            bool visibility = (bool)result;

            Assert.IsTrue(visibility);
        }

        [Test]
        public void Convert_int()
        {
            object result = subject.Convert(4, null, null, null);

            Assert.IsInstanceOf(typeof(bool), result);

            bool visibility = (bool)result;

            Assert.IsTrue(visibility);
        }

        #endregion
        #region ConvertBack

        [Test]
        public void ConvertBack_True()
        {
            object result = subject.ConvertBack(true, null, null, null);

            Assert.IsNotNull(result);
        }

        [Test]
        public void ConvertBack_False()
        {
            object result = subject.ConvertBack(false, null, null, null);

            Assert.IsNull(result);
        }

        [Test]
        public void ConvertBack_Null()
        {
            Exception e = Assert.Throws<ArgumentException>(() => subject.ConvertBack(null, null, null, null));
            Assert.AreEqual("The given value must be a boolean", e.Message);
        }

        [Test]
        public void ConvertBack_UnepectedObject()
        {
            Exception e = Assert.Throws<ArgumentException>(() => subject.ConvertBack(new Style(), null, null, null));
            Assert.AreEqual("The given value must be a boolean", e.Message);
        }

        #endregion
    }
}