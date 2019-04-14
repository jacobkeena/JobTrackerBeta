﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobSearchLibrary;
using JobSearchLibrary.Entities;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JobTrackerBeta.ViewModel;


namespace JobTrackerBeta.ViewModel
{
    public class JobsModel : INotifyPropertyChanged
    {
        public JobsModel()
        {
            FillAllJobs();

        }

        private void FillAllJobs()
        {
            SQLConnections sql = new SQLConnections();
            AllJobs = sql.GetAllJobs();
        }


        private List<Jobs> _allJobs;
        public List<Jobs> AllJobs
        {
            get
            {
                return _allJobs;
            }
            set
            {
                _allJobs = value;
                NotifyPropertyChanged();
            }
        }



        private List<Position> _allPositions;
        public List<Position> AllPositions
        {
            get
            {
                return _allPositions;
            }
            set
            {
                _allPositions = value;
                NotifyPropertyChanged();
            }
        }

        private Jobs _selectedJob;
        public Jobs SelectedJob
        {
            get
            {
                return _selectedJob;
            }
            set
            {
                _selectedJob = value;
                NotifyPropertyChanged();
                FillLinkedLocation();
                FillLinkedRecrutier();

            }
        }
        private Location _linkedLocation;
        public Location LinkedLocation
        {
            get
            {
                return _linkedLocation;
            }
            set
            {
                NotifyPropertyChanged();
            }
        }
        private Jobs _newJobEntry;
        public Jobs NewJobEntry
        {
            get
            {
                return _newJobEntry;
            }
            set
            {
                _newJobEntry = value;
                NotifyPropertyChanged();
            }
        }
        private Recruiter _linkedRecruiter;
        public Recruiter LinkedRecruiter
        {
            get
            {
                return _linkedRecruiter;
            }
            set
            {
                _linkedRecruiter = value;
                NotifyPropertyChanged();
            }
        }



        private void FillLinkedLocation()
        {
            LocationViewModel locationView = new LocationViewModel();
            Jobs job = this.SelectedJob;
            var location = locationView.AllLocations.Where(x => x.LocationId == job.LocationId);
            foreach (var item in location)
            {
                _linkedLocation = item;
            }
        }
        private void FillLinkedRecrutier()
        {
            RecruiterModel recruiterModel = new RecruiterModel();
            Jobs job = this.SelectedJob;
            var recruiter = recruiterModel.AllRecruiters.Where(x => x.RecruiterId == job.RecruiterId);
            foreach(var item in recruiter)
            {
                _linkedRecruiter = item;
            }
            
        }






        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
