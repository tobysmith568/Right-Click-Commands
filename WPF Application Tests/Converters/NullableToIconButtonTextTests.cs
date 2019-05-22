using NUnit.Framework;
using System;

namespace Right_Click_Commands.WPF.Converters.Tests
{
    [TestFixture]
    public class NullableToIconButtonTextTests
    {
        NullableToIconButtonText subject;

        [SetUp]
        public void SetUp()
        {
            subject = new NullableToIconButtonText();
        }

        [Test]
        public void Test_Convert_Null()
        {
            object result = subject.Convert(null, null, null, null);

            Assert.AreEqual("Locate", result);
        }

        [Test]
        public void Test_Convert_NotNull()
        {
            object result = subject.Convert(new object(), null, null, null);

            Assert.AreEqual("Remove", result);
        }

        [Test]
        public void Test_ConvertBack_Null()
        {
            Assert.Throws<NotImplementedException>(() => subject.ConvertBack(null, null, null, null));
        }
    }
}