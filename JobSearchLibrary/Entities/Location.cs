using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSearchLibrary.Entities
{
    public class Location
    {
        private string _state;
        private string _ratingConverted;

        public int LocationId { get; set; }
        public string City { get; set; }
        public int StateId { get; set; }
        public Nullable<int> CityRating { get; set; }
        public string Notes { get; set; }
        public string LargestCity
        {
            get
            {
                SQLConnections sql = new SQLConnections();
                return sql.GetLargestCity(StateId);
            }
        }
        public string StateCapital
        {
            get
            {
                SQLConnections sql = new SQLConnections();
                return sql.GetStateCapital(StateId);
            }
        }

        public string State
        {
            get
            {
                if (this.StateId != 0)
                {
                    SQLConnections sql = new SQLConnections();
                    _state = sql.GetStateName(this.StateId);
                }
                return _state;
            }
        }
        public string RatingConverted
        {
            get
            {
                if(this.CityRating != null)
                {
                    _ratingConverted = FillRatingConverted(CityRating);
                }
                return _ratingConverted;
            }
        }

        private string FillRatingConverted(int? id)
        {
            if (id is null)
            {
                return "";
            }
            if (id >= 0 && id < 3)
            {
                return "Last resort";
            }
            else if (id > 2 && id < 6)
            {
                return "Ehh there's better";
            }
            else if (id > 5 && id < 9)
            {
                return "Pretty nice, could do better";
            }
            else
                return "Doesn't get much better";
        }
    }
}

