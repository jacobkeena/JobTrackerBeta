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
    /// Interaction logic for SearchResults.xaml
    /// </summary>
    public partial class SearchResults : Page
    {
        private string SearchString {get; set;}
        List<string> SearchResult = new List<string>();
        public SearchResults()
        {
            InitializeComponent();
            SearchString = ((MenuWindow)Application.Current.MainWindow).SearchBox.Text;
        }

        private void SearchListBox_Loaded(object sender, RoutedEventArgs e)
        {

            if (SearchResult.Count == 0)
            {
                SearchListBox.BorderBrush = new SolidColorBrush(Colors.Red);
                SearchResult.Add("No results found...");
                SearchListBox.ItemsSource = SearchResult;
                SearchListBox.IsHitTestVisible = false;
            }
            else
            {
                SearchListBox.ItemsSource = SearchResult;
            }
        }
    }
}
