using FeatureFlagEngine.Interface;
using FeatureFlagEngine.Models;
using FeatureFlagEngine.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FeatureFlagEngine.Tests.Services
{
    [TestClass]
    public class FeatureManagementServiceTest
    {
        private Mock<IFeatureDataStore> _storeMock;
        private FeatureManagementService _service;

        [TestInitialize]
        public void Setup()
        {
            _storeMock = new Mock<IFeatureDataStore>();
            _service = new FeatureManagementService(_storeMock.Object);
        }

        // ---------------- CREATE FEATURE ----------------

        [TestMethod]
        public void CreateFeature_Adds_Feature_When_Not_Exists()
        {
            // Arrange
            _storeMock.Setup(x => x.Features)
                .Returns(new List<Feature>().AsQueryable());

            Feature savedFeature = null;

            _storeMock
                .Setup(x => x.AddFeature(It.IsAny<Feature>()))
                .Callback<Feature>(f => savedFeature = f);

            // Act
            var result =
                _service.CreateFeature("checkout", true, "new flow");

            // Assert
            Assert.IsNotNull(savedFeature);
            Assert.AreEqual("checkout", savedFeature.Name);

            _storeMock.Verify(x => x.SaveChanges(), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CreateFeature_Throws_When_Duplicate_Name()
        {
            // Arrange
            _storeMock.Setup(x => x.Features)
                .Returns(new List<Feature>
                {
                    new Feature { Name = "checkout" }
                }.AsQueryable());

            // Act
            _service.CreateFeature("checkout", true, "dup");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateFeature_Throws_When_Name_Is_Empty()
        {
            // Act
            _service.CreateFeature("", true, null);
        }

        // ---------------- GET ALL ----------------

        [TestMethod]
        public void GetAllFeatures_Returns_All()
        {
            // Arrange
            _storeMock.Setup(x => x.Features)
                .Returns(new List<Feature>
                {
                    new Feature { Name = "a" },
                    new Feature { Name = "b" }
                }.AsQueryable());

            // Act
            var result = _service.GetAllFeatures();

            // Assert
            Assert.AreEqual(2, result.Count());
        }

        // ---------------- UPDATE GLOBAL ----------------

        [TestMethod]
        public void UpdateGlobalState_Updates_State_And_Saves()
        {
            // Arrange
            var feature = new Feature { Name = "checkout", State = false };

            _storeMock.Setup(x => x.Features)
                .Returns(new List<Feature> { feature }.AsQueryable());

            // Act
            _service.UpdateGlobalState("checkout", true);

            // Assert
            Assert.IsTrue(feature.State);
            _storeMock.Verify(x => x.SaveChanges(), Times.Once);
        }

        // ---------------- USER OVERRIDE ----------------

        [TestMethod]
        public void AddUserOverride_Adds_When_Not_Exists()
        {
            // Arrange
            var feature = new Feature { Id = 1, Name = "checkout" };

            _storeMock.Setup(x => x.Features)
                .Returns(new List<Feature> { feature }.AsQueryable());

            _storeMock.Setup(x => x.UserOverrides)
                .Returns(new List<UserOverride>().AsQueryable());

            // Act
            _service.AddOrUpdateUserOverride("checkout", "u1", true);

            // Assert
            _storeMock.Verify(x =>
                x.AddUserOverride(It.Is<UserOverride>(o =>
                    o.UserId == "u1" &&
                    o.State == true)),
                Times.Once);

            _storeMock.Verify(x => x.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void AddUserOverride_Updates_When_Exists()
        {
            // Arrange
            var feature = new Feature { Id = 2, Name = "checkout" };

            var existing = new UserOverride
            {
                FeatureId = 2,
                UserId = "u1",
                State = false
            };

            _storeMock.Setup(x => x.Features)
                .Returns(new List<Feature> { feature }.AsQueryable());

            _storeMock.Setup(x => x.UserOverrides)
                .Returns(new List<UserOverride> { existing }.AsQueryable());

            // Act
            _service.AddOrUpdateUserOverride("checkout", "u1", true);

            // Assert
            Assert.IsTrue(existing.State);
            _storeMock.Verify(x => x.SaveChanges(), Times.Once);
        }

        // ---------------- REMOVE USER ----------------

        [TestMethod]
        public void RemoveUserOverride_Removes_When_Exists()
        {
            // Arrange
            var feature = new Feature { Id = 3, Name = "checkout" };

            var existing = new UserOverride
            {
                FeatureId = 3,
                UserId = "u1"
            };

            _storeMock.Setup(x => x.Features)
                .Returns(new List<Feature> { feature }.AsQueryable());

            _storeMock.Setup(x => x.UserOverrides)
                .Returns(new List<UserOverride> { existing }.AsQueryable());

            // Act
            _service.RemoveUserOverride("checkout", "u1");

            // Assert
            _storeMock.Verify(x =>
                x.RemoveUserOverride(existing),
                Times.Once);

            _storeMock.Verify(x => x.SaveChanges(), Times.Once);
        }

        // ---------------- GROUP OVERRIDE ----------------

        [TestMethod]
        public void AddGroupOverride_Adds_When_Not_Exists()
        {
            // Arrange
            var feature = new Feature { Id = 5, Name = "checkout" };

            _storeMock.Setup(x => x.Features)
                .Returns(new List<Feature> { feature }.AsQueryable());

            _storeMock.Setup(x => x.GroupOverrides)
                .Returns(new List<GroupOverride>().AsQueryable());

            // Act
            _service.AddOrUpdateGroupOverride("checkout", "beta", false);

            // Assert
            _storeMock.Verify(x =>
                x.AddGroupOverride(It.Is<GroupOverride>(o =>
                    o.GroupName == "beta" &&
                    o.State == false)),
                Times.Once);

            _storeMock.Verify(x => x.SaveChanges(), Times.Once);
        }

        // ---------------- FEATURE NOT FOUND ----------------

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void UpdateGlobalState_Throws_When_Feature_Not_Found()
        {
            // Arrange
            _storeMock.Setup(x => x.Features)
                .Returns(new List<Feature>().AsQueryable());

            // Act
            _service.UpdateGlobalState("missing", false);
        }
    }
}
