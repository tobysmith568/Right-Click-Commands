using NUnit.Framework;

namespace Right_Click_Commands.Models.Updater.Tests
{
    public class GitHubReleaseTests
    {
        GithubRelease subject;

        [SetUp]
        public void Setup()
        {
            subject = new GithubRelease();
        }

        [Test]
        public void URL_GetSet()
        {
            string testValue = "Test Value";

            subject.URL = testValue;

            Assert.AreEqual(testValue, subject.URL);
        }

        [Test]
        public void Tag_GetSet()
        {
            string testValue = "Test Value";

            subject.Tag = testValue;

            Assert.AreEqual(testValue, subject.Tag);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void IsDraft_GetSet(bool testValue)
        {
            subject.IsDraft = testValue;

            Assert.AreEqual(testValue, subject.IsDraft);
        }

        [Test]
        public void Assets_GetSet()
        {
            Asset[] testValue = new Asset[]
            {
                new Asset(),
                new Asset()
            };

            subject.Assets = testValue;

            Assert.AreSame(testValue, subject.Assets);
        }

        [Test]
        public void Assets_SetsURLOnAllAssets()
        {
            string testString = "Test String";
            Asset asset1 = new Asset();
            Asset asset2 = new Asset();
            Asset asset3 = new Asset();

            Given_subject_URL_Equals(testString);

            subject.Assets = new Asset[]
            {
                asset1,
                asset2,
                asset3
            };

            Assert.AreEqual(testString, asset1.UpdateURL);
            Assert.AreEqual(testString, asset2.UpdateURL);
            Assert.AreEqual(testString, asset3.UpdateURL);
        }

        private void Given_subject_URL_Equals(string value)
        {
            subject.URL = value;
        }
    }
}
