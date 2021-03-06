﻿using IconPicker;
using Moq;
using NUnit.Framework;
using Right_Click_Commands.Models.MessagePrompts;
using Right_Click_Commands.Models.Settings;
using System.IO;

namespace Right_Click_Commands.WPF.Models.Scripts.Tests
{
    [TestFixture]
    public class PowershellScriptConfigTests
    {
        PowershellScriptConfig subject;
        Mock<ISettings> settings;
        Mock<IMessagePrompt> messagePrompt;
        Mock<IIconPicker> iconPicker;

        readonly string location = "scriptLocation";
        readonly string name = "name";
        readonly string id = "id";

        [SetUp]
        public void Setup()
        {
            settings = new Mock<ISettings>();
            settings.Setup(s => s.ScriptLocation).Returns(string.Empty);

            messagePrompt = new Mock<IMessagePrompt>();

            iconPicker = new Mock<IIconPicker>();

            subject = new PowershellScriptConfig(name, id, settings.Object, messagePrompt.Object, iconPicker.Object);
        }

        #region Constructor

        [Test]
        public void Test_Constructor_SetsScriptLocation()
        {
            Given_settings_ScriptLocation_Returns(location);

            subject = new PowershellScriptConfig(name, id, settings.Object, messagePrompt.Object, iconPicker.Object);

            Assert.AreEqual(location + Path.DirectorySeparatorChar + name + ".ps1", subject.ScriptLocation);
        }

        #endregion
        #region ScriptArgs

        [Test]
        public void Test_ScriptArgs_UsesNoExitWhenKeepingWindowOpen()
        {
            Given_settings_ScriptLocation_Returns(location);
            Given_subject_KeepWindowOpen_Equals(true);

            Assert.True(subject.ScriptArgs.StartsWith("\"powershell\" \"-NoExit"));
        }

        [Test]
        public void Test_ScriptArgs_UsesNothingWhenClosingWindow()
        {
            Given_settings_ScriptLocation_Returns(location);
            Given_subject_KeepWindowOpen_Equals(false);

            Assert.True(subject.ScriptArgs.StartsWith("\"powershell\" \" -nologo -ExecutionPolicy Bypass -command |"));
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