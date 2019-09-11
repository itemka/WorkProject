using System;
using RedRat.USB;
using System.Text;
using RedRat.RedRat3;
using RedRat.RedRat3.USB;
using System.Windows.Forms;

namespace RedRat3
{
    // Поиск RedRat
    public class SearchRedRat
    {
        public IRedRat3 RedRat3;

        // Просто находит первый RedRat3, подключенный к этому компьютеру.
        public IRedRat3 FindRedRat()
        {
            try
            {
                var devices = RedRat3USBImpl.FindDevices();
                if (devices.Count > 0)
                {
                    //Возьмем первое найденное устройство или пустоту
                    RedRat3 = (IRedRat3)devices[0].GetRedRat();
                }
                else
                {
                    RedRat3 = null;
                    MessageBox.Show("RedRat не найден.", "Поиск RedRat", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка поиска RedRat.\n" + ex.Message, "Поиск RedRat", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return RedRat3;
        }


        // Просто находит первый RedRat3, подключенный к этому компьютеру. + выводит информацию
        public IRedRat3 FindRedRatInfo()
        {
            try
            {
                // Поис RedRat3
                var devices = RedRat3USBImpl.FindDevices();
                //Экземпляр метода добавления к другой строке
                var RedRatInfo = new StringBuilder();

                if (devices.Count > 0)
                {
                    // Возьмем первое найденное устройство.
                    RedRat3 = (IRedRat3)devices[0].GetRedRat();

                    // Физическая информация об оборудовании USB
                    var devInfo = (USBDeviceInfo)RedRat3.DeviceInformation;
                    RedRatInfo.Append("RedRat подключен!");
                    RedRatInfo.Append("\n\nОписание:");
                    // Физическая позиция возвращается в объекте LocationInfo
                    RedRatInfo.Append("\n- Location: " + RedRat3.LocationInformation);
                    RedRatInfo.Append("\n- Info: " + devInfo);
                    RedRatInfo.Append("\n- Hardware Version: " + devInfo.ProductName + "." + devInfo.ProductDescriptor.Version);
                    // Информация о версии прошивки читается напрямую с RedRat3
                    RedRatInfo.Append("\n- Firmware Version: " + RedRat3.FirmwareVersion);
                    RedRatInfo.Append("\n- Serial Number: " + devInfo.SerialNumber);

                    MessageBox.Show(RedRatInfo.ToString(), "Поиск RedRat", MessageBoxButtons.OK);
                }
                else
                    MessageBox.Show("RedRat не найден.", "Поиск RedRat", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка поиска RedRat.\n" + ex.Message, "Поиск RedRat", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return RedRat3;
        }
    }
}