using System.Web.Http;
using Unity;
using Unity.Lifetime;
using Unity.WebApi;
using FeatureFlagEngine.DataLayer;
using FeatureFlagEngine.Interface;
using FeatureFlagEngine.Services;

namespace FeatureFlagEngine
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // DbContext per request
            container.RegisterType<FeatureDbContext>(
                new HierarchicalLifetimeManager());

            // Services
            container.RegisterType<IFeatureEvaluationService, FeatureEvaluationService>();
            container.RegisterType<IFeatureManagementService, FeatureManagementService>();
            container.RegisterType<IFeatureDataStore, EfFeatureDataStore>();

            GlobalConfiguration.Configuration.DependencyResolver =
                new UnityDependencyResolver(container);
        }
    }
}
