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

namespace AisleSwitcherLibrary.ExtronIN1606
{
    ///<summary>IN1606 Switcher class.</summary>
    public partial class IN1606 : Switcher
    {
        private const string inputChange = "IN";
        private const string micChange = "DsM";
        private const string muteChange = "GrpmD2";
        private const string volChange = "GrpmD1";

        ///<summary>
        ///Constructor.
        ///</summary>
        ///<param name="userInterface">User Interface displays feedback from the switcher. </param>
        ///<param name="switcher">ComPort the switchre is connected to. </param>
        public IN1606(BasicTriListWithSmartObject userInterface, ComPort switcher)
        {
            this.userInterface = userInterface;
            this.switcher = switcher;

            this.switcher.Register();
            this.switcher.SerialDataReceived += new ComPortDataReceivedEvent(switcher_SerialDataReceived);
            this.switcher.SetComPortSpec(ComPort.eComBaudRates.ComspecBaudRate9600,
                                            ComPort.eComDataBits.ComspecDataBits8,
                                            ComPort.eComParityType.ComspecParityNone,
                                            ComPort.eComStopBits.ComspecStopBits1,
                                            ComPort.eComProtocolType.ComspecProtocolRS232,
                                            ComPort.eComHardwareHandshakeType.ComspecHardwareHandshakeNone,
                                            ComPort.eComSoftwareHandshakeType.ComspecSoftwareHandshakeNone,
                                            false);
        }

        ///<summary>Input select join numbers.</summary>
        public override uint[] InputJoins
        {
            set { inputJoin = value; }
        }

        ///<summary>Volume up join number.</summary>
        public override uint VolumeUpJoin
        {
            set { volUpJoin = value; }
        }

        ///<summary>Volume down join number.</summary>
        public override uint VolumeDownJoin
        {
            set { volDownJoin = value; }
        }

        ///<summary>Volume mute join number.</summary>
        public override uint VolumeMuteJoin
        {
            set { volMuteJoin = value; }
        }

        ///<summary>Volume gauge join number.</summary>
        public override uint VolumeGaugeJoin
        {
            set { volGaugeJoin = value; }
        }

        ///<summary>Microphone join numbers.</summary>
        public override uint[] MicJoin
        {
            set { micJoin = value; }
        }
    }
}