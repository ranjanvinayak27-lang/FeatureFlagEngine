using FeatureFlagEngine.Interface;
using FeatureFlagEngine.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FeatureFlagEngine.Tests.Services
{
    [TestClass]
    public class FeatureEvaluationServiceTest
    {
        private Mock<IFeatureDataStore> _storeMock;

        [TestInitialize]
        public void Setup()
        {
            _storeMock = new Mock<IFeatureDataStore>();
        }

        [TestMethod]
        public void Returns_UserOverride_When_Present()
        {
            // Arrange
            var feature = new Feature
            {
                Id = 1,
                Name = "checkout",
                State = false
            };

            _storeMock.Setup(x => x.Features)
                .Returns(new List<Feature> { feature }.AsQueryable());

            _storeMock.Setup(x => x.UserOverrides)
                .Returns(new List<UserOverride>
                {
                    new UserOverride
                    {
                        FeatureId = 1,
                        UserId = "u1",
                        State = true
                    }
                }.AsQueryable());

            _storeMock.Setup(x => x.GroupOverrides)
                .Returns(new List<GroupOverride>().AsQueryable());

            var service =
                new FeatureEvaluationService(_storeMock.Object);

            // Act
            var result =
                service.IsFeatureEnabled("checkout", "u1", "beta");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Returns_GroupOverride_When_UserOverride_Missing()
        {
            // Arrange
            var feature = new Feature
            {
                Id = 2,
                Name = "search",
                State = false
            };

            _storeMock.Setup(x => x.Features)
                .Returns(new List<Feature> { feature }.AsQueryable());

            _storeMock.Setup(x => x.UserOverrides)
                .Returns(new List<UserOverride>().AsQueryable());

            _storeMock.Setup(x => x.GroupOverrides)
                .Returns(new List<GroupOverride>
                {
                    new GroupOverride
                    {
                        FeatureId = 2,
                        GroupName = "beta",
                        State = true
                    }
                }.AsQueryable());

            var service =
                new FeatureEvaluationService(_storeMock.Object);

            // Act
            var result =
                service.IsFeatureEnabled("search", null, "beta");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Returns_Global_When_No_Overrides()
        {
            // Arrange
            var feature = new Feature
            {
                Id = 3,
                Name = "payments",
                State = true
            };

            _storeMock.Setup(x => x.Features)
                .Returns(new List<Feature> { feature }.AsQueryable());

            _storeMock.Setup(x => x.UserOverrides)
                .Returns(new List<UserOverride>().AsQueryable());

            _storeMock.Setup(x => x.GroupOverrides)
                .Returns(new List<GroupOverride>().AsQueryable());

            var service =
                new FeatureEvaluationService(_storeMock.Object);

            // Act
            var result =
                service.IsFeatureEnabled("payments");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void UserOverride_Takes_Precedence_Over_Group()
        {
            // Arrange
            var feature = new Feature
            {
                Id = 4,
                Name = "profile",
                State = false
            };

            _storeMock.Setup(x => x.Features)
                .Returns(new List<Feature> { feature }.AsQueryable());

            _storeMock.Setup(x => x.UserOverrides)
                .Returns(new List<UserOverride>
                {
                    new UserOverride
                    {
                        FeatureId = 4,
                        UserId = "u1",
                        State = false
                    }
                }.AsQueryable());

            _storeMock.Setup(x => x.GroupOverrides)
                .Returns(new List<GroupOverride>
                {
                    new GroupOverride
                    {
                        FeatureId = 4,
                        GroupName = "beta",
                        State = true
                    }
                }.AsQueryable());

            var service =
                new FeatureEvaluationService(_storeMock.Object);

            // Act
            var result =
                service.IsFeatureEnabled("profile", "u1", "beta");

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Throws_When_Feature_Not_Found()
        {
            // Arrange
            _storeMock.Setup(x => x.Features)
                .Returns(new List<Feature>().AsQueryable());

            var service =
                new FeatureEvaluationService(_storeMock.Object);

            // Act
            service.IsFeatureEnabled("missing-feature");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Throws_When_FeatureName_Is_Empty()
        {
            // Arrange
            var service =
                new FeatureEvaluationService(_storeMock.Object);

            // Act
            service.IsFeatureEnabled("");
        }
    }
}
