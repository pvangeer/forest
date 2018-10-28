using System;
using System.Collections.Generic;
using StoryTree.Data.Hydraulics;

namespace StoryTree.IO
{
    public class HydraulicConditionsWaterLevelComparer : IEqualityComparer<HydraulicCondition>
    {
        public bool Equals(HydraulicCondition x, HydraulicCondition y)
        {
            return x != null && y != null && Math.Abs(x.WaterLevel - y.WaterLevel) < 1e-6;
        }

        public int GetHashCode(HydraulicCondition obj)
        {
            return obj.GetHashCode();
        }
    }
}