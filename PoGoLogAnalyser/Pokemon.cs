using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGoLogAnalyser
{
    class Pokemon
    {
        private string _Name;
        private int _CP;
        private double _IV;

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public int Cp
        {
            get { return _CP; }
            set { _CP = value; }
        }

        public double Iv
        {
            get { return _IV; }
            set { _IV = value; }
        }
    }
}
