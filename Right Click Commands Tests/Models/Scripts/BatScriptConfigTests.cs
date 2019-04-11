using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    [TestClass]
    public class BatScriptConfigTests
    {
        const string FolderPath = "testDir/";
        const string FilePath = FolderPath + "file.bat";
        BatScriptConfig subject;

        /// <exception cref="Exception">Ignore.</exception>
        [TestInitialize]
        public void TestInitialize()
        {
            subject = new BatScriptConfig("name", "id");
            subject.SetPrivateStaticField("appDataFolder", FolderPath);
            subject.SetPrivateAutoProperty("ScriptLocation", FilePath);
        }

        /// <exception cref="Exception">Ignore.</exception>
        [TestCleanup]
        public void TestCleanup()
        {
            if (Directory.Exists(FolderPath))
            {
                Directory.Delete(FolderPath, true);
            }
        }

        #region LoadScript Tests

        /// <exception cref="Exception">Ignore.</exception>
        [TestMethod]
        public void Test_LoadScript_WhenDirectoryDoesNotExist()
        {
            Assert.AreEqual(null, subject.Script);
            subject.LoadScript();
            Assert.AreEqual(string.Empty, subject.Script);
        }

        /// <exception cref="Exception">Ignore.</exception>
        [TestMethod]
        public void Test_LoadScript_WhenFileDoesNotExist()
        {
            Directory.CreateDirectory(FolderPath);

            Assert.AreEqual(null, subject.Script);
            subject.LoadScript();
            Assert.AreEqual(string.Empty, subject.Script);
        }

        /// <exception cref="Exception">Ignore.</exception>
        [TestMethod]
        public void Test_LoadScript_WhenFileIsEmpty()
        {
            Directory.CreateDirectory(FolderPath);
            File.WriteAllText(FilePath, string.Empty);

            Assert.AreEqual(null, subject.Script);
            subject.LoadScript();
            Assert.AreEqual(string.Empty, subject.Script);
        }

        /// <exception cref="Exception">Ignore.</exception>
        [TestMethod]
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
        [TestMethod]
        [ExpectedException(typeof(ScriptAccessException), "Cannot open the script file for the script [name]")]
        public void Test_LoadScript_WhenFileisLocked()
        {
            string content = "File Content";

            Directory.CreateDirectory(FolderPath);
            File.WriteAllText(FilePath, content);

            using (File.Open(FilePath, FileMode.Open))
            {
                subject.LoadScript();
            }
        }

        #endregion
        #region SaveScript Tests

        /// <exception cref="Exception">Ignore.</exception>
        [TestMethod]
        public void Test_SaveScript_WhenDirectoryDoesNotExist()
        {
            string content = "File Content";

            subject.Script = content;
            subject.SaveScript();

            string result = File.ReadAllText(FilePath);
            Assert.AreEqual(content, result);
        }

        /// <exception cref="Exception">Ignore.</exception>
        [TestMethod]
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
        [TestMethod]
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
        [TestMethod]
        [ExpectedException(typeof(ScriptAccessException), "Cannot open the script file for the script [name]")]
        public void Test_SaveScript_WhenFileisLocked()
        {
            string content = "File Content";

            Directory.CreateDirectory(FolderPath);
            File.WriteAllText(FilePath, content);

            using (File.Open(FilePath, FileMode.Open))
            {
                subject.Script = content;
                subject.SaveScript();
            }
        }

        #endregion
        #region ModifyLocation Tests

        /// <exception cref="Exception">Ignore.</exception>
        [DataTestMethod]
        [DataRow(MenuLocation.Background, true, false, false, false, true)]
        [DataRow(MenuLocation.Background, true, true, false, true, true)]
        [DataRow(MenuLocation.Background, true, false, true, false, true)]
        [DataRow(MenuLocation.Background, false, false, false, false, false)]
        [DataRow(MenuLocation.Background, false, false, true, false, false)]
        [DataRow(MenuLocation.Background, false, true, false, true, false)]
        [DataRow(MenuLocation.Directory, true, false, false, true, false)]
        [DataRow(MenuLocation.Directory, true, true, false, true, false)]
        [DataRow(MenuLocation.Directory, true, false, true, true, true)]
        [DataRow(MenuLocation.Directory, false, false, false, false, false)]
        [DataRow(MenuLocation.Directory, false, false, true, false, true)]
        [DataRow(MenuLocation.Directory, false, true, false, false, false)]
        [DataRow(MenuLocation.Both, true, false, false, true, true)]
        [DataRow(MenuLocation.Both, true, true, false, true, true)]
        [DataRow(MenuLocation.Both, true, false, true, true, true)]
        [DataRow(MenuLocation.Both, false, false, false, false, false)]
        [DataRow(MenuLocation.Both, false, false, true, false, false)]
        [DataRow(MenuLocation.Both, false, true, false, false, false)]
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
        [DataTestMethod]
        [DataRow(MenuLocation.Background, false, false, false)]
        [DataRow(MenuLocation.Background, false, true, true)]
        [DataRow(MenuLocation.Background, true, false, false)]
        [DataRow(MenuLocation.Background, true, true, true)]
        [DataRow(MenuLocation.Directory, false, false, false)]
        [DataRow(MenuLocation.Directory, false, true, false)]
        [DataRow(MenuLocation.Directory, true, false,true)]
        [DataRow(MenuLocation.Directory, true, true, true)]
        public void Test_IsForLocation(MenuLocation location, bool onDirectory, bool onBackground, bool expectedRusult)
        {
            subject.OnBackground = onBackground;
            subject.OnDirectory = onDirectory;

            bool result = subject.IsForLocation(location);

            Assert.AreEqual(expectedRusult, result);
        }

        #endregion
    }
}