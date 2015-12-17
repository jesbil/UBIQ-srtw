using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SRTWgui
{
    class ForceSensorCutlery : Sensor
    {
        private List<int> intervalList;
        private List<double> averageList;
        private Stopwatch stopwatch;
        private int prevState;


        public ForceSensorCutlery()
        {
            intervalList = new List<int>();
            averageList = new List<double>();
            stopwatch = Stopwatch.StartNew();
            prevState = State.s_high_force;
        }
   
        
        int Sensor.addEvent(int value)
        {
           if (stopwatch.Elapsed.Seconds < 3)
            {
                intervalList.Add(value);
            }
            else
            {
                if (intervalList.Count() != 0)
                {
                    averageList.Add(intervalList.Average());
                    intervalList.Clear();
                }
                

                if (averageList.Count() == 5)
                {
                    if (averageList.Average() >= 30)
                    {
                        averageList.Clear();
                        prevState = State.s_high_force;
                        return State.s_high_force;
                    }
                    else
                    {
                        averageList.Clear();
                        prevState = State.s_no_force;
                        return State.s_no_force;
                    }
                }

                stopwatch = Stopwatch.StartNew();
            }
            return prevState;
                     

        }
        
    }
}
