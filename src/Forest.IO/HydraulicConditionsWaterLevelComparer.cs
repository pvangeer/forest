using System;
using System.Collections.Generic;
using Forest.Data.Hydrodynamics;

namespace Forest.IO
{
    public class HydraulicConditionsWaterLevelComparer : IEqualityComparer<HydrodynamicCondition>
    {
        public bool Equals(HydrodynamicCondition x, HydrodynamicCondition y)
        {
            return x != null && y != null && Math.Abs(x.WaterLevel - y.WaterLevel) < 1e-6;
        }

        public int GetHashCode(HydrodynamicCondition obj)
        {
            return obj.GetHashCode();
        }
    }
}