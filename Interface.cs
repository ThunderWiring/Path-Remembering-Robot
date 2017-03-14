using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace RobotNamespace
{
    class Interface
    {
        //static USB_1208LS.MiniMccBoard board = new USB_1208LS.MiniMccBoard(0);
        #region constructor
        public Interface()
        {
            //board.SetDigitalPortDirection(0, USB_1208LS.MiniMccBoard.IODirection.Output);
        }
        #endregion // constructor
        #region variables
        private byte b = (byte)0;
        public bool turnLeftIndicator = true;
        public bool turnRightIndicator = true;
        #endregion//variables
       
        public void litLED(int ledNumber, int highLow)
        {
            //highLow: the status of the LED, while High=On & Low=off
            //ledNumber: there are 2 LEDs, '1' reffers to the first LED, '2' reffers to the secomd LED
            if (highLow == 1)
            {
                if (ledNumber == 1)//yellowLight
                {
                   // board.SetAnalogOutput((int)0, (float)3.0);
                }
                else if (ledNumber == 2)//greenLight(red)
                {
                   // board.SetAnalogOutput((int)1, (float)3.0);
                }
            }
            else if (highLow == 0)
            {
               // board.SetAnalogOutput((int)0, (float)0);
               // board.SetAnalogOutput((int)1, (float)0);
            }
        }
        #region Orientation
        public void forward()
        {
            //board.SetDigitalOutput(0, (byte)149); //01011001
        }
        public void backward()
        {
            //board.SetDigitalOutput(0, (byte)106); //10100110
        }
        public void turnLeft()
        {
           // int tmp = b | 166; //01101010
           // board.SetDigitalOutput(0, (byte)tmp);
        }
        public void turnRight()
        {
            //int tmp = b | 89;
            //board.SetDigitalOutput(0, (byte)tmp);
        }
        public void stop()
        {
           // board.SetDigitalOutput(0,0);
        }
        #endregion //Orientation
    }
}
