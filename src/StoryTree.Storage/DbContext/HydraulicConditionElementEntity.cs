//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace StoryTree.Storage.DbContext
{
    using System;
    using System.Collections.Generic;
    
    public partial class HydraulicConditionElementEntity
    {
        public long HydraulicConditionElementId { get; set; }
        public long FragilityCurveElementId { get; set; }
        public long ProjectId { get; set; }
        public Nullable<decimal> WavePeriod { get; set; }
        public Nullable<decimal> WaveHeight { get; set; }
    
        public virtual FragilityCurveElementEntity FragilityCurveElementEntity { get; set; }
        public virtual ProjectEntity ProjectEntity { get; set; }
    }
}
