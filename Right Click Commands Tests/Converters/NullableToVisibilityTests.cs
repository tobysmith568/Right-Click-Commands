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
    public class NullableToVisibilityTests
    {
        NullableToVisibility subject;

        [TestInitialize]
        public void TestInitialize()
        {
            subject = new NullableToVisibility();
        }

        [TestMethod]
        public void Test_Convert_Null()
        {
            object result = subject.Convert(null, null, null, null);

            Assert.IsInstanceOfType(result, typeof(Visibility));

            Visibility visibility = (Visibility)result;

            Assert.AreEqual(Visibility.Collapsed, visibility);
        }

        [TestMethod]
        public void Test_Convert_Object()
        {
            object result = subject.Convert(new object(), null, null, null);

            Assert.IsInstanceOfType(result, typeof(Visibility));

            Visibility visibility = (Visibility)result;

            Assert.AreEqual(Visibility.Visible, visibility);
        }

        [TestMethod]
        public void Test_Convert_int()
        {
            object result = subject.Convert(4, null, null, null);

            Assert.IsInstanceOfType(result, typeof(Visibility));

            Visibility visibility = (Visibility)result;

            Assert.AreEqual(Visibility.Visible, visibility);
        }

        [TestMethod]
        public void Test_ConvertBack_Visible()
        {
            Visibility visibility = Visibility.Visible;

            object result = subject.ConvertBack(visibility, null, null, null);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Test_ConvertBack_Collapsed()
        {
            Visibility visibility = Visibility.Collapsed;

            object result = subject.ConvertBack(visibility, null, null, null);

            Assert.IsNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "The given value must be a Visibility")]
        public void Test_ConvertBack_Null()
        {
            object result = subject.ConvertBack(null, null, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "The given value must be a Visibility")]
        public void Test_ConvertBack_UnepectedObject()
        {
            object result = subject.ConvertBack(new Style(), null, null, null);
        }
    }
}