using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace AisleSwitcherLibrary
{
    internal class JoinException : Exception
    {
        public JoinException()
            : base("The called join number located in the InputJoin array can not be equal to 0.")
        {
        }
    }
}