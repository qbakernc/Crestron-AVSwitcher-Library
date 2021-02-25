using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharpPro.CrestronThread;

namespace AisleSwitcherLibrary.ExtronIN1606
{
    public partial class IN1606
    {
        ///<summary>
        ///This method will set the volume to the specific level.
        ///</summary>
        ///<param name="volumePreset">Value to set the volume. </param>
        public override void VolumePreset(int volumePreset)
        {
            switcher.Send(String.Format("\u001BD1*{0}GRPM\u000D", volumePreset));
        }

        ///<summary>
        ///This method will increment the volume by 10 until the VolumeStop method is called.
        ///</summary>
        public override void VolumeUp()
        {
            volumeChangeActive = true;
            Thread t = new Thread(Volume, -10, Thread.eThreadStartOptions.Running);
        }

        ///<summary>
        ///This method will decrement the volume by 10 until the VolumeStop method is called.
        ///</summary>
        public override void VolumeDown()
        {
            volumeChangeActive = true;
            Thread t = new Thread(Volume, 10, Thread.eThreadStartOptions.Running);
        }

        ///<summary>
        ///This method will stop the volume from incrementing or decrementing.
        ///</summary>
        public override void VolumeStop()
        {
            volumeChangeActive = false;
        }

        private object Volume(object volIncriment)
        {
            /* If volume is muted, unmute it before adjusting the volume. */
            if (userInterface.BooleanInput[volMuteJoin].BoolValue)
                VolumeMuteOff();

            while (volumeChangeActive && (volumeFeedback > -1000 || volumeFeedback < 0)) {
                lock (locker) {
                    volumeUpdate = volumeFeedback - (int)volIncriment;
                    switcher.Send(String.Format("\u001BD1*{0}GRPM\u000D", volumeUpdate));
                }
                Thread.Sleep(80);
            }
            return null;
        }

        ///<summary>
        ///This method will mute the volume.
        ///</summary>
        public override void VolumeMuteOn()
        {
            busyFlag = true;
            userInterface.BooleanInput[volMuteJoin].BoolValue = true;
            switcher.Send("\x001BD2*1GRPM\x0D");
        }

        ///<summary>
        ///This method will Unmute the volume.
        ///</summary>
        public override void VolumeMuteOff()
        {
            busyFlag = true;
            userInterface.BooleanInput[volMuteJoin].BoolValue = false;
            switcher.Send("\u001BD2*0GRPM\u000D");
        }

        ///<summary>
        ///This method will toggle the volume mute.
        ///</summary>
        public override bool VolumeMuteToggle()
        {
            busyFlag = true;
            userInterface.BooleanInput[volMuteJoin].BoolValue = !userInterface.BooleanInput[volMuteJoin].BoolValue;
            bool volumeMuteState = false;

            switch (userInterface.BooleanInput[volMuteJoin].BoolValue) {
                case false:
                    switcher.Send("\u001BD2*0GRPM\u000D");
                    volumeMuteState = false;
                    break;
                case true:
                    switcher.Send("\u001BD2*1GRPM\u000D");
                    volumeMuteState = true;
                    break;
            }

            return volumeMuteState;
        }
    }
}