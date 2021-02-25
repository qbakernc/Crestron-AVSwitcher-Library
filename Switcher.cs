using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Crestron.SimplSharp;                          	// For Basic SIMPL# Classes
using Crestron.SimplSharpPro;                       	// For Basic SIMPL#Pro classes
using Crestron.SimplSharpPro.CrestronThread;        	// For Threading
using Crestron.SimplSharpPro.Diagnostics;		    	// For System Monitor Access
using Crestron.SimplSharpPro.DeviceSupport;         	// For Generic Device Support
using Crestron.SimplSharpPro.UI;

namespace AisleSwitcherLibrary
{
    ///<summary>A/V Switcher class. Abstract class.</summary>
    public abstract class Switcher
    {
        ///<summary>User Interface displays feedback from the switcher.</summary>
        protected BasicTriListWithSmartObject userInterface;

        ///<summary>ComPort the switchre is connected to.</summary>
        protected ComPort switcher;

        ///<summary>Locker for volume change.</summary>
        protected Object locker = new Object();

        ///<summary>Input select join numbers.</summary>
        protected uint[] inputJoin = new uint[6];

        ///<summary>Volume up join number.</summary>
        protected uint volUpJoin;

        ///<summary>Volume down join number.</summary>
        protected uint volDownJoin;

        ///<summary>Volume mute join number.</summary>
        protected uint volMuteJoin;

        ///<summary>Volume gauge join number.</summary>
        protected uint volGaugeJoin;

        ///<summary>Volume change is currently happening.</summary>
        protected bool volumeChangeActive;

        ///<summary>Microphone join numbers.</summary>
        protected uint[] micJoin = new uint[2];

        ///<summary>Update volume level.</summary>
        protected int volumeUpdate = 0;

        ///<summary>Volume level feedback received from the switcher.</summary>
        protected int volumeFeedback = 0;

        ///<summary>A response was received and the processor is busy analyzing the data.</summary>
        protected bool busyFlag = false;

        ///<summary>Data received from the switcher.</summary>
        protected string receiveData = null;

        ///<summary>Input join numbers accessor.</summary>
        public abstract uint[] InputJoins {set;}

        ///<summary>Volume up join number accessor.</summary>
        public abstract uint VolumeUpJoin {set;}

        ///<summary>Volume down join number accessor.</summary>
        public abstract uint VolumeDownJoin {set;}

        ///<summary>Volume mute join number accessor.</summary>
        public abstract uint VolumeMuteJoin {set;}
        
        ///<summary>Volume gauge join number accessor.</summary>
        public abstract uint VolumeGaugeJoin {set;}

        ///<summary>Microphone join numbers accessor.</summary>
        public abstract uint[] MicJoin { set; }

        ///<summary>This method will select an input to show.</summary>
        ///<param name="input">Which input to show.</param>
        public abstract void InputSelect(int input);

        ///<summary>This method will set the volume to the specific level.</summary>
        ///<param name="volumePreset">Value to set the volume. </param>
        public abstract void VolumePreset(int volumePreset);

        ///<summary>This method will increment the volume by 10 until the VolumeStop method is called.</summary>
        public abstract void VolumeUp();

        ///<summary>This method will decrement the volume by 10 until the VolumeStop method is called.</summary>
        public abstract void VolumeDown();

        ///<summary>This method will stop the volume from incrementing or decrementing.</summary>
        public abstract void VolumeStop();

        ///<summary>This method will mute the volume.</summary>
        public abstract void VolumeMuteOn();

        ///<summary>This method will Unmute the volume.</summary>
        public abstract void VolumeMuteOff();

        ///<summary>This method will toggle the volume mute.</summary>
        public abstract bool VolumeMuteToggle();

        ///<summary>This method will turn the microphone on.</summary>
        ///<param name="mic">Which mic to turn on.</param>
        public abstract void MicOn(int mic);

        ///<summary>This method will mute the volume.</summary>
        ///<param name="mic">Which mic to turn off. Amount of mics will vary between devices. </param>
        public abstract void MicOff(int mic);

        ///<summary>This method will toggle the microphone.</summary>
        ///<param name="mic">Which mic to toggle. Amount of mics will vary between devices. </param>
        public abstract bool MicToggle(int mic);

        ///<summary>This method determines the type of response we received from the switcher.</summary>
        protected abstract object CheckResponse(object temp);

        ///<summary>This method will received the data.</summary>
        protected abstract void switcher_SerialDataReceived(ComPort ReceivingComPort, ComPortSerialDataEventArgs args);
    }
}