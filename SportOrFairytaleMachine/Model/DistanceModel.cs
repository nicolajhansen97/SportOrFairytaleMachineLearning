using System;
using System.Collections.Generic;
using System.Text;

namespace SportOrFairytaleMachine.Model
{
    //Model class. Its used to get the informations and save the informations so it can be used from anywhere.
    class DistanceModel : Bindable
    {
        private double sum;

        public double Sum
        {
            get { return sum; }
            set { sum = value; propertyIsChanged(); }
        }

        private double distance;

        public double Distance
        {
            get { return distance; }
            set { distance = value; propertyIsChanged(); }
        }

        private string type;

        public string Type
        {
            get { return type; }
            set { type = value; propertyIsChanged(); }
        }



    }
}
