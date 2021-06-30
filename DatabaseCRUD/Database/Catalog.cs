using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCRUD.Database
{
    public partial class Catalog
    {
        public Catalog(CatalogGET_Result get)
        {
            id = get.id;
            Cod_Disciplina = get.Cod_Disciplina;
            Denumire = get.Denumire;
            NumarCredite = get.NumarCredite;
            NumarMatricol = get.NumarMatricol;
            Nume = get.Nume;
            Prenume = get.Prenume;
            Nota = get.Nota;
        }

        public Catalog(CatalogMERGE_Result merge)
        {
            id = (int)merge.id;
            Cod_Disciplina = merge.Cod_Disciplina;
            Denumire = merge.Denumire;
            NumarCredite = merge.NumarCredite;
            NumarMatricol = merge.NumarMatricol;
            Nume = merge.Nume;
            Prenume = merge.Prenume;
            Nota = (int)merge.Nota;
        }

        public Catalog(CatalogDELETE_Result delete)
        {
            id = (int)delete.id;
            Cod_Disciplina = delete.Cod_Disciplina;
            Denumire = delete.Denumire;
            NumarCredite = delete.NumarCredite;
            NumarMatricol = delete.NumarMatricol;
            Nume = delete.Nume;
            Prenume = delete.Prenume;
            Nota = (int)delete.Nota;
        }
        public long id { get; set; }
        public string Cod_Disciplina { get; set; }
        public string Denumire { get; set; }
        public int NumarCredite { get; set; }
        public int? NumarMatricol { get; set; }
        public string Nume { get; set; }
        public string Prenume { get; set; }
        public int Nota { get; set; }
    }
}
