using NUnit.Framework;

namespace Right_Click_Commands.Models.Updater.Tests
{
    [TestFixture]
    public class AssetTests
    {
        Asset subject;

        [SetUp]
        public void SetUp()
        {
            subject = new Asset();
        }

        [Test]
        public void URL_GetSet()
        {
            string testValue = "Test Value";

            subject.URL = testValue;

            Assert.AreEqual(testValue, subject.URL);
        }

        [Test]
        public void Bytes_GetSet()
        {
            int testValue = 50;

            subject.Bytes = testValue;

            Assert.AreEqual(testValue, subject.Bytes);
        }
        
        [TestCase(-2048,                    "-2048 B")]
        [TestCase(-5,                       "-5 B")]
        [TestCase(0,                        "0 B")]
        [TestCase(1,                        "1 B")]
        [TestCase(1024 - 1,                 "1023 B")]
        [TestCase(1024,                     "1 KB")]
        [TestCase((1024 * 1024) - 1,        "1023 KB")]
        [TestCase((1024 * 1024),            "1 MB")]
        [TestCase((1024 * 1024 * 1024) - 1, "1023 MB")]
        [TestCase((1024 * 1024 * 1024),     "1 GB")]
        public void ReadableSize_Get(int value, string expectedResult)
        {
            subject.Bytes = value;

            Assert.AreEqual(expectedResult, subject.ReadableSize);
        }

        [Test]
        public void UpdateURL_GetSet()
        {
            string testValue = "Test Value";

            subject.UpdateURL = testValue;

            Assert.AreEqual(testValue, subject.UpdateURL);
        }
    }
}