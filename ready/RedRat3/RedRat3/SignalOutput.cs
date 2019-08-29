using System;
using RedRat.IR;
using RedRat.Util;
using RedRat.RedRat3;

namespace RedRat3
{
    // Вывод сигнала
    public class SignalOutput
    {
        protected IRPacket irPacket = null;

        // Вывод одного сигнала
        public void OutputOneIRsignal(IRedRat3 RedRat3,IRPacket signal)
        {
            RedRat3.OutputModulatedSignal(signal);
        }

        // Converting XML to IRsignal
        public IRPacket ConvertingBINARYtoIRsignal(String FileName)
        {
            return irPacket = RRUtil.DeserializePacketFromBinary(FileName);
        }
    }
}