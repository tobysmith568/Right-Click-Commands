using NUnit.Framework;
using Right_Click_Commands.Models.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Right_Click_Commands.Models.Scripts.Tests
{
    [TestFixture]
    public class ScriptAccessExceptionTests
    {
        ScriptAccessException subject;

        [SetUp]
        public void SetUp()
        {
            subject = null;
        }

        [Test]
        public void Test_Constructor_ValidData()
        {
            string message = "This is an exception message";
            Exception inner = new Exception("This is an inner exception");

            subject = new ScriptAccessException(message, inner);

            Assert.AreEqual(message, subject.Message);
            Assert.AreSame(inner, subject.InnerException);
        }
    }
}