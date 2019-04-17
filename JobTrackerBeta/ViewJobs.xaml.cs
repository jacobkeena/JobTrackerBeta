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
        LocationViewModel locationModel = new LocationViewModel();
        SQLConnections sql = new SQLConnections();


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
        private bool IsRecruiterInitialized { get; set; }
       

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Button visibility
            editButton.Visibility = Visibility.Visible;
            HideAllEditingButtons();
            HideAllEditingPlanes();

            try
            {
                // Populate SelectedJob field of JobsModel 
                var nameSelected = lbDisplayJobs.SelectedItem;

                jobsModel.SelectedJob = jobsModel.AllJobs.Where(x => x.CompanyName == nameSelected.ToString()).FirstOrDefault();

                //Sets ID so it current job selection is maintained upon updates
                SelectedJobID = jobsModel.SelectedJob.CompanyId;

                // Set all text to SelectedJob on page
                SetAllTextBoxes();
            }
            catch (Exception)
            {
                lbDisplayJobs.SelectedIndex = -1;
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

            // Update Recruiter Information
            if (recNameBox.IsReadOnly == false)
            {
                Recruiter newRecruiter = new Recruiter();

                newRecruiter.RecruiterId = jobsModel.LinkedRecruiter.RecruiterId;
                newRecruiter.RecruiterName = recNameBox.Text;
                newRecruiter.Email = recEmailBox.Text;
                newRecruiter.LinkedInLink = recLinkBox.Text;
                newRecruiter.PhoneNumber = recPhoneBox.Text;
                sql.UpdateRecruiter(newRecruiter);

                ReinitializeCurrentJob();
                SetAllTextBoxes();
            }

            // Update Location Information
            if (cityCBox.IsHitTestVisible == true)
            {
                sql.UpdateJobLocationID(locationModel.NewLocation.LocationId, jobsModel.SelectedJob.CompanyId);
                ReinitializeCurrentJob();
                SetAllTextBoxes();
            }

            // Update Job Information
            if (companyNameBox.IsReadOnly == false)
            {
                jobsModel.NewJobEntry = new Jobs();
                jobsModel.NewJobEntry.CompanyId = SelectedJobID;
                jobsModel.NewJobEntry.CompanyName = companyNameBox.Text;
                jobsModel.NewJobEntry.SalaryRange = salaryBox.Text;
                jobsModel.NewJobEntry.CEOName = ceoNameBox.Text;
                jobsModel.NewJobEntry.MissionStatement = missionStatementBox.Text;
                jobsModel.NewJobEntry.Benefits = benefitsBox.Text;
                jobsModel.NewJobEntry.Comments = commentsBox.Text;
                jobsModel.NewJobEntry.JobLink = runLinkBox.Text;

                PositionModel positionModel = new PositionModel();
                var position = positionModel.AllPositions.Where(x => x.JobTitle == positionCBox.Text);
                foreach (var item in position)
                {
                    jobsModel.NewJobEntry.PositionId = item.PositionID;
                }

                int ratingId = sql.GetCompanyRatingID(ratingCBox.SelectedItem.ToString());
                jobsModel.NewJobEntry.RatingId = ratingId;

                // Validation
                if (string.IsNullOrEmpty(companyNameBox.Text) || string.IsNullOrEmpty(positionCBox.Text) || string.IsNullOrEmpty(ratingCBox.Text))
                {
                    MessageBox.Show("Please make sure Company Name, Postion, and Rating are all filled out.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                sql.UpdateJobInformation(jobsModel.NewJobEntry);
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
            MessageBoxResult result = new MessageBoxResult();
            if (jobsModel.SelectedJob.RecruiterId.HasValue)
            {
                result = MessageBox.Show("Do you want to delete the linked recruiter information with this job as well?", "Delete", MessageBoxButton.YesNoCancel, MessageBoxImage.Exclamation);
                if (result == MessageBoxResult.Yes)
                {
                    sql.DeleteSpecificJobWithRecruiter(jobsModel.SelectedJob.CompanyId, jobsModel.SelectedJob.RecruiterId.Value);

                    //button visibility
                    HideAllEditingButtons();
                    HideAllEditingPlanes();
                    SetRecruiterBoxesToReadOnly();
                    SetLocationBoxToReadOnly();
                    SetJobBoxesToReadOnly();

                    ReinitializeListBox();
                }
                if (result == MessageBoxResult.No)
                {
                    result = MessageBox.Show("Recruiter information can be found in View Attributes page. Delete job information now?", "Delete", MessageBoxButton.OKCancel, MessageBoxImage.None);
                    if (result == MessageBoxResult.OK)
                    {
                        sql.DeleteSpecificJob(jobsModel.SelectedJob.CompanyId);

                        //button visibility
                        HideAllEditingButtons();
                        HideAllEditingPlanes();
                        SetRecruiterBoxesToReadOnly();
                        SetLocationBoxToReadOnly();
                        SetJobBoxesToReadOnly();

                        ReinitializeListBox();
                    }
                    else
                        return;
                }
                
            }
            else
            {
                result = MessageBox.Show("Are you sure you want to delete this job? This cannot be undone", "Delete", MessageBoxButton.YesNoCancel, MessageBoxImage.Exclamation);
                if (result == MessageBoxResult.Yes)
                {
                    sql.DeleteSpecificJob(jobsModel.SelectedJob.CompanyId);

                    //button visibility
                    HideAllEditingButtons();
                    HideAllEditingPlanes();
                    SetRecruiterBoxesToReadOnly();
                    SetLocationBoxToReadOnly();
                    SetJobBoxesToReadOnly();

                    ReinitializeListBox();
                }
                else
                    return;
            }

        }
        #endregion

        #region Events for blue edit selector panels

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
                    MessageBoxResult result;
                    if (IsRecruiterInitialized == false)
                    {
                        result = MessageBox.Show("This job was not created with a recruiter. Would you like to add one?", "Recruiter", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result == MessageBoxResult.Yes)
                        {
                            Recruiter recruiter = new Recruiter()
                            {
                                RecruiterName = "",
                                PhoneNumber = "",
                                LinkedInLink = "",
                                Email = ""
                            };
                            SQLConnections sql = new SQLConnections();
                            int newRecruiterID = sql.AddRecruiterGetID(recruiter);
                            sql.UpdateJobWithRecruiterID(newRecruiterID, jobsModel.SelectedJob.CompanyId);
                            jobsModel = new JobsModel();
                            jobsModel.SelectedJob = jobsModel.AllJobs.Where(x => x.CompanyId == SelectedJobID).FirstOrDefault();
                            IsRecruiterInitialized = true;
                        }
                        else
                        {
                            ShowAllEditingPlanes();
                            return;
                        }
                    }

                    SetRecruiterBoxesToReadWrite();
                    break;

            }
        }
        #endregion

        #region Event Handlers for ComboBoxes

        private void PositionBox_Loaded(object sender, EventArgs e)
        {
            PositionModel positionModel = new PositionModel();
            var positionCBox = sender as ComboBox;
            var positions = positionModel.AllPositions;
            if (positions.Last() != positions.Where(x => x.JobTitle == "AddLocation"))
            {
                positions.Add(new JobSearchLibrary.Entities.Position { JobTitle = "Add Position" });
            }
            positionCBox.ItemsSource = positions.Select(x => x.JobTitle);
        }
        private void PositionBox_Changed(object sender, SelectionChangedEventArgs e)
        {
            bool addPosition = false;
            var comboBox = sender as ComboBox;
            string selection = comboBox.SelectedItem as string;
            addPosition = CheckPositionSelected(selection);
            if (addPosition == true)
            {
                AddPositionWindow addPositionWindow = new AddPositionWindow();
                addPositionWindow.Show();
                comboBox.SelectedIndex = -1;
            }

        }
        private void RatingBox_Loaded(object sender, RoutedEventArgs e)
        {
            RatingModel ratingModel = new RatingModel();
            var ratings = ratingModel.AllRatings.Select(x => x.RatingDescription);

            // get combobox ref
            var comboBox = sender as ComboBox;

            // assign itemsource to list above
            comboBox.ItemsSource = ratings;
        }
        private void CityBox_Loaded(object sender, RoutedEventArgs e)
        {

            var locations = locationModel.AllLocations;
            if (locations.Last() != locations.Where(x => x.City == "AddLocation"))
            {
                locations.Add(new JobSearchLibrary.Entities.Location { City = "Add Location" });
            }
            cityCBox.ItemsSource = locations.Select(x => x.City);

        }
        private void CityBox_Changed(object sender, SelectionChangedEventArgs e)
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

            if (cityCBox.IsHitTestVisible == true)
            {
                locationModel.NewLocation = new Location();
                var selectedLocation = locationModel.AllLocations.Where(x => x.City == cityCBox.SelectedItem.ToString());
                foreach (var item in selectedLocation)
                {
                    locationModel.NewLocation.LocationId = item.LocationId;
                    locationModel.NewLocation.StateId = item.StateId;
                    locationModel.NewLocation.Notes = item.Notes;
                    locationModel.NewLocation.CityRating = item.CityRating;
                }
                stateBox.Text = locationModel.NewLocation.State;
                cityRatingBox.Text = $"{locationModel.NewLocation.CityRating.ToString()}: {locationModel.NewLocation.RatingConverted}";
                cityNotesBox.Text = locationModel.NewLocation.Notes;

                stateCapitalBlock.Text = locationModel.NewLocation.StateCapital;
                largestCityBlock.Text = locationModel.NewLocation.LargestCity;
            }
        }

        #endregion

        #region Functions to set or hide boxes

        private void SetAllTextBoxes()
        {
            companyNameBox.Text = jobsModel.SelectedJob.CompanyName;
            commentsBox.Text = jobsModel.SelectedJob.Comments;
            positionCBox.Text = jobsModel.SelectedJob.Position;
            salaryBox.Text = jobsModel.SelectedJob.SalaryRange;
            ceoNameBox.Text = jobsModel.SelectedJob.CEOName;
            commentsBox.Text = jobsModel.SelectedJob.Comments;
            ratingCBox.Text = jobsModel.SelectedJob.Rating;
            missionStatementBox.Text = jobsModel.SelectedJob.MissionStatement;
            benefitsBox.Text = jobsModel.SelectedJob.Benefits;
            runLinkBox.Text = jobsModel.SelectedJob.JobLink;

            cityCBox.Text = jobsModel.LinkedLocation.City;
            stateBox.Text = jobsModel.LinkedLocation.State;
            cityNotesBox.Text = jobsModel.LinkedLocation.Notes;
            string cityRatingString = jobsModel.LinkedLocation.CityRating.ToString() + ": " + jobsModel.LinkedLocation.RatingConverted;
            cityRatingBox.Text = cityRatingString;

            stateCapitalBlock.Text = jobsModel.LinkedLocation.StateCapital;
            largestCityBlock.Text = jobsModel.LinkedLocation.LargestCity;
            try
            {
                recNameBox.Text = jobsModel.LinkedRecruiter.RecruiterName;
                recEmailBox.Text = jobsModel.LinkedRecruiter.Email;
                recPhoneBox.Text = jobsModel.LinkedRecruiter.PhoneNumber;
                recLinkBox.Text = jobsModel.LinkedRecruiter.LinkedInLink;
                IsRecruiterInitialized = true;
            }
            catch (NullReferenceException)
            {
                IsRecruiterInitialized = false;
                recNameBox.Text = jobsModel.LinkedRecruiter?.RecruiterName;
                recEmailBox.Text = jobsModel.LinkedRecruiter?.Email;
                recPhoneBox.Text = jobsModel.LinkedRecruiter?.PhoneNumber;
                recLinkBox.Text = jobsModel.LinkedRecruiter?.LinkedInLink;
            }

        }
        private void ClearAllTextBoxes()
        {
            companyNameBox.Clear();
            commentsBox.Clear();
            positionCBox.SelectedItem = null;
            positionCBox.Text = ""; // Position box will not display null value for some reason. This was the easy fix
            salaryBox.Clear();
            ceoNameBox.Clear();
            commentsBox.Clear();
            ratingCBox.SelectedItem = null;
            missionStatementBox.Clear();
            benefitsBox.Clear();
            runLinkBox.Clear();

            cityCBox.SelectedItem = null;
            stateBox.Clear();
            cityNotesBox.Clear();
            cityRatingBox.Clear();

            stateCapitalBlock.Text = null;
            largestCityBlock.Text = null;
            recNameBox.Clear();
            recEmailBox.Clear();
            recPhoneBox.Clear();
            recLinkBox.Clear();
            IsRecruiterInitialized = false;
            SelectedJobID = 0;

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

        private void SetJobBoxesToReadOnly()
        {
            companyNameBox.IsReadOnly = true;
            positionCBox.IsHitTestVisible = false;
            salaryBox.IsReadOnly = true;
            ceoNameBox.IsReadOnly = true;
            commentsBox.IsReadOnly = true;
            ratingCBox.IsHitTestVisible = false;
            missionStatementBox.IsReadOnly = true;
            benefitsBox.IsReadOnly = true;
            runLinkBox.IsReadOnly = true;
        }

        private void SetJobBoxesToReadWrite()
        {
            companyNameBox.IsReadOnly = false;
            positionCBox.IsHitTestVisible = true;
            salaryBox.IsReadOnly = false;
            ceoNameBox.IsReadOnly = false;
            commentsBox.IsReadOnly = false;
            ratingCBox.IsHitTestVisible = true;
            missionStatementBox.IsReadOnly = false;
            benefitsBox.IsReadOnly = false;
            runLinkBox.IsReadOnly = false;
        }

        private void SetLocationBoxToReadOnly()
        {
            cityCBox.IsHitTestVisible = false;
            stateBox.IsReadOnly = true;
            cityRatingBox.IsReadOnly = true;
            cityNotesBox.IsReadOnly = true;
        }
        private void SetLocationBoxesToReadWrite()
        {
            cityCBox.IsHitTestVisible = true;
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

        #endregion

        #region Tool functions

        private void ReinitializeCurrentJob()
        {
            jobsModel = new JobsModel();
            jobsModel.SelectedJob = jobsModel.AllJobs.Where(x => x.CompanyId == SelectedJobID).FirstOrDefault();
        }

        private void ReinitializeListBox()
        {
            jobsModel = new JobsModel();
            lbDisplayJobs.ItemsSource = jobsModel.AllJobs.Select(x => x.CompanyName);
            ClearAllTextBoxes();
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

        #endregion




        #region Hyperlink Functionality Event Handlers

        /*
         * I copied this code from another resource online. Its intent was to allow for an interactive hyperlink inside 
         * of a textbox. I have yet to make it work though. Lower priority as of right now.
         * 
         */
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

    }
}
