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
using JobSearchLibrary.Entities;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace JobTrackerBeta
{
    /// <summary>
    /// Interaction logic for ViewJobs.xaml
    /// </summary>
    public partial class ViewJobs : Page, INotifyPropertyChanged
    {
        JobsModel jobsModel = new JobsModel();

        public ViewJobs()
        {
            InitializeComponent();
            var companyNames = jobsModel.AllJobs.Select(x => x.CompanyName.ToString());
            lbDisplayJobs.ItemsSource = companyNames;

            //button visibility
            HideAllEditingButtons();
            HideAllEditingPlanes();
            SetJobBoxesToReadOnly();
            SetLocationBoxToReadOnly();
            SetRecruiterBoxesToReadOnly();
            editButton.Visibility = Visibility.Hidden;
        }


        private int SelectedJobID { get; set; }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Button visibility
            editButton.Visibility = Visibility.Visible;
            HideAllEditingButtons();
            HideAllEditingPlanes();

            // Set index to maintain current job in case of updates

            // Populate SelectedJob field of JobsModel 
            var nameSelected = lbDisplayJobs.SelectedItem;

            var jobSelected = jobsModel.AllJobs.Where(x => x.CompanyName == nameSelected.ToString());
            foreach (var item in jobSelected)
            {
                jobsModel.SelectedJob = item;
            }
            SelectedJobID = jobsModel.SelectedJob.CompanyId;

            // Set all text to SelectedJob on page
            SetAllTextBoxes();
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
        #region Editing Button Event Handlers (Edit, Submit, Cancel, Delete)

        //Edit Button
        private void Click_EditButton(object sender, RoutedEventArgs e)
        {
            //button visiblitiy 
            ShowAllEditingButtons();
            ShowAllEditingPlanes();

        }
        //Submit Button
        private void Click_SubmitButton(object sender, RoutedEventArgs e)
        {
            SQLConnections sql = new SQLConnections();

            if (recNameBox.IsReadOnly == false)
            {
                jobsModel.LinkedRecruiter.RecruiterName = recNameBox.Text;
                jobsModel.LinkedRecruiter.Email = recEmailBox.Text;
                jobsModel.LinkedRecruiter.LinkedInLink = recLinkBox.Text;
                jobsModel.LinkedRecruiter.PhoneNumber = recPhoneBox.Text;

                sql.UpdateRecruiter(jobsModel.LinkedRecruiter);
                ReinitializeCurrentJob();
                SetAllTextBoxes();
            }
            if (cityBox.IsReadOnly == false)
            {
                jobsModel.LinkedLocation.City = cityBox.Text;
                jobsModel.LinkedLocation.Notes = cityNotesBox.Text;

                //finish code for location updating

                //
                ReinitializeCurrentJob();
                SetAllTextBoxes();
            }

            //button visibility
            HideAllEditingButtons();
            HideAllEditingPlanes();
            SetRecruiterBoxesToReadOnly();
            SetLocationBoxToReadOnly();
            SetJobBoxesToReadOnly();

        }
        //Cancel Button
        private void Click_CancelButton(object sender, RoutedEventArgs e)
        {
            // button visibility
            HideAllEditingButtons();
            HideAllEditingPlanes();
            SetJobBoxesToReadOnly();
            SetLocationBoxToReadOnly();
            SetRecruiterBoxesToReadOnly();
            SetAllTextBoxes();
        }

        //Delete Button
        private void Click_DeleteButton(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        #region Event to make edit planes blue when mouse hovers over them 
        int jobMouseCounter = 0;
        int locationMouseCounter = 0;
        int recruiterMouseCounter = 0;
        private void MousePosition_Changed(object sender, DependencyPropertyChangedEventArgs e)
        {
            var panel = sender as Grid;
            string panelName = panel.Name;
            switch (panelName)
            {
                case "jobInfoChangedGrid":
                    if (jobMouseCounter % 2 == 0)
                    {
                        jobInfoChangedGrid.Opacity = .4;
                    }
                    if (jobMouseCounter % 2 == 1)
                    {
                        jobInfoChangedGrid.Opacity = 0;
                    }
                    jobMouseCounter++;
                    break;
                case "locationInfoChangedGrid":
                    if (locationMouseCounter % 2 == 0)
                    {
                        locationInfoChangedGrid.Opacity = .4;
                    }
                    if (locationMouseCounter % 2 == 1)
                    {
                        locationInfoChangedGrid.Opacity = 0;
                    }
                    locationMouseCounter++;
                    break;
                case "recruiterInfoChangedGrid":
                    if (recruiterMouseCounter % 2 == 0)
                    {
                        recruiterInfoChangedGrid.Opacity = .4;
                    }
                    if (recruiterMouseCounter % 2 == 1)
                    {
                        recruiterInfoChangedGrid.Opacity = 0;
                    }
                    recruiterMouseCounter++;
                    break;
            }
        }
        #endregion

        private void EditorSelector_Clicked(object sender, MouseButtonEventArgs e)
        {
            var panel = sender as Grid;
            string panelName = panel.Name;
            switch (panelName)
            {
                case "jobInfoChangedGrid":
                    HideAllEditingPlanes();
                    SetJobBoxesToReadWrite();
                    companyNameBox.IsReadOnly = false;
                    
                    break;
                case "locationInfoChangedGrid":
                    HideAllEditingPlanes();
                    SetLocationBoxesToReadWrite();
                    break;
                case "recruiterInfoChangedGrid":
                    HideAllEditingPlanes();
                    SetRecruiterBoxesToReadWrite();
                    break;

            }
        }
        private void HideAllEditingButtons()
        {
            editButton.Visibility = Visibility.Visible;
            submitButton.Visibility = Visibility.Hidden;
            cancelButton.Visibility = Visibility.Hidden;
            deleteButton.Visibility = Visibility.Hidden;
            changePageHeader.Visibility = Visibility.Hidden;
        }
        private void ShowAllEditingButtons()
        {
            deleteButton.Visibility = Visibility.Visible;
            submitButton.Visibility = Visibility.Visible;
            cancelButton.Visibility = Visibility.Visible;
            editButton.Visibility = Visibility.Hidden;
            changePageHeader.Visibility = Visibility.Visible;
        }
        private void HideAllEditingPlanes()
        {
            jobInfoChangedGrid.Visibility = Visibility.Hidden;
            locationInfoChangedGrid.Visibility = Visibility.Hidden;
            recruiterInfoChangedGrid.Visibility = Visibility.Hidden;

        }
        private void ShowAllEditingPlanes()
        {
            jobInfoChangedGrid.Visibility = Visibility.Visible;
            locationInfoChangedGrid.Visibility = Visibility.Visible;
            recruiterInfoChangedGrid.Visibility = Visibility.Visible;

        }

        private void SetAllTextBoxes()
        {
            companyNameBox.Text = jobsModel.SelectedJob.CompanyName;
            commentsBox.Text = jobsModel.SelectedJob.Comments;
            positionBox.Text = jobsModel.SelectedJob.Position;
            salaryBox.Text = jobsModel.SelectedJob.SalaryRange;
            ceoNameBox.Text = jobsModel.SelectedJob.CEOName;
            commentsBox.Text = jobsModel.SelectedJob.Comments;
            ratingBox.Text = jobsModel.SelectedJob.Rating;
            missionStatementBox.Text = jobsModel.SelectedJob.MissionStatement;
            benefitsBox.Text = jobsModel.SelectedJob.Benefits;
            runLinkBox.Text = jobsModel.SelectedJob.JobLink;

            cityBox.Text = jobsModel.LinkedLocation.City;
            stateBox.Text = jobsModel.LinkedLocation.State;
            cityNotesBox.Text = jobsModel.LinkedLocation.Notes;
            string cityRatingString = jobsModel.LinkedLocation.CityRating.ToString() + ": " + jobsModel.LinkedLocation.RatingConverted;
            cityRatingBox.Text = cityRatingString;
           
            stateCapitalBlock.Text = jobsModel.LinkedLocation.StateCapital;
            largestCityBlock.Text = jobsModel.LinkedLocation.LargestCity;

            recNameBox.Text = jobsModel.LinkedRecruiter.RecruiterName;
            recEmailBox.Text = jobsModel.LinkedRecruiter.Email;
            recPhoneBox.Text = jobsModel.LinkedRecruiter.PhoneNumber;
            recLinkBox.Text = jobsModel.LinkedRecruiter.LinkedInLink;

        }

        private void SetAllJobBoxes()
        {
            companyNameBox.Text = jobsModel.SelectedJob.CompanyName;
            commentsBox.Text = jobsModel.SelectedJob.Comments;
            positionBox.Text = jobsModel.SelectedJob.Position;
            salaryBox.Text = jobsModel.SelectedJob.SalaryRange;
            ceoNameBox.Text = jobsModel.SelectedJob.CEOName;
            commentsBox.Text = jobsModel.SelectedJob.Comments;
            ratingBox.Text = jobsModel.SelectedJob.Rating;
            missionStatementBox.Text = jobsModel.SelectedJob.MissionStatement;
            benefitsBox.Text = jobsModel.SelectedJob.Benefits;
            runLinkBox.Text = jobsModel.SelectedJob.JobLink;
        }

        private void SetJobBoxesToReadOnly()
        {
            companyNameBox.IsReadOnly = true;
            positionBox.IsReadOnly = true;
            salaryBox.IsReadOnly = true;
            ceoNameBox.IsReadOnly = true;
            commentsBox.IsReadOnly = true;
            ratingBox.IsReadOnly = true;
            missionStatementBox.IsReadOnly = true;
            benefitsBox.IsReadOnly = true;
            runLinkBox.IsReadOnly = true;
        }

        private void SetJobBoxesToReadWrite()
        {
            companyNameBox.IsReadOnly = false;
            positionBox.IsReadOnly = false;
            salaryBox.IsReadOnly = false;
            ceoNameBox.IsReadOnly = false;
            commentsBox.IsReadOnly = false;
            ratingBox.IsReadOnly = false;
            missionStatementBox.IsReadOnly = false;
            benefitsBox.IsReadOnly = false;
            runLinkBox.IsReadOnly = false;
        }

        private void SetLocationBoxToReadOnly()
        {
            cityBox.IsReadOnly = true;
            stateBox.IsReadOnly = true;
            cityBox.IsReadOnly = true;
            cityNotesBox.IsReadOnly = true;

        }
        private void SetLocationBoxesToReadWrite()
        {
            cityBox.IsReadOnly = false;
            stateBox.IsReadOnly = false;
            cityBox.IsReadOnly = false;
            cityNotesBox.IsReadOnly = false;
        }
        private void SetRecruiterBoxesToReadOnly()
        {
            recNameBox.IsReadOnly = true;
            recEmailBox.IsReadOnly = true;
            recPhoneBox.IsReadOnly = true;
            recLinkBox.IsReadOnly = true;
        }
        private void SetRecruiterBoxesToReadWrite()
        {
            recNameBox.IsReadOnly = false;
            recEmailBox.IsReadOnly = false;
            recPhoneBox.IsReadOnly = false;
            recLinkBox.IsReadOnly = false;
        }

        private void ReinitializeCurrentJob()
        {
            jobsModel = new JobsModel();
            jobsModel.SelectedJob = jobsModel.AllJobs.Where(x => x.CompanyId == SelectedJobID).FirstOrDefault();
        }


        #region Hyperlink Functionality Event Handlers
        private bool _hasValidURI;

        public bool HasValidURI
        {
            get { return _hasValidURI; }
            set { _hasValidURI = value; OnPropertyChanged("HasValidURI"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Uri uri;
            HasValidURI = Uri.TryCreate((sender as TextBox).Text, UriKind.Absolute, out uri);
        }

        private void TextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Uri uri;
            if (Uri.TryCreate((sender as TextBox).Text, UriKind.Absolute, out uri))
            {
                Process.Start(new ProcessStartInfo(uri.AbsoluteUri));
            }
        }
        #endregion
    }
}
