using NUnit.Framework;
using System;
using Right_Click_Commands.Utils;

namespace Right_Click_Commands.Utils.Tests
{
    [TestFixture]
    public class EnumUtilsTests
    {
        public enum TestEnum
        {
            Value1,
            Value2
        }

        public enum OtherEnum
        {
            Value100,
            Value200
        }

        private string subject;

        [SetUp]
        public void SetUp()
        {
            subject = null;
        }

        [TestCase("Value1", TestEnum.Value1)]
        [TestCase("value1", TestEnum.Value1)]
        [TestCase("VALUE1", TestEnum.Value1)]
        [TestCase("Value2", TestEnum.Value2)]
        [TestCase("value2", TestEnum.Value2)]
        [TestCase("VALUE2", TestEnum.Value2)]
        public void ToEnum_ConvertsValueString(string inputvalue, TestEnum expectedResult)
        {
            Given_SubjectEquals(inputvalue);

            TestEnum result = subject.ToEnum<TestEnum>();

            Assert.AreEqual(expectedResult, result);
        }

        [TestCase("Value3")]
        [TestCase("SOMTHING")]
        [TestCase("")]
        [TestCase(null)]
        public void ToEnum_ThrowsForInvalidString(string value)
        {
            Given_SubjectEquals(value);

            ArgumentOutOfRangeException e = Assert.Throws<ArgumentOutOfRangeException>(() => subject.ToEnum<TestEnum>());

            Assert.AreEqual("value", e.ParamName);
            Assert.AreEqual("Specified argument was out of the range of valid values.\r\nParameter name: value", e.Message);
        }

        public void Given_SubjectEquals(string value)
        {
            subject = value;
        }
    }
}