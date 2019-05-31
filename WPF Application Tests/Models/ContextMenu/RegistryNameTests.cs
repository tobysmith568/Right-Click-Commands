using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Right_Click_Commands.WPF.Models.ContextMenu.Tests
{
    [TestFixture]
    public class RegistryNameTests
    {
        private RegistryName? subject;

        [SetUp]
        public void Setup()
        {
            subject = null;
        }

        #region Constructor

        [Test]
        public void Constructor_SetsProperties()
        {
            string id = "ID";
            string name = "Name";

            subject = new RegistryName(id, name);

            Assert.IsTrue(subject.HasValue);
            Assert.AreEqual(id, subject.Value.ID);
            Assert.AreEqual(name, subject.Value.Name);
        }

        #endregion
    }
}
