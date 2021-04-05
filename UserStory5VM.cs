﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Flight_Sim
{
    class UserStory5VM : INotifyPropertyChanged
    {
        FlightdataModel model;
        public UserStory5VM(FlightdataModel fdm)
        {
            this.model = fdm;
            model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e) { NotifyPropertyChanged("VM_" + e.PropertyName); };
        }




        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        public event PropertyChangedEventHandler PropertyChanged;





        private double altimeter = 0;
        public string altimeterText { get { return "Aircraft height: " + VM_Altimeter_indicated_altitude; }}
        public double VM_Altimeter_indicated_altitude
        {
            get { return model.Altimeter_indicated_altitude; }
            set { altimeter = value; }
        }




        private double airspeed = 0;
        public string airspeedText { get { return "Airspeed: " + VM_Airspeed; } }
        public double VM_Airspeed
        {
            get { return airspeed; }
            set { airspeed = value; }
        }

        ////need to add the others
    }
}
