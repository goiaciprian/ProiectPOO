//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProiectPOO
{
    using System;
    using System.Collections.Generic;
    
    public partial class Catalog_Note
    {
        public long id { get; set; }
        public string Cod_Disciplina { get; set; }
        public int NumarMatricol { get; set; }
        public int Nota { get; set; }
        public Nullable<byte> Sters { get; set; }
    
        public virtual Studenti Studenti { get; set; }
        public virtual Discipline Discipline { get; set; }
    }
}
