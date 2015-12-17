using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phidgets;
using Phidgets.Events;
using System.Threading;

namespace SRTWgui
{
    
    class Controller
    {
        static private Dictionary<int, Sensor> sensorMap;
        static private Dictionary<Sensor, int> stateMap;
        static private int tableState;
        private Sensor irSensorL;
        private Sensor irSensorR;
        private Sensor forceSensorCutlery;
        private Sensor forceSensorGlass;
        private Sensor forceSensorPlate;
        private Sensor lightSensorChair;
        private Sensor lightSensorMenu;
        private InterfaceKit ifKit;
        private MainGUI gui;

        public Controller(MainGUI gui)
        {
            this.gui = gui;
            sensorMap = new Dictionary<int, Sensor>();
            stateMap = new Dictionary<Sensor, int>();
            ifKit = new InterfaceKit();
            //Hook the basica event handlers
            ifKit.Attach += new AttachEventHandler(ifKit_Attach);
            ifKit.Detach += new DetachEventHandler(ifKit_Detach);
            ifKit.Error += new ErrorEventHandler(ifKit_Error);

            //Hook the phidget spcific event handlers
            ifKit.InputChange += new InputChangeEventHandler(ifKit_InputChange);
            ifKit.OutputChange += new OutputChangeEventHandler(ifKit_OutputChange);

            ifKit.SensorChange += new SensorChangeEventHandler(ifKit_SensorChange);
 
            //Open the object for device connections
            ifKit.open();


            //Wait for an InterfaceKit phidget to be attached
            Console.WriteLine("Waiting for InterfaceKit to be attached...");
            ifKit.waitForAttachment();


            irSensorL = new IRSensor();
            irSensorR = new IRSensor();
            forceSensorCutlery = new ForceSensorCutlery();
            forceSensorGlass = new ForceSensorGlass();
            forceSensorPlate = new ForceSensorPlate();
            lightSensorChair = new LightSensorChair();
            lightSensorMenu = new LightSensorMenu();


            sensorMap.Add(1, lightSensorChair);
            sensorMap.Add(2, lightSensorMenu);
            sensorMap.Add(3, forceSensorCutlery);
            sensorMap.Add(4, forceSensorGlass);
            sensorMap.Add(5, forceSensorPlate);
            sensorMap.Add(6, irSensorL);
            sensorMap.Add(7, irSensorR);

            stateMap.Add(lightSensorChair, State.s_wait);
            stateMap.Add(lightSensorMenu, State.s_wait);
            stateMap.Add(forceSensorCutlery, State.s_wait);
            stateMap.Add(forceSensorGlass, State.s_wait);
            stateMap.Add(forceSensorPlate, State.s_wait);
            stateMap.Add(irSensorL, State.s_wait);
            stateMap.Add(irSensorR, State.s_wait);

            Thread t = new Thread(new ThreadStart(getValueFromSensor));

            t.Start();

        }

        //Attach event handler...Display the serial number of the attached InterfaceKit 
        //to the console
        static void ifKit_Attach(object sender, AttachEventArgs e)
        {
            Console.WriteLine("InterfaceKit {0} attached!",
                                e.Device.SerialNumber.ToString());
        }

        //Detach event handler...Display the serial number of the detached InterfaceKit 
        //to the console
        static void ifKit_Detach(object sender, DetachEventArgs e)
        {
            Console.WriteLine("InterfaceKit {0} detached!",
                                e.Device.SerialNumber.ToString());
        }

        //Error event handler...Display the error description to the console
        static void ifKit_Error(object sender, ErrorEventArgs e)
        {
            Console.WriteLine(e.Description);
        }

        //Input Change event handler...Display the input index and the new value to the 
        //console
        static void ifKit_InputChange(object sender, InputChangeEventArgs e)
        {
            Console.WriteLine("Input index {0} value (1)", e.Index, e.Value.ToString());
        }

        //Output change event handler...Display the output index and the new valu to 
        //the console
        static void ifKit_OutputChange(object sender, OutputChangeEventArgs e)
        {
            Console.WriteLine("Output index {0} value {0}", e.Index, e.Value.ToString());
        }

        //Sensor Change event handler...Display the sensor index and it's new value to 
        //the console
        void ifKit_SensorChange(object sender, SensorChangeEventArgs e)
        {
            if (e != null && sensorMap.ContainsKey(e.Index) && (e.Index==7 || e.Index==6))
            {
                int state = sensorMap[e.Index].addEvent(e.Value);
                if ( state!= State.s_wait)
                {
                    if (stateMap[sensorMap[e.Index]] != state)
                    {
                        stateMap[sensorMap[e.Index]] = state;
                        calculateTableState();
                        gui.changeState(tableState);
                    }
                    
                }
            }
            
        }

        static void calculateTableState()
        {
            for (int i = 1; i < 8; i++)
            {
                Console.WriteLine("Senor " + i + ": " + stateMap[sensorMap[i]]);
            }
            Console.WriteLine("");
                // table is set
                if (stateMap[sensorMap[1]] == State.s_high_light && stateMap[sensorMap[3]] == State.s_high_force && stateMap[sensorMap[4]] == State.s_no_force && stateMap[sensorMap[5]] == State.s_no_force)
                {
                    tableState = State.g_table_is_set;
                }

                // Table is not made
                else if (stateMap[sensorMap[1]] == State.s_high_light && stateMap[sensorMap[3]] == State.s_no_force && stateMap[sensorMap[4]] == State.s_low_force && stateMap[sensorMap[5]] == State.s_low_force && stateMap[sensorMap[6]] == State.s_low_activity && stateMap[sensorMap[7]] == State.s_low_activity)
                {
                    tableState = State.g_table_is_unset;
                }

                // Ready to receive menu
                else if (stateMap[sensorMap[1]] == State.s_low_light &&  tableState==State.g_table_is_set && stateMap[sensorMap[3]] == State.s_high_force && stateMap[sensorMap[4]] == State.s_no_force && stateMap[sensorMap[5]] == State.s_no_force)
                {
                    tableState = State.g_seated;
                }

                // Ready to order
                else if (stateMap[sensorMap[1]] == State.s_low_light && stateMap[sensorMap[2]] == State.s_low_light && tableState == State.g_seated)
                {
                    tableState = State.g_ready_to_order;
                }

                // Waiting food
                else if (stateMap[sensorMap[1]] == State.s_low_light && stateMap[sensorMap[2]] == State.s_high_light && tableState == State.g_ready_to_order)
                {
                    tableState = State.g_waiting_food;
                }

                // More to drink
                else if (stateMap[sensorMap[1]] == State.s_low_light && stateMap[sensorMap[4]] == State.s_low_force && (stateMap[sensorMap[6]] == State.s_high_activity || stateMap[sensorMap[7]] == State.s_high_activity) && tableState == State.g_eating)
                {
                    tableState = State.g_more_water;
                }

                //Eating
                else if (stateMap[sensorMap[1]] == State.s_low_light && stateMap[sensorMap[3]] == State.s_no_force && stateMap[sensorMap[4]] == State.s_high_force && stateMap[sensorMap[5]] == State.s_high_force && (stateMap[sensorMap[6]] == State.s_high_activity || stateMap[sensorMap[7]] == State.s_high_activity))
                {
                    tableState = State.g_eating;
                }

                // finnished eating
                else if (stateMap[sensorMap[1]] == State.s_low_light && stateMap[sensorMap[3]] == State.s_no_force && stateMap[sensorMap[4]] == State.s_low_force && stateMap[sensorMap[5]] == State.s_low_force && stateMap[sensorMap[6]] == State.s_low_activity && stateMap[sensorMap[7]] == State.s_low_activity)
                {
                    tableState = State.g_finished_eating;
                }
           
        }
        private void getValueFromSensor(){
            
            while (true)
            {

                for (int i = 1; i < 8; i++)
                {
                    int state = sensorMap[i].addEvent(ifKit.sensors[i].Value);
                    if (stateMap[sensorMap[i]] != state)
                    {
                        stateMap[sensorMap[i]] = state;
                        
                    }
                    if (i == 7)
                    {
                        calculateTableState();
                        gui.changeState(tableState);

                    }
                }
                
                Thread.Sleep(200);
            }
            

        }

    }
}
