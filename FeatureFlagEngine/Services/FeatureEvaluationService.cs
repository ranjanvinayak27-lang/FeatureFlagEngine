using System;
using System.Linq;
using FeatureFlagEngine.DataLayer;
using FeatureFlagEngine.Models;
using FeatureFlagEngine.Interface;

public class FeatureEvaluationService : IFeatureEvaluationService
{
    private readonly IFeatureDataStore _store;

    public FeatureEvaluationService(IFeatureDataStore store)
    {
        _store = store;
    }

    public bool IsFeatureEnabled(string featureName, string userId = null, string groupName = null)
    {
        if (string.IsNullOrWhiteSpace(featureName))
            throw new ArgumentException("Feature name is required.");

        var feature = _store.Features
            .FirstOrDefault(f => f.Name == featureName);

        if (feature == null)
            throw new InvalidOperationException(
                $"Feature '{featureName}' does not exist.");

        // 1️: User override
        if (!string.IsNullOrWhiteSpace(userId))
        {
            var userOverride = _store.UserOverrides
                .FirstOrDefault(o =>
                    o.FeatureId == feature.Id &&
                    o.UserId == userId);

            if (userOverride != null)
                return userOverride.State;
        }

        // 2️: Group override
        if (!string.IsNullOrWhiteSpace(groupName))
        {
            var groupOverride = _store.GroupOverrides
                .FirstOrDefault(o =>
                    o.FeatureId == feature.Id &&
                    o.GroupName == groupName);

            if (groupOverride != null)
                return groupOverride.State;
        }

        // 3️: Global default
        return feature.State;
    }
}
