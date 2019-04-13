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
using JobTrackerBeta.ViewModel;
using JobSearchLibrary.Entities;

namespace JobTrackerBeta
{
    /// <summary>
    /// Interaction logic for JobEntry.xaml
    /// </summary>
    public partial class JobEntry : Page
    {
        JobsModel jobsModel = new JobsModel();
        PositionModel positionModel = new PositionModel();
        LocationViewModel locationView = new LocationViewModel();
        RatingModel ratingModel = new RatingModel();
        RecruiterModel recruiterModel = new RecruiterModel();
       
        public JobEntry()
        {
            InitializeComponent();
            canceledMessage.Visibility = Visibility.Hidden;
        }
        

        private void Click_SubmitButton(object sender, RoutedEventArgs e)
        {
            SQLConnections sqlconnection = new SQLConnections();
           
            // get comobox
            var comboBoxRating = boxRating as ComboBox;
            var comboBoxPosition = boxPosition as ComboBox;
            var comboBoxLocation = boxLocation as ComboBox;
            
            //set selected value to string
            string ratingSelected = comboBoxRating.SelectedItem as string;
            string positionSelected = comboBoxPosition.SelectedItem as string;
            string locationSelected = comboBoxLocation.SelectedItem as string;

            int? recruiterID = null;
            if (!string.IsNullOrEmpty(txtRecruiterName.Text) || !string.IsNullOrEmpty(txtRecruiterEmail.Text) || !string.IsNullOrEmpty(txtRecruiterPhoneNumber.Text) || !string.IsNullOrEmpty(txtLinkedIn.Text))
            {
                //Assign properties for new recruiter
                recruiterModel.NewRecruiter = new Recruiter();
                recruiterModel.NewRecruiter.RecruiterName = txtRecruiterName.Text;
                recruiterModel.NewRecruiter.PhoneNumber = txtRecruiterPhoneNumber.Text;
                recruiterModel.NewRecruiter.Email = txtRecruiterEmail.Text;
                recruiterModel.NewRecruiter.LinkedInLink = txtLinkedIn.Text;
                recruiterID = sqlconnection.AddRecruiterGetID(recruiterModel.NewRecruiter);
            }


            //Assign properties for job entry
            jobsModel.NewJobEntry = new Jobs();
            jobsModel.NewJobEntry.CompanyName = txtCompanyName.Text;
            jobsModel.NewJobEntry.SalaryRange = txtSalary.Text;
            jobsModel.NewJobEntry.CEOName = txtCEO.Text;
            jobsModel.NewJobEntry.MissionStatement = txtMissionStatement.Text;
            jobsModel.NewJobEntry.Benefits = txtBenefits.Text;
            jobsModel.NewJobEntry.Comments = txtComments.Text;
            jobsModel.NewJobEntry.JobLink = txtJobLink.Text;
            jobsModel.NewJobEntry.RecruiterId = recruiterID;


            //Update positionModel to reflect updates on positions
            positionModel = new PositionModel();
            var position = positionModel.AllPositions.Where(x => x.JobTitle == positionSelected);
            foreach (var item in position)
            {
                jobsModel.NewJobEntry.PositionId = item.PositionID;
            }
            var location = locationView.AllLocations.Where(x => x.City == locationSelected);
            foreach (var item in location)
            {
                jobsModel.NewJobEntry.LocationId = item.LocationId;
            }
            int ratingId = GetRatingID(ratingSelected);
            jobsModel.NewJobEntry.RatingId = ratingId;



            // Validation
            if (string.IsNullOrEmpty(txtCompanyName.Text) || positionSelected is null || locationSelected is null || ratingSelected is null)
            {
                MessageBox.Show("Please make sure Company Name, Postion, Rating, and Location are all filled out.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Insert to Jobs table in database
            sqlconnection.NewJob(jobsModel.NewJobEntry);
            MessageBoxResult result = MessageBox.Show("Submission complete!", "Success!", MessageBoxButton.OKCancel, MessageBoxImage.None);
            if (result == MessageBoxResult.OK)
            {
                //clear text boxes
                txtBenefits.Clear();
                txtCEO.Clear();
                txtComments.Clear();
                txtCompanyName.Clear();
                txtJobLink.Clear();
                txtMissionStatement.Clear();
                txtRecruiterEmail.Clear();
                txtRecruiterName.Clear();
                txtRecruiterPhoneNumber.Clear();
                txtSalary.Clear();
                boxLocation.SelectedItem = null;
                boxPosition.SelectedItem = null;
                txtLinkedIn.Clear();
                 
                boxRating.SelectedItem = null;
                canceledMessage.Visibility = Visibility.Hidden;
            }
            else
            {
                sqlconnection.DeleteLastJobRecord();
                canceledMessage.Visibility = Visibility.Visible;
            }


        }
        
        private void PositionBox_Changed(object sender, SelectionChangedEventArgs e)
        {
            bool addPosition = false;
            var comboBox = sender as ComboBox;
            string selection = comboBox.SelectedItem as string;
            addPosition = CheckPositionSelected(selection);
            if(addPosition == true)
            {
                AddPositionWindow addPositionWindow = new AddPositionWindow();
                addPositionWindow.Show();
                comboBox.SelectedIndex = -1;
            }

        }

        private void PositionBox_Loaded(object sender, EventArgs e)
        {
            positionModel = new PositionModel();
            var comboBox = sender as ComboBox;
            var positions = positionModel.AllPositions;
            if (positions.Last() != positions.Where(x => x.JobTitle == "AddLocation"))
            {
                positions.Add(new JobSearchLibrary.Entities.Position { JobTitle = "Add Position" });
            }
            comboBox.ItemsSource = positions.Select(x => x.JobTitle);
        }
        private void LocationBox_Changed(object sender, SelectionChangedEventArgs e)
        {
            bool addLocation = false;
            var locationBox = sender as ComboBox;
            string selection = locationBox.SelectedItem as string;
            addLocation = CheckPositionSelected(selection);
            if (addLocation == true)
            {
                MessageBoxResult result = MessageBox.Show("You are about to navigate away from this page. All input will have to be re-entered after adding a location", "Warning", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    NavigationService nav = NavigationService.GetNavigationService(this);
                    nav.Navigate(new System.Uri("LocationEntry.xaml", UriKind.RelativeOrAbsolute));
                }
                else
                {
                    locationBox.SelectedIndex = -1;
                    return;
                }
            }

        }

        private void LocationBox_Loaded(object sender, EventArgs e)
        {
            var locationModel = new LocationViewModel();
            var comboBox = sender as ComboBox;
            var locations = locationModel.AllLocations;
            if(locations.Last() != locations.Where(x=>x.City =="AddLocation"))
            {
                locations.Add(new JobSearchLibrary.Entities.Location { City = "Add Location" });
            }
            comboBox.ItemsSource = locations.Select(x => x.City);
        }

        private void RatingBox_Loaded(object sender, RoutedEventArgs e)
        {
            var ratings = ratingModel.AllRatings.Select(x => x.RatingDescription);

            // get combobox ref
            var comboBox = sender as ComboBox;

            // assign itemsource to list above
            comboBox.ItemsSource = ratings;
        }

       
        private bool CheckPositionSelected(string boxSelection)
        {
            if (boxSelection == "Add Position")
            {
                return true;
            }
            if (boxSelection == "Add Location")
            {
                return true;
            }
            else
                return false;
        }

        private int GetRatingID(string rating)
        {
            if (rating == "Overqualified")
            {
                return 1;
            }
            else if (rating == "Qualified")
            {
                return 2;
            }
            else if (rating == "Challenging")
            {
                return 3;
            }
            else if (rating == "Need more to apply")
            {
                return 5;
            }
            else
                return 0;
        }
    }
}
