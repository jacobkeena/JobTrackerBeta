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
using System.Windows.Shapes;

namespace JobTrackerBeta
{
    /// <summary>
    /// Interaction logic for MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        public MenuWindow()
        {
            InitializeComponent();
        }
        private void MenuWindow_Loaded(object sender, RoutedEventArgs e)
        {
            PopulatePage.Navigate(new System.Uri("HomePage.xaml",
    UriKind.RelativeOrAbsolute));
        }
        private void HomeButton_Clicked(object sender, RoutedEventArgs e)
        {

            PopulatePage.Navigate(new System.Uri("HomePage.xaml",
             UriKind.RelativeOrAbsolute));
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

      
        private void ManageButton_Click(object sender, RoutedEventArgs e)
        {
            PopulatePage.Navigate(new System.Uri("Manage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void SearchBox_Clicked(object sender, RoutedEventArgs e)
        {
            SearchBox.Visibility = Visibility.Visible;
            SearchBox.Focus();
        }

        private void SearchBox_Unfocused(object sender, RoutedEventArgs e)
        {
            //SearchBox.Visibility = Visibility.Hidden;
            //SearchBox.Clear();
        }

        private void SearchChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void SearchButton_Clicked(object sender, RoutedEventArgs e)
        {
            PopulatePage.Navigate(new System.Uri("SearchResults.xaml", UriKind.RelativeOrAbsolute));

        }
    }
}
