using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSearchLibrary.Entities
{
    public class States
    {

        public int StateID { get; set; }
        public string StateAbbreviation { get; set; }
        public string State { get; set; }
        public string Capital { get; set; }
        public string LargestCity { get; set; }
    }
}
