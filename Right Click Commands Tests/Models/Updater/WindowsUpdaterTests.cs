using Moq;
using NUnit.Framework;
using RestSharp;
using Right_Click_Commands.Models.MessagePrompts;
using Right_Click_Commands.Models.Updater;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Right_Click_Commands.Tests;
using System.Net;
using Right_Click_Commands.Models.JSON_Converter;

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
            Mock<IRestResponse> mockResponse = new Mock<IRestResponse>();
            mockResponse.Setup(m => m.ResponseStatus).Returns(responseStatus);

            restClient.Setup(r => r.Execute(It.IsAny<RestRequest>())).Returns(mockResponse.Object);

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
            Mock<IRestResponse> mockResponse = new Mock<IRestResponse>();
            mockResponse.Setup(m => m.ResponseStatus).Returns(ResponseStatus.Completed);
            mockResponse.Setup(m => m.StatusCode).Returns(httpStatusCode);

            restClient.Setup(r => r.Execute(It.IsAny<RestRequest>())).Returns(mockResponse.Object);

            Asset result = await subject.CheckForUpdateAsync();

            Assert.IsNull(result);
        }

        [Test]
        public async Task Test_CheckForUpdateAsync_ReturnsNullOnNullRelease()
        {
            Mock<IRestResponse> mockResponse = new Mock<IRestResponse>();
            mockResponse.Setup(m => m.ResponseStatus).Returns(ResponseStatus.Completed);
            mockResponse.Setup(m => m.StatusCode).Returns(HttpStatusCode.OK);

            restClient.Setup(r => r.Execute(It.IsAny<RestRequest>())).Returns(mockResponse.Object);

            jsonConverter.Setup(j => j.FromJson<GithubRelease>(It.IsAny<string>())).Returns((GithubRelease)null);

            Asset result = await subject.CheckForUpdateAsync();

            Assert.IsNull(result);
        }

        [Test]
        public async Task Test_CheckForUpdateAsync_ReturnsNullOnDraftRelease()
        {
            Mock<IRestResponse> mockResponse = new Mock<IRestResponse>();
            mockResponse.Setup(m => m.ResponseStatus).Returns(ResponseStatus.Completed);
            mockResponse.Setup(m => m.StatusCode).Returns(HttpStatusCode.OK);

            restClient.Setup(r => r.Execute(It.IsAny<RestRequest>())).Returns(mockResponse.Object);

            GithubRelease release = new GithubRelease();
            release.IsDraft = true;

            jsonConverter.Setup(j => j.FromJson<GithubRelease>(It.IsAny<string>())).Returns(release);

            Asset result = await subject.CheckForUpdateAsync();

            Assert.IsNull(result);
        }
    }
}