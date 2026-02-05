using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeatureFlagEngine.Models
{
    public class Feature
    {
        public int Id { get; set; }
        public string Name { get; set; }   
        public bool State { get; set; }   
        public string Description { get; set; }
    }
}