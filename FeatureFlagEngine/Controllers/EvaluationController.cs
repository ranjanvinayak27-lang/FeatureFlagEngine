using FeatureFlagEngine.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace FeatureFlagEngine.Controllers
{
    [RoutePrefix("api/evaluate")]
    public class EvaluationController : ApiController
    {
        private readonly IFeatureEvaluationService _evaluationService;

        public EvaluationController(
            IFeatureEvaluationService evaluationService)
        {
            _evaluationService = evaluationService;
        }

        [HttpGet]
        [Route("{featureName}")]
        public IHttpActionResult Evaluate(
            string featureName,
            string userId = null,
            string groupName = null)
        {
            var enabled = _evaluationService.IsFeatureEnabled(
                featureName,
                userId,
                groupName);

            return Ok(new
            {
                Feature = featureName,
                Enabled = enabled
            });
        }
    }
}