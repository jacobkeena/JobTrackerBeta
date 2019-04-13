using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSearchLibrary.Entities
{
    public class Recruiter
    {
        public int RecruiterId { get; set; }
        public string RecruiterName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string LinkedInLink { get; set; }
    }
}
