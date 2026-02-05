using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeatureFlagEngine.Models
{
    public class UserOverride
    {
        public int Id { get; set; }
        public int FeatureId { get; set; }
        public string UserId { get; set; }
        public bool State { get; set; }
    }
}