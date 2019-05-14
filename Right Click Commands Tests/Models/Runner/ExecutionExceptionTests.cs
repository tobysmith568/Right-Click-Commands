using NUnit.Framework;
using Right_Click_Commands.Models.Runner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Right_Click_Commands.Models.Runner.Tests
{
    [TestFixture]
    public class ExecutionExceptionTests
    {
        ExecutionException subject;

        [SetUp]
        public void SetUp()
        {
            subject = null;
        }

        #region Constructor

        [Test]
        public void Constructor_MessageAndException()
        {
            string message = "Message";
            Exception exception = new Exception();

            Given_subject_IsConstructedWith(message, exception);

            Assert.AreEqual(message, subject.Message);
            Assert.AreSame(exception, subject.InnerException);
        }

        [Test]
        public void Constructor_MessageAndNullException()
        {
            string message = "Message";

            Given_subject_IsConstructedWith(message, null);

            Assert.AreEqual(message, subject.Message);
            Assert.IsNull(subject.InnerException);
        }

        [Test]
        public void Constructor_MessageAndNoException()
        {
            string message = "Message";

            Given_subject_IsConstructedWith(message);

            Assert.AreEqual(message, subject.Message);
            Assert.IsNull(subject.InnerException);
        }

        [Test]
        public void Constructor_NullMessageAndException()
        {
            Exception exception = new Exception();

            Given_subject_IsConstructedWith(null, exception);

            Assert.AreEqual("Exception of type 'Right_Click_Commands.Models.Runner.ExecutionException' was thrown.", subject.Message);
            Assert.AreSame(exception, subject.InnerException);
        }

        [Test]
        public void Constructor_NoMessageAndException()
        {
            Exception exception = new Exception();

            Given_subject_IsConstructedWith(exception);

            Assert.AreEqual(string.Empty, subject.Message);
            Assert.AreSame(exception, subject.InnerException);
        }

        [Test]
        public void Constructor_Neither()
        {
            Given_subject_IsConstructedWith();

            Assert.AreEqual(string.Empty, subject.Message);
            Assert.IsNull(subject.InnerException);
        }

        #endregion

        private void Given_subject_IsConstructedWith(string message, Exception exception)
        {
            subject = new ExecutionException(message, exception);
        }

        private void Given_subject_IsConstructedWith(string message)
        {
            subject = new ExecutionException(message);
        }

        private void Given_subject_IsConstructedWith(Exception exception)
        {
            subject = new ExecutionException(e: exception);
        }

        private void Given_subject_IsConstructedWith()
        {
            subject = new ExecutionException();
        }
    }
}