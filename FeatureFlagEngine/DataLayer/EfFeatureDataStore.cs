using FeatureFlagEngine.Interface;
using FeatureFlagEngine.Models;
using System.Linq;

namespace FeatureFlagEngine.DataLayer
{
    public class EfFeatureDataStore : IFeatureDataStore
    {
        private readonly FeatureDbContext _context;

        public EfFeatureDataStore(FeatureDbContext context)
        {
            _context = context;
        }

        public IQueryable<Feature> Features => _context.Features;
        public IQueryable<UserOverride> UserOverrides => _context.UserOverrides;
        public IQueryable<GroupOverride> GroupOverrides => _context.GroupOverrides;

        public void SaveChanges()
            => _context.SaveChanges();

        public void AddFeature(Feature feature)
    => _context.Features.Add(feature);

        public void AddUserOverride(UserOverride entity)
            => _context.UserOverrides.Add(entity);

        public void AddGroupOverride(GroupOverride entity)
            => _context.GroupOverrides.Add(entity);

        public void RemoveUserOverride(UserOverride entity)
            => _context.UserOverrides.Remove(entity);

        public void RemoveGroupOverride(GroupOverride entity)
            => _context.GroupOverrides.Remove(entity);
    }
}