using System.Data.Entity;
using FeatureFlagEngine.Models;

namespace FeatureFlagEngine.DataLayer
{
    public class FeatureDbContext : DbContext
    {
        public FeatureDbContext() : base("FeatureDb")
        {
        }

        public DbSet<Feature> Features { get; set; }
        public DbSet<UserOverride> UserOverrides { get; set; }
        public DbSet<GroupOverride> GroupOverrides { get; set; }
    }
}
