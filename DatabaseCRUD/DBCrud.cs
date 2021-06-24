namespace DatabaseCRUD
{
    using DatabaseCRUD.Database;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    public static class DBCrud
    {
        public static Task<IEnumerable<Studenti>> StudentiGET()
        {
            return Task.Run<IEnumerable<Studenti>>(() =>
            {
                List<StudentiGET_Result> studentiGET = null;
                using(var dbcontext = new databaseiesc())
                {
                    studentiGET = dbcontext.StudentiGET().ToList();
                }
                if (studentiGET == null)
                    return new List<Studenti>();
                return studentiGET.Select(elem => new Studenti() { NumarMatricol = elem.NumarMatricol, Nume = elem.Nume, Prenume = elem.Prenume }).ToList();
            });
        }

        public static Task<Studenti> StudentiDELETE(int id)
        {
            return Task.Run(() =>
            {
                Studenti stud = null;
                using (var dbcontext = new databaseiesc())
                {
                    var studDelete = dbcontext.StudentiDELETE(id).Single();
                    stud = new Studenti() { NumarMatricol = (int)studDelete.NumarMatricol, Nume = studDelete.Nume, Prenume = studDelete.Prenume };
                }
                return stud;
            });
        }

        public static Task<Studenti> StudentiMERGE(int id, string nume, string prenume)
        {
            return Task.Run(() => 
            {
                Studenti stud = null;
                using(var dbcontext = new databaseiesc())
                {
                    var studMerge = dbcontext.StudentiMERGE(id, nume, prenume).Single();
                    stud = new Studenti() { NumarMatricol = (int)studMerge.NumarMatricol, Nume = studMerge.Nume, Prenume = studMerge.Prenume};
                }
                return stud;
            });
        }

        public static Task<Discipline> DisciplineMERGE(string CodDisciplina, string Denumire, int NumarCredite)
        {
            return Task.Run(() =>
            {
                DisciplineMERGE_Result recentAdaugat = null;
                using (var context = new databaseiesc())
                {
                    recentAdaugat = context.DisciplineMERGE(CodDisciplina, Denumire, NumarCredite).Single();
                }
                return new Discipline() { Cod_Disciplina = recentAdaugat.Cod_Disciplina, Denumire = recentAdaugat.Denumire, NumarCredite = (int)recentAdaugat.NumarCredite };
            });
        }

        public static Task<Discipline> DisciplineDELETE(string Cod_Disciplina)
        {
            return Task.Run(() =>
            {
                DisciplineDELETE_Result recentSters = null;
                using (var context = new databaseiesc())
                {
                    recentSters = context.DisciplineDELETE(Cod_Disciplina).Single();
                }
                return new Discipline() { Cod_Disciplina = recentSters.CodDisciplina, Denumire = recentSters.Denumire, NumarCredite = (int)recentSters.NumarCredite};
            });
        }

        public static Task<IEnumerable<Discipline>> DisciplineGET()
        {
            return Task.Run<IEnumerable<Discipline>>(() =>
            {
                List <DisciplineGET_Result> listaDiscipline = null;
                using (var dbcontext = new databaseiesc())
                {
                    listaDiscipline = dbcontext.DisciplineGET().ToList();
                }
                if (listaDiscipline == null)
                    return new List<Discipline>();
                return listaDiscipline.Select(elem => new Discipline() { Cod_Disciplina = elem.Cod_Disciplina, Denumire = elem.Denumire, NumarCredite = elem.NumarCredite }).ToList();
            });
        }

        public static Task<Catalog> CatalogMERGE(int? id, string codDisciplina, int numarMatricol, int Nota)
        {
            return Task.Run(() =>
            {
                CatalogMERGE_Result recentAdd = null;
                using (var context = new databaseiesc())
                {
                    recentAdd = context.CatalogMERGE(id, codDisciplina, numarMatricol, Nota).Single();
                }
                return new Catalog(recentAdd);
            });
        }

        public static Task<Catalog> CatalogDELETE(long id )
        {
            return Task.Run(() => 
            {
                CatalogDELETE_Result recentSters = null;
                using (var context = new databaseiesc())
                {
                    recentSters = context.CatalogDELETE(id).Single();
                }
                return new Catalog(recentSters);
            });
        }

        public static Task<IEnumerable<Catalog>> CatalogGET()
        {
            return Task.Run<IEnumerable<Catalog>>(() =>
            {
                List<CatalogGET_Result> note = null;
                using(var context = new databaseiesc())
                {
                    note = context.CatalogGET().ToList();
                }
                if (note == null)
                    return new List<Catalog>();
                return note.Select(elem => new Catalog(elem)).ToList();
            });
        }
    }
}
