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
        ///This method will select an input to show.
        ///</summary>
        ///<param name="input">Which input to show.</param>
        public override void InputSelect(int input)
        {
            busyFlag = true;
            try {
                if (inputJoin[input - 1] == 0) 
                    throw new JoinException();
                inputInterlock(input);
                switcher.Send(String.Format("{0}!", input));
            }
            catch (Exception e) {
                CrestronConsole.PrintLine(e.ToString());
            }
        }

        private void inputInterlock(int input)
        {
            foreach (uint join in inputJoin.Where(x => x!= 0)) {
                 userInterface.BooleanInput[join].BoolValue = false;
            }
            userInterface.BooleanInput[inputJoin[input - 1]].BoolValue = true;
        }
    }
}