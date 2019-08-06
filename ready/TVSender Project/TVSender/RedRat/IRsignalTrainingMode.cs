using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NLog;
using RedRat;
using RedRat.AVDeviceMngmt;
using RedRat.IR;
using RedRat.RedRat3;
using RedRat.RedRat3.USB;

namespace TVSender
{
    public class IRsignalTrainingMode
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        protected IRedRat3 redRat3;
        ModulatedSignal modSignal = null;
        //Установите истинный один раз сигнал (или ex)
        protected bool haveSignal = false;
        // == Создаем объект AV-устройства, чтобы мы могли хранить данные сигнала в файле XML.
        //private AVDeviceDB avDeviceDB;

        private IRedRat3 FindRedRat3()
        {
            try
            {
                var devices = RedRat3USBImpl.FindRedRat3s();

                if (devices.Length > 0)
                {
                    //Возьмем первое найденное устройство.
                    redRat3 = (IRedRat3)devices[0].GetRedRat();
                    //MessageBox.Show("Подключено устройство: \n\n\"" + redRat3.LocationInformation + "\"", "Проверка подключения", MessageBoxButtons.OK);
                }
            } catch (Exception ex)
            {
                new EntryPointNotFoundException("Error in search", ex).Throw();
            }
            return redRat3;
        }

        //Проверяет, xml это или нет
        bool IsXmlFile(string fileName)
        {
            var extension = fileName.Substring(startIndex: fileName.Length - 3);
            return string.Equals(extension, "xml", StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Обрабатывает входной сигнал (или ошибку) из RedRat.
        /// </summary>
        public void SignalDataHandler(object sender, EventArgs e)
        {
            if (e is SignalEventArgs)
            {
                var siea = (SignalEventArgs)e;

                switch (siea.Action)
                {
                    //case SignalEventAction.EXCEPTION:
                    //    MessageBox.Show(siea.Exceptionex.Info());
                    //    break;

                    case SignalEventAction.MODULATED_SIGNAL:
                        //MessageBox.Show("Есть ИК-данные...");
                        modSignal = siea.ModulatedSignal;
                        break;

                    //case SignalEventAction.IRDA_PACKET:
                    //    MessageBox.Show("Have IR data IRDA_PACKET...");
                    //    irPacket = siea.IrDaPacket;
                    //    break;
                    case SignalEventAction.INPUT_CANCELLED:
                        break;

                    default:
                        logger.Trace("Сигнал не пришел");
                        //MessageBox.Show("Сигнал не пришел");
                        break;
                }

                haveSignal = true;
            }
            else
            {
                logger.Warn("Event of unknown type....");
                Ex.Show("Event of unknown type....");
            }
        }

        //Захват сигнала
        public void CaptureSignal(string fileName = "")
        {
            FindRedRat3();

            redRat3.LearningSignalIn += SignalDataHandler;

            //Ввод сигнала в RedRat3 осуществляется в течении 10с
            redRat3.GetModulatedSignal(10000);

            haveSignal = false;
            while (!haveSignal)
            {
                Thread.Sleep(100);
            }
            if (modSignal != null)
            {
                #region old
                //if (IsXmlFile(fileName) )
                //{

                //    //Создаем объект AV-устройства, чтобы мы могли хранить данные сигнала в файле XML.
                //    var avDeviceDB = new AVDeviceDB();

                //    var avDevice = new AVDevice("Sample Device", AVDevice.AVDeviceType.SET_TOP_BOX);
                //    avDeviceDB.AddAVDevice(avDevice);

                //    irPacket.Name = "New Signal";

                //    //Добавить сигналы в «Sample Device».
                //    avDevice.AddSignal(irPacket, false);

                //    //Храните это как файл XML...
                //    var ser = new XmlSerializer(typeof(AVDeviceDB) );
                //    TextWriter writer = new StreamWriter( (new FileInfo(fileName) ).FullName);
                //    ser.Serialize(writer, avDeviceDB);
                //    writer.Close();

                //    MessageBox.Show("ИК-данные c ПДУ сохранены в формате XML в файл:\n" + fileName);
                //}
                //else
                //{
                //    RRUtil.SerializePacketToBinary(fileName, irPacket);
                //    MessageBox.Show("Saved in binary format to file: " + fileName);
                //}

                //xmlDoc.WriteEndDocument();
                //xmlDoc.Close();
                #endregion
            }
            else
            {
                logger.Info("Сигнал не был получен.");
                return;
            }
        }
        public ModulatedSignal GetSignal()
        {
            return modSignal;
        }
    }
}
