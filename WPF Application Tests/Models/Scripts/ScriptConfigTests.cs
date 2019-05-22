using Right_Click_Commands.Tests;
using NUnit.Framework;
using System;
using System.IO;
using Moq;
using Right_Click_Commands.Models.Settings;
using Right_Click_Commands.Models.Scripts;
using Right_Click_Commands.Models.MessagePrompts;
using IconPicker;

namespace Right_Click_Commands.WPF.Models.Scripts.Tests
{
    [TestFixture]
    public class ScriptConfigTests
    {
        string folderPath;
        string filePath;
        Mock<ISettings> settings;
        Mock<IMessagePrompt> messagePrompt;
        Mock<IIconPicker> iconPicker;
        Mock<ScriptConfig> subject;

        /// <exception cref="Exception">Ignore.</exception>
        [SetUp]
        public void SetUp()
        {
            folderPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "testDir");
            filePath = Path.Combine(folderPath, "file.bat");

            settings = new Mock<ISettings>();
            settings.Setup(s => s.ScriptLocation).Returns(folderPath);

            iconPicker = new Mock<IIconPicker>();

            messagePrompt = new Mock<IMessagePrompt>();
            messagePrompt.Setup(m => m.PromptOK(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<MessageType>())).Verifiable();

            subject = new Mock<ScriptConfig>("name", "id", settings.Object, messagePrompt.Object, iconPicker.Object)
            {
                CallBase = true
            };
            subject.Setup(s => s.ScriptLocation).Returns(filePath);
        }

        /// <exception cref="Exception">Ignore.</exception>
        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(folderPath))
            {
                Directory.Delete(folderPath, true);
            }
        }

        #region LoadScript Tests

        /// <exception cref="Exception">Ignore.</exception>
        [Test]
        public void Test_LoadScript_WhenDirectoryDoesNotExist()
        {
            Assert.AreEqual(null, subject.Object.Script);
            subject.Object.LoadScript();
            Assert.AreEqual(string.Empty, subject.Object.Script);
        }

        /// <exception cref="Exception">Ignore.</exception>
        [Test]
        public void Test_LoadScript_WhenFileDoesNotExist()
        {
            Directory.CreateDirectory(folderPath);

            Assert.AreEqual(null, subject.Object.Script);
            subject.Object.LoadScript();
            Assert.AreEqual(string.Empty, subject.Object.Script);
        }

        /// <exception cref="Exception">Ignore.</exception>
        [Test]
        public void Test_LoadScript_WhenFileIsEmpty()
        {
            Directory.CreateDirectory(folderPath);
            File.WriteAllText(filePath, string.Empty);

            Assert.AreEqual(null, subject.Object.Script);
            subject.Object.LoadScript();
            Assert.AreEqual(string.Empty, subject.Object.Script);
        }

        /// <exception cref="Exception">Ignore.</exception>
        [Test]
        public void Test_LoadScript_WhenFileHasContent()
        {
            string content = "File Content";

            Directory.CreateDirectory(folderPath);
            File.WriteAllText(filePath, content);

            Assert.AreEqual(null, subject.Object.Script);
            subject.Object.LoadScript();
            Assert.AreEqual(content, subject.Object.Script);
        }

        /// <exception cref="Exception">Ignore.</exception>
        [Test]
        public void Test_LoadScript_WhenFileisLocked()
        {
            string content = "File Content";

            Directory.CreateDirectory(folderPath);
            File.WriteAllText(filePath, content);

            using (File.Open(filePath, FileMode.Open))
            {
                Exception e = Assert.Throws<ScriptAccessException>(() => subject.Object.LoadScript());
                Assert.AreEqual("Cannot open the script file for the script [name]", e.Message);
            }
        }

        #endregion
        #region SaveScript Tests

        /// <exception cref="Exception">Ignore.</exception>
        [Test]
        public void Test_SaveScript_WhenDirectoryDoesNotExist()
        {
            string content = "File Content";

            subject.Object.Script = content;
            subject.Object.SaveScript();

            string result = File.ReadAllText(filePath);
            Assert.AreEqual(content, result);
        }

        /// <exception cref="Exception">Ignore.</exception>
        [Test]
        public void Test_SaveScript_WhenFileDoesNotExist()
        {
            string content = "File Content";

            Directory.CreateDirectory(folderPath);
            subject.Object.Script = content;
            subject.Object.SaveScript();

            string result = File.ReadAllText(filePath);
            Assert.AreEqual(content, result);
        }

        /// <exception cref="Exception">Ignore.</exception>
        [Test]
        public void Test_SaveScript_WhenFileHasContent()
        {
            string content = "File Content";

            Directory.CreateDirectory(folderPath);
            File.WriteAllText(filePath, content);

            subject.Object.Script = content;
            subject.Object.SaveScript();

            string result = File.ReadAllText(filePath);
            Assert.AreEqual(content, result);
        }

        /// <exception cref="Exception">Ignore.</exception>
        [Test]
        public void Test_SaveScript_WhenFileisLocked()
        {
            string content = "File Content";
            string label = "Label";

            Directory.CreateDirectory(folderPath);
            File.WriteAllText(filePath, content);

            using (File.Open(filePath, FileMode.Open))
            {
                subject.Object.Script = content;
                subject.Object.Label = label;

                subject.Object.SaveScript();

                messagePrompt.Verify(m => m.PromptOK($"Cannot open the script file for the script [{label}]", "Error saving script", MessageType.Error), Times.Once);
            }
        }

        #endregion
        #region ModifyLocation Tests

        /// <exception cref="Exception">Ignore.</exception>
        [TestCase(MenuLocation.Background, true, false, false, false, true)]
        [TestCase(MenuLocation.Background, true, true, false, true, true)]
        [TestCase(MenuLocation.Background, true, false, true, false, true)]
        [TestCase(MenuLocation.Background, false, false, false, false, false)]
        [TestCase(MenuLocation.Background, false, false, true, false, false)]
        [TestCase(MenuLocation.Background, false, true, false, true, false)]
        [TestCase(MenuLocation.Directory, true, false, false, true, false)]
        [TestCase(MenuLocation.Directory, true, true, false, true, false)]
        [TestCase(MenuLocation.Directory, true, false, true, true, true)]
        [TestCase(MenuLocation.Directory, false, false, false, false, false)]
        [TestCase(MenuLocation.Directory, false, false, true, false, true)]
        [TestCase(MenuLocation.Directory, false, true, false, false, false)]
        [TestCase(MenuLocation.Both, true, false, false, true, true)]
        [TestCase(MenuLocation.Both, true, true, false, true, true)]
        [TestCase(MenuLocation.Both, true, false, true, true, true)]
        [TestCase(MenuLocation.Both, false, false, false, false, false)]
        [TestCase(MenuLocation.Both, false, false, true, false, false)]
        [TestCase(MenuLocation.Both, false, true, false, false, false)]
        public void Test_ModifyLocation(MenuLocation location, bool enable, bool onDirectoryState, bool onBackgroundState, bool onDirectoryResult, bool onBackgroundResult)
        {
            subject.Object.OnDirectory = onDirectoryState;
            subject.Object.OnBackground = onBackgroundState;

            subject.Object.ModifyLocation(location, enable);

            Assert.AreEqual(onDirectoryResult, subject.Object.OnDirectory);
            Assert.AreEqual(onBackgroundResult, subject.Object.OnBackground);
        }

        #endregion
        #region IsForLocation Tests

        /// <exception cref="Exception">Ignore.</exception>
        [TestCase(MenuLocation.Background, false, false, ExpectedResult = false)]
        [TestCase(MenuLocation.Background, false, true, ExpectedResult = true)]
        [TestCase(MenuLocation.Background, true, false, ExpectedResult = false)]
        [TestCase(MenuLocation.Background, true, true, ExpectedResult = true)]
        [TestCase(MenuLocation.Directory, false, false, ExpectedResult = false)]
        [TestCase(MenuLocation.Directory, false, true, ExpectedResult = false)]
        [TestCase(MenuLocation.Directory, true, false, ExpectedResult = true)]
        [TestCase(MenuLocation.Directory, true, true, ExpectedResult = true)]
        public bool Test_IsForLocation(MenuLocation location, bool onDirectory, bool onBackground)
        {
            subject.Object.OnBackground = onBackground;
            subject.Object.OnDirectory = onDirectory;

            bool result = subject.Object.IsForLocation(location);

            return result;
        }

        #endregion
    }
}