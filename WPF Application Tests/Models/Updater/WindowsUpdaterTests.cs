using Moq;
using NUnit.Framework;
using RestSharp;
using System;
using System.Threading.Tasks;
using System.Net;
using Right_Click_Commands.WPF.Models.Updater;
using Right_Click_Commands.Models.MessagePrompts;
using Right_Click_Commands.Models.JSON_Converter;
using Right_Click_Commands.Tests;

namespace Right_Click_Commands.Models.Updater.Tests
{
    [TestFixture]
    public class WindowsUpdaterTests
    {
        private WindowsUpdater subject;

        Mock<IMessagePrompt> messagePrompt;
        Mock<IJSONConverter> jsonConverter;
        Mock<IRestClient> restClient;

        [SetUp]
        public void SetUp()
        {
            messagePrompt = new Mock<IMessagePrompt>();
            jsonConverter = new Mock<IJSONConverter>();
            restClient = new Mock<IRestClient>();

            subject = new WindowsUpdater(messagePrompt.Object, jsonConverter.Object);

            subject.SetPrivateField("restClient", restClient.Object);
        }

        [Test]
        public async Task Test_CheckForUpdateAsync_ReturnsNullOnNullResponse()
        {
            restClient.Setup(r => r.Execute(It.IsAny<RestRequest>())).Returns((IRestResponse)null);

            Asset result = await subject.CheckForUpdateAsync();

            Assert.IsNull(result);
        }

        [TestCase(ResponseStatus.Aborted)]
        [TestCase(ResponseStatus.Error)]
        [TestCase(ResponseStatus.TimedOut)]
        public async Task Test_CheckForUpdateAsync_ReturnsNullOnInvalidResponseStatus(ResponseStatus responseStatus)
        {
            SetupMockResponseForRestClient(responseStatus, HttpStatusCode.OK);

            Asset result = await subject.CheckForUpdateAsync();

            Assert.IsNull(result);
        }

        [TestCase(HttpStatusCode.BadRequest)]
        [TestCase(HttpStatusCode.Continue)]
        [TestCase(HttpStatusCode.Created)]
        [TestCase(HttpStatusCode.Forbidden)]
        [TestCase(HttpStatusCode.GatewayTimeout)]
        [TestCase(HttpStatusCode.InternalServerError)]
        [TestCase(HttpStatusCode.Moved)]
        [TestCase(HttpStatusCode.MovedPermanently)]
        [TestCase(HttpStatusCode.NotFound)]
        [TestCase(HttpStatusCode.Redirect)]
        [TestCase(HttpStatusCode.RequestTimeout)]
        [TestCase(HttpStatusCode.ServiceUnavailable)]
        [TestCase(HttpStatusCode.Unauthorized)]
        [TestCase(HttpStatusCode.UpgradeRequired)]
        public async Task Test_CheckForUpdateAsync_ReturnsNullOnInvalidStatusCode(HttpStatusCode httpStatusCode)
        {
            SetupMockResponseForRestClient(ResponseStatus.Completed, httpStatusCode);

            Asset result = await subject.CheckForUpdateAsync();

            Assert.IsNull(result);
        }

        [Test]
        public async Task Test_CheckForUpdateAsync_ReturnsNullOnNullRelease()
        {
            SetupMockResponseForRestClient(ResponseStatus.Completed, HttpStatusCode.OK);
            GivenJSONConverterReturns<GithubRelease>(null);

            Asset result = await subject.CheckForUpdateAsync();

            Assert.IsNull(result);
        }

        [Test]
        public async Task Test_CheckForUpdateAsync_ReturnsNullOnDraftRelease()
        {
            SetupMockResponseForRestClient(ResponseStatus.Completed, HttpStatusCode.OK);
            GivenJSONConverterReturns(new GithubRelease
            {
                IsDraft = true
            });

            Asset result = await subject.CheckForUpdateAsync();

            Assert.IsNull(result);
        }

        [Test]
        public async Task Test_CheckForUpdateAsync_ReturnsNullOnNullVersion()
        {
            SetupMockResponseForRestClient(ResponseStatus.Completed, HttpStatusCode.OK);
            GivenSubjectAssemblyVersionIs(5, 0, 0);
            GivenJSONConverterReturns(new GithubRelease
            {
                Tag = null
            });

            Asset result = await subject.CheckForUpdateAsync();

            Assert.IsNull(result);
        }

        [TestCase("")]
        [TestCase("test")]
        [TestCase("-0.0.0")]
        [TestCase("-4.0.0")]
        public async Task Test_CheckForUpdateAsync_ReturnsNullOnInvalidVersions(string updateVersion)
        {
            SetupMockResponseForRestClient(ResponseStatus.Completed, HttpStatusCode.OK);
            GivenSubjectAssemblyVersionIs(5, 0, 0);
            GivenJSONConverterReturns(new GithubRelease
            {
                Tag = updateVersion
            });

            Asset result = await subject.CheckForUpdateAsync();

            Assert.IsNull(result);
        }

        [TestCase("5.0.0")]
        [TestCase("4.3.0")]
        [TestCase("4.2.1")]
        [TestCase("4.0.0")]
        [TestCase("0.0.0")]
        public async Task Test_CheckForUpdateAsync_ReturnsNullOnTheSameOrOlderVersions(string updateVersion)
        {
            SetupMockResponseForRestClient(ResponseStatus.Completed, HttpStatusCode.OK);
            GivenSubjectAssemblyVersionIs(5, 0, 0);
            GivenJSONConverterReturns(new GithubRelease
            {
                Tag = updateVersion
            });

            Asset result = await subject.CheckForUpdateAsync();

            Assert.IsNull(result);
        }

        [Test]
        public async Task Test_CheckForUpdateAsync_ReturnsNullWithNoMSIAsset()
        {
            SetupMockResponseForRestClient(ResponseStatus.Completed, HttpStatusCode.OK);
            GivenSubjectAssemblyVersionIs(5, 0, 0);
            GivenJSONConverterReturns(new GithubRelease
            {
                Tag = "6.0.0",
                Assets = new Asset[]
                {
                    new Asset()
                    {
                        URL = "something.not_msi"
                    }
                }
            });

            Asset result = await subject.CheckForUpdateAsync();

            Assert.IsNull(result);
        }

        [Test]
        public async Task Test_CheckForUpdateAsync_ReturnsFirstMSIAsset()
        {
            SetupMockResponseForRestClient(ResponseStatus.Completed, HttpStatusCode.OK);
            GivenSubjectAssemblyVersionIs(5, 0, 0);

            Asset msiAsset = new Asset()
            {
                URL = "something.msi"
            };

            GivenJSONConverterReturns(new GithubRelease
            {
                Tag = "6.0.0",
                Assets = new Asset[]
                {
                    new Asset()
                    {
                        URL = "something.not_msi"
                    },
                    msiAsset,
                    new Asset()
                    {
                        URL = "something.msi"
                    }
                }
            });

            Asset result = await subject.CheckForUpdateAsync();

            Assert.AreSame(msiAsset, result);
        }

        #region Helper Methods

        public Mock<IRestResponse> SetupMockResponseForRestClient(ResponseStatus responseStatus, HttpStatusCode httpStatusCode)
        {
            Mock<IRestResponse> mockResponse = new Mock<IRestResponse>();
            mockResponse.Setup(m => m.ResponseStatus).Returns(responseStatus);
            mockResponse.Setup(m => m.StatusCode).Returns(httpStatusCode);

            restClient.Setup(r => r.Execute(It.IsAny<RestRequest>())).Returns(mockResponse.Object);

            return mockResponse;
        }

        public void GivenSubjectAssemblyVersionIs(int major, int minor, int build)
        {
            System.Reflection.AssemblyName mockAssembly = new System.Reflection.AssemblyName
            {
                Version = new Version(major, minor, build)
            };

            subject.SetPrivateField("assemblyName", mockAssembly);
        }

        public void GivenJSONConverterReturns<T>(T response)
        {
            jsonConverter.Setup(j => j.FromJson<T>(It.IsAny<string>())).Returns(response);
        }

        #endregion
    }
}