//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MVC
{
    using System;
    using System.Collections.Generic;
    
    public partial class borrowBook
    {
        public int Borrow_ID { get; set; }
        public int Book_ID { get; set; }
        public int Member_ID { get; set; }
        public System.DateTime Borrow_date { get; set; }
        public System.DateTime return_date { get; set; }
        public Nullable<System.DateTime> ActualReturnDate { get; set; }
    
        public virtual Book Book { get; set; }
        public virtual Member Member { get; set; }
    }
}
