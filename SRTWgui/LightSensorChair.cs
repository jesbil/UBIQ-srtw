using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SRTWgui
{
    class LightSensorChair : Sensor
    {
        
        private List<int> intervalList;
        private int prevState;
        public LightSensorChair()
        {
            intervalList = new List<int>();
            prevState = State.s_high_light;
            
        }
        int Sensor.addEvent(int value)
        {
            intervalList.Add(value);
                
                if (intervalList.Count>11 && intervalList.GetRange(intervalList.Count()-11,intervalList.Count()-1).Average() >= 70)
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
