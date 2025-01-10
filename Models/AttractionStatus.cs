using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkbeheerderDashboard
{
    public class AttractionStatus
    {
        private string _name;
        private string _status;
        private string _opmerkingen;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public string Opmerkingen
        {
            get { return _opmerkingen; }
            set { _opmerkingen = value; }
        }
    }
}
