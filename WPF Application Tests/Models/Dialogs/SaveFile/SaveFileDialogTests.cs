using Moq;
using NUnit.Framework;
using Right_Click_Commands.WPF.Models.Dialogs.SaveFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pose;
using WinSaveFileDialog = Microsoft.Win32.SaveFileDialog;
using System.IO;

namespace Right_Click_Commands.WPF.Models.Dialogs.SaveFile.Tests
{
    [TestFixture]
    public class SaveFileDialogTests
    {
        private SaveFileDialog subject;
        private Shim shim_File_WriteAllText_DoesNothing;
        private Shim shimSaveFileDialog_FileName_Gets;
        private Shim shimSaveFileDialog_ShowDialog_Returns;

        private StringWriter consoleOutput;

        /// <exception cref="System.IO.IOException">Ignore.</exception>
        /// <exception cref="UnauthorizedAccessException">Ignore.</exception>
        /// <exception cref="System.Security.SecurityException">Ignore.</exception>
        [SetUp]
        public void SetUp()
        {
            subject = new SaveFileDialog();

            consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);
        }

        [TearDown]
        public void TearDown()
        {
            consoleOutput.Dispose();
        }

        [Test]
        public void Save_WritesToDiskWhenDialogReturns_True()
        {
            Given_ShimSaveFileDialog_ShowDialog_Returns(true);
            Given_ShimSaveFileDialog_FileName_Gets("fileName");
            Given_Shim_File_WriteAllText_DoesNothing();

            bool? result = null;

            PoseContext.Isolate(() =>
            {
                result = subject.Save("content");
            }, shimSaveFileDialog_ShowDialog_Returns, shimSaveFileDialog_FileName_Gets, shim_File_WriteAllText_DoesNothing);

            Assert.IsTrue(result);
            Assert.AreEqual("Saving content to fileName\r\n", consoleOutput.ToString());
        }

        [Test]
        public void Save_DoesNotWriteToDiskWhenDialogReturns_False()
        {
            Given_ShimSaveFileDialog_ShowDialog_Returns(false);
            Given_ShimSaveFileDialog_FileName_Gets("fileName");
            Given_Shim_File_WriteAllText_DoesNothing();

            bool? result = null;

            PoseContext.Isolate(() =>
            {
                result = subject.Save("content");
            }, shimSaveFileDialog_ShowDialog_Returns, shimSaveFileDialog_FileName_Gets, shim_File_WriteAllText_DoesNothing);

            Assert.IsFalse(result);
            Assert.AreEqual("", consoleOutput.ToString());
        }

        [TestCase("suggestedFileName", "suggestedFileName")]
        [TestCase(null, "")]
        public void Save_CorrectlySetsSuggestedFileName(string input, string expectedResult)
        {
            Given_ShimSaveFileDialog_ShowDialog_LogsFileNameAndReturnsFalse();

            bool? result = null;

            PoseContext.Isolate(() =>
            {
                result = subject.Save("content", "fileExtension", "fileExtensionName", input);
            }, shimSaveFileDialog_ShowDialog_Returns);

            Assert.IsFalse(result);
            Assert.AreEqual($"Filename: {expectedResult}\r\n", consoleOutput.ToString());
        }

        [TestCase(".bat", "bat")]
        [TestCase(".ps1", "ps1")]
        [TestCase(null, "")]
        public void Save_CorrectlySetsSuggestedFileExtension(string input, string expectedResult)
        {
            Given_ShimSaveFileDialog_ShowDialog_LogsFileExtensionAndReturnsFalse();

            bool? result = null;

            PoseContext.Isolate(() =>
            {
                result = subject.Save("content", input, "fileExtensionName", "fileName");
            }, shimSaveFileDialog_ShowDialog_Returns);

            Assert.IsFalse(result);
            Assert.AreEqual($"File extension: {expectedResult}\r\n", consoleOutput.ToString());
        }

        [TestCase(".bat", "Batch Script", "Batch Script|*.bat|All Files|*.*")]
        [TestCase(".ps1", "Powershell Script", "Powershell Script|*.ps1|All Files|*.*")]
        [TestCase(null, "ANYTHING", "All Files|*.*")]
        [TestCase(".bat", null, "|*.bat|All Files|*.*")]
        public void Save_CorrectlySetsFilter(string suggestedFileExtension, string suggestedFileExtensionName, string expectedResult)
        {
            Given_ShimSaveFileDialog_ShowDialog_LogsFilterAndReturnsFalse();

            bool? result = null;

            PoseContext.Isolate(() =>
            {
                result = subject.Save("content", suggestedFileExtension, suggestedFileExtensionName, "fileName");
            }, shimSaveFileDialog_ShowDialog_Returns);

            Assert.IsFalse(result);
            Assert.AreEqual($"Filter: {expectedResult}\r\n", consoleOutput.ToString());
        }

        private void Given_ShimSaveFileDialog_ShowDialog_Returns(bool? value)
        {
            shimSaveFileDialog_ShowDialog_Returns = Shim.Replace(() => Pose.Is.A<WinSaveFileDialog>().ShowDialog())
                                                        .With((WinSaveFileDialog thing) => Method());

            bool? Method()
            {
                return value;
            }
        }

        /// <exception cref="IOException">Ignore.</exception>
        private void Given_ShimSaveFileDialog_ShowDialog_LogsFileNameAndReturnsFalse()
        {
            shimSaveFileDialog_ShowDialog_Returns = Shim.Replace(() => Pose.Is.A<WinSaveFileDialog>().ShowDialog())
                                                        .With((WinSaveFileDialog thing) => Method(thing));

            bool? Method(WinSaveFileDialog thing)
            {
                Console.WriteLine($"Filename: {thing.FileName}");
                return false;
            }
        }

        /// <exception cref="IOException">Ignore.</exception>
        private void Given_ShimSaveFileDialog_ShowDialog_LogsFileExtensionAndReturnsFalse()
        {
            shimSaveFileDialog_ShowDialog_Returns = Shim.Replace(() => Pose.Is.A<WinSaveFileDialog>().ShowDialog())
                                                        .With((WinSaveFileDialog thing) => Method(thing));

            bool? Method(WinSaveFileDialog thing)
            {
                Console.WriteLine($"File extension: {thing.DefaultExt}");
                return false;
            }
        }

        /// <exception cref="IOException">Ignore.</exception>
        private void Given_ShimSaveFileDialog_ShowDialog_LogsFilterAndReturnsFalse()
        {
            shimSaveFileDialog_ShowDialog_Returns = Shim.Replace(() => Pose.Is.A<WinSaveFileDialog>().ShowDialog())
                                                        .With((WinSaveFileDialog thing) => Method(thing));

            bool? Method(WinSaveFileDialog thing)
            {
                Console.WriteLine($"Filter: {thing.Filter}");
                return false;
            }
        }

        private void Given_ShimSaveFileDialog_FileName_Gets(string value)
        {
            shimSaveFileDialog_FileName_Gets = Shim.Replace(() => Pose.Is.A<WinSaveFileDialog>().FileName)
                                                        .With((WinSaveFileDialog thing) => value);
        }

        /// <exception cref="System.IO.PathTooLongException">Ignore.</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">Ignore.</exception>
        /// <exception cref="System.IO.IOException">Ignore.</exception>
        /// <exception cref="UnauthorizedAccessException">Ignore.</exception>
        /// <exception cref="System.Security.SecurityException">Ignore.</exception>
        private void Given_Shim_File_WriteAllText_DoesNothing()
        {
            shim_File_WriteAllText_DoesNothing = Shim.Replace(() => File.WriteAllText(Pose.Is.A<string>(), Pose.Is.A<string>()))
                                                     .With((string path, string contents) => Method(path, contents));

            void Method(string path, string contents)
            {
                Console.WriteLine($"Saving {contents} to {path}");
            }
        }
    }
}