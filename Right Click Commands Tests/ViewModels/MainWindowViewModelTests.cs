using Moq;
using NUnit.Framework;
using Right_Click_Commands.Models.ContextMenu;
using Right_Click_Commands.Models.MessagePrompts;
using Right_Click_Commands.Models.Scripts;
using Right_Click_Commands.Models.Settings;
using Right_Click_Commands.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Right_Click_Commands.ViewModels.Tests
{
    [TestFixture]
    public class MainWindowViewModelTests
    {
        MainWindowViewModel subject;
        Mock<IContextMenuWorker> contextMenuWorker;
        Mock<ISettings> settings;
        Mock<IMessagePrompt> messagePrompt;
        Mock<ScriptConfig> mockScriptOne;
        Mock<ScriptConfig> mockScriptTwo;
        Mock<ScriptConfig> mockScriptThree;

        ObservableCollection<ScriptConfig> configs;

        [SetUp]
        public void SetUp()
        {
            contextMenuWorker = new Mock<IContextMenuWorker>();
            settings = new Mock<ISettings>();
            messagePrompt = new Mock<IMessagePrompt>();

            mockScriptOne = new Mock<ScriptConfig>();
            mockScriptTwo = new Mock<ScriptConfig>();
            mockScriptThree = new Mock<ScriptConfig>();

            configs = new ObservableCollection<ScriptConfig>
            {
                mockScriptOne.Object,
                mockScriptTwo.Object,
                mockScriptThree.Object
            };
            contextMenuWorker.Setup(c => c.GetScriptConfigs()).Returns(configs);

            subject = new MainWindowViewModel(contextMenuWorker.Object, settings.Object, messagePrompt.Object)
            {
                SelectedScriptConfigIndex = 1
            };
        }

        #region WindowCloseComamnd

        /// <exception cref="Exception"></exception>
        [Test]
        public void Test_WindowCloseCommand_SavesAllServices()
        {
            subject.WindowCloseCommand.DoExecute(null);

            settings.Verify(s => s.SaveAll(), Times.Once);
            mockScriptOne.Verify(s => s.SaveScript(), Times.Once);
            mockScriptOne.Verify(s => s.SaveScript(), Times.Once);
            mockScriptOne.Verify(s => s.SaveScript(), Times.Once);
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
            Assert.AreEqual(3, subject.ScriptConfigs.Count);
            Assert.AreSame(mockScriptOne.Object, subject.ScriptConfigs[0]);
            Assert.AreSame(mockScriptTwo.Object, subject.ScriptConfigs[1]);
            Assert.AreSame(mockScriptThree.Object, subject.ScriptConfigs[2]);

            subject.SelectedScriptConfigIndex = index;
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
            subject.SelectedScriptConfigIndex = index;

            subject.MoveSelectedUp.DoExecute(null);

            Assert.AreEqual(-1, subject.SelectedScriptConfigIndex);
        }

        [TestCase(1)]
        [TestCase(2)]
        public void Test_MoveSelectedUp_MovesScriptsWithAValidIndex(int index)
        {
            ScriptConfig configAtIndex = subject.ScriptConfigs[index];

            subject.SelectedScriptConfigIndex = index;
            subject.MoveSelectedUp.DoExecute(null);

            Assert.AreSame(configAtIndex, subject.ScriptConfigs[index - 1]);
        }

        [TestCase(1)]
        [TestCase(2)]
        public void Test_MoveSelectedUp_MovesSelectedScriptWithAValidIndex(int index)
        {
            subject.SelectedScriptConfigIndex = index;

            subject.MoveSelectedUp.DoExecute(null);

            Assert.AreEqual(index - 1, subject.SelectedScriptConfigIndex);
        }

        [TestCase(3)]
        [TestCase(4)]
        [TestCase(10)]
        public void Test_MoveSelectedUp_MovesNothingWhenTheSelectedIndexIsTooHigh(int index)
        {
            Assert.AreEqual(3, subject.ScriptConfigs.Count);
            Assert.AreSame(mockScriptOne.Object, subject.ScriptConfigs[0]);
            Assert.AreSame(mockScriptTwo.Object, subject.ScriptConfigs[1]);
            Assert.AreSame(mockScriptThree.Object, subject.ScriptConfigs[2]);

            subject.SelectedScriptConfigIndex = index;
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
            subject.SelectedScriptConfigIndex = index;

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
            Assert.AreEqual(3, subject.ScriptConfigs.Count);
            Assert.AreSame(mockScriptOne.Object, subject.ScriptConfigs[0]);
            Assert.AreSame(mockScriptTwo.Object, subject.ScriptConfigs[1]);
            Assert.AreSame(mockScriptThree.Object, subject.ScriptConfigs[2]);

            subject.SelectedScriptConfigIndex = index;
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
            subject.SelectedScriptConfigIndex = index;

            subject.MoveSelectedDown.DoExecute(null);

            Assert.AreEqual(-1, subject.SelectedScriptConfigIndex);
        }

        [TestCase(0)]
        [TestCase(1)]
        public void Test_MoveSelectedDown_MovesScriptsWithAValidIndex(int index)
        {
            ScriptConfig configAtIndex = subject.ScriptConfigs[index];

            subject.SelectedScriptConfigIndex = index;
            subject.MoveSelectedDown.DoExecute(null);

            Assert.AreSame(configAtIndex, subject.ScriptConfigs[index + 1]);
        }

        [TestCase(0)]
        [TestCase(1)]
        public void Test_MoveSelectedDown_MovesSelectedScriptWithAValidIndex(int index)
        {
            subject.SelectedScriptConfigIndex = index;

            subject.MoveSelectedDown.DoExecute(null);

            Assert.AreEqual(index + 1, subject.SelectedScriptConfigIndex);
        }

        [TestCase(2)]
        [TestCase(3)]
        [TestCase(10)]
        public void Test_MoveSelectedDown_MovesNothingWhenTheSelectedIndexIsTooHigh(int index)
        {
            Assert.AreEqual(3, subject.ScriptConfigs.Count);
            Assert.AreSame(mockScriptOne.Object, subject.ScriptConfigs[0]);
            Assert.AreSame(mockScriptTwo.Object, subject.ScriptConfigs[1]);
            Assert.AreSame(mockScriptThree.Object, subject.ScriptConfigs[2]);

            subject.SelectedScriptConfigIndex = index;
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
            subject.SelectedScriptConfigIndex = index;

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
        public void Test_DeleteSelected_MovesNothingWithAnIndexOutOfRange(int index)
        {
            Assert.AreEqual(3, subject.ScriptConfigs.Count);
            Assert.AreSame(mockScriptOne.Object, subject.ScriptConfigs[0]);
            Assert.AreSame(mockScriptTwo.Object, subject.ScriptConfigs[1]);
            Assert.AreSame(mockScriptThree.Object, subject.ScriptConfigs[2]);

            subject.SelectedScriptConfigIndex = index;
            subject.MoveSelectedDown.DoExecute(null);

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
            subject.SelectedScriptConfigIndex = index;

            subject.DeleteSelected.DoExecute(null);

            Assert.AreEqual(-1, subject.SelectedScriptConfigIndex);
        }

        [Test]
        public void Test_DeleteSelected_MovesNothingIfConfirmationBoxReturnsNo()
        {
            Assert.AreEqual(3, subject.ScriptConfigs.Count);
            Assert.AreSame(mockScriptOne.Object, subject.ScriptConfigs[0]);
            Assert.AreSame(mockScriptTwo.Object, subject.ScriptConfigs[1]);
            Assert.AreSame(mockScriptThree.Object, subject.ScriptConfigs[2]);

            messagePrompt.Setup(m => m.PromptYesNo(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<MessageType>())).Returns(MessageResult.No);

            subject.DeleteSelected.DoExecute(null);

            Assert.AreEqual(3, subject.ScriptConfigs.Count);
            Assert.AreSame(mockScriptOne.Object, subject.ScriptConfigs[0]);
            Assert.AreSame(mockScriptTwo.Object, subject.ScriptConfigs[1]);
            Assert.AreSame(mockScriptThree.Object, subject.ScriptConfigs[2]);
        }

        [Test]
        public void Test_DeleteSelected_DeletesWhenGivenAValidSelectedIndex()
        {
            Assert.AreEqual(3, subject.ScriptConfigs.Count);
            Assert.AreSame(mockScriptOne.Object, subject.ScriptConfigs[0]);
            Assert.AreSame(mockScriptTwo.Object, subject.ScriptConfigs[1]);
            Assert.AreSame(mockScriptThree.Object, subject.ScriptConfigs[2]);

            messagePrompt.Setup(m => m.PromptYesNo(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<MessageType>())).Returns(MessageResult.Yes);

            subject.SelectedScriptConfigIndex = 1;
            subject.DeleteSelected.DoExecute(null);

            Assert.AreEqual(2, subject.ScriptConfigs.Count);
            Assert.AreSame(mockScriptOne.Object, subject.ScriptConfigs[0]);
            Assert.AreSame(mockScriptThree.Object, subject.ScriptConfigs[1]);
        }

        [TestCase(0)]
        [TestCase(1)]
        public void Test_DeleteSelected_KeepsTheSameSelectedIndexWhenItDeletesAnythingButTheLastIndex(int index)
        {
            subject.SelectedScriptConfigIndex = index;

            subject.DeleteSelected.DoExecute(null);

            Assert.AreEqual(index, subject.SelectedScriptConfigIndex);
        }

        [Test]
        public void Test_DeleteSelected_SelectsTheNewLastIndexWhenItDeletesTheLastIndex()
        {
            int originalIndex = subject.ScriptConfigs.Count - 1;

            subject.SelectedScriptConfigIndex = originalIndex;
            subject.DeleteSelected.DoExecute(null);

            Assert.AreEqual(originalIndex - 1, subject.SelectedScriptConfigIndex);
        }

        #endregion
    }
}