using NUnit.Framework;
using Right_Click_Commands.WPF.Converters;
using Right_Click_Commands.WPF.Models.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Right_Click_Commands.WPF.Converters.Tests
{
    [TestFixture]
    public class ScriptTypeToStringConveterTests
    {
        ScriptTypeToStringConveter subject;

        [SetUp]
        public void SetUp()
        {
            subject = new ScriptTypeToStringConveter();
        }

        #region Convert

        [TestCase(ScriptType.Batch, "Batch")]
        [TestCase(ScriptType.Powershell, "Powershell")]
        public void Test_Convert_ValueInRange(ScriptType value, string expectedResult)
        {
            object result = subject.Convert(value, null, null, null);

            Assert.IsInstanceOf(typeof(string), result);

            string @string = (string)result;

            Assert.AreEqual(expectedResult, @string);
        }

        [Test]
        public void Test_Convert_Null()
        {
            ArgumentException e = Assert.Throws<ArgumentException>(() => subject.Convert(null, null, null, null));
            Assert.AreEqual("The given value must be a ScriptType", e.Message);
        }

        #endregion
        #region ConvertBack
        
        [TestCase("string")]
        [TestCase("")]
        [TestCase(0)]
        [TestCase(5)]
        [TestCase(null)]
        [TestCase(ScriptType.Batch)]
        [TestCase(ScriptType.Powershell)]
        public void Test_ConvertBack_ThrowsNotImplemented(object value)
        {
            Assert.Throws<NotImplementedException>(() => subject.ConvertBack(value, null, null, null));
        }

        #endregion
    }
}