using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phidgets;
using Phidgets.Events;
using System.Diagnostics;

namespace SRTWgui
{
    class IRSensor : Sensor
    {
        private List<int> intervalList;
        private List<double> averageList;
        private Stopwatch stopwatch;
        private int prevState;
        public IRSensor()
        {
            intervalList = new List<int>();
            averageList = new List<double>();
            stopwatch = Stopwatch.StartNew();
            prevState = State.s_low_activity;
        }

        int Sensor.addEvent(int value) {
            if (stopwatch.Elapsed.Seconds < 3)
            {
                if (value < 900)
                {
                    intervalList.Add(value);
                }
            }
            else
            {
                if (intervalList.Count() == 0)
                {
                    averageList.Add(0);
                }
                else
                {
                    averageList.Add(intervalList.Count()/3);
                }
                intervalList.Clear();

                if (averageList.Count() == 5)
                {
                    if(averageList.Average() > 0.5)
                    {
                        prevState = State.s_high_activity;
                        averageList.Clear();
                        return State.s_high_activity;
                    }
                    else 
                    {
                        prevState = State.s_low_activity;
                        averageList.Clear();
                        return State.s_low_activity;
                    }
                }

                stopwatch = Stopwatch.StartNew();
            }
            return prevState;
        }

       
    }
}
