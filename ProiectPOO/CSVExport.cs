using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseCRUD;
using System.IO;

namespace ProiectPOO
{
    public static class CSVExport
    {
        private static readonly SaveFileDialog dialogSalvare = new SaveFileDialog()
        {
            DefaultExt = "csv",
            Filter = "CSV Files (*.csv)|*.csv",
            RestoreDirectory = true,
            InitialDirectory = System.Reflection.Assembly.GetExecutingAssembly().Location,
            FileName = "Catalog"
        };

        public static void SalveazaFisier()
        {
            if ((bool)dialogSalvare.ShowDialog())
            {
                File.WriteAllText(dialogSalvare.FileName, ConvertDateDinBazaDeDate());
            }
        }

        public static void SalveazaFisier(Action ok,Action err)
        {
            try
            {
                if ((bool)dialogSalvare.ShowDialog())
                {
                    File.WriteAllText(dialogSalvare.FileName, ConvertDateDinBazaDeDate());
                    ok();
                }
            } catch (Exception)
            {
                err();
            }
            
        }

        private static string ConvertDateDinBazaDeDate()
        {
            StringBuilder continut = new StringBuilder();
            continut.Append("Student,Disciplina,Nota,Media,Punctaj,Status\n");

            var intrariCatalog = DBCrud.CatalogGET().Result;

            foreach (var student in intrariCatalog)
            {
                int credite = intrariCatalog.Where(elem => elem.NumarMatricol == student.NumarMatricol).Select(elem => elem.NumarCredite).Sum();
                float medie= intrariCatalog.Where(elem => elem.NumarMatricol == student.NumarMatricol).Select(elem => elem.Nota).Sum() / (float)intrariCatalog.Where(elem => elem.NumarMatricol == student.NumarMatricol).Count();

                string integralist = credite >= 40 ? "integralist" : "ne-integralist";

                continut.Append($"{student.Nume} {student.Prenume},{student.Denumire},{student.Nota},{medie},{credite},{integralist}\n");
            }

            return continut.ToString();
        }
    }
}
