using FeatureFlagEngine.Controllers;
using FeatureFlagEngine.Dtos;
using FeatureFlagEngine.Interface;
using FeatureFlagEngine.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Web.Http.Results;

namespace FeatureFlagEngine.Tests.Controllers
{
    [TestClass]
    public class FeaturesControllerTests
    {
        [TestMethod]
        public void Create_Returns_Ok_With_Feature()
        {
            // Arrange
            var mockService = new Mock<IFeatureManagementService>();

            var feature = new Feature
            {
                Id = 1,
                Name = "checkout",
                State = true,
                Description = "new flow"
            };

            mockService
                .Setup(x => x.CreateFeature("checkout", true, "new flow"))
                .Returns(feature);

            var controller =
                new FeaturesController(mockService.Object);

            var request = new CreateFeatureRequest
            {
                Name = "checkout",
                State = true,
                Description = "new flow"
            };

            // Act
            var result = controller.Create(request);

            // Assert
            var okResult =
                result as OkNegotiatedContentResult<Feature>;

            Assert.IsNotNull(okResult);
            Assert.AreEqual("checkout", okResult.Content.Name);
        }

        [TestMethod]
        public void Create_Returns_BadRequest_When_Request_Is_Null()
        {
            // Arrange
            var mockService = new Mock<IFeatureManagementService>();

            var controller =
                new FeaturesController(mockService.Object);

            // Act
            var result = controller.Create(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
        }

        [TestMethod]
        public void GetAll_Returns_Features()
        {
            // Arrange
            var mockService = new Mock<IFeatureManagementService>();

            mockService
                .Setup(x => x.GetAllFeatures())
                .Returns(new List<Feature>
                {
                    new Feature { Name = "f1" },
                    new Feature { Name = "f2" }
                });

            var controller =
                new FeaturesController(mockService.Object);

            // Act
            var result = controller.GetAll();

            // Assert
            var okResult =
                result as OkNegotiatedContentResult<IEnumerable<Feature>>;

            Assert.IsNotNull(okResult);
        }

        [TestMethod]
        public void UpdateGlobal_Calls_Service()
        {
            // Arrange
            var mockService = new Mock<IFeatureManagementService>();

            var controller =
                new FeaturesController(mockService.Object);

            var request = new UpdateGlobalStateRequest
            {
                State = true
            };

            // Act
            var result = controller.UpdateGlobal("checkout", request);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));

            mockService.Verify(x =>
                x.UpdateGlobalState("checkout", true),
                Times.Once);
        }

        [TestMethod]
        public void AddUserOverride_Calls_Service()
        {
            // Arrange
            var mockService = new Mock<IFeatureManagementService>();

            var controller =
                new FeaturesController(mockService.Object);

            var request = new OverrideRequest
            {
                State = false
            };

            // Act
            var result =
                controller.AddUserOverride("checkout", "u1", request);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));

            mockService.Verify(x =>
                x.AddOrUpdateUserOverride(
                    "checkout",
                    "u1",
                    false),
                Times.Once);
        }

        [TestMethod]
        public void RemoveUserOverride_Calls_Service()
        {
            // Arrange
            var mockService = new Mock<IFeatureManagementService>();

            var controller =
                new FeaturesController(mockService.Object);

            // Act
            var result =
                controller.RemoveUserOverride("checkout", "u1");

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));

            mockService.Verify(x =>
                x.RemoveUserOverride("checkout", "u1"),
                Times.Once);
        }

        [TestMethod]
        public void AddGroupOverride_Calls_Service()
        {
            // Arrange
            var mockService = new Mock<IFeatureManagementService>();

            var controller =
                new FeaturesController(mockService.Object);

            var request = new OverrideRequest
            {
                State = true
            };

            // Act
            var result =
                controller.AddGroupOverride("checkout", "beta", request);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));

            mockService.Verify(x =>
                x.AddOrUpdateGroupOverride(
                    "checkout",
                    "beta",
                    true),
                Times.Once);
        }

        [TestMethod]
        public void RemoveGroupOverride_Calls_Service()
        {
            // Arrange
            var mockService = new Mock<IFeatureManagementService>();

            var controller =
                new FeaturesController(mockService.Object);

            // Act
            var result =
                controller.RemoveGroupOverride("checkout", "beta");

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));

            mockService.Verify(x =>
                x.RemoveGroupOverride(
                    "checkout",
                    "beta"),
                Times.Once);
        }
    }
}
