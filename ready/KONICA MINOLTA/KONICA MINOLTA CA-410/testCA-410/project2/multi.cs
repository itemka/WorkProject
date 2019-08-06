using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CASDK2;

//(C) Copyright KONICA MINOLTA, INC. All rights reserved. 2017.

namespace project2
{
    ///<summary>
    ///[Multiple measurement]
    ///This class connect and measure by multi devices
    ///</summary>
    class multi
    {
        const int MAX_COUNT = 10;
        int err = 0;
        int ca_count = 1;
        CASDK2Ca200 objCa200;
        CASDK2Cas objCas;
        CASDK2Ca [] objCa;
        CASDK2Probes [] objProbes;
        CASDK2OutputProbes [] objOutputProbes;
        CASDK2Probe [] objProbe;
        CASDK2Memory [] objMemory;

        public multi(int count)
        {
            if(count <1)
            {
                ca_count = 1;
            }
            else if(count >MAX_COUNT)
            {
                ca_count = MAX_COUNT;
            }
            else
            {
                ca_count = count;
            }
            objCa = new CASDK2Ca[ca_count];
            objProbes = new CASDK2Probes[ca_count];
            objOutputProbes = new CASDK2OutputProbes[ca_count];
            objProbe = new CASDK2Probe[ca_count];
            objMemory = new CASDK2Memory[ca_count];
        }

        ~multi()
        {
            Discconect();
        }

        ///<summary>
        ///[Multiple measurement]
        ///This method connect multi devices by USB
        ///</summary>
        public void MultiConnect()
        {
            objCa200 = new CASDK2Ca200();

            for (int ca = 0; ca < ca_count; ca++)
            {
                //Use another ID ex. ca+1
                GetErrorMessage(objCa200.SetConfiguration(ca + 1, "1", 0, 38400, 0));

            }
            // Substitute object variables
            err = objCa200.get_Cas(ref objCas);
            for (int ca = 0; ca < ca_count; ca++)
            {
                GetErrorMessage(objCas.get_Item(ca + 1, ref objCa[ca]));
                GetErrorMessage(objCa[ca].get_Probes(ref objProbes[ca]));
                GetErrorMessage(objCa[ca].get_OutputProbes(ref objOutputProbes[ca]));
                GetErrorMessage(objCa[ca].get_Memory(ref objMemory[ca]));
                GetErrorMessage(objProbes[ca].get_Item(1, ref objProbe[ca]));
                GetErrorMessage(objOutputProbes[ca].AddAll());
                GetErrorMessage(objOutputProbes[ca].get_Item(1, ref objProbe[ca]));
            }
        }

        ///<summary>
        ///[Multiple measurement]
        ///This method disconnect multi devices and delete objects
        ///</summary>
        public void Discconect()
        {
            //Disconnect CA-410
            GetErrorMessage(objCa200.DisconnectAll());
        }

        ///<summary>
        ///[Multiple measurement]
        ///This method set configuration for multi devices
        ///</summary>
        public void MultiSetting()
        {
            int freqmode = 4;   // SyncMode : INT 
            double freq = 60.0; //frequency = 60.0Hz
            int speed = 1;      //Measurement speed : FAST
            int Lvmode = 1;     //Lv : cd/m2

            for (int ca = 0; ca < ca_count; ca++)
            {

                GetErrorMessage(objCa[ca].CalZero());                      //Zero-Calibration
                GetErrorMessage(objCa[ca].put_DisplayProbe("P1"));         //Set display probe to P1
                GetErrorMessage(objCa[ca].put_SyncMode(freqmode, freq));   //Set sync mode and frequency
                GetErrorMessage(objCa[ca].put_AveragingMode(speed));       //Set measurement speed
                GetErrorMessage(objCa[ca].put_BrightnessUnit(Lvmode));     //Set Brightness unit
            }
        }

        ///<summary>
        ///[Multiple measurement]
        ///This method measure color for multi devices
        ///</summary>
        public void MultiMeasurement()
        {
            for (int ca = 0; ca < ca_count; ca++)
            {
                GetErrorMessage(objCa[ca].put_DisplayMode(0));     //Set Lvxy mode
            }
            //Use SendMsr() and ReceiveMsr() for multi probes
            GetErrorMessage(objCas.SendMsr());         //Measure
            GetErrorMessage(objCas.ReceiveMsr());      //Get results

            // Declaration variables for measurement data
            double [] Lv = new double[ca_count];
            double [] sx = new double[ca_count];
            double [] sy = new double[ca_count];
            double [] X = new double[ca_count];
            double [] Y = new double[ca_count];
            double [] Z = new double[ca_count];

            for (int ca = 0; ca < ca_count; ca++)
            {

                // Get measurement data
                GetErrorMessage(objProbe[ca].get_Lv(ref Lv[ca]));
                GetErrorMessage(objProbe[ca].get_sx(ref sx[ca]));
                GetErrorMessage(objProbe[ca].get_sy(ref sy[ca]));
                GetErrorMessage(objProbe[ca].get_X(ref X[ca]));
                GetErrorMessage(objProbe[ca].get_Y(ref Y[ca]));
                GetErrorMessage(objProbe[ca].get_Z(ref Z[ca]));

                Console.WriteLine("Lv：" + Lv[ca] + " x:" + sx[ca] + " y:" + sy[ca]);
                Console.WriteLine("X:" + X[ca] + " Y:" + Y[ca] + " Z:" + Z[ca]);
            }
        }

        ///<summary>
        ///[Errorhandling]
        ///This method display Error message from Error number
        ///</summary>
        ///<param name = "errornum">Error number from API of SDK</param>
        private void GetErrorMessage(int errornum)
        {
            string errormessage = "";
            if (errornum != 0)
            {
                //Get Error message from Error number
                err = GlobalFunctions.CASDK2_GetLocalizedErrorMsgFromErrorCode(0, errornum, ref errormessage);
                Console.WriteLine(errormessage);
            }
        }
    }

    ///<summary>
    ///[Multiple measurement for Single Data processor]
    ///This class connect and measure by multi probes connected to single Data processor
    ///<param name = "connectprobe">Connect probes. true:connect, false: not connect[0 to 9 -> P1 to P10]</param>
    ///</summary>
    class singleDPmultiprobe
    {
        const int MAX_COUNT = 10;
        int err = 0;
        int probe_count = 0;
        bool [] IsEnablePort = Enumerable.Repeat<bool>(false, MAX_COUNT).ToArray();
        string port = "";
        CASDK2Ca200 objCa200;
        CASDK2Cas objCas;
        CASDK2Ca objCa;
        CASDK2Probes objProbes;
        CASDK2OutputProbes objOutputProbes;
        CASDK2Probe[] objProbe;
        CASDK2Memory objMemory;

        public singleDPmultiprobe(bool [] connectprobe)
        {
            objCa200 = new CASDK2Ca200();
            objProbe = new CASDK2Probe[MAX_COUNT];
            for (int probe = 0; probe < MAX_COUNT; probe++)
            {
                IsEnablePort[probe] = connectprobe[probe];      //Set connect port by bool(IsEnablePort)
            }
            //port = createchar();	//Change to character from bool
            createstring();
        }

        ///<summary>
        ///[Multiple measurement for Single Data processor]
        ///This method change connect probe to string(port) for SetConfiguration method from bool(IsEnablePort).
        ///</summary>
        private void createstring()
        {
            string portbuf = "0";
            for (int probe = 0; probe < MAX_COUNT; ++probe)
            {
                if (IsEnablePort[probe])
                {
                    if (portbuf == "0")
                    {
                        portbuf = "";
                    }
                    else
                    {
                        portbuf += " ";
                    }
                    portbuf += (probe + 1);
                    probe_count++;
                }
            }
            port = portbuf;
            
        }

        ///<summary>
        ///[Multiple measurement for Single Data processor]
        ///This method connect multi probes by USB
        ///</summary>
        public void sDPConnect()
        {
            GetErrorMessage(objCa200.SetConfiguration(1, port, 0, 38400, 0));
            err = objCa200.get_Cas(ref objCas);
            GetErrorMessage(objCas.get_Item(1, ref objCa));
            GetErrorMessage(objCa.get_Probes(ref objProbes));
            GetErrorMessage(objCa.get_OutputProbes(ref objOutputProbes));
            GetErrorMessage(objCa.get_Memory(ref objMemory));
            for (int probe = 0; probe < probe_count; probe++)
            {
                GetErrorMessage(objProbes.get_Item(probe + 1, ref objProbe[probe]));
                GetErrorMessage(objOutputProbes.AddAll());
                GetErrorMessage(objOutputProbes.get_Item(probe + 1, ref objProbe[probe]));
            }
        }

        ~singleDPmultiprobe()
        {
            sDPDisconnect();
        }

        ///<summary>
        ///[Multiple measurement for Single Data processor]
        ///This method disconnect multi probes and delete objects
        ///</summary>
        private void sDPDisconnect()
        {
            //Disconnect CA-410
            err = objCa200.DisconnectAll();
        }

        ///<summary>
        ///[Multiple measurement for Single Data processor]
        ///This method set configuration for multi probes
        ///</summary>
        public void sDPSetting()
        {
            int freqmode = 4;   // SyncMode : INT 
            double freq = 60.0; //frequency = 60.0Hz
            int speed = 1;      //Measurement speed : FAST
            int Lvmode = 1;     //Lv : cd/m2
            
            string ID = "";
            GetErrorMessage(objProbe[0].get_ID(ref ID));
            GetErrorMessage(objCa.CalZero());                       //Zero-Calibration
            GetErrorMessage(objCa.put_DisplayProbe(ID));            //Set display probe to P1
            GetErrorMessage(objCa.put_SyncMode(freqmode, freq));    //Set sync mode and frequency
            GetErrorMessage(objCa.put_AveragingMode(speed));        //Set measurement speed
            GetErrorMessage(objCa.put_BrightnessUnit(Lvmode));      //Set Brightness unit
        }

        ///<summary>
        ///[Multiple measurement for Single Data processor]
        ///This method measure color for multi probes
        ///</summary>
        public void sDPMeasurement()
        {
            GetErrorMessage(objCa.put_DisplayMode(0));     //Set Lvxy mode
            GetErrorMessage(objCa.Measure());

            // Declaration variables for measurement data
            double [] Lv = new double[probe_count];
            double [] sx = new double[probe_count];
            double [] sy = new double[probe_count];
            double [] X = new double[probe_count];
            double [] Y = new double[probe_count];
            double [] Z = new double[probe_count];

            for (int probe = 0; probe < probe_count; probe++)
            {
                string ID = "";
                GetErrorMessage(objProbe[probe].get_ID(ref ID));
                Console.WriteLine(ID + " ");
                // Get measurement data
                GetErrorMessage(objProbe[probe].get_Lv(ref Lv[probe]));
                GetErrorMessage(objProbe[probe].get_sx(ref sx[probe]));
                GetErrorMessage(objProbe[probe].get_sy(ref sy[probe]));
                GetErrorMessage(objProbe[probe].get_X(ref X[probe]));
                GetErrorMessage(objProbe[probe].get_Y(ref Y[probe]));
                GetErrorMessage(objProbe[probe].get_Z(ref Z[probe]));

                Console.WriteLine("Lv:" + Lv[probe] + " x:" + sx[probe] + " y:" + sy[probe]);
                Console.WriteLine("X:" + X[probe] + " Y:" + Y[probe] + " Z:" + Z[probe]);
            }
        }

        ///<summary>
        ///[Errorhandling]
        ///This method display Error message from Error number
        ///</summary>
        ///<param name = "errornum">Error number from API of SDK</param>
        private void GetErrorMessage(int errornum)
        {
            string errormessage = "";
            if (errornum != 0)
            {
                //Get Error message from Error number
                err = GlobalFunctions.CASDK2_GetLocalizedErrorMsgFromErrorCode(0, errornum, ref errormessage);
                Console.WriteLine(errormessage);
            }
        }
    }

}
