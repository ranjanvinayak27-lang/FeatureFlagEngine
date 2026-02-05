using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeatureFlagEngine.Interface
{
    public interface IFeatureEvaluationService
    {
        bool IsFeatureEnabled(
            string featureName,
            string userId = null,
            string groupName = null);
    }
}
