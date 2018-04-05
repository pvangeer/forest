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
    
    public partial class TreeEventEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TreeEventEntity()
        {
            this.ExpertClassEstimationEntities = new HashSet<ExpertClassEstimationEntity>();
            this.TreeEventEntity1 = new HashSet<TreeEventEntity>();
            this.TreeEventEntity11 = new HashSet<TreeEventEntity>();
            this.TreeEventFragilityCurveElementEntities = new HashSet<TreeEventFragilityCurveElementEntity>();
        }
    
        public long TreeEventId { get; set; }
        public string Name { get; set; }
        public Nullable<long> FailingEventId { get; set; }
        public Nullable<long> PassingEventId { get; set; }
        public string Details { get; set; }
        public string Summary { get; set; }
        public Nullable<decimal> FixedProbability { get; set; }
        public long ProbabilitySpecificationTypeId { get; set; }
        public Nullable<long> FixedFragilityCurveId { get; set; }
        public Nullable<long> EventTreeId { get; set; }
    
        public virtual EventTreeEntity EventTreeEntity { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExpertClassEstimationEntity> ExpertClassEstimationEntities { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TreeEventEntity> TreeEventEntity1 { get; set; }
        public virtual TreeEventEntity TreeEventEntity2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TreeEventEntity> TreeEventEntity11 { get; set; }
        public virtual TreeEventEntity TreeEventEntity3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TreeEventFragilityCurveElementEntity> TreeEventFragilityCurveElementEntities { get; set; }
    }
}
