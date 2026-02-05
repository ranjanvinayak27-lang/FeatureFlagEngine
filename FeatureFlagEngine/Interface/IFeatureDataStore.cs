using FeatureFlagEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeatureFlagEngine.Interface
{
    public interface IFeatureDataStore
    {
        IQueryable<Feature> Features { get; }
        IQueryable<UserOverride> UserOverrides { get; }
        IQueryable<GroupOverride> GroupOverrides { get; }

        void AddFeature(Feature feature);
        void AddUserOverride(UserOverride userOverride);
        void AddGroupOverride(GroupOverride groupOverride);
        void RemoveUserOverride(UserOverride userOverride);
        void RemoveGroupOverride(GroupOverride groupOverride);

        void SaveChanges();
    }
}
