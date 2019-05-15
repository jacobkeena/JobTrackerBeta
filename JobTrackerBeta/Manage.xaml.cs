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
using JobSearchLibrary;
using JobSearchLibrary.Entities;

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

        private void Filter_Selected(object sender, SelectionChangedEventArgs e)
        {
            string filter = FilterBox.SelectedValue.ToString();
            switch (filter)
            {
                case "System.Windows.Controls.ComboBoxItem: All":
                    SetGridToAll();
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
            Column0Label.Content = "Keyword: ";
            Column1Label.Visibility = Visibility.Hidden;
            Column2Label.Visibility = Visibility.Hidden;
            Column1TextBox.Visibility = Visibility.Hidden;
            Column2TextBox.Visibility = Visibility.Hidden;

            SQLConnections sql = new SQLConnections();
            List<Jobs> jobsAll = sql.GetAllJobs();
            List<Location> locationsAll = sql.GetAllLocations();
            List<Position> positionsAll = sql.GetAllPositions();
            List<ColumnData> columnDataAll = new List<ColumnData>();
            foreach (var item in jobsAll)
            {
                ColumnData data = new ColumnData
                {
                    Column1 = "Job",
                    Column2 = item.CompanyName,
                    Column3 = item.CEOName,
                    Column4 = item.Rating,
                    Column5 = "Update",
                    Column6 = "Delete"
                };
                columnDataAll.Add(data);
            }
            foreach (var item in locationsAll)
            {
                ColumnData data = new ColumnData
                {
                    Column1 = "Location",
                    Column2 = item.City,
                    Column3 = item.State,
                    Column4 = item.RatingConverted,
                    Column5 = "Update",
                    Column6 = "Delete"
                };
                columnDataAll.Add(data);
            }
            foreach (var item in positionsAll)
            {
                ColumnData data = new ColumnData
                {
                    Column1 = "Position",
                    Column2 = item.JobTitle,
                    Column3 = null,
                    Column4 = null,
                    Column5 = "Update",
                    Column6 = "Delete"

                };
                columnDataAll.Add(data);
            }
            ResultBox.ItemsSource = columnDataAll;
        }
        public void SetGridToLocations()
        {
            ShowAllFilterBoxes();
            Column0Label.Content = "City: ";
            Column1Label.Content = "State: ";
            Column2Label.Content = "Rating: ";


            SQLConnections sql = new SQLConnections();

            List<Location> locations = sql.GetAllLocations();
            List<ColumnData> columnDataLocations = new List<ColumnData>();
            foreach (var item in locations)
            {

                ColumnData data = new ColumnData
                {
                    Column1 = item.City,
                    Column2 = item.State,
                    Column3 = item.RatingConverted,
                    Column4 = item.Notes,
                    Column5 = "Update",
                    Column6 = "Delete"
                };
                columnDataLocations.Add(data);
            }
            ResultBox.ItemsSource = columnDataLocations;
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

            SQLConnections sql = new SQLConnections();

            List<Position> positions = sql.GetAllPositions();
            List<ColumnData> columnDataPositions = new List<ColumnData>();
            foreach (var item in positions)
            {
                ColumnData data = new ColumnData
                {
                    Column1 = item.JobTitle,
                    Column2 = null,
                    Column3 = null,
                    Column4 = null,
                    Column5 = "Update",
                    Column6 = "Delete"
                };
                
                columnDataPositions.Add(data);
            }
            ResultBox.ItemsSource = columnDataPositions;


        }
        public void SetGridToJobs()
        {
            ShowAllFilterBoxes();
            Column0Label.Content = "Co Name: ";
            Column1Label.Content = "Rating: ";
            Column2Label.Content = "Position: ";

            SQLConnections sql = new SQLConnections();

            List<Jobs> jobs = sql.GetAllJobs();
            List<ColumnData> columnDataJobs = new List<ColumnData>();
            foreach (var item in jobs)
            {
                ColumnData data = new ColumnData
                {
                    Column1 = item.CompanyName,
                    Column2 = item.CEOName,
                    Column3 = item.Rating,
                    Column4 = item.Comments,
                    Column5 = "Update",
                    Column6 = "Delete"
                };
                columnDataJobs.Add(data);
            }
            ResultBox.ItemsSource = columnDataJobs;

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

        

        private void SearchButton_Clicked(object sender, RoutedEventArgs e)
        {
            SQLConnections sql = new SQLConnections();

            switch (FilterBox.Text)
            {
                case "All":

                    break;
                case "Jobs":

                    break;
                case "Locations":

                    break;
                case "Positions":

                    break;
            }
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {

        }
    }
    internal class ColumnData
    {
        public string Column1 { get; set; }
        public string Column2 { get; set; }
        public string Column3 { get; set; }
        public string Column4 { get; set; }
        public string Column5 { get; set; }
        public object Column6 { get; set; }
    }
}
