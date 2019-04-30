using Moq;
using NUnit.Framework;
using Right_Click_Commands.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Right_Click_Commands.Tests;

namespace Right_Click_Commands.Converters.Tests
{
    [TestFixture]
    public class ConverterBaseTests
    {
        public class GenericType { }

        Mock<ConverterBase<GenericType>> subject;

        [SetUp]
        public void SetUp()
        {
            subject = new Mock<ConverterBase<GenericType>>
            {
                CallBase = true
            };
        }

        [Test]
        public void Test_ProvideValue_CreatesGenericTypeWhenNull()
        {
            object result = subject.Object.ProvideValue(null);

            Assert.IsNotNull(result);
        }

        [Test]
        public void Test_ProvideValue_SameObjectIsAlwaysReturned()
        {
            object expectedResult = subject.Object.ProvideValue(null);
            object result = subject.Object.ProvideValue(null);

            Assert.AreSame(expectedResult, result);
        }
    }
}