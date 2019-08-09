using System;
using System.IO;
using System.Data;
using System.Linq;
using System.Text;
//using WindowsInput;
using System.Drawing;
using System.Threading;
using System.Reflection;
using System.Diagnostics;
//using WindowsInput.Native;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading.Tasks;
//using System.Windows.Automation;
using System.Collections.Generic;
using System.Runtime.InteropServices;
/*using Automation = System.Windows.Automation;*/
using CA200SRVRLib;

namespace MinoltaReset
{
    public partial class Form2 : Form
    {
        //propertisForWithBalans
        public static ICas m_ICas;
        public static ICa m_ICa;
        public static IProbe m_IProbe;
        public static IMemory m_IMemory;
        public static ICa200 m_ICa200; //= (ICa200)new Ca200Class();
        public static bool isConnectedMinolta = false;
        public static int timeAfterAutostart;

        public Form2()
        {
            InitializeComponent();
        }

        public void MinoltaRestart()
        {
            Connect_Minolta();
            //Thread.Sleep(1000);
            Disconnect_CA210();
        }


        #region White Balance
        public static void Connect_Minolta()
        {
            try
            {
                m_ICa.Measure(1);
            }
            catch (NullReferenceException)
            {
                Connect_Minolta_0();
                return;
            }
            catch (Exception)
            {
                try
                {
                    m_ICa.RemoteMode = 1;
                }
                catch (Exception)
                {
                    Connect_Minolta_0();
                }
            }
            isConnectedMinolta = true;
        }

        private static void Connect_Minolta_0()
        {
            m_ICa200 = new Ca200();

            if (m_ICa200 == null)
            {
                MessageBox.Show("Не установлен драйвер для Minolta CA-210.\nБаланс белого не будет доступен.",
                    "Driver Minolta CA210 not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isConnectedMinolta = false;
                return;
            }
            //write_info("Подключение к Minolta. Ожидайте...");
            try
            {
                m_ICa200.AutoConnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка! Проверьте USB соединение с Минолтой.\n\n\n" + ex,
                    "Не удается подключить к Минолте по USB ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //this.buttonConnectUSB.Enabled = true;
                isConnectedMinolta = false;
                return;
            }
            Thread.Sleep(50);
            isConnectedMinolta = true;
            m_ICas = (ICas)m_ICa200.Cas;
            m_ICa = (ICa)m_ICas.get_ItemOfNumber(1);
            m_IProbe = (IProbe)m_ICa.SingleProbe;
            m_IMemory = (IMemory)m_ICa.Memory;
            Thread.Sleep(50);
            loopInit_CA210();
            //write_info("Подключение Minolta завершенно.");
        }

        private static void loopInit_CA210()
        {
            try
            {
                CalibrateZero();
                //Init_CA210();
            }
            catch (Exception ex)
            {
                string error1 = "SDK Command Error\n--measurement fail\n--check probe/display_setting";
                if (ex.Message.Contains(error1))
                {
                    CalibrateZero();
                    Thread.Sleep(50);
                    loopInit_CA210();
                    return;
                }
                m_ICa.RemoteMode = 0;
                isConnectedMinolta = false;
                MessageBox.Show("Error! Try again." + ex.Message, "Can't connect USB CA210",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void Init_CA210()
        {
            m_IMemory.ChannelNO = 15;
            Thread.Sleep(50);
            m_ICa.SetAnalogRange(2.5f, 2.5f);
            //if (m_ICa.DisplayMode != 0)
            //    m_ICa.DisplayMode = 0;
            //m_IMemory.SetChannelID(" ");
            //if (m_IMemory.ChannelID != "WB AutoAdj")
            //    m_IMemory.SetChannelID("WB AutoAdj");
            //m_ICa.Measure(1);

        }

        public static void CalibrateZero(string mes = "")
        {
            if (MessageBox.Show("CalZero?", "CalZero", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                return;
            }

            try
            {
                m_ICa.CalZero();
            }
            catch (Exception exCal)
            {
                MessageBox.Show("Колибровка не выполнена.\n\n" + exCal);
                string error2 = "CA Command Error\n--too bright\n--block light";
                if (exCal.Message == error2)
                {
                    CalibrateZero("Слишком ярко!");
                }
            }
        }

        public static void Disconnect_CA210()
        {
            try
            {
                m_ICa.RemoteMode = 0;
                
                m_ICa = null;
                m_ICa200 = null;
                m_IProbe = null;
                m_ICas = null;
                m_IProbe = null;
            }
            catch { }
        }
        #endregion
    }
}
