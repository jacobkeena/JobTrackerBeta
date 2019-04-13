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
    public class PositionModel : INotifyPropertyChanged
    {
        public PositionModel()
        {
            FillAllPositions();
        }

        private void FillAllPositions()
        {
            SQLConnections sql = new SQLConnections();
            var x = sql.GetAllPositions();
            AllPositions = x;
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

        private Position _newPosition;
        public Position NewPosition
        {
            get
            {
                return _newPosition;
            }
            set
            {
                _newPosition = value;
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
