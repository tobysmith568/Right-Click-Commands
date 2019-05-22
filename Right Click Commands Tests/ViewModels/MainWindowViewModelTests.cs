using IconPicker;
using Moq;
using NUnit.Framework;
using Right_Click_Commands.Models.ContextMenu;
using Right_Click_Commands.Models.MessagePrompts;
using Right_Click_Commands.Models.Scripts;
using Right_Click_Commands.Models.Settings;
using Right_Click_Commands.Models.Updater;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace Right_Click_Commands.ViewModels.Tests
{
    [TestFixture]
    public class MainWindowViewModelTests
    {
        MainWindowViewModel subject;
        Mock<IContextMenuWorker> contextMenuWorker;
        Mock<ISettings> settings;
        Mock<IUpdater> updater;
        Mock<IIconPicker> iconPicker;
        Mock<IMessagePrompt> messagePrompt;
        Mock<IScriptConfig> mockScriptOne;
        Mock<IScriptConfig> mockScriptTwo;
        Mock<IScriptConfig> mockScriptThree;

        List<Mock<IScriptConfig>> mockConfigs;
        ObservableCollection<IScriptConfig> configs;

        [SetUp]
        public void SetUp()
        {
            contextMenuWorker = new Mock<IContextMenuWorker>();

            settings = new Mock<ISettings>();
            settings.Setup(s => s.ScriptLocation).Returns(Path.Combine(TestContext.CurrentContext.TestDirectory, "testDir"));

            messagePrompt = new Mock<IMessagePrompt>();

            updater = new Mock<IUpdater>();

            iconPicker = new Mock<IIconPicker>();

            mockScriptOne = new Mock<IScriptConfig>();
            mockScriptOne.Setup(m => m.Label).Returns("One");
            mockScriptTwo = new Mock<IScriptConfig>();
            mockScriptTwo.Setup(m => m.Label).Returns("Two");
            mockScriptThree = new Mock<IScriptConfig>();
            mockScriptThree.Setup(m => m.Label).Returns("Three");

            mockConfigs = new List<Mock<IScriptConfig>>
            {
                mockScriptOne,
                mockScriptTwo,
                mockScriptThree
            };

            configs = new ObservableCollection<IScriptConfig>
            {
                mockScriptOne.Object,
                mockScriptTwo.Object,
                mockScriptThree.Object
            };
            contextMenuWorker.Setup(c => c.GetScriptConfigs()).Returns(configs);

            subject = new MainWindowViewModel(contextMenuWorker.Object, settings.Object, messagePrompt.Object, updater.Object, iconPicker.Object)
            {
                SelectedScriptConfigIndex = 1
            };
        }

        #region Constructor

        [Test]
        public void Test_Constructor_ThrowsArgumentNullOnNullContextMenuWorker()
        {
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => new MainWindowViewModel(null, settings.Object, messagePrompt.Object, updater.Object, iconPicker.Object));
            Assert.AreEqual("contextMenuWorker", ex.ParamName);
        }

        [Test]
        public void Test_Constructor_ThrowsArgumentNullOnNullSettings()
        {
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => new MainWindowViewModel(contextMenuWorker.Object, null, messagePrompt.Object, updater.Object, iconPicker.Object));
            Assert.AreEqual("settings", ex.ParamName);
        }

        [Test]
        public void Test_Constructor_ThrowsArgumentNullOnNullMessagePrompt()
        {
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => new MainWindowViewModel(contextMenuWorker.Object, settings.Object, null, updater.Object, iconPicker.Object));
            Assert.AreEqual("messagePrompt", ex.ParamName);
        }

        [Test]
        public void Test_Constructor_ThrowsArgumentNullOnNullUpdater()
        {
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => new MainWindowViewModel(contextMenuWorker.Object, settings.Object, messagePrompt.Object, null, iconPicker.Object));
            Assert.AreEqual("updater", ex.ParamName);
        }

        [Test]
        public void Test_Constructor_ThrowsArgumentNullOnNullIconPicker()
        {
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => new MainWindowViewModel(contextMenuWorker.Object, settings.Object, messagePrompt.Object, updater.Object, null));
            Assert.AreEqual("iconPicker", ex.ParamName);
        }

        #endregion
        #region ViewFullyLoaded

        /// <exception cref="Exception"></exception>
        [Test]
        public void Test_ViewFullyLoaded_DoesNotUpdateWithNullAsset()
        {
            Given_Updater_CheckForUpdateAsync_Returns(null);

            subject.ViewFullyLoaded.DoExecute(null);

            updater.Verify(u => u.UpdateTo(It.IsAny<Asset>()), Times.Never);
        }

        /// <exception cref="Exception"></exception>
        [Test]
        public void Test_ViewFullyLoaded_UpdatesToAnyGivenAsset()
        {
            Asset asset = new Asset();
            Given_Updater_CheckForUpdateAsync_Returns(asset);

            subject.ViewFullyLoaded.DoExecute(null);

            updater.Verify(u => u.UpdateTo(asset), Times.Once);
        }

        #endregion
        #region WindowCloseComamnd

        /// <exception cref="Exception"></exception>
        [Test]
        public void Test_WindowCloseCommand_SavesAllServices()
        {
            subject.WindowCloseCommand.DoExecute(null);

            settings.Verify(s => s.SaveAll(), Times.Once);
            mockScriptOne.Verify(s => s.SaveScript(), Times.Once);
            mockScriptTwo.Verify(s => s.SaveScript(), Times.Once);
            mockScriptThree.Verify(s => s.SaveScript(), Times.Once);
            contextMenuWorker.Verify(c => c.SaveScriptConfigs(configs), Times.Once);
        }

        #endregion
        #region CreateNewScript

        [Test]
        public void Test_CreateNewScript_AddsNewScript()
        {
            int scriptCount = configs.Count;

            subject.CreateNewScript.DoExecute(null);

            Assert.AreEqual(scriptCount + 1, subject.ScriptConfigs.Count);
        }

        [Test]
        public void Test_CreateNewScript_SelectsLastScript()
        {
            int scriptCount = configs.Count;

            subject.CreateNewScript.DoExecute(null);

            Assert.AreEqual(scriptCount, subject.SelectedScriptConfigIndex);
        }

        #endregion
        #region MoveSelectedUp

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-50)]
        public void Test_MoveSelectedUp_MovesNothingWithAnIndexOfZeroOrLower(int index)
        {
            Given_All3ScriptConfigsAreInOrder();
            Given_SelectedScriptConfigIndex_Equals(index);

            subject.MoveSelectedUp.DoExecute(null);

            Assert.AreEqual(3, subject.ScriptConfigs.Count);
            Assert.AreSame(mockScriptOne.Object, subject.ScriptConfigs[0]);
            Assert.AreSame(mockScriptTwo.Object, subject.ScriptConfigs[1]);
            Assert.AreSame(mockScriptThree.Object, subject.ScriptConfigs[2]);
        }

        [TestCase(-2)]
        [TestCase(-3)]
        [TestCase(-50)]
        public void Test_MoveSelectedUp_ResetsTheSelectedIndexWhenTheSelectedIndexIsMinus2OrLower(int index)
        {
            Given_SelectedScriptConfigIndex_Equals(index);

            subject.MoveSelectedUp.DoExecute(null);

            Assert.AreEqual(-1, subject.SelectedScriptConfigIndex);
        }

        [TestCase(1)]
        [TestCase(2)]
        public void Test_MoveSelectedUp_MovesScriptsWithAValidIndex(int index)
        {
            IScriptConfig configAtIndex = subject.ScriptConfigs[index];

            Given_SelectedScriptConfigIndex_Equals(index);

            subject.MoveSelectedUp.DoExecute(null);

            Assert.AreSame(configAtIndex, subject.ScriptConfigs[index - 1]);
        }

        [TestCase(1)]
        [TestCase(2)]
        public void Test_MoveSelectedUp_MovesSelectedScriptWithAValidIndex(int index)
        {
            Given_SelectedScriptConfigIndex_Equals(index);

            subject.MoveSelectedUp.DoExecute(null);

            Assert.AreEqual(index - 1, subject.SelectedScriptConfigIndex);
        }

        [TestCase(3)]
        [TestCase(4)]
        [TestCase(10)]
        public void Test_MoveSelectedUp_MovesNothingWhenTheSelectedIndexIsTooHigh(int index)
        {
            Given_All3ScriptConfigsAreInOrder();
            Given_SelectedScriptConfigIndex_Equals(index);

            subject.MoveSelectedUp.DoExecute(null);

            Assert.AreEqual(3, subject.ScriptConfigs.Count);
            Assert.AreSame(mockScriptOne.Object, subject.ScriptConfigs[0]);
            Assert.AreSame(mockScriptTwo.Object, subject.ScriptConfigs[1]);
            Assert.AreSame(mockScriptThree.Object, subject.ScriptConfigs[2]);
        }

        [TestCase(3)]
        [TestCase(4)]
        [TestCase(10)]
        public void Test_MoveSelectedUp_ResetsTheSelectedIndexWhenTheSelectedIndexIsTooHigh(int index)
        {
            Given_SelectedScriptConfigIndex_Equals(index);

            subject.MoveSelectedUp.DoExecute(null);

            Assert.AreEqual(-1, subject.SelectedScriptConfigIndex);
        }

        #endregion
        #region MoveSelectedDown

        [TestCase(-1)]
        [TestCase(-2)]
        [TestCase(-50)]
        public void Test_MoveSelectedDown_MovesNothingWithAnIndexOfMinusOneLower(int index)
        {
            Given_All3ScriptConfigsAreInOrder();
            Given_SelectedScriptConfigIndex_Equals(index);

            subject.MoveSelectedDown.DoExecute(null);

            Assert.AreEqual(3, subject.ScriptConfigs.Count);
            Assert.AreSame(mockScriptOne.Object, subject.ScriptConfigs[0]);
            Assert.AreSame(mockScriptTwo.Object, subject.ScriptConfigs[1]);
            Assert.AreSame(mockScriptThree.Object, subject.ScriptConfigs[2]);
        }

        [TestCase(-2)]
        [TestCase(-3)]
        [TestCase(-50)]
        public void Test_MoveSelectedDown_ResetsTheSelectedIndexWhenTheSelectedIndexIsMinusTwoOrLower(int index)
        {
            Given_SelectedScriptConfigIndex_Equals(index);

            subject.MoveSelectedDown.DoExecute(null);

            Assert.AreEqual(-1, subject.SelectedScriptConfigIndex);
        }

        [TestCase(0)]
        [TestCase(1)]
        public void Test_MoveSelectedDown_MovesScriptsWithAValidIndex(int index)
        {
            IScriptConfig configAtIndex = subject.ScriptConfigs[index];
            Given_SelectedScriptConfigIndex_Equals(index);

            subject.MoveSelectedDown.DoExecute(null);

            Assert.AreSame(configAtIndex, subject.ScriptConfigs[index + 1]);
        }

        [TestCase(0)]
        [TestCase(1)]
        public void Test_MoveSelectedDown_MovesSelectedScriptWithAValidIndex(int index)
        {
            Given_SelectedScriptConfigIndex_Equals(index);

            subject.MoveSelectedDown.DoExecute(null);

            Assert.AreEqual(index + 1, subject.SelectedScriptConfigIndex);
        }

        [TestCase(2)]
        [TestCase(3)]
        [TestCase(10)]
        public void Test_MoveSelectedDown_MovesNothingWhenTheSelectedIndexIsTooHigh(int index)
        {
            Given_All3ScriptConfigsAreInOrder();
            Given_SelectedScriptConfigIndex_Equals(index);

            subject.MoveSelectedDown.DoExecute(null);

            Assert.AreEqual(3, subject.ScriptConfigs.Count);
            Assert.AreSame(mockScriptOne.Object, subject.ScriptConfigs[0]);
            Assert.AreSame(mockScriptTwo.Object, subject.ScriptConfigs[1]);
            Assert.AreSame(mockScriptThree.Object, subject.ScriptConfigs[2]);
        }

        [TestCase(2)]
        [TestCase(3)]
        [TestCase(10)]
        public void Test_MoveSelectedDown_ResetsTheSelectedIndexWhenTheSelectedIndexIsTooHigh(int index)
        {
            Given_SelectedScriptConfigIndex_Equals(index);

            subject.MoveSelectedDown.DoExecute(null);

            Assert.AreEqual(-1, subject.SelectedScriptConfigIndex);
        }

        #endregion
        #region DeleteSelected

        [TestCase(50)]
        [TestCase(4)]
        [TestCase(3)]
        [TestCase(-1)]
        [TestCase(-2)]
        [TestCase(-50)]
        public void Test_DeleteSelected_DeletesNothingWithAnIndexOutOfRange(int index)
        {
            Given_All3ScriptConfigsAreInOrder();
            Given_SelectedScriptConfigIndex_Equals(index);

            subject.DeleteSelected.DoExecute(null);

            Assert.AreEqual(3, subject.ScriptConfigs.Count);
            Assert.AreSame(mockScriptOne.Object, subject.ScriptConfigs[0]);
            Assert.AreSame(mockScriptTwo.Object, subject.ScriptConfigs[1]);
            Assert.AreSame(mockScriptThree.Object, subject.ScriptConfigs[2]);
        }

        [TestCase(50)]
        [TestCase(4)]
        [TestCase(3)]
        [TestCase(-2)]
        [TestCase(-3)]
        [TestCase(-50)]
        public void Test_DeleteSelected_ResetsTheSelectedIndexWhenTheSelectedIndexIsOutOfRage(int index)
        {
            Given_SelectedScriptConfigIndex_Equals(index);

            subject.DeleteSelected.DoExecute(null);

            Assert.AreEqual(-1, subject.SelectedScriptConfigIndex);
        }

        [Test]
        public void Test_DeleteSelected_DeletesNothingIfConfirmationBoxReturnsNo()
        {
            Given_All3ScriptConfigsAreInOrder();
            Given_MessagePrompt_PromptYesNo_Returns(MessageResult.No);

            subject.DeleteSelected.DoExecute(null);

            Assert.AreEqual(3, subject.ScriptConfigs.Count);
            Assert.AreSame(mockScriptOne.Object, subject.ScriptConfigs[0]);
            Assert.AreSame(mockScriptTwo.Object, subject.ScriptConfigs[1]);
            Assert.AreSame(mockScriptThree.Object, subject.ScriptConfigs[2]);
        }

        [Test]
        public void Test_DeleteSelected_DeletesWhenGivenAValidSelectedIndex()
        {
            Given_All3ScriptConfigsAreInOrder();
            Given_MessagePrompt_PromptYesNo_Returns(MessageResult.Yes);
            Given_SelectedScriptConfigIndex_Equals(1);

            subject.DeleteSelected.DoExecute(null);

            Assert.AreEqual(2, subject.ScriptConfigs.Count);
            Assert.AreSame(mockScriptOne.Object, subject.ScriptConfigs[0]);
            Assert.AreSame(mockScriptThree.Object, subject.ScriptConfigs[1]);
        }

        [TestCase(0)]
        [TestCase(1)]
        public void Test_DeleteSelected_KeepsTheSameSelectedIndexWhenItDeletesAnythingButTheLastIndex(int index)
        {
            Given_SelectedScriptConfigIndex_Equals(index);

            subject.DeleteSelected.DoExecute(null);

            Assert.AreEqual(index, subject.SelectedScriptConfigIndex);
        }

        [Test]
        public void Test_DeleteSelected_SelectsTheNewLastIndexWhenItDeletesTheLastIndex()
        {
            int originalIndex = subject.ScriptConfigs.Count - 1;
            Given_SelectedScriptConfigIndex_Equals(originalIndex);

            subject.DeleteSelected.DoExecute(null);

            Assert.AreEqual(originalIndex - 1, subject.SelectedScriptConfigIndex);
        }

        #endregion
        #region Select New Icon

        [TestCase(50)]
        [TestCase(4)]
        [TestCase(3)]
        [TestCase(-1)]
        [TestCase(-2)]
        [TestCase(-50)]
        public void Test_SelectNewIcon_DoesNothingWithAnIndexOutOfRange(int index)
        {
            Given_All3ScriptConfigsAreInOrder();
            Given_SelectedScriptConfigIndex_Equals(index);

            subject.SelectNewIcon.DoExecute(null);

            Assert.AreEqual(3, subject.ScriptConfigs.Count);
            Assert.AreSame(mockScriptOne.Object, subject.ScriptConfigs[0]);
            Assert.AreSame(mockScriptTwo.Object, subject.ScriptConfigs[1]);
            Assert.AreSame(mockScriptThree.Object, subject.ScriptConfigs[2]);
        }

        [TestCase(50)]
        [TestCase(4)]
        [TestCase(3)]
        [TestCase(-2)]
        [TestCase(-3)]
        [TestCase(-50)]
        public void Test_SelectNewIcon_ResetsTheSelectedIndexWhenTheSelectedIndexIsOutOfRage(int index)
        {
            Given_SelectedScriptConfigIndex_Equals(index);

            subject.SelectNewIcon.DoExecute(null);

            Assert.AreEqual(-1, subject.SelectedScriptConfigIndex);
        }

        [Test]
        public void Test_SelectNewIcon_DoesNothingIfIconPickerDialogIsCancelled()
        {
            Given_All3ScriptConfigsAreInOrder();
            Given_SelectedScriptConfigIndex_Equals(1);
            Given_IconPicker_SelectIconReference_Returns(null);

            subject.SelectNewIcon.DoExecute(null);

            Assert.AreEqual(3, subject.ScriptConfigs.Count);
            Assert.AreSame(mockScriptOne.Object, subject.ScriptConfigs[0]);
            Assert.AreSame(mockScriptTwo.Object, subject.ScriptConfigs[1]);
            Assert.AreSame(mockScriptThree.Object, subject.ScriptConfigs[2]);
        }

        /// <exception cref="MockException">Ignore.</exception>
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void Test_SelectNewIcon_OverwritesIconReferenceIfDialogDoesnotReturnNull(int index)
        {
            IconReference iconReference = new IconReference("filepath", 7);

            Given_All3ScriptConfigsAreInOrder();
            Given_IconPicker_SelectIconReference_Returns(iconReference);
            Given_SelectedScriptConfigIndex_Equals(index);

            subject.SelectNewIcon.DoExecute(null);

            mockConfigs[index].VerifySet(m => m.Icon = iconReference);
        }

        /// <exception cref="MockException"></exception>
        [Test]
        public void Test_SelectNewIcon_PromptsUserAboutRemovingTheIconIfThereAlreadyisOne()
        {
            IconReference iconReference = new IconReference("filepath", 7);

            Given_All3ScriptConfigsAreInOrder();
            Given_SelectedScriptConfigIndex_Equals(0);
            Given_ScriptConfigs_IconReferences_Equal(iconReference);
            Given_MessagePrompt_PromptYesNo_IsVerifiable();

            subject.SelectNewIcon.DoExecute(null);

            messagePrompt.Verify(m => m.PromptYesNo("Are you sure you want to remove the icon from One?", "Are you sure?", MessageType.Warning), Times.Once);
        }

        /// <exception cref="MockException"></exception>
        [Test]
        public void Test_SelectNewIcon_RemovesTheIconIfThereAlreadyisOneAndThePromptReturnsYes()
        {
            IconReference iconReference = new IconReference("filepath", 7);

            Given_All3ScriptConfigsAreInOrder();
            Given_SelectedScriptConfigIndex_Equals(0);
            Given_ScriptConfigs_IconReferences_Equal(iconReference);
            Given_MessagePrompt_PromptYesNo_Returns(MessageResult.Yes);

            subject.SelectNewIcon.DoExecute(null);

            mockScriptOne.VerifySet(m => m.Icon = null, Times.Once);
        }

        /// <exception cref="MockException"></exception>
        [Test]
        public void Test_SelectNewIcon_DoesNotRemoveTheIconIfThereAlreadyisOneButThePromptReturnsNo()
        {
            IconReference iconReference = new IconReference("filepath", 7);

            Given_All3ScriptConfigsAreInOrder();
            Given_SelectedScriptConfigIndex_Equals(0);
            Given_ScriptConfigs_IconReferences_Equal(iconReference);
            Given_MessagePrompt_PromptYesNo_Returns(MessageResult.No);

            subject.SelectNewIcon.DoExecute(null);

            mockScriptOne.VerifySet(m => m.Icon = null, Times.Never);
        }

        #endregion

        private void Given_Updater_CheckForUpdateAsync_Returns(Asset result)
        {
            updater.Setup(u => u.CheckForUpdateAsync()).ReturnsAsync(result);
        }

        private void Given_All3ScriptConfigsAreInOrder()
        {
            Assert.AreEqual(3, subject.ScriptConfigs.Count);
            Assert.AreSame(mockScriptOne.Object, subject.ScriptConfigs[0]);
            Assert.AreSame(mockScriptTwo.Object, subject.ScriptConfigs[1]);
            Assert.AreSame(mockScriptThree.Object, subject.ScriptConfigs[2]);
        }

        private void Given_SelectedScriptConfigIndex_Equals(int value)
        {
            subject.SelectedScriptConfigIndex = value;
        }

        private void Given_MessagePrompt_PromptYesNo_Returns(MessageResult result)
        {
            messagePrompt.Setup(m => m.PromptYesNo(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<MessageType>())).Returns(result);
        }

        private void Given_IconPicker_SelectIconReference_Returns(IconReference result)
        {
            iconPicker.Setup(i => i.SelectIconReference()).Returns(result);
        }

        private void Given_ScriptConfigs_IconReferences_Equal(IconReference iconReference)
        {
            Assert.AreEqual(3, subject.ScriptConfigs.Count);
            mockScriptOne.Setup(m => m.Icon).Returns(iconReference);
            mockScriptTwo.Setup(m => m.Icon).Returns(iconReference);
            mockScriptThree.Setup(m => m.Icon).Returns(iconReference);
        }

        private void Given_MessagePrompt_PromptYesNo_IsVerifiable()
        {
            messagePrompt.Setup(m => m.PromptYesNo(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<MessageType>())).Verifiable();
        }
    }
}