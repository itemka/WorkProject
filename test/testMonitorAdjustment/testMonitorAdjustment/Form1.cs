using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Management;
using System.Runtime.InteropServices;

namespace testMonitorAdjustment
{

    public partial class Form1 : Form
    {
        

        public Form1()
        {
            InitializeComponent();
        }

        public void SetBrightness(byte targetBrightness)
        {
            ManagementScope scope = new ManagementScope(System.Environment.MachineName +"\\root\\WMI");
            SelectQuery query = new SelectQuery("WmiMonitorBrightnessMethods");
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query))
            {
                using (ManagementObjectCollection objectCollection = searcher.Get())
                {
                    foreach (ManagementObject mObj in objectCollection)
                    {
                        mObj.InvokeMethod("WmiSetBrightness",
                            new Object[] { UInt32.MaxValue, targetBrightness });
                        break;
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(System.Environment.MachineName);
            SetBrightness(10);
        }
    }

    /// <summary>
    /// Class for manipulating the brightness of the screen
    /// </summary>
    //public static class Brightness
    //{
    //    [DllImport("gdi32.dll")]
    //    private unsafe static extern bool SetDeviceGammaRamp(Int32 hdc, void* ramp);

    //    private static bool initialized = false;
    //    private static Int32 hdc;


    //    private static void InitializeClass()
    //    {
    //        if (initialized)
    //            return;

    //        //Get the hardware device context of the screen, we can do
    //        //this by getting the graphics object of null (IntPtr.Zero)
    //        //then getting the HDC and converting that to an Int32.
    //        hdc = Graphics.FromHwnd(IntPtr.Zero).GetHdc().ToInt32();

    //        initialized = true;
    //    }

    //    public static unsafe bool SetBrightness(short brightness)
    //    {
    //        InitializeClass();

    //        if (brightness > 255)
    //            brightness = 255;

    //        if (brightness < 0)
    //            brightness = 0;

    //        short* gArray = stackalloc short[3 * 256];
    //        short* idx = gArray;

    //        for (int j = 0; j < 3; j++)
    //        {
    //            for (int i = 0; i < 256; i++)
    //            {
    //                int arrayVal = i * (brightness + 128);

    //                if (arrayVal > 65535)
    //                    arrayVal = 65535;

    //                *idx = (short)arrayVal;
    //                idx++;
    //            }
    //        }

    //        //For some reason, this always returns false?
    //        bool retVal = SetDeviceGammaRamp(hdc, gArray);

    //        //Memory allocated through stackalloc is automatically free'd
    //        //by the CLR.

    //        return retVal;

    //    }
    //}

}
