using log4net;
using Moq;
using NUnit.Framework;
using Right_Click_Commands.WPF.Models.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Right_Click_Commands.WPF.Models.Logger.Tests
{
    [TestFixture]
    public class WindowsLoggerTests
    {
        public class TestClass { }

        private WindowsLogger subject;
        private Mock<ILog> ilog;

        [SetUp]
        public void SetUp()
        {
            subject = null;
            ilog = new Mock<ILog>();
        }

        #region Debug

        /// <exception cref="MockException">Ignore.</exception>
        [Test]
        public void Debug_CallsInternalLoggerWithMessage()
        {
            string message = "Test Message";

            Given_Subject_IsOfType(typeof(TestClass));
            Given_ilog_DebugIsSpied();
            Given_ilog_IsReflectedInto(subject);

            subject.Debug(message);

            ilog.Verify(i => i.Debug(It.IsRegex(message + "$")), Times.Once);
        }

        /// <exception cref="MockException">Ignore.</exception>
        [Test]
        public void Debug_CallsInternalLoggerWithLoglevel()
        {
            string message = "Test Message";

            Given_Subject_IsOfType(typeof(TestClass));
            Given_ilog_DebugIsSpied();
            Given_ilog_IsReflectedInto(subject);

            subject.Debug(message);

            ilog.Verify(i => i.Debug(It.IsRegex(@"^.* \[DEBUG\] .*$")), Times.Once);
        }

        #endregion
        #region Info

        /// <exception cref="MockException">Ignore.</exception>
        [Test]
        public void Info_CallsInternalLogger()
        {
            string message = "Test Message";

            Given_Subject_IsOfType(typeof(TestClass));
            Given_ilog_InfoIsSpied();
            Given_ilog_IsReflectedInto(subject);

            subject.Info(message);

            ilog.Verify(i => i.Info(It.IsRegex(message + "$")), Times.Once);
        }

        /// <exception cref="MockException">Ignore.</exception>
        [Test]
        public void Info_CallsInternalLoggerWithLoglevel()
        {
            string message = "Test Message";

            Given_Subject_IsOfType(typeof(TestClass));
            Given_ilog_InfoIsSpied();
            Given_ilog_IsReflectedInto(subject);

            subject.Info(message);

            ilog.Verify(i => i.Info(It.IsRegex(@"^.* \[INFO\] .*$")), Times.Once);
        }

        #endregion
        #region Warn

        /// <exception cref="MockException">Ignore.</exception>
        [Test]
        public void Warn_CallsInternalLogger()
        {
            string message = "Test Message";

            Given_Subject_IsOfType(typeof(TestClass));
            Given_ilog_WarnIsSpied();
            Given_ilog_IsReflectedInto(subject);

            subject.Warn(message);

            ilog.Verify(i => i.Warn(It.IsRegex(message + "$")), Times.Once);
        }

        /// <exception cref="MockException">Ignore.</exception>
        [Test]
        public void Warn_CallsInternalLoggerWithLoglevel()
        {
            string message = "Test Message";

            Given_Subject_IsOfType(typeof(TestClass));
            Given_ilog_WarnIsSpied();
            Given_ilog_IsReflectedInto(subject);

            subject.Warn(message);

            ilog.Verify(i => i.Warn(It.IsRegex(@"^.* \[WARN\] .*$")), Times.Once);
        }

        #endregion
        #region Error

        /// <exception cref="MockException">Ignore.</exception>
        [Test]
        public void Error_CallsInternalLogger()
        {
            string message = "Test Message";

            Given_Subject_IsOfType(typeof(TestClass));
            Given_ilog_ErrorIsSpied();
            Given_ilog_IsReflectedInto(subject);

            subject.Error(message);

            ilog.Verify(i => i.Error(It.IsRegex(message + "$")), Times.Once);
        }

        /// <exception cref="MockException">Ignore.</exception>
        [Test]
        public void Error_CallsInternalLoggerWithLoglevel()
        {
            string message = "Test Message";

            Given_Subject_IsOfType(typeof(TestClass));
            Given_ilog_ErrorIsSpied();
            Given_ilog_IsReflectedInto(subject);

            subject.Error(message);

            ilog.Verify(i => i.Error(It.IsRegex(@"^.* \[ERROR\] .*$")), Times.Once);
        }

        #endregion
        #region Fatal

        /// <exception cref="MockException">Ignore.</exception>
        [Test]
        public void Fatal_CallsInternalLogger()
        {
            string message = "Test Message";

            Given_Subject_IsOfType(typeof(TestClass));
            Given_ilog_FatalIsSpied();
            Given_ilog_IsReflectedInto(subject);

            subject.Fatal(message);

            ilog.Verify(i => i.Fatal(It.IsRegex(message + "$")), Times.Once);
        }

        /// <exception cref="MockException">Ignore.</exception>
        [Test]
        public void Fatal_CallsInternalLoggerWithLoglevel()
        {
            string message = "Test Message";

            Given_Subject_IsOfType(typeof(TestClass));
            Given_ilog_FatalIsSpied();
            Given_ilog_IsReflectedInto(subject);

            subject.Fatal(message);

            ilog.Verify(i => i.Fatal(It.IsRegex(@"^.* \[FATAL\] .*$")), Times.Once);
        }

        #endregion
        #region Script

        /// <exception cref="MockException">Ignore.</exception>
        [Test]
        public void Script_CallsInternalLogger()
        {
            string script = "Script";
            string message = "Test Message";

            Given_Subject_IsOfType(typeof(TestClass));
            Given_ilog_InfoIsSpied();
            Given_ilog_IsReflectedInto(subject);

            subject.Script(script, message);

            ilog.Verify(i => i.Info(It.IsRegex(message + "$")), Times.Once);
        }

        /// <exception cref="MockException">Ignore.</exception>
        [Test]
        public void Script_CallsInternalLoggerWithLoglevel()
        {
            string script = "Script";
            string message = "Test Message";

            Given_Subject_IsOfType(typeof(TestClass));
            Given_ilog_InfoIsSpied();
            Given_ilog_IsReflectedInto(subject);

            subject.Script(script, message);

            ilog.Verify(i => i.Info(It.IsRegex($@"^.* \[{script}\] .*$")), Times.Once);
        }

        #endregion

        public void Given_Subject_IsOfType(Type type)
        {
            subject = new WindowsLogger(type);
        }

        public void Given_ilog_DebugIsSpied()
        {
            ilog.Setup(i => i.Debug(It.IsAny<string>())).Verifiable();
        }

        public void Given_ilog_InfoIsSpied()
        {
            ilog.Setup(i => i.Info(It.IsAny<string>())).Verifiable();
        }

        public void Given_ilog_WarnIsSpied()
        {
            ilog.Setup(i => i.Warn(It.IsAny<string>())).Verifiable();
        }

        public void Given_ilog_ErrorIsSpied()
        {
            ilog.Setup(i => i.Error(It.IsAny<string>())).Verifiable();
        }

        public void Given_ilog_FatalIsSpied()
        {
            ilog.Setup(i => i.Fatal(It.IsAny<string>())).Verifiable();
        }

        /// <exception cref="FieldAccessException">Ignore.</exception>
        /// <exception cref="TargetException">Ignore.</exception>
        public void Given_ilog_IsReflectedInto(object ob)
        {
            Type type = ob.GetType();
            FieldInfo fieldInfo = type.GetField("log", BindingFlags.Instance | BindingFlags.NonPublic);
            fieldInfo.SetValue(ob, ilog.Object);
        }
    }
}