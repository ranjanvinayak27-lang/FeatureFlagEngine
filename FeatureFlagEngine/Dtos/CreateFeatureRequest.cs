using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeatureFlagEngine.Dtos
{
    public class CreateFeatureRequest
    {
        public CreateFeatureRequest()
        {
        }

        public string Name { get; set; }
        public bool State { get; set; }
        public string Description { get; set; }
    }

}