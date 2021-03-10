//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Ezy.Module.Selenium.Core
{
    using System;
    using System.Collections.Generic;
    
    public partial class AutomationTestData
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AutomationTestData()
        {
            this.AutomationTestData1 = new HashSet<AutomationTestData>();
        }
    
        public long Id { get; set; }
        public Nullable<System.DateTime> StartTime { get; set; }
        public Nullable<long> ParentId { get; set; }
        public int LinkOpenCount { get; set; }
        public string ErrorLink { get; set; }
        public string Error { get; set; }
        public string LocalImagePath { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> Log_CreatedDate { get; set; }
        public string Log_CreatedBy { get; set; }
        public Nullable<System.DateTime> Log_UpdatedDate { get; set; }
        public string Log_UpdatedBy { get; set; }
        public bool IsExpanderError { get; set; }
        public bool IsLinkError { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AutomationTestData> AutomationTestData1 { get; set; }
        public virtual AutomationTestData AutomationTestData2 { get; set; }
    }
}