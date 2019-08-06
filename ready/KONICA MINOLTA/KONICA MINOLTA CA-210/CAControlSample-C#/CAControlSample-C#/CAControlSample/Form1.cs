//Sample Code for CA-SDK
//Copyright (c) 2016 KONICA MINOLTA, INC.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CAControlSample
{
    public partial class Form1 : Form
    {
        private CA200SRVRLib.Ca200 objCa200;
        private CA200SRVRLib.Ca objCa;
        private CA200SRVRLib.Probe objProbe;
        private CA200SRVRLib.Memory objMemory;
        private CA200SRVRLib.IProbeInfo objProbeInfo;

        private CaControl.CaControl objCaControl;

        public Form1()
        {
            InitializeComponent();

            objCa200 = new CA200SRVRLib.Ca200();
            objCa200.AutoConnect();
            objCa = objCa200.SingleCa;
            objProbe = objCa.SingleProbe;
            objMemory = objCa.Memory;
            objProbeInfo = (CA200SRVRLib.IProbeInfo)objProbe;

            objCaControl = caControlWrapper1.cacontrolobj;

            objCaControl.Ca = objCa;
            objCaControl.Probe = objProbe;
            objCaControl.Memory = objMemory;

            objCaControl.UpdateCaInfo();
            objCaControl.UpdateMemoryInfo();

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            objCa.RemoteMode = 0;
            objCa200 = null;
            objCa = null;
            objProbe = null;
            objMemory = null;
            objProbeInfo = null;

            objCaControl = null;
        }
    }
}
