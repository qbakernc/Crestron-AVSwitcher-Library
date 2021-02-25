using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace AisleSwitcherLibrary.ExtronIN1606
{
    public partial class IN1606 : Switcher
    {
        ///<summary>
        ///This method will turn the microphone on.
        ///</summary>
        ///<param name="mic">Which mic to turn on. Microphone 1 or Microphone 2.</param>
        public override void MicOn(int mic)
        {
            busyFlag = true;
            userInterface.BooleanInput[micJoin[mic]].BoolValue = true;
            switcher.Send(String.Format("\u001BM4000{0}*0AU\u000D", mic));
        }

        ///<summary>
        ///This method will mute the volume.
        ///</summary>
        ///<param name="mic">Which mic to turn off. Microphone 1 or Microphone 2. </param>
        public override void MicOff(int mic)
        {
            busyFlag = true;
            userInterface.BooleanInput[micJoin[mic]].BoolValue = false;
            switcher.Send(String.Format("\u001BM4000{0}*1AU\u000D", mic));
        }

        ///<summary>
        ///This method will toggle the microphone.
        ///</summary>
        ///<param name="mic">Which mic to toggle. Microphone 1 or Microphone 2. </param>
        public override bool MicToggle(int mic)
        {
            bool micState = false;
            busyFlag = true;
            userInterface.BooleanInput[micJoin[mic]].BoolValue = !userInterface.BooleanInput[micJoin[mic]].BoolValue;

            switch (userInterface.BooleanInput[micJoin[mic]].BoolValue) {
                case false:
                    switcher.Send(String.Format("\u001BM4000{0}*1AU\u000D", mic));
                    micState =  false;
                    break;
                case true:
                    switcher.Send(String.Format("\u001BM4000{0}*0AU\u000D", mic));
                    micState = true;
                    break;
            }

            return micState;
        }
    }
}