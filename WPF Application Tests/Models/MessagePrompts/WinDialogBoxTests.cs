using NUnit.Framework;
using Right_Click_Commands.WPF.Models.MessagePrompts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pose;
using Is = Pose.Is;
using System.Windows;
using Right_Click_Commands.Models.MessagePrompts;

namespace Right_Click_Commands.WPF.Models.MessagePrompts.Tests
{
    [TestFixture]
    public class WinDialogBoxTests
    {
        private WinDialogBox subject;

        [SetUp]
        public void SetUp()
        {
            subject = new WinDialogBox();
        }

        [Test]
        public void PromptOK_Returns_Nothing()
        {
            Shim shim = Given_MessageBox_Show_Returns(MessageBoxResult.OK);

            PoseContext.Isolate(() =>
            {
                subject.PromptOK("content", "title");
            }, shim);
        }

        [TestCase(MessageBoxResult.Cancel, MessageResult.Cancel)]
        [TestCase(MessageBoxResult.No, MessageResult.Cancel)]
        [TestCase(MessageBoxResult.None, MessageResult.Cancel)]
        [TestCase(MessageBoxResult.OK, MessageResult.OK)]
        [TestCase(MessageBoxResult.Yes, MessageResult.Cancel)]
        public void PromptPromptOKCancel_ReturnsCorrectValue(MessageBoxResult messageBoxResult, MessageResult expectedResult)
        {
            Shim shim = Given_MessageBox_Show_Returns(messageBoxResult);

            MessageResult? actualResult = null;

            PoseContext.Isolate(() =>
            {
                actualResult = subject.PromptOKCancel("content", "title");
            }, shim);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestCase(MessageBoxResult.Cancel, MessageResult.No)]
        [TestCase(MessageBoxResult.No, MessageResult.No)]
        [TestCase(MessageBoxResult.None, MessageResult.No)]
        [TestCase(MessageBoxResult.OK, MessageResult.No)]
        [TestCase(MessageBoxResult.Yes, MessageResult.Yes)]
        public void PromptPromptYesNo_ReturnsCorrectValue(MessageBoxResult messageBoxResult, MessageResult expectedResult)
        {
            Shim shim = Given_MessageBox_Show_Returns(messageBoxResult);

            MessageResult? actualResult = null;

            PoseContext.Isolate(() =>
            {
                actualResult = subject.PromptYesNo("content", "title");
            }, shim);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestCase(MessageBoxResult.Cancel, MessageResult.Cancel)]
        [TestCase(MessageBoxResult.No, MessageResult.No)]
        [TestCase(MessageBoxResult.None, MessageResult.Cancel)]
        [TestCase(MessageBoxResult.OK, MessageResult.Cancel)]
        [TestCase(MessageBoxResult.Yes, MessageResult.Yes)]
        public void PromptPromptYesNoCancel_ReturnsCorrectValue(MessageBoxResult messageBoxResult, MessageResult expectedResult)
        {
            Shim shim = Given_MessageBox_Show_Returns(messageBoxResult);

            MessageResult? actualResult = null;

            PoseContext.Isolate(() =>
            {
                actualResult = subject.PromptYesNoCancel("content", "title");
            }, shim);

            Assert.AreEqual(expectedResult, actualResult);
        }

        private Shim Given_MessageBox_Show_Returns(MessageBoxResult wantedResult)
        {
            return Shim.Replace(() => MessageBox.Show(Is.A<string>(), Is.A<string>(), Is.A<MessageBoxButton>(), Is.A<MessageBoxImage>(), Is.A<MessageBoxResult>()))
                .With((string content, string title, MessageBoxButton button, MessageBoxImage image, MessageBoxResult result) => NewAction(content, title, button, image, result));

            MessageBoxResult NewAction(string passedMessage, string passedTitle, MessageBoxButton passedButton, MessageBoxImage PassedImage, MessageBoxResult passedResult)
            {
                return wantedResult;
            }
        }
    }
}