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
    }
}
