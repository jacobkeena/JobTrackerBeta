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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void NewLocationButton_Click(object sender, RoutedEventArgs e)
        {

            PopulatePage.Navigate(new System.Uri("LocationEntry.xaml",
             UriKind.RelativeOrAbsolute));
        }

        private void NewJobButton_Click(object sender, RoutedEventArgs e)
        {
            PopulatePage.Navigate(new System.Uri("JobEntry.xaml",
             UriKind.RelativeOrAbsolute));
        }

        private void ViewJobButton_Click(object sender, RoutedEventArgs e)
        {
            PopulatePage.Navigate(new System.Uri("ViewJobs.xaml", UriKind.RelativeOrAbsolute));
        }

        private void ViewLocationsButton_Click(object sender, RoutedEventArgs e)
        {
            PopulatePage.Navigate(new System.Uri("ViewLocations.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}
