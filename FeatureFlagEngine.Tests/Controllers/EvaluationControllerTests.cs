using FeatureFlagEngine.Controllers;
using FeatureFlagEngine.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Results;

namespace FeatureFlagEngine.Tests.Controllers
{
    [TestClass]
    public class EvaluationControllerTests
    {
        [TestMethod]
        public void Evaluate_Returns_Enabled_True_When_Service_Returns_True()
        {
            // Arrange
            var mockService = new Mock<IFeatureEvaluationService>();

            mockService
                .Setup(x => x.IsFeatureEnabled("checkout", "u1", "beta"))
                .Returns(true);

            var controller =
                new EvaluationController(mockService.Object);

            // REQUIRED for ApiController unit tests
            controller.Request =
                new System.Net.Http.HttpRequestMessage();

            controller.Configuration =
                new System.Web.Http.HttpConfiguration();

            // Act
            var result =
                controller.Evaluate("checkout", "u1", "beta");

            // Assert
            Assert.IsNotNull(result);

            var response =
                result.ExecuteAsync(
                    new System.Threading.CancellationToken())
                .Result;

            Assert.AreEqual(
                System.Net.HttpStatusCode.OK,
                response.StatusCode);

            var body =
                response.Content
                    .ReadAsAsync<Dictionary<string, object>>()
                    .Result;

            Assert.AreEqual("checkout", body["Feature"].ToString());
            Assert.AreEqual(true, (bool)body["Enabled"]);
        }

        [TestMethod]
        public void Evaluate_Returns_Enabled_False_When_Service_Returns_False()
        {
            // Arrange
            var mockService = new Mock<IFeatureEvaluationService>();

            mockService
                .Setup(x => x.IsFeatureEnabled("search", null, null))
                .Returns(false);

            var controller =
                new EvaluationController(mockService.Object);

            controller.Request =
                new System.Net.Http.HttpRequestMessage();

            controller.Configuration =
                new System.Web.Http.HttpConfiguration();

            // Act
            var result =
                controller.Evaluate("search");

            // Assert
            Assert.IsNotNull(result);

            var response =
                result.ExecuteAsync(
                    new System.Threading.CancellationToken())
                .Result;

            Assert.AreEqual(
                System.Net.HttpStatusCode.OK,
                response.StatusCode);

            var body =
    response.Content
        .ReadAsAsync<Dictionary<string, object>>()
        .Result;

            Assert.AreEqual("search", body["Feature"].ToString());
            Assert.AreEqual(false, (bool)body["Enabled"]);
        }

        [TestMethod]
        public void Evaluate_Calls_Service_With_Correct_Parameters()
        {
            // Arrange
            var mockService = new Mock<IFeatureEvaluationService>();

            var controller =
                new EvaluationController(mockService.Object);

            // REQUIRED for ApiController tests
            controller.Request =
                new System.Net.Http.HttpRequestMessage();

            controller.Configuration =
                new System.Web.Http.HttpConfiguration();

            // Act
            controller.Evaluate("checkout", "user123", "admins");

            // Assert
            mockService.Verify(x =>
                x.IsFeatureEnabled(
                    "checkout",
                    "user123",
                    "admins"),
                Times.Once);
        }
    }
}
