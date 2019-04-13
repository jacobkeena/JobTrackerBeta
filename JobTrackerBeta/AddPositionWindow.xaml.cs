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
using JobSearchLibrary;
using JobTrackerBeta.ViewModel;
using JobSearchLibrary.Entities;

namespace JobTrackerBeta
{
    /// <summary>
    /// Interaction logic for AddPositionWindow.xaml
    /// </summary>
    public partial class AddPositionWindow : Window
    {
        SQLConnections sqlConnections = new SQLConnections();
        PositionModel positionModel = new PositionModel();
        public AddPositionWindow()
        {
            InitializeComponent();
        }

        private void AddPosition_Clicked(object sender, RoutedEventArgs e)
        {
            positionModel.NewPosition = new Position();
            positionModel.NewPosition.JobTitle = NewPosition.Text;
            sqlConnections.AddPosition(positionModel.NewPosition.JobTitle);
            this.Close();
        }
    }
}
