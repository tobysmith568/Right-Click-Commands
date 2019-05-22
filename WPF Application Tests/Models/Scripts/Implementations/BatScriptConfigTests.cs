using Moq;
using NUnit.Framework;
using Right_Click_Commands.Models.MessagePrompts;
using Right_Click_Commands.Models.Settings;
using System.IO;

namespace Right_Click_Commands.WPF.Models.Scripts.Tests
{
    [TestFixture]
    public class BatScriptConfigTests
    {
        BatScriptConfig subject;
        Mock<ISettings> settings;
        Mock<IMessagePrompt> messagePrompt;

        readonly string location = "scriptLocation";
        readonly string name = "name";
        readonly string id = "id";

        [SetUp]
        public void Setup()
        {
            settings = new Mock<ISettings>();
            settings.Setup(s => s.ScriptLocation).Returns(string.Empty);

            messagePrompt = new Mock<IMessagePrompt>();

            subject = new BatScriptConfig(name, id, settings.Object, messagePrompt.Object);
        }

        #region Constructor

        [Test]
        public void Test_Constructor_SetsScriptLocation()
        {
            Given_settings_ScriptLocation_Returns(location);

            subject = new BatScriptConfig(name, id, settings.Object, messagePrompt.Object);

            Assert.AreEqual(location + Path.DirectorySeparatorChar + name + ".bat", subject.ScriptLocation);
        }

        #endregion
        #region ScriptArgs

        [Test]
        public void Test_ScriptArgs_UsesKWhenKeepingWindowOpen()
        {
            Given_settings_ScriptLocation_Returns(location);
            Given_subject_KeepWindowOpen_Equals(true);

            Assert.True(subject.ScriptArgs.StartsWith("\"cmd\" \"/K"));
        }

        [Test]
        public void Test_ScriptArgs_UsesCWhenClosingWindow()
        {
            Given_settings_ScriptLocation_Returns(location);
            Given_subject_KeepWindowOpen_Equals(false);

            Assert.True(subject.ScriptArgs.StartsWith("\"cmd\" \"/C"));
        }

        #endregion

        private void Given_settings_ScriptLocation_Returns(string value)
        {
            settings.Setup(s => s.ScriptLocation).Returns(value);
        }

        private void Given_subject_KeepWindowOpen_Equals(bool value)
        {
            subject.KeepWindowOpen = value;
        }
    }
}