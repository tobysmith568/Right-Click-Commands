using Microsoft.VisualStudio.TestTools.UnitTesting;
using Right_Click_Commands.Models.Scripts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace Right_Click_Commands.Models.Scripts.Tests
{
    [TestClass]
    public class ScriptConfigUtilsTests
    {
        ObservableCollection<ScriptConfig> subject;

        Mock<ScriptConfig> first;
        Mock<ScriptConfig> second;
        Mock<ScriptConfig> third;

        [TestInitialize]
        public void TestInitialize()
        {
            first = new Mock<ScriptConfig>();
            second = new Mock<ScriptConfig>();
            third = new Mock<ScriptConfig>();

            subject = new ObservableCollection<ScriptConfig>
            {
                first.Object,
                second.Object,
                third.Object
            };
        }

        [TestMethod]
        public void Test_MoveUpOne_AbleToMove()
        {
            subject.MoveUpOne(1);

            Assert.AreSame(second.Object, subject[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "The given index must be greater than zero")]
        public void Test_MoveUpOne_NegativeIndex()
        {
            subject.MoveUpOne(-4);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "The given index must not be greater than the collections size - 1")]
        public void Test_MoveUpOne_AlreadyFirstIndex()
        {
            subject.MoveUpOne(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "The given index must not be greater than the collections size - 1")]
        public void Test_MoveUpOne_PositiveIndexOutOfRange()
        {
            subject.MoveUpOne(4);
        }

        [TestMethod]
        public void Test_MoveDownOne_AbleToMove()
        {
            subject.MoveDownOne(1);

            Assert.AreSame(second.Object, subject[2]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "The given index must be greater than zero")]
        public void Test_MoveDownOne_NegativeIndex()
        {
            subject.MoveDownOne(-4);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "The given index must not be greater than the collections size - 2")]
        public void Test_MoveDownOne_AlreadyLastIndex()
        {
            subject.MoveDownOne(2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "The given index must not be greater than the collections size - 2")]
        public void Test_MoveDownOne_PositiveIndexOutOfRange()
        {
            subject.MoveDownOne(4);
        }

        [TestMethod]
        public void Test_DeleteAtIndex_AbleToDelete()
        {
            subject.DeleteAtIndex(1);

            Assert.AreEqual(2, subject.Count);
            Assert.AreSame(first.Object, subject[0]);
            Assert.AreSame(third.Object, subject[1]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "The given index must be greater than zero")]
        public void Test_DeleteAtIndex_NegativeIndex()
        {
            subject.DeleteAtIndex(-4);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "The given index must not be greater than the collections size - 1")]
        public void Test_DeleteAtIndex_PositiveIndexOutOfRange()
        {
            subject.DeleteAtIndex(4);
        }
    }
}