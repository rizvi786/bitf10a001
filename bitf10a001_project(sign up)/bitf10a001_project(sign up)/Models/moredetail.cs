//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace bitf10a001_project_sign_up_.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class moredetail
    {
        public int Id { get; set; }
        public string subcatagory { get; set; }
        public string model { get; set; }
        public string accesories { get; set; }
        public Nullable<int> postid { get; set; }
    
        public virtual allad allad { get; set; }
    }
}
