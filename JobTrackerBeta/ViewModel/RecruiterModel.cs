using System;
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
    public class RecruiterModel : INotifyPropertyChanged
    {
        public RecruiterModel()
        {
            FillAllRecruiters();
        }

        private List<Recruiter> _allRecruiters;
        public List<Recruiter> AllRecruiters
        {
            get
            {
                return _allRecruiters;
            }
            set
            {
                _allRecruiters = value;
                NotifyPropertyChanged();
            }
        }
        private Recruiter _newRecruiter;
        public Recruiter NewRecruiter
        {
            get
            {
                return _newRecruiter;
            }
            set
            {
                _newRecruiter = value;
                NotifyPropertyChanged();
            }
        }

        private void FillAllRecruiters()
        {
            SQLConnections sql = new SQLConnections();
            AllRecruiters = sql.GetAllRecruiters();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
