using FeatureFlagEngine.DataLayer;
using FeatureFlagEngine.Interface;
using FeatureFlagEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeatureFlagEngine.Services
{
    public class FeatureManagementService : IFeatureManagementService
    {
        private readonly IFeatureDataStore _store;

        public FeatureManagementService(IFeatureDataStore store)
        {
            _store = store;
        }

        public Feature CreateFeature(string name, bool state, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Feature name required.");

            if (_store.Features.Any(f => f.Name == name))
                throw new InvalidOperationException($"Feature '{name}' already exists.");

            var feature = new Feature
            {
                Name = name,
                State = state,
                Description = description
            };

            _store.AddFeature(feature);
            _store.SaveChanges();

            return feature;
        }

        public IEnumerable<Feature> GetAllFeatures()
        {
            return _store.Features.ToList();
        }

        public void UpdateGlobalState(string featureName, bool state)
        {
            var feature = GetFeatureByName(featureName);

            feature.State = state;
            _store.SaveChanges();
        }

        public void AddOrUpdateUserOverride(string featureName, string userId, bool state)
        {
            var feature = GetFeatureByName(featureName);

            var existing = _store.UserOverrides
                .FirstOrDefault(o =>
                    o.FeatureId == feature.Id &&
                    o.UserId == userId);

            if (existing == null)
            {
                _store.AddUserOverride(new UserOverride
                {
                    FeatureId = feature.Id,
                    UserId = userId,
                    State = state
                });
            }
            else
            {
                existing.State = state;
            }

            _store.SaveChanges();
        }

        public void RemoveUserOverride(string featureName, string userId)
        {
            var feature = GetFeatureByName(featureName);

            var existing = _store.UserOverrides
                .FirstOrDefault(o =>
                    o.FeatureId == feature.Id &&
                    o.UserId == userId);

            if (existing != null)
            {
                _store.RemoveUserOverride(existing);
                _store.SaveChanges();
            }
        }

        public void AddOrUpdateGroupOverride(string featureName, string groupName, bool state)
        {
            var feature = GetFeatureByName(featureName);

            var existing = _store.GroupOverrides
                .FirstOrDefault(o =>
                    o.FeatureId == feature.Id &&
                    o.GroupName == groupName);

            if (existing == null)
            {
                _store.AddGroupOverride(new GroupOverride
                {
                    FeatureId = feature.Id,
                    GroupName = groupName,
                    State = state
                });
            }
            else
            {
                existing.State = state;
            }

            _store.SaveChanges();
        }

        public void RemoveGroupOverride(string featureName, string groupName)
        {
            var feature = GetFeatureByName(featureName);

            var existing = _store.GroupOverrides
                .FirstOrDefault(o =>
                    o.FeatureId == feature.Id &&
                    o.GroupName == groupName);

            if (existing != null)
            {
                _store.RemoveGroupOverride(existing);
                _store.SaveChanges();
            }
        }

        private Feature GetFeatureByName(string featureName)
        {
            var feature = _store.Features
                .FirstOrDefault(f => f.Name == featureName);

            if (feature == null)
                throw new InvalidOperationException($"Feature '{featureName}' not found.");

            return feature;
        }
    }
}