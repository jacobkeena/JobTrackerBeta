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
    class StatesViewModel : INotifyPropertyChanged
    {
        public StatesViewModel()
        {
            FillAllStates();
        }

        private void FillAllStates()
        {
            SQLConnections sql = new SQLConnections();
            var x = sql.GetAllStates();
            AllStates = x;
        }


        private List<States> _allStates;
        public List<States> AllStates
        {
            get
            {
                return _allStates;
            }
            set
            {
                _allStates = value;
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
