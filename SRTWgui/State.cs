using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRTWgui
{
    class State
    {
        // Sensor
        public const int s_wait = 100;

        // IR sensor
        public const int s_low_activity = 123;
        public const int s_high_activity = 124;

        // Force sensor
        public const int s_low_force = 223;
        public const int s_high_force = 224;
        public const int s_no_force = 225;
   
        // Light sensor
        public const int s_low_light = 323;
        public const int s_high_light = 324;
        
 

        // GUI states
        public const int g_table_is_set = 900;
        public const int g_seated = 901;
        public const int g_ready_to_order = 902;
        public const int g_more_water = 903;
        public const int g_finished_eating = 904;
        public const int g_table_is_unset = 905;
        public const int g_eating = 906;
        public const int g_waiting_food = 907;
    }
}
