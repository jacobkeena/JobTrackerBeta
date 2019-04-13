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
    class RatingModel
    {
        public RatingModel()
        {
            FillAllRatings();
        }
        private void FillAllRatings()
        {
            SQLConnections sql = new SQLConnections();
            AllRatings = sql.GetAllRatings();
        }


        private List<Rating> _allRatings;
        public List<Rating> AllRatings
        {
            get
            {
                return _allRatings;
            }
            set
            {
                _allRatings = value;
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
