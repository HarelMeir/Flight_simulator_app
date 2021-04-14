﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Flight_Sim.cppToCSharp;

namespace Flight_Sim
{
    /// <summary>
    /// Interaction logic for UserStory9.xaml
    /// </summary>
    public partial class UserStory9 : UserControl
    {
        public UserStory9()
        {
            UserStory9VM vm = new UserStory9VM(Single.SingleDataModel());
            InitializeComponent();
            DataContext = vm;
            AddAnomalies();
        }

        private void AddAnomalies()
        {
            FlightdataModel fdm = Single.SingleDataModel();
            foreach (AnomalyReport ar in fdm.AnomalyReports)
            {
                Anomalies_List.Items.Add(ar.ts + " : " + ar.desc);
            }
        }
    }
}