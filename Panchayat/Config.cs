//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Panchayat
{
    using System;
    using System.Collections.Generic;
    
    public partial class Config
    {
        public int ConfigID { get; set; }
        public string VP { get; set; }
        public Nullable<decimal> DemandIncPerc { get; set; }
        public Nullable<decimal> ArrearsPerc { get; set; }
        public Nullable<int> RowsPerPage { get; set; }
        public string PanchHead { get; set; }
        public string PanchSeceretary { get; set; }
        public Nullable<int> MeetingAlert { get; set; }
    }
}
