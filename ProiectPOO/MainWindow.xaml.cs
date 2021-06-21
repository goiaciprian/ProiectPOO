using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Diagnostics;
using System.Linq;
using DatabaseCRUD;
using DatabaseCRUD.Database;
using System.Windows.Data;
using System.Collections.Generic;

namespace ProiectPOO
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Studenti studentSelectat = null;
        public MainWindow()
        {
            InitializeComponent();
            initAllLV();
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

            enableDeleteButtonsWhenItemsAreSelected();
        }


        private void initAllLV()
        {
            initDisciplineLV();
            initStudentiLV();

        }

        private void initDisciplineLV ()
        {
            Task.Run(() =>
            {
                    foreach (var disciplina in DBCrud.DisciplineGET().Result)
                        Dispatcher.Invoke(() => disciplineLV.Items.Add(disciplina));
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

        private void Window_Activated(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Normal) this.WindowState = WindowState.Minimized;
            else this.WindowState = WindowState.Normal;
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
        }

        private void listViewOnClick(object sender, MouseButtonEventArgs e)
        {
            if (studentiTab.IsSelected) studentSelectat = null;
            resetLVToDefault();
            enableDeleteButtonsWhenItemsAreSelected();
        }

        private void stergeButtonStudent_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                var deleted = DBCrud.StudentiDELETE(studentSelectat.NumarMatricol).Result;
                Dispatcher.Invoke(() =>
                    studentiLV.Items.Remove(studentiLV.Items.Cast<Studenti>().Where(elem => elem.NumarMatricol == deleted.NumarMatricol).First())
                );
            });
        }

        private void resetLVToDefault()
        {
            studentiLV.SelectedIndex = -1;
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
                        studentiLV.Items.Add(DBCrud.StudentiMERGE(int.Parse(codMatricol), nume, prenume).Result);
                        studentiLV.Items.Refresh();
                    });
                } else
                {
                    Dispatcher.Invoke(() => 
                    {
                        studentiLV.Items.RemoveAt(studentiLV.SelectedIndex);
                        studentiLV.Items.Add(DBCrud.StudentiMERGE(int.Parse(codMatricol), nume, prenume).Result);
                        studentiLV.Items.Refresh();
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
            if(codMatricolTextBox.IsFocused == false && numeTextBox.IsFocused == false && prenumeTextBox.IsFocused == false)
            {

                if (e.Key == Key.D1)
                    catalogTab.IsSelected = true;
                if (e.Key == Key.D2)
                    disciplineTab.IsSelected = true;
                if (e.Key == Key.D3)
                    studentiTab.IsSelected = true;
            }
        }
    }
}
