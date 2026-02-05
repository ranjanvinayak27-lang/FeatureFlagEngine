using FeatureFlagEngine.Dtos;
using FeatureFlagEngine.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace FeatureFlagEngine.Controllers
{
    [RoutePrefix("api/features")]
    public class FeaturesController : ApiController
    {
        private readonly IFeatureManagementService _managementService;

        public FeaturesController(IFeatureManagementService managementService)
        {
            _managementService = managementService;
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(CreateFeatureRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request body is null");
            }

            var feature = _managementService.CreateFeature(
                request.Name,
                request.State,
                request.Description);

            return Ok(feature);
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll()
        {

            return Ok(_managementService.GetAllFeatures());
        }

        [HttpPut]
        [Route("{name}/global")]
        public IHttpActionResult UpdateGlobal(string name, UpdateGlobalStateRequest request)
        {
            _managementService.UpdateGlobalState(name, request.State);
            return Ok();
        }

        [HttpPost]
        [Route("{name}/users/{userId}")]
        public IHttpActionResult AddUserOverride(
            string name,
            string userId,
            OverrideRequest request)
        {
            _managementService.AddOrUpdateUserOverride(
                name,
                userId,
                request.State);

            return Ok();
        }

        [HttpDelete]
        [Route("{name}/users/{userId}")]
        public IHttpActionResult RemoveUserOverride(
            string name,
            string userId)
        {
            _managementService.RemoveUserOverride(name, userId);
            return Ok();
        }

        [HttpPost]
        [Route("{name}/groups/{groupName}")]
        public IHttpActionResult AddGroupOverride(
            string name,
            string groupName,
            OverrideRequest request)
        {
            _managementService.AddOrUpdateGroupOverride(
                name,
                groupName,
                request.State);

            return Ok();
        }

        [HttpDelete]
        [Route("{name}/groups/{groupName}")]
        public IHttpActionResult RemoveGroupOverride(
            string name,
            string groupName)
        {
            _managementService.RemoveGroupOverride(name, groupName);
            return Ok();
        }
    }
}