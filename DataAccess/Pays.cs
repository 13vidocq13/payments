//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class Pays
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public int MonthNumber { get; set; }
        public int IdService { get; set; }
        public Nullable<int> IdTariff { get; set; }
        public Nullable<double> CounterFirst { get; set; }
        public Nullable<double> CounterSecond { get; set; }
        public Nullable<int> Difference { get; set; }
        public Nullable<double> Sum { get; set; }
    }
}