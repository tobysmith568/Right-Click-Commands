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
    public class NullableToVisibilityTests
    {
        NullableToVisibility subject;

        [SetUp]
        public void TestInitialize()
        {
            subject = new NullableToVisibility();
        }

        [Test]
        public void Test_Convert_Null()
        {
            object result = subject.Convert(null, null, null, null);

            Assert.IsInstanceOf(typeof(Visibility), result);

            Visibility visibility = (Visibility)result;

            Assert.AreEqual(Visibility.Collapsed, visibility);
        }

        [Test]
        public void Test_Convert_Object()
        {
            object result = subject.Convert(new object(), null, null, null);

            Assert.IsInstanceOf(typeof(Visibility), result);

            Visibility visibility = (Visibility)result;

            Assert.AreEqual(Visibility.Visible, visibility);
        }

        [Test]
        public void Test_Convert_int()
        {
            object result = subject.Convert(4, null, null, null);

            Assert.IsInstanceOf(typeof(Visibility), result);

            Visibility visibility = (Visibility)result;

            Assert.AreEqual(Visibility.Visible, visibility);
        }

        [Test]
        public void Test_ConvertBack_Visible()
        {
            Visibility visibility = Visibility.Visible;

            object result = subject.ConvertBack(visibility, null, null, null);

            Assert.IsNotNull(result);
        }

        [Test]
        public void Test_ConvertBack_Collapsed()
        {
            Visibility visibility = Visibility.Collapsed;

            object result = subject.ConvertBack(visibility, null, null, null);

            Assert.IsNull(result);
        }

        [Test]
        public void Test_ConvertBack_Null()
        {
            Exception e = Assert.Throws<ArgumentException>(() => subject.ConvertBack(null, null, null, null));
            Assert.AreEqual("The given value must be a Visibility", e.Message);
        }

        [Test]
        public void Test_ConvertBack_UnepectedObject()
        {
            Exception e = Assert.Throws<ArgumentException>(() => subject.ConvertBack(new Style(), null, null, null));
            Assert.AreEqual("The given value must be a Visibility", e.Message);
        }
    }
}