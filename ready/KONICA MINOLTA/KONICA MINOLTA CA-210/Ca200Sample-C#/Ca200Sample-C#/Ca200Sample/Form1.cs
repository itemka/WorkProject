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

namespace Ca200Sample
{
    public partial class Form1 : Form
    {
        private CA200SRVRLib.Ca200 objCa200;
        private CA200SRVRLib.Ca objCa;
        private CA200SRVRLib.Probe objProbe;
        private Boolean isMsr;
        long vbObjectError = -2147221504;

        public Form1()
        {
            InitializeComponent();
            try
            {
                objCa200 = new CA200SRVRLib.Ca200();
                objCa200.AutoConnect();
                objCa = objCa200.SingleCa;
                objProbe = objCa.SingleProbe;

                objCa.ExeCalZero += new CA200SRVRLib._ICaEvents_ExeCalZeroEventHandler(objCa_ExeCalZero);

                ButtonCancel.Enabled = false;
                ButtonMeasure.Enabled = true;
                ButtonCalZero.Enabled = true;
            }
            catch (Exception er)
            {
                DisplayError(er);
                Application.Exit();
            }
        }

        private void DisplayError(Exception er)
        {
            String msg;
            msg = "Error from" + er.Source + "\r\n";
            msg += er.Message + "\r\n";
            msg += "HR:" + (er.HResult - vbObjectError).ToString();
            MessageBox.Show(msg);
        }

        private void ButtonMeasure_Click(object sender, EventArgs e)
        {
            int i;
            try
            {
                isMsr = true;

                ButtonCancel.Enabled = true;
                ButtonMeasure.Enabled = false;
                ButtonCalZero.Enabled = false;

                for (i = 0; i < 20; i++)
                {
                    objCa.Measure();
                    LabelLv.Text = objProbe.Lv.ToString("##0.00");
                    Labelx.Text = objProbe.sx.ToString("0.0000");
                    Labely.Text = objProbe.sy.ToString("0.0000");
                    LabelT.Text = objProbe.T.ToString("####");
                    Labelduv.Text = objProbe.duv.ToString("0.0000");
                    Application.DoEvents();

                    if (isMsr == false)
                    {
                        break;
                    }
                }

                ButtonCancel.Enabled = false;
                ButtonMeasure.Enabled = true;
                ButtonCalZero.Enabled = true;

            }
            catch (Exception er)
            {
                DisplayError(er);
                Application.Exit();
            }
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            isMsr = false;
            ButtonCancel.Enabled = false;
            ButtonMeasure.Enabled = false;
            ButtonCalZero.Enabled = false;
        }

        private void ButtonCalZero_Click(object sender, EventArgs e)
        {
            bool calzero_success = false;

            while (calzero_success == false)
            {
                ButtonMeasure.Enabled = false;
                ButtonCalZero.Enabled = false;

                try
                {
                    objCa.CalZero();
                    calzero_success = true;
                }
                catch (Exception er)
                {
                    DisplayError(er);
                    if (MessageBox.Show("Zero Cal Error\r\nRetry?", "CalZero", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                    {
                        objCa.RemoteMode = 0;
                        Application.Exit();
                    }
                }
            }

            ButtonMeasure.Enabled = true;
            ButtonCalZero.Enabled = true;
        }

        private void objCa_ExeCalZero()
        {
            if (MessageBox.Show("CalZero?", "CalZero", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                return;
            }
            ButtonMeasure.Enabled = false;
            ButtonCalZero.Enabled = false;

            try
            {
                objCa.CalZero();
            }
            catch (Exception er)
            {
                DisplayError(er);
            }

            ButtonMeasure.Enabled = true;
            ButtonCalZero.Enabled = true;

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            objCa.RemoteMode = 0;
            objCa200 = null;
            objCa = null;
            objProbe = null;
        }

    }
}
