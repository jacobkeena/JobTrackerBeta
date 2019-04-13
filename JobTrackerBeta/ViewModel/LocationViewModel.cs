using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobSearchLibrary.Entities;
using JobSearchLibrary;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace JobTrackerBeta.ViewModel
{
    public class LocationViewModel : INotifyPropertyChanged
    {
        public LocationViewModel()
        {
            FillAllLocations();
        }

        private void FillAllLocations()
        {
            SQLConnections sql = new SQLConnections();
            AllLocations = sql.GetAllLocations();   
        }

        private List<Location> _allLocations;
        public List<Location> AllLocations
        {
            get
            {
                return _allLocations;
            }
            set
            {
                _allLocations = value;
                NotifyPropertyChanged();
            }
        }

        private Location _selectedLocation;
        public Location SelectedLocation
        {
            get
            {
                return _selectedLocation;
            }
            set
            {
                _selectedLocation = value;
                NotifyPropertyChanged();
            }
        }

        private Location _newLocation;
        public Location NewLocation
        {
            get
            {
                return _newLocation;
            }
            set
            {
                _newLocation = value;
                NotifyPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
