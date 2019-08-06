using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Text;
using System.Timers;
using System.Drawing;
using System.IO.Ports;
using System.Threading;
using System.Reflection;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using RedRat;
using RedRat.IR;
using RedRat.USB;
using RedRat.Util;
using RedRat.RedRat3;
using RedRat.RedRat3.USB;
using RedRat.AVDeviceMngmt;

namespace RedRat3
{
    /// Вывод сигнала
    public class SignalOutput
    {
        protected IRPacket irPacket = null;

        /// Вывод одного сигнала
        public void OutputOneIRsignal(IRedRat3 RedRat3,IRPacket signal)
        {
            RedRat3.OutputModulatedSignal(signal);
        }

        /// Converting XML to IRsignal
        public IRPacket ConvertingBINARYtoIRsignal(String FileName)
        {
            return irPacket = RRUtil.DeserializePacketFromBinary(FileName);
        }
    }
}