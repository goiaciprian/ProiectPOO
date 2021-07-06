using DatabaseCRUD;
using DatabaseCRUD.Database;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ProiectPOO
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Studenti studentSelectat = null;
        Discipline disciplinaSelectata = null;
        Catalog catalogSelectat = null;

        public MainWindow()
        {
            
            InitializeComponent();
            initAllLV();
            loadStudentiDropDown();
            loadDsiciplineDropDown();

        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(studentiTab.IsSelected)
            {
                studentSelectat = (Studenti)studentiLV.SelectedItem;
                if(studentSelectat != null)
                {
                    codMatricolTextBox.Text = studentSelectat.NumarMatricol.ToString();
                    numeTextBox.Text = studentSelectat.Nume;
                    prenumeTextBox.Text = studentSelectat.Prenume;
                    adaugareModificareButton.Content = "Modificare";
                } else
                {
                    codMatricolTextBox.Text = "";
                    numeTextBox.Text = "";
                    prenumeTextBox.Text = "";
                    adaugareModificareButton.Content = "Adaugare";
                }
                
            }

            if(disciplineTab.IsSelected)
            {
                disciplinaSelectata = (Discipline)disciplineLV.SelectedItem;
                if(disciplinaSelectata != null)
                {
                    codDisciplinaTextBox.Text = disciplinaSelectata.Cod_Disciplina;
                    disciplinaTextBox.Text = disciplinaSelectata.Denumire;
                    crediteTextBox.Text = disciplinaSelectata.NumarCredite.ToString();
                    adaugareModificareDisciplina.Content = "Modificare";
                } else
                {
                    codDisciplinaTextBox.Text = "";
                    disciplinaTextBox.Text = "";
                    crediteTextBox.Text = "";
                    adaugareModificareDisciplina.Content = "Adaugare";
                }
            }

            if (catalogTab.IsSelected)
            {
                catalogSelectat = (Catalog)catalogLV.SelectedItem;
                if(catalogSelectat != null)
                {
                    var item = studentiDropdown.Items.Cast<object>().ToArray();
                    var itemsDiscipline = disciplinaDropdown.Items.Cast<object>().ToArray();
                    studentiDropdown.SelectedItem = item.Cast<StudentDropItem>().Where(elem => elem.NumeComplet == catalogSelectat.Nume + " " + catalogSelectat.Prenume).FirstOrDefault();
                    disciplinaDropdown.SelectedItem = itemsDiscipline.Cast<DisciplinaDropItem>().Where(elem => elem.Denumire == catalogSelectat.Denumire).FirstOrDefault();
                    notaTextBox.Text = catalogSelectat.Nota.ToString();
                    adaugareModificareCatalog.Content = "Modificare";
                } else
                {
                    studentiDropdown.SelectedIndex = -1;
                    disciplinaDropdown.SelectedIndex = -1;
                    notaTextBox.Text = "";
                    adaugareModificareCatalog.Content = "Adaugare";
                }
            }

            enableDeleteButtonsWhenItemsAreSelected();
        }


        private void initAllLV()
        {
            try
            {
                showText("Incarcare informatii...");
                initDisciplineLV();
                initStudentiLV();
                initCatalogLV();

            } catch(Exception)
            {
                showText("Unele informatii nu au fost incarcate.");
            }

        }

        private void initDisciplineLV ()
        {
            Task.Run(() =>
            {        
                foreach (var disciplina in DBCrud.DisciplineGET().Result)
                    Dispatcher.Invoke(() => disciplineLV.Items.Add(disciplina));
            });

        }

        private void initCatalogLV()
        {
            Task.Run(()=> { 
                foreach(var nota in DBCrud.CatalogGET().Result)
                {
                    Dispatcher.Invoke(()=>catalogLV.Items.Add(nota));
                }
            });
        }

        private void initStudentiLV ()
        {
            Task.Run(() =>
            {
                Dispatcher.Invoke(() => studentiLV.Items.Clear());
                var studentiList = DBCrud.StudentiGET().Result;;
                foreach (var stud in DBCrud.StudentiGET().Result)
                {
                    Dispatcher.Invoke(() => studentiLV.Items.Add(stud));
                }

            });
        }

        private void loadStudentiDropDown()
        {
            Task.Run(()=>
            {
                try
                {
                    if (studentiDropdown.Items.Count > 0)
                        Dispatcher.Invoke(() => studentiDropdown.Items.Clear());
                    foreach(var student in DBCrud.StudentiGET().Result)
                    {
                        Dispatcher.Invoke(() => studentiDropdown.Items.Add(new StudentDropItem()
                        {
                            NumarMatricol = student.NumarMatricol,
                            NumeComplet = $"{student.Nume} {student.Prenume}"
                        }));
                    }
                } catch(Exception) { }
            });
        }

        private void loadDsiciplineDropDown()
        {


            Task.Run(()=> {
                try
                {
                    if (disciplinaDropdown.Items.Count > 0)
                        Dispatcher.Invoke(() => disciplinaDropdown.Items.Clear());
                    foreach(var disciplina in DBCrud.DisciplineGET().Result)
                    {
                        Dispatcher.Invoke(() => disciplinaDropdown.Items.Add(new DisciplinaDropItem()
                        {
                            CodDisciplina = disciplina.Cod_Disciplina,
                            Denumire = disciplina.Denumire
                        }));
                    }
                } catch(Exception) { }
            });
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }


        private void minimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
/*
        private int counter = 0;*/
        private void Window_Activated(object sender, EventArgs e)
        {
            /*if (counter == 0)
                counter++;
            else 
                if (this.WindowState == WindowState.Normal) this.WindowState = WindowState.Minimized;
                else this.WindowState = WindowState.Normal;*/
        }

        private void enableDeleteButtonsWhenItemsAreSelected()
        {
            if (studentSelectat != null)
            {
                stergeButtonStudent.IsEnabled = true;
            }
            else
            {
                stergeButtonStudent.IsEnabled = false;
            }
            if(disciplinaSelectata != null)
            {
                stergeButtonDisciplina.IsEnabled = true;
            }
            else
            {
                stergeButtonDisciplina.IsEnabled = false;
            }
            if (catalogSelectat != null)
                stergeButtonCatalog.IsEnabled = true;
            else
            {
                stergeButtonCatalog.IsEnabled = false;
            }
        }

        private void listViewOnClick(object sender, MouseButtonEventArgs e)
        {
            if (studentiTab.IsSelected) studentSelectat = null;
            if (disciplineTab.IsSelected) disciplinaSelectata = null;
            if (catalogTab.IsSelected) catalogSelectat = null;
            resetLVToDefault();
            enableDeleteButtonsWhenItemsAreSelected();
        }

        private void stergeButtonStudent_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                
                Dispatcher.Invoke(() => {
                    try
                    {
                        var deleted = DBCrud.StudentiDELETE(studentSelectat.NumarMatricol).Result;
                        studentiLV.Items.Remove(studentSelectat);
                        showText("Studentul a fost sters.");
                        loadStudentiDropDown();
                    } catch (Exception)
                    {
                        showText("Studentul nu a fost sters.");
                    }
                });
            });
        }

        private void resetLVToDefault()
        {
            studentiLV.SelectedIndex = -1;
            disciplineLV.SelectedIndex = -1;
            catalogLV.SelectedIndex = -1;
        }

        private void adaugareModificareButton_Click(object sender, RoutedEventArgs e)
        {
            string codMatricol = codMatricolTextBox.Text, nume = numeTextBox.Text, prenume = prenumeTextBox.Text;
            if (codMatricol == "" || !int.TryParse(codMatricol, out _) )
                codMatricolTextBox.BorderBrush = System.Windows.Media.Brushes.Red;
            if (nume == "")
                numeTextBox.BorderBrush = System.Windows.Media.Brushes.Red;
            if (prenume == "")
                prenumeTextBox.BorderBrush = System.Windows.Media.Brushes.Red;
            if (codMatricol == "" || nume == "" || prenume == "" || !int.TryParse(codMatricol, out _))
                return;

            Task.Run(() =>
            {
                if (studentSelectat == null)
                {
                    Dispatcher.Invoke(() =>
                    {
                        try
                        {
                            studentiLV.Items.Add(DBCrud.StudentiMERGE(int.Parse(codMatricol), nume, prenume).Result);
                            studentiLV.Items.Refresh();
                            
                            showText("Studentul a fost adaugat.");
                            loadStudentiDropDown();
                        }
                        catch (Exception)
                        {
                            showText("Studentul nu a fost adaugat.");
                        }
                    });
                }
                else
                {
                    Dispatcher.Invoke(() =>
                    {
                        try
                        {
                            studentiLV.Items.RemoveAt(studentiLV.SelectedIndex);
                            studentiLV.Items.Add(DBCrud.StudentiMERGE(int.Parse(codMatricol), nume, prenume).Result);
                            studentiLV.Items.Refresh();
                            showText("Studentul a fost modificat.");
                        }
                        catch (Exception)
                        {
                            showText("Studentul nu a fost modificat.");
                        }
                    });
                }

            });

            codMatricolTextBox.Text = "";
            numeTextBox.Text = "";
            prenumeTextBox.Text = "";
            codMatricolTextBox.BorderBrush = System.Windows.Media.Brushes.Transparent;
            numeTextBox.BorderBrush = System.Windows.Media.Brushes.Transparent;
            prenumeTextBox.BorderBrush = System.Windows.Media.Brushes.Transparent;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            
            if(codMatricolTextBox.IsFocused == false && numeTextBox.IsFocused == false && prenumeTextBox.IsFocused == false &&
                codDisciplinaTextBox.IsFocused == false && disciplinaTextBox.IsFocused == false && crediteTextBox.IsFocused == false &&
                notaTextBox.IsFocused == false)
            {

                if (e.Key == Key.D1)
                    catalogTab.IsSelected = true;
                if (e.Key == Key.D2)
                    disciplineTab.IsSelected = true;
                if (e.Key == Key.D3)
                    studentiTab.IsSelected = true;
            }

        }

        private void showText(string text)
        {
            Dispatcher.Invoke(() => {
                statusbar.Content = text;
                Storyboard sb = Resources["hideAnimatie"] as Storyboard;
                sb.Begin(statusbar);

            });
        }

        private void stergeButtonDisciplina_MouseDown(object sender, MouseButtonEventArgs e)
        {
           
        }

        private void adaugareModificareDisciplina_Click(object sender, RoutedEventArgs e)
        {
            string codDisciplina = codDisciplinaTextBox.Text, denumire = disciplinaTextBox.Text, credite = crediteTextBox.Text;
            if (codDisciplina == "")
                codDisciplinaTextBox.BorderBrush = Brushes.Red;
            if (denumire == "")
                disciplinaTextBox.BorderBrush = Brushes.Red;
            if (credite == "" || int.TryParse(credite, out _))
                crediteTextBox.BorderBrush = Brushes.Red;

            if (codDisciplina == "" || denumire == "" || !int.TryParse(credite, out _))
                return;

            Task.Run(() =>
            {
                Dispatcher.Invoke(()=> {
                    try
                    {
                        var merged = DBCrud.DisciplineMERGE(codDisciplina, denumire, int.Parse(credite)).Result;
                        if (disciplinaSelectata == null)
                        {
                            disciplineLV.Items.Add(merged);
                            disciplineLV.Items.Refresh();
                            showText("Disciplina a fost adaugata.");

                        }
                        else
                        {
                            disciplineLV.Items.Remove(disciplinaSelectata);
                            disciplineLV.Items.Add(merged);
                            disciplineLV.Items.Refresh();
                            showText("Disciplina a fost modificata.");
                            
                        }
                        loadDsiciplineDropDown();
                    }
                    catch (Exception)
                    {
                        showText("Disciplina nu a fost " + (disciplinaSelectata == null ? "adaugata." : "modificata.").ToString());
                    }

                    codDisciplinaTextBox.Text = "";
                    disciplinaTextBox.Text = "";
                    crediteTextBox.Text = "";
                    codDisciplinaTextBox.BorderBrush = Brushes.Transparent;
                    disciplinaTextBox.BorderBrush = Brushes.Transparent;
                    crediteTextBox.BorderBrush = Brushes.Transparent;

                });
                
            });
        }

        private void stergeButtonDisciplina_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                Dispatcher.Invoke(() =>
                {
                    try
                    {

                        var deleted = DBCrud.DisciplineDELETE(disciplinaSelectata.Cod_Disciplina).Result;
                        disciplineLV.Items.Remove(disciplinaSelectata);
                        showText("Disciplina a fost stearsa.");
                        loadDsiciplineDropDown();
                    }
                    catch (Exception)
                    {
                        showText("Disciplina nu a fost stearsa.");
                    }
                });
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CSVExport.SalveazaFisier(
                () => showText("Fisierul a fost salvat."),
                () => showText("Fisierul nu a fost salvat.")
            );
        }

        private void studentiDropdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void disciplinaDropdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }

        private void stergeButtonCatalog_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(()=> 
            {
                Dispatcher.Invoke(()=> 
                {
                    try
                    {
                        var notaStearsa = DBCrud.CatalogDELETE(catalogSelectat.id);
                        catalogLV.Items.Remove(catalogSelectat);
                        showText("Nota a fost stearsa.");

                    }
                    catch (Exception)
                    {
                        showText("Nota nu fost stearsa.");
                    }
                });
            });
        }

        private void adaugareModificareCatalog_Click(object sender, RoutedEventArgs e)
        {
            if (studentiDropdown.SelectedIndex == -1)
                studentiDropdown.BorderBrush = Brushes.Red;
            if (disciplinaDropdown.SelectedIndex == -1)
                disciplinaDropdown.BorderBrush = Brushes.Red;
            if (!int.TryParse(notaTextBox.Text, out _ ))
                notaTextBox.BorderBrush = Brushes.Red;

            if (studentiDropdown.SelectedIndex == -1 || disciplinaDropdown.SelectedIndex == -1 || !int.TryParse(notaTextBox.Text, out _))
                return;

            Task.Run(() => 
            {
                Dispatcher.Invoke(()=>
                {
                    try
                    {
                        Debug.WriteLine(((DisciplinaDropItem)disciplinaDropdown.SelectedItem).CodDisciplina);
                        Debug.WriteLine(((StudentDropItem)studentiDropdown.SelectedItem).NumarMatricol);

                        var newItem = DBCrud.CatalogMERGE((int?)catalogSelectat?.id, ((DisciplinaDropItem)disciplinaDropdown.SelectedItem).CodDisciplina, ((StudentDropItem)studentiDropdown.SelectedItem).NumarMatricol, int.Parse(notaTextBox.Text)).Result;
                        catalogLV.Items.Add(newItem);
                        if(catalogSelectat != null)
                        {
                            catalogLV.Items.Remove(catalogSelectat);
                            showText("Nota a fost modificata.");
                        }else
                        {
                            showText("Nota a fost adaugata.");
                        }

                        notaTextBox.Text = "";
                        notaTextBox.BorderBrush = Brushes.Transparent;
                        disciplinaDropdown.SelectedIndex = -1;
                        disciplinaDropdown.BorderBrush = Brushes.Transparent;
                        studentiDropdown.SelectedIndex = -1;
                        studentiDropdown.BorderBrush = Brushes.Transparent;
                    }
                    catch (Exception)
                    {
                        showText("Nota nu a fost " + (catalogSelectat == null ? "adaugata." : "modificata"));
                    }
                });
            });
        }

        private void tabsComponent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
    }
}
