using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeatureFlagEngine.Dtos
{
    public class UpdateGlobalStateRequest
    {
        public bool State { get; set; }
    }
}