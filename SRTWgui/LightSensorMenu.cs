using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SRTWgui
{
    class LightSensorMenu : Sensor
    {
        private List<int> intervalList;
        private List<double> averageList;
        private Stopwatch stopwatch;
        private int prevState;

        public LightSensorMenu()
        {
            intervalList = new List<int>();
            averageList = new List<double>();
            stopwatch = Stopwatch.StartNew();
            prevState = State.s_low_light;
        }
        int Sensor.addEvent(int value)
        {
            intervalList.Add(value);

            if (intervalList.Count > 11 && intervalList.GetRange(intervalList.Count() - 11, intervalList.Count() - 1).Average() >= 70)
            {
                intervalList.Clear();
                prevState = State.s_high_light;
                return State.s_high_light;
            }
            else if (intervalList.Count > 11 && intervalList.GetRange(intervalList.Count() - 11, intervalList.Count() - 1).Average() < 70)
            {
                intervalList.Clear();
                prevState = State.s_low_light;
                return State.s_low_light;
            }

            return prevState;
        }
    }
}
