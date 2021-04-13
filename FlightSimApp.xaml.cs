﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Flight_Sim.cppToCSharp;
using System.IO;
using Flight_Sim.Model;

namespace Flight_Sim
{
    /// <summary>
    /// Interaction logic for FlightSimApp.xaml
    /// </summary>
    public partial class FlightSimApp : Window
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int useDetector(string trainPath, string testPath);
        public FlightSimApp()
        {
            InitializeComponent();
        }

        private void UserStory9_Loaded(object sender, RoutedEventArgs e)
        {
            /*string dllFile = @"C:\Users\avivi\Desktop\AP2\SimpleAnomalyDetectorDLL.dll";
            var assembly = Assembly.LoadFrom(dllFile);
            var type = assembly.GetType("SimpleAnomalyDetectorDLL.API");
            Console.WriteLine("type is: " + type + "\n");
            var obj = Activator.CreateInstance(type);
            var method = type.GetMethod("useDetector");
            var a = method.Invoke(obj, new object[] {Single.SingleDataModel().Table,
            Single.SingleDataModel().Table});
            List<AnomalyReport> b = (List<AnomalyReport>)a;
            Console.WriteLine(b.ElementAt(0).desc);*/

            FlightSimM fsm = Single.SingleFlightSimM();
            // creates a new csv like the received one but with the properties in the first row.
            string noPropTrainCSVPath =  fsm.CsvPath; // change to train
            string noPropTestCSVPath = fsm.CsvPath; // change to test
            string csvTrain = "train.csv";
            string csvTest = "test.csv";
            // adds the first line with properties
            File.WriteAllText(csvTrain, fsm.ColDataNames[0]);
            File.WriteAllText(csvTest, fsm.ColDataNames[0]);
            for (int i = 1; i < fsm.ColDataNames.Count; i++)
            {
                File.AppendAllText(csvTrain, "," + fsm.ColDataNames[i]);
                File.AppendAllText(csvTest, "," + fsm.ColDataNames[i]);
            }
            // copying the rest of the file as it is
            string[] noPropTrainLines = File.ReadAllLines(noPropTrainCSVPath);
            foreach(string ln in noPropTrainLines)
            {
                File.AppendAllLines(csvTrain, noPropTrainLines);
            }
            // copying the rest of the file as it is
            string[] noPropTestLines = File.ReadAllLines(noPropTestCSVPath);
            foreach (string ln in noPropTestLines)
            {
                File.AppendAllLines(csvTest, noPropTestLines);
            }

            // use the dll function, it creates a file named Anomalies.csv that holds all the
            // AnomalyReports, each line holds: Timestep,Description.
            string dllPath = fsm.DLLPath;
            IntPtr pDll = NativeMethods.LoadLibrary(@dllPath);
            IntPtr pAddressofFunctionToCall = NativeMethods.GetProcAddress(pDll, "useDetector");
            useDetector upload = (useDetector)Marshal.GetDelegateForFunctionPointer(
                pAddressofFunctionToCall,
                typeof(useDetector));
            int theResult = upload(csvTrain, csvTest);

            // saves the anomaly reports that in the file Animalies.csv
            // locally in a list of AnomalyReports
            string anomaliesPath = "Anomalies.csv";
            List<AnomalyReport> anomalyReports = new List<AnomalyReport>();
            string[] lines = File.ReadAllLines(anomaliesPath);
            foreach(string ln in lines)
            {
                string[] splitted = ln.Split(',');
                anomalyReports.Add(new AnomalyReport(splitted[1], long.Parse(splitted[0])));
            }
        }
    }
}
