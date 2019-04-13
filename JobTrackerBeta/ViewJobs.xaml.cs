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
using System.Diagnostics;
using JobTrackerBeta.ViewModel;

namespace JobTrackerBeta
{
    /// <summary>
    /// Interaction logic for ViewJobs.xaml
    /// </summary>
    public partial class ViewJobs : Page
    {
        Dictionary<int, string> items = new Dictionary<int, string>();
        JobsModel jobsModel = new JobsModel();
        public ViewJobs()
        {
            InitializeComponent();
            var companyNames = jobsModel.AllJobs.Select(x => x.CompanyName.ToString());
            lbDisplayJobs.ItemsSource = companyNames;

            //button visibility
            editButton.Visibility = Visibility.Hidden;
            submitButton.Visibility = Visibility.Hidden;
            cancelButton.Visibility = Visibility.Hidden;
            deleteButton.Visibility = Visibility.Hidden;
            changePageHeader.Visibility = Visibility.Hidden;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //button visibility
            editButton.Visibility = Visibility.Visible;
            submitButton.Visibility = Visibility.Hidden;
            cancelButton.Visibility = Visibility.Hidden;
            deleteButton.Visibility = Visibility.Hidden;

            var nameSelected = lbDisplayJobs.SelectedItem;

            var jobSelected = jobsModel.AllJobs.Where(x => x.CompanyName == nameSelected.ToString());
         

            foreach (var item in jobSelected)
            {
                jobsModel.SelectedJob = item;
            }

            // get empty textblocks
            var companyName = companyNameBlock as TextBlock;
            var jobTitle = positionBlock as TextBlock;
            var salaryRange = salaryBlock as TextBlock;
            var ceoName = ceoNameBlock as TextBlock;
            var comments = commentsBlock as TextBlock;
            var rating = ratingBlock as TextBlock;
            var missionStatement = missionStatementBlock as TextBlock;
            var joblink = runBox as Run;
            var benefits = benefitsBlock as TextBlock;

            var city = cityBlock as TextBlock;
            var state = stateBlock as TextBlock;
            var cityRating = cityRatingBlock as TextBlock;
            var cityNotes = notesBlock as TextBox;
            var stateCaptital = stateCapitalBlock as TextBlock;
            var largestCity = largestCityBlock as TextBlock;

            var rName = recruiterNameTxt as TextBlock;
            var rEmail = recruiterEmailTxt as TextBlock;
            var rPhone = recruiterPhoneTxt as TextBlock;
            var rLink = recruiterRunBox as Run;


            // set text for display
            companyName.Text = jobsModel.SelectedJob.CompanyName;
            comments.Text = jobsModel.SelectedJob.Comments;
            jobTitle.Text = jobsModel.SelectedJob.Position;
            salaryRange.Text = jobsModel.SelectedJob.SalaryRange;
            ceoName.Text = jobsModel.SelectedJob.CEOName;
            comments.Text = jobsModel.SelectedJob.Comments;
            rating.Text = jobsModel.SelectedJob.Rating;
            missionStatement.Text = jobsModel.SelectedJob.MissionStatement;
            benefits.Text = jobsModel.SelectedJob.Benefits;
            joblink.Text = jobsModel.SelectedJob.JobLink;
            city.Text = jobsModel.SelectedJob.Location;

            state.Text = jobsModel.LinkedLocation.State;
            cityRating.Text = jobsModel.LinkedLocation.RatingConverted;
            cityNotes.Text = jobsModel.LinkedLocation.Notes;
            stateCaptital.Text = jobsModel.LinkedLocation.StateCapital;
            largestCity.Text = jobsModel.LinkedLocation.LargestCity;

            rName.Text = jobsModel.LinkedRecruiter.RecruiterName;
            rEmail.Text = jobsModel.LinkedRecruiter.Email;
            rPhone.Text = jobsModel.LinkedRecruiter.PhoneNumber;
            rLink.Text = jobsModel.LinkedRecruiter.LinkedInLink;
        }

        private void Hyperlink_Requested(object sender, RequestNavigateEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo(jobsModel?.SelectedJob.JobLink));
                e.Handled = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Sorry but that link is invalid. Try copying and pasting it into your browser!", "Error", MessageBoxButton.OK);
            }
        }

        private void Click_EditButton(object sender, RoutedEventArgs e)
        {
            //button visiblitiy 
            deleteButton.Visibility = Visibility.Visible;
            submitButton.Visibility = Visibility.Visible;
            cancelButton.Visibility = Visibility.Visible;
            editButton.Visibility = Visibility.Hidden;
            changePageHeader.Visibility = Visibility.Visible;

        }

        private void Click_SubmitButton(object sender, RoutedEventArgs e)
        {

        }

        private void Click_CancelButton(object sender, RoutedEventArgs e)
        {
            // button visibility
            editButton.Visibility = Visibility.Visible;
            submitButton.Visibility = Visibility.Hidden;
            cancelButton.Visibility = Visibility.Hidden;
            deleteButton.Visibility = Visibility.Hidden;
            changePageHeader.Visibility = Visibility.Hidden;
        }
    }
}
