using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JobTrackerBeta
{
    /// <summary>
    /// Interaction logic for Manage.xaml
    /// </summary>
    public partial class Manage : Page
    {
        public Manage()
        {
            InitializeComponent();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dgEmp_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

        }

        private void dgEmp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void dgEmp_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {

        }

        private void Filter_Selected(object sender, SelectionChangedEventArgs e)
        {
            string filter = FilterBox.SelectedValue.ToString();
            switch (filter)
            {
                case "System.Windows.Controls.ComboBoxItem: All":
                    break;
                case "System.Windows.Controls.ComboBoxItem: Jobs":
                    SetGridToJobs();
                    break;
                case "System.Windows.Controls.ComboBoxItem: Locations":
                    SetGridToLocations();
                    break;
                case "System.Windows.Controls.ComboBoxItem: Positions":
                    SetGridToPositions();
                    break;
            }
        }
        private void SetGridToAll()
        {
            ShowAllFilterBoxes();
        }
        public void SetGridToLocations()
        {
            ShowAllFilterBoxes();
            Column0Label.Content = "City: ";
            Column1Label.Content = "State: ";
            Column2Label.Content = "Rating: ";
            Column1.Header = "City";
            Column2.Header = "State";
            Column3.Header = "Rating";
            Column4.Header = "Notes";
        }
        public void SetGridToPositions()
        {
            ShowAllFilterBoxes();
            Column0Label.Content = "Position: ";
            Column1Label.Visibility = Visibility.Hidden;
            Column2Label.Visibility = Visibility.Hidden;
            Column1TextBox.Visibility = Visibility.Hidden;
            Column2TextBox.Visibility = Visibility.Hidden;
            Column0Label.Content = "Position: ";

            Column1.Header = "Position";
            Column2.Header = null;
            Column3.Header = null;
            Column4.Header = null;
        }
        public void SetGridToJobs()
        {
            ShowAllFilterBoxes();
            Column0Label.Content = "Co Name: ";
            Column1Label.Content = "Rating: ";
            Column2Label.Content = "Position: ";

            Column1.Header = "Company Name";
            Column2.Header = "CEO Name";
            Column3.Header = "Salary Range";
            Column4.Header = "Comments";
        }

        private void ShowAllFilterBoxes()
        {
            Column0Label.Visibility = Visibility.Visible;
            Column1Label.Visibility = Visibility.Visible;
            Column2Label.Visibility = Visibility.Visible;
            Column0TextBox.Visibility = Visibility.Visible;
            Column1TextBox.Visibility = Visibility.Visible;
            Column2TextBox.Visibility = Visibility.Visible;

        }
    }
}
