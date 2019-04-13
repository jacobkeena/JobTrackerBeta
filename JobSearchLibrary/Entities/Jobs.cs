using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSearchLibrary.Entities
{
    public class Jobs
    {
        private string _position;
        private string _location;
        private string _rating;

        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int? PositionId { get; set; }
        public int? LocationId { get; set; }
        public string SalaryRange { get; set; }
        public int? RatingId { get; set; }
        public string CEOName { get; set; }
        public string MissionStatement { get; set; }
        public string Benefits { get; set; }
        public string Comments { get; set; }
        public string JobLink { get; set; }
        public int? RecruiterId { get; set; }

        public string Position
        {
            get
            {
                if (PositionId != null)
                {
                    SQLConnections sql = new SQLConnections();
                    _position = sql.GetPositionName(this.PositionId);
                }
                return _position;
            }
        }

        public string Location
        {
            get
            {
                if (LocationId != null)
                {
                    SQLConnections sql = new SQLConnections();
                    _location = sql.GetCityName(this.LocationId);
                }
                return _location;
            }
        }

        public string Rating
        {
            get
            {
                if (RatingId != null)
                {
                    SQLConnections sql = new SQLConnections();
                    _rating = sql.GetRatingString(this.RatingId);
                }
                return _rating;
            }
        }
    }
}
