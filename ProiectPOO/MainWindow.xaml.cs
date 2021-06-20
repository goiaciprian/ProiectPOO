using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Diagnostics;


namespace ProiectPOO
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        StudentiGET_Result studentSelectat = null;
        public MainWindow()
        {
            InitializeComponent();
            initAllLV();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(studentiTab.IsSelected) studentSelectat = (StudentiGET_Result)studentiLV.SelectedItem;

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
                using (var dbContext = new databaseiesc())
                {
                    foreach (var disciplina in dbContext.DisciplineGET())
                        Dispatcher.Invoke(() => disciplineLV.Items.Add(disciplina));
                }

            });

        }

        private void initStudentiLV ()
        {
            Task.Run(() =>
            {
                Dispatcher.Invoke(() => studentiLV.Items.Clear());
                using (var dbContext = new databaseiesc())
                {
                    foreach (StudentiGET_Result student in dbContext.StudentiGET())
                        Dispatcher.Invoke(() => studentiLV.Items.Add(student));
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
            if (studentSelectat != null) stergeButtonStudent.IsEnabled = true;
            else stergeButtonStudent.IsEnabled = false;
        }

        private void listViewOnClick(object sender, MouseButtonEventArgs e)
        {
            resetLVToDefault();
            if (studentiTab.IsSelected) studentSelectat = null;
            enableDeleteButtonsWhenItemsAreSelected();
        }

        private void stergeButtonStudent_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                using (var dbContext = new databaseiesc())
                {
                    dbContext.StudentiDELETE(studentSelectat.NumarMatricol);
                    initStudentiLV();
                }
            });
            resetLVToDefault();
        }

        private void resetLVToDefault()
        {
            studentiLV.SelectedIndex = -1;
        }

        private void adaugareModificareButton_Click(object sender, RoutedEventArgs e)
        {
            if(studentSelectat == null )
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
                    using (var context = new databaseiesc())
                    {
                        foreach (var adaugat in context.StudentiMERGE(int.Parse(codMatricol), nume, prenume))
                        {
                            Debug.WriteLine(adaugat.NumarMatricol);
                        }
                        initStudentiLV();
                    }
                });

            }
            codMatricolTextBox.Text = "";
            numeTextBox.Text = "";
            prenumeTextBox.Text = "";
            codMatricolTextBox.BorderBrush = System.Windows.Media.Brushes.Transparent;
            numeTextBox.BorderBrush = System.Windows.Media.Brushes.Transparent;
            prenumeTextBox.BorderBrush = System.Windows.Media.Brushes.Transparent;
        }

    }
}
