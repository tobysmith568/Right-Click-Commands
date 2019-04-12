using NUnit.Framework;
using Right_Click_Commands.Models.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Right_Click_Commands.Tests;
using System.IO;

namespace Right_Click_Commands.Models.Scripts.Tests
{
    [TestFixture]
    public class BatScriptConfigTests
    {
        string FolderPath;
        string FilePath;
        BatScriptConfig subject;

        /// <exception cref="Exception">Ignore.</exception>
        [SetUp]
        public void TestInitialize()
        {
            FolderPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "testDir");
            FilePath = Path.Combine(FolderPath, "file.bat");

            subject = new BatScriptConfig("name", "id");
            subject.SetPrivateStaticField("appDataFolder", FolderPath);
            subject.SetPrivateAutoProperty("ScriptLocation", FilePath);
        }

        /// <exception cref="Exception">Ignore.</exception>
        [TearDown]
        public void TestCleanup()
        {
            if (Directory.Exists(FolderPath))
            {
                Directory.Delete(FolderPath, true);
            }
        }

        #region LoadScript Tests

        /// <exception cref="Exception">Ignore.</exception>
        [Test]
        public void Test_LoadScript_WhenDirectoryDoesNotExist()
        {
            Assert.AreEqual(null, subject.Script);
            subject.LoadScript();
            Assert.AreEqual(string.Empty, subject.Script);
        }

        /// <exception cref="Exception">Ignore.</exception>
        [Test]
        public void Test_LoadScript_WhenFileDoesNotExist()
        {
            Directory.CreateDirectory(FolderPath);

            Assert.AreEqual(null, subject.Script);
            subject.LoadScript();
            Assert.AreEqual(string.Empty, subject.Script);
        }

        /// <exception cref="Exception">Ignore.</exception>
        [Test]
        public void Test_LoadScript_WhenFileIsEmpty()
        {
            Directory.CreateDirectory(FolderPath);
            File.WriteAllText(FilePath, string.Empty);

            Assert.AreEqual(null, subject.Script);
            subject.LoadScript();
            Assert.AreEqual(string.Empty, subject.Script);
        }

        /// <exception cref="Exception">Ignore.</exception>
        [Test]
        public void Test_LoadScript_WhenFileHasContent()
        {
            string content = "File Content";

            Directory.CreateDirectory(FolderPath);
            File.WriteAllText(FilePath, content);

            Assert.AreEqual(null, subject.Script);
            subject.LoadScript();
            Assert.AreEqual(content, subject.Script);
        }

        /// <exception cref="Exception">Ignore.</exception>
        [Test]
        public void Test_LoadScript_WhenFileisLocked()
        {
            string content = "File Content";

            Directory.CreateDirectory(FolderPath);
            File.WriteAllText(FilePath, content);

            using (File.Open(FilePath, FileMode.Open))
            {
                Exception e = Assert.Throws<ScriptAccessException>(() => subject.LoadScript());
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

            subject.Script = content;
            subject.SaveScript();

            string result = File.ReadAllText(FilePath);
            Assert.AreEqual(content, result);
        }

        /// <exception cref="Exception">Ignore.</exception>
        [Test]
        public void Test_SaveScript_WhenFileDoesNotExist()
        {
            string content = "File Content";

            Directory.CreateDirectory(FolderPath);
            subject.Script = content;
            subject.SaveScript();

            string result = File.ReadAllText(FilePath);
            Assert.AreEqual(content, result);
        }

        /// <exception cref="Exception">Ignore.</exception>
        [Test]
        public void Test_SaveScript_WhenFileHasContent()
        {
            string content = "File Content";

            Directory.CreateDirectory(FolderPath);
            File.WriteAllText(FilePath, content);

            subject.Script = content;
            subject.SaveScript();

            string result = File.ReadAllText(FilePath);
            Assert.AreEqual(content, result);
        }

        /// <exception cref="Exception">Ignore.</exception>
        [Test]
        public void Test_SaveScript_WhenFileisLocked()
        {
            string content = "File Content";

            Directory.CreateDirectory(FolderPath);
            File.WriteAllText(FilePath, content);

            using (File.Open(FilePath, FileMode.Open))
            {
                subject.Script = content;

                Exception e = Assert.Throws<ScriptAccessException>(() => subject.SaveScript());
                Assert.AreEqual("Cannot open the script file for the script [name]", e.Message);
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
            subject.OnDirectory = onDirectoryState;
            subject.OnBackground = onBackgroundState;

            subject.ModifyLocation(location, enable);

            Assert.AreEqual(onDirectoryResult, subject.OnDirectory);
            Assert.AreEqual(onBackgroundResult, subject.OnBackground);
        }

        #endregion
        #region IsForLocation Tests

        /// <exception cref="Exception">Ignore.</exception>
        [TestCase(MenuLocation.Background, false, false,    ExpectedResult = false)]
        [TestCase(MenuLocation.Background, false, true,     ExpectedResult = true)]
        [TestCase(MenuLocation.Background, true, false,     ExpectedResult = false)]
        [TestCase(MenuLocation.Background, true, true,      ExpectedResult = true)]
        [TestCase(MenuLocation.Directory, false, false,     ExpectedResult = false)]
        [TestCase(MenuLocation.Directory, false, true,      ExpectedResult = false)]
        [TestCase(MenuLocation.Directory, true, false,      ExpectedResult = true)]
        [TestCase(MenuLocation.Directory, true, true,       ExpectedResult = true)]
        public bool Test_IsForLocation(MenuLocation location, bool onDirectory, bool onBackground)
        {
            subject.OnBackground = onBackground;
            subject.OnDirectory = onDirectory;

            bool result = subject.IsForLocation(location);

            return result;
        }

        #endregion
    }
}