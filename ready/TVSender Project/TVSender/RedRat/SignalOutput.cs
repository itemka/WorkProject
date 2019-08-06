using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using DisableDevice;
using NLog;
using RedRat.AVDeviceMngmt;
using RedRat.IR;
using RedRat.RedRat3;
using RedRat.RedRat3.USB;

namespace TVSender
{
    public class SignalOutput
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public IRedRat3 redRat3;
        public string[] deviceName;
        public string[] signalName;
        public IRPacket signal;

        public SignalOutput() { }
        public SignalOutput(IRPacket _signal)
        {
            signal = _signal;
        }

        public void Output()
        {
            //logger.Trace("start");
            var foundRedrat = FindRedRat3();
            try
            {
                foundRedrat.OutputModulatedSignal(signal);
            }
            catch (Exception ex)
            {
                ex.Log();
                PublicData.write_info($"Ошибка RedRat: {ex.Message}");
                if (RestartRedratDevice())
                {
                    foundRedrat = FindRedRat3();
                    foundRedrat.OutputModulatedSignal(signal);
                }
            }
            //logger.Trace("end");
        }

        public void XmlOutput(string pathToFile)
        {

            try
            {
                //if (FindRedRat3() != null)
                //{

                using (var RR3 = FindRedRat3())
                {
                    var signalDB = LoadSignalDB(pathToFile);
                    string deviceN = string.Join(null, deviceName);
                    //MessageBox.Show("Device - " + deviceN);

                    //Задаем дивайс, чтобы вытащить имя сигнала
                    var Device = signalDB.GetAVDevice(deviceN);
                    signalName = Device.GetSignalNames();

                    string signalN = string.Join(null, signalName);
                    //MessageBox.Show("Signal - " + signalN);
                    signal = GetSignal(signalDB, deviceN, signalN);
                    //RR3.OutputModulatedSignal(signal);
                    //MessageBox.Show("Вывод сигнала " + deviceN + " -> " + signalN + ". \n");
                }
            } catch (Exception ex)
            {
                ex.Show("Error in signal upload");
            }

        }

        /// <summary>
        /// Просто находит первый RedRat3, подключенный к этому компьютеру.
        /// </summary>
        public IRedRat3 FindRedRat3()
        {
            //logger.Trace("start");
            try
            {
                //var devices = RedRat3USBImpl.FindDevices();
                var devices = RedRat3USBImpl.FindRedRat3s();
                redRat3 = (devices.Length > 0) ? (IRedRat3)devices[0].GetRedRat() : null;
            } catch (Exception ex)
            {
                //MessageBox.Show("Ошибка в поиске RedRat.\n" + ex.Info(), "Ошибка в поиске RedRat.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                logger.Info($"Ошибка {ex.Message}");
                PublicData.write_info($"Не найден RedRat: {ex.Message}");
                if (RestartRedratDevice())
                {
                    var devices = RedRat3USBImpl.FindRedRat3s();
                    logger.Trace($"Попытка after WinApi restart found Redrat {nameof(devices)}={devices?.Length}");
                    redRat3 = (devices.Length > 0) ? (IRedRat3)devices[0].GetRedRat() : null;
                    logger.Trace("Успешно");
                }
            }
            //logger.Trace("end");
            return redRat3;
        }

        private static bool RestartRedratDevice()
        {
            try
            {
                DeviceManagerApi.ToogleRedRat(false);
                Thread.Sleep(100);
                DeviceManagerApi.ToogleRedRat(true);
                Thread.Sleep(50);
                return true;
            } catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
                PublicData.write_info($"Не удалось перезапустить Redrat - придется сделать это вручную.\n{ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Загружает XML-файл базы данных сигналов.
        /// </summary>
        private AVDeviceDB LoadSignalDB(string path)
        {
            //Чтение базы данных сигналов из файла XML.
            AVDeviceDB newAVDeviceDB = null;

            var serializer = new XmlSerializer(typeof(AVDeviceDB));
            //var fileInfo = new FileInfo(openFileDialog.FileName);
            FileStream fs = null;

            try
            {
                fs = new FileStream((new FileInfo(path)).FullName, FileMode.Open);
                newAVDeviceDB = (AVDeviceDB)serializer.Deserialize(fs);
            } catch (Exception ex)
            {
                ex.Show($"Ошибка открытия файла:\n{path}");
            } finally
            {
                if (fs != null)
                    fs.Close();
            }

            var dev = newAVDeviceDB.AVDevices;

            deviceName = newAVDeviceDB.GetAVDeviceNames();

            return newAVDeviceDB;
        }

        /// <summary>
        /// Возвращает объект ИК-сигнала из файла DB сигнала, используя имя deviceName и signalName для поиска.
        /// </summary>
        private IRPacket GetSignal(AVDeviceDB signalDB, string deviceName, string signalName)
        {
            var device = signalDB.GetAVDevice(deviceName);
            if (device == null)
            {
                throw new Exception(string.Format("В базе данных сигналов нет устройства с именем '{0}'.", deviceName));
            }
            var signal = device.GetSignal(signalName);
            if (signal == null)
            {
                throw new Exception(string.Format("Нет сигнала с именем '{0}', найденным для устройства '{1}' в базе данных сигналов.", signalName, deviceName));
            }
            return signal;
        }
    }
}
