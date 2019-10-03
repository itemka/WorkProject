﻿using System;
using RedRat;
using RedRat.IR;
using RedRat.Util;
using RedRat.RedRat3;
using System.Threading;
using System.Windows.Forms;

namespace RedRat3
{
    // Захват сигнала
    public class IRsignalTrainingMode : Form
    {
        protected IRedRat3 RedRat3;
        IRPacket irPacket = null;
        ModulatedSignal modSignal = null;
        //Установите истинный один раз сигнал (или ex)
        protected bool haveSignal = false;
        public static string tempMessage = "";

        // Обрабатывает входной сигнал (или ошибку) из RedRat.
        public void SignalDataHandler(object sender, EventArgs e)
        {
            if (e is SignalEventArgs)
            {
                var siea = (SignalEventArgs)e;

                switch (siea.Action)
                {
                    //case SignalEventAction.EXCEPTION:
                    //    MessageBox.Show(siea.Exception.Message);
                    //    break;
                    case SignalEventAction.MODULATED_SIGNAL:
                        //MessageBox.Show("Есть ИК-данные...");
                        irPacket = siea.ModulatedSignal;
                        modSignal = siea.ModulatedSignal;
                        break;
                    //case SignalEventAction.IRDA_PACKET:
                    //    MessageBox.Show("Have IR data IRDA_PACKET...");
                    //    irPacket = siea.IrDaPacket;
                    //    break;
                    case SignalEventAction.INPUT_CANCELLED:
                        break;

                    default:
                        MessageBox.Show("Сигнал не пришел...");
                        break;
                }

                haveSignal = true;
            }
            else
            {
                MessageBox.Show("Событие неизвестного типа...");
            }
        }

        // Захват сигнала
        public void CaptureSignal(string fileName = "")
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.InitialDirectory = Form1.pathClick;
            SearchRedRat SRR = new SearchRedRat();
            if (SRR.FindRedRat() != null)
            {
                RedRat3 = SRR.FindRedRat();

                RedRat3.LearningSignalIn += SignalDataHandler;
                RedRat3.GetModulatedSignal(10000);//Ввод сигнала в RedRat3 осуществляется в течении 10с
                haveSignal = false;
                while (!haveSignal) { Thread.Sleep(100); }
                if (modSignal != null)
                {
                    var mes = MessageBox.Show("OK - cохранить XML" + Environment.NewLine + "CANCEL - cохранить BIN", "Вариант сохранения файла", MessageBoxButtons.OKCancel);
                    if (mes == DialogResult.OK)
                    {
                        inputName IN = new inputName(); IN.ShowDialog();
                        RRUtil.SerializePacketToXML(Form1.pathClick + "\\" + IN.name + ".xml", modSignal);
                        tempMessage = "XML файл \"" + IN.name + "\" сохранен.";
                    }
                    else
                    {
                        inputName IN = new inputName(); IN.ShowDialog();
                        RRUtil.SerializePacketToBinary(Form1.pathClick + "\\" + IN.name + ".bin", modSignal);//SerializePacketToXML                    
                        //var m = MessageBox.Show("Файл \"" + IN.name + "\" сохранен.", "Прием сигнала", MessageBoxButtons.OK);
                        tempMessage = "BIN файл \"" + IN.name + "\" сохранен.";
                    }
                }
                else
                {
                    MessageBox.Show("Сигнал не был получен. Прием окончен.", "Прием сигнала", MessageBoxButtons.OK);
                }
            }
        }

        // Взять сигнал
        public ModulatedSignal GetSignal() { return modSignal; }
    }
}