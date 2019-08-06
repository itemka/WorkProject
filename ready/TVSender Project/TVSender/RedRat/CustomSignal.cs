using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using RedRat.IR;

namespace TVSender
{
    public class CustomSignal
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public ModulatedSignal sigMod;
        public ModulatedSignal Nec(string code)
        {
            string Data_RR = CodeTransform.NecToSignal(code);
            return CodeTransform.SignalStringToModSig(Data_RR);
        }
    }
}
