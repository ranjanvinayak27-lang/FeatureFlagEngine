using FeatureFlagEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeatureFlagEngine.Interface
{
    public interface IFeatureManagementService
    {
        Feature CreateFeature(string name, bool state, string description);

        void UpdateGlobalState(string featureName, bool state);

        void AddOrUpdateUserOverride(string featureName, string userId, bool state);

        void RemoveUserOverride(string featureName, string userId);

        void AddOrUpdateGroupOverride(string featureName, string groupName, bool state);

        void RemoveGroupOverride(string featureName, string groupName);

        IEnumerable<Feature> GetAllFeatures();
    }

}
