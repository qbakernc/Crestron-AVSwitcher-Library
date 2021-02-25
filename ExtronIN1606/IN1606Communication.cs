using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.CrestronThread;

namespace AisleSwitcherLibrary.ExtronIN1606
{
    public partial class IN1606 : Switcher
    {
        ///<summary>Event. Data received from the switcher</summary>
        protected override void switcher_SerialDataReceived(ComPort ReceivingComPort, ComPortSerialDataEventArgs args)
        {
            /* Receives the data from the switcher. */
            receiveData += switcher.RcvdString;

            if (receiveData.EndsWith("\x0A") && busyFlag == false) {
                Thread t = new Thread(CheckResponse, receiveData, Thread.eThreadStartOptions.Running);
                receiveData = null;
            }
            busyFlag = false;
        }

        ///<summary>This method determines the type of response we received from the switcher.</summary>
        protected override object CheckResponse(object temp)
        {
            busyFlag = true;
            string receiveData = (string)temp;

            /* Input change response. */
            if (receiveData.Contains(inputChange)) {
                inputInterlock(int.Parse(receiveData.Substring(3, 1)));
            }

            /* Mic Change response. */
            if (receiveData.Contains(micChange)) {
                switch (receiveData.Substring(3, 7)) {
                    case "40000*0":
                        userInterface.BooleanInput[micJoin[0]].BoolValue = true;
                        break;
                    case "40000*1":
                        userInterface.BooleanInput[micJoin[0]].BoolValue = false;
                        break;
                    case "40001*0":
                        userInterface.BooleanInput[micJoin[1]].BoolValue = true;
                        break;
                    case "40001*1":
                        userInterface.BooleanInput[micJoin[1]].BoolValue = false;
                        break;
                }
            }

            /* Volume Mute change response. */
            if (receiveData.Contains(muteChange)) {
                switch (receiveData.Substring(7, 1)) {
                    case "0":
                        userInterface.BooleanInput[volMuteJoin].BoolValue = false;
                        break;
                    case "1":
                        userInterface.BooleanInput[volMuteJoin].BoolValue = true;
                        break;
                }
            }

            /* Volume change response. */
            if (receiveData.Contains(volChange)) {
                lock (locker) {
                    volumeFeedback = int.Parse(receiveData.Substring(7));
                    userInterface.UShortInput[volGaugeJoin].UShortValue = (ushort)(volumeFeedback + 1000);
                    int percent = (volumeFeedback + 1000)/10;
                    userInterface.StringInput[volGaugeJoin].StringValue = String.Format("background:linear-gradient(to top, rgb(0,112,60) {0}%, white 0%)", percent);
                }
            }
            return null;
        }
    }
}