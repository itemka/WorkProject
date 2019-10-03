using System;
using RedRat.IR;
using RedRat.Util;
using RedRat.RedRat3;
using System.Windows.Forms;

namespace RedRat3
{
    // Вывод сигнала
    public class SignalOutput
    {
        protected IRPacket irPacket = null;

        // Вывод одного сигнала
        public void OutputOneIRsignal(IRedRat3 RedRat3, IRPacket signal)
        {
            RedRat3.OutputModulatedSignal(signal);
        }

        //Проверяет, xml это или нет
        bool IsXmlFile(string fileName)
        {
            var extension = fileName.Substring(startIndex: fileName.Length - 3);
            return string.Equals(extension, "xml", StringComparison.InvariantCultureIgnoreCase);
        }

        // Converting BIN/XML to IRsignal
        public IRPacket ConvertingBINARYorXMLtoIRsignal(String FileName)
        {
            if (IsXmlFile(FileName)) { return irPacket = RRUtil.DeserializePacketFromXML(FileName); }
            else { return irPacket = RRUtil.DeserializePacketFromBinary(FileName); }
        }
    }
}