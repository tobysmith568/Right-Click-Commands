using NUnit.Framework;
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
    [TestFixture]
    public class ScriptConfigUtilsTests
    {
        ObservableCollection<ScriptConfig> subject;

        Mock<ScriptConfig> first;
        Mock<ScriptConfig> second;
        Mock<ScriptConfig> third;

        [SetUp]
        public void SetUp()
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

        [Test]
        public void Test_MoveUpOne_AbleToMove()
        {
            subject.MoveUpOne(1);

            Assert.AreSame(second.Object, subject[0]);
        }

        [Test]
        public void Test_MoveUpOne_NegativeIndex()
        {
            Exception e = Assert.Throws<ArgumentException>(() => subject.MoveUpOne(-4));
            Assert.AreEqual("The given index must be greater than 0", e.Message);
        }

        [Test]
        public void Test_MoveUpOne_AlreadyFirstIndex()
        {
            Exception e = Assert.Throws<ArgumentException>(() => subject.MoveUpOne(0));
            Assert.AreEqual("The given index must be greater than 0", e.Message);
        }

        [Test]
        public void Test_MoveUpOne_PositiveIndexOutOfRange()
        {
            Exception e = Assert.Throws<ArgumentException>(() => subject.MoveUpOne(4));
            Assert.AreEqual("The given index must not be greater than the collections size - 1", e.Message);
        }

        [Test]
        public void Test_MoveDownOne_AbleToMove()
        {
            subject.MoveDownOne(1);

            Assert.AreSame(second.Object, subject[2]);
        }

        [Test]
        public void Test_MoveDownOne_NegativeIndex()
        {
            Exception e = Assert.Throws<ArgumentException>(() => subject.MoveDownOne(-4));
            Assert.AreEqual("The given index must be greater or equal to 0", e.Message);
        }

        [Test]
        public void Test_MoveDownOne_AlreadyLastIndex()
        {
            Exception e = Assert.Throws<ArgumentException>(() => subject.MoveDownOne(2));
            Assert.AreEqual("The given index must not be greater than the collections size - 2", e.Message);
        }

        [Test]
        public void Test_MoveDownOne_PositiveIndexOutOfRange()
        {
            Exception e = Assert.Throws<ArgumentException>(() => subject.MoveDownOne(4));
            Assert.AreEqual("The given index must not be greater than the collections size - 2", e.Message);
        }

        [Test]
        public void Test_DeleteAtIndex_AbleToDelete()
        {
            subject.DeleteAtIndex(1);

            Assert.AreEqual(2, subject.Count);
            Assert.AreSame(first.Object, subject[0]);
            Assert.AreSame(third.Object, subject[1]);
        }

        [Test]
        public void Test_DeleteAtIndex_NegativeIndex()
        {
            Exception e = Assert.Throws<ArgumentException>(() => subject.DeleteAtIndex(-4));
            Assert.AreEqual("The given index must be greater or equal to 0", e.Message);
        }

        [Test]
        public void Test_DeleteAtIndex_PositiveIndexOutOfRange()
        {
            Exception e = Assert.Throws<ArgumentException>(() => subject.DeleteAtIndex(4));
            Assert.AreEqual("The given index must not be greater than the collections size - 1", e.Message);
        }
    }
}