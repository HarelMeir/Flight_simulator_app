﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flight_Sim.Model;

namespace Flight_Sim.cppToCSharp
{
    class CorrelatedFeatures
    {
        string feature1, feature2;  // names of the correlated features
        public string Feature1 { get { return feature1; } set { feature1 = value; } }
        public string Feature2 { get { return feature2; } set { feature2 = value; } }
        float correlation;
        public float Corr { get { return correlation; } set { correlation= value; } }
        Line lin_reg;
        public Line Lin_Reg{ get { return lin_reg; } set { lin_reg = value; } }
        float threshold;
        public float Threshold { get { return threshold; } set { threshold = value; } }
        Circle circleThreshold;
        public Circle CircleThreshold { get { return circleThreshold; } set { circleThreshold = value; } }
    };
    class SimpleAnomalyDetector : ITimeSeriesAnomalyDetector
    {
		List<CorrelatedFeatures> cf;
        float correlationThreshold = 0.9f;
        public void setCorrelationThreshold(float newThreshold)
        {
            correlationThreshold = newThreshold;
        }
        public void setNormalModel(List<CorrelatedFeatures> correlatedFeatures)
        {
            cf = correlatedFeatures;
        }
        public void learnNormal(IDictionary<string, List<float>> table)
        {
            int columnSize = table.ElementAt(0).Value.Count; //table.Keys.ElementAt(0)
            int rowSize = table.Count;
            for (int i = 0; i < table.Count; i++)
            {
                CorrelatedFeatures cfs = new CorrelatedFeatures();
                cfs.Corr = -1;
                cfs.Feature1 = table.ElementAt(i).Key;
                for (int j = i + 1; j < table.Count; j++)
                {
                    float correlation = AnomalyUtils.Pearson(table.ElementAt(i).Value, table.ElementAt(j).Value);
                    if (correlation > correlationThreshold)
                    {
                        if (correlation > cfs.Corr)
                        {
                            cfs.Feature2 = table.ElementAt(j).Key;
                            cfs.Corr = correlation;
                            cfs.Lin_Reg = AnomalyUtils.LinearReg(table.ElementAt(i).Value,
                                table.ElementAt(j).Value);
                            cfs.Threshold = highestDev(table.ElementAt(i).Value,
                                table.ElementAt(j).Value, cfs.Lin_Reg);
                        }
                    }
                }
                if (cfs.Corr > -1)
                {
                    cf.Add(cfs);
                }
            }
        }
        //sends each x y pair to dev and returns the highest
        public float highestDev(List<float> x, List<float> y, Line line)
        {
            float maxDev = 0;
            for (int i = 0; i < x.Count; i++)
            {
                Point p = new Point(x[i], y[i]);
                float newDev = AnomalyUtils.Dev(p, line);
                if (newDev > maxDev)
                {
                    maxDev = newDev;
                }
            }
            return maxDev;
        }
        // not implemented yet
        public List<AnomalyReport> detect(IDictionary<string, List<float>> table)
        {
            return new List<AnomalyReport>();
        }

    }
}