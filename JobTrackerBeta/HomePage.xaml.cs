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
using JobTrackerBeta.ViewModel;
using JobSearchLibrary.Entities;
using System.Diagnostics;

namespace JobTrackerBeta
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
            SetDataGrid();
        }
        private void SetDataGrid()
        {
            JobsModel jobsModel = new JobsModel();
            LocationViewModel locationModel = new LocationViewModel();
            PositionModel positionModel = new PositionModel();

            var recentJobs = (from j in jobsModel.AllJobs
                              from l in locationModel.AllLocations
                              from p in positionModel.AllPositions
                              where j.LocationId == l.LocationId
                              where j.PositionId == p.PositionID
                              orderby j.CompanyId descending
                              select new { j.CompanyName, l.City, p.JobTitle, j.JobLink }).Take(10).ToList();


            RecentJobsGrid.ItemsSource = recentJobs;
            //RecentJobsGrid.Columns[0].Width = RecentJobsGrid.Width / 5;
            //RecentJobsGrid.Columns[1].Width = RecentJobsGrid.Width / 5;
            //RecentJobsGrid.Columns[2].Width = RecentJobsGrid.Width / 5;
            //RecentJobsGrid.Columns[3].Width = RecentJobsGrid.Width / 5;

        }

        private void URL_Clicked(object sender, RequestNavigateEventArgs e)
        {
                try
                {
                    Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
                    e.Handled = true;
                }
                catch (Exception)
                {
                    MessageBox.Show("Sorry but that link is invalid. Try copying and pasting it into your browser!", "Error", MessageBoxButton.OK);
                }
        }
    }
}
