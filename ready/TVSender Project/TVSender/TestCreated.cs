using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Windows.Forms;

namespace TVSender
{
    public class TestCreated
    {
        #region params
        public delegate bool boolEvent(string str);
        private delegate void voidEvent(string str);
        private IniFileDuplicate.IniSectionDuplicate section;
        private ListofPairs<string, string> ListActionsValues = new ListofPairs<string, string>();
        private static Dictionary<string, string> StaticCopyListStringToTest;
        private Dictionary<string, boolEvent> ListStringToTest;
        private Dictionary<string, voidEvent> FillingMethodsList;
        List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
        private string display;
        private string msg;
        private string image;
        private string label;
        #endregion

        public TestCreated(IniFileDuplicate.IniSectionDuplicate _section)
        {
            section = _section;

            DefaultSets();

            CheckAndAddReadyReading();

            foreach (IniFileDuplicate.IniSectionDuplicate.IniKey key in section.Keys)
            {
                if (key.Value == null) key.Value = "";
                ListActionsValues.Add(key.Name, key.Value);                
                if (FillingMethodsList.ContainsKey(key.Name))
                    FillingMethodsList[key.Name](key.Value);
            }
        }

        public static System.Collections.ICollection KeysToTests { get { return StaticCopyListStringToTest; } }
        public string Path { get { return section.IniFile.Path; } }

        public bool Run()
        {
            bool FirstSelected = false;
            foreach (var todo in ListActionsValues)
                if (ListStringToTest.ContainsKey(todo.Key))
                {
                    bool result = ListStringToTest[todo.Key](todo.Value);
                    if (!result) return false;
                }
                else
                {
                    PublicData.write_pc(string.Format("{0}: \"{1}\" - программа не знает такого пункта. (F4 редактировать - \"{2}\")", display, todo.Key, section.Name));
                    if (!FirstSelected) PublicData.SetFokusListView(this);
                    FirstSelected = true;
                }
            PublicData.stopread = false;
            return true;
        }
        bool Port1_Hex(string str)
        {
            try
            {
                byte[] send = Code.StrToByte(str);
                Code.write(send);
                return true;
            }
            catch (Exception ex)
            {
                PublicData.write_pc(string.Format("{0}: {1} (F4 редактировать - \"{2}\")", display, ex.Message, section.Name));
                return false;
            }
        }
        bool Port1_Txt(string str)
        {
            byte[] send = Encoding.Default.GetBytes(str);
            Code.write(send);
            return true;
        }
        bool GenrSend(string income)
        {
            string[] parts = income.Split('_');
            string str = "";
            if (parts.Length >= 1)
            {
                str = string.Format("load tim {0};load pat ;run;", parts[0]);
            }
            if (parts.Length >= 2)
            {
                str = string.Format("load tim {0};load pat {1};run;", parts[0], parts[1]);
            }

            byte[] send = Encoding.Default.GetBytes(str);
            Code.write_genr(send);
            return true;
        }
        bool Port2_Hex(string income)
        {
            byte[] send = Code.StrToByte(income);
            Code.write_genr(send);
            return true;
        }
        bool Port2_Txt(string income)
        {
            byte[] send = Encoding.Default.GetBytes(income);
            Code.write_genr(send);
            return true;
        }
        bool IRSend(string str)
        {
            return Tests.sound(str);
        }
        bool Pause(string str)
        {
            int number;
            if(Int32.TryParse(str, out number))
                Thread.Sleep(number);
            return true;
        }
        bool FormUsual(string str)
        {
            return Tests.loadmenu(Run, msg, label, image);
        }
        bool FormOscillograph(string str)
        {
            return Tests.Oscillograph(Run, msg, label);
        }
        bool GetReadyToRead(string str)
        {
            PublicData.stopread = true;
            Code.port.DiscardInBuffer();
            Code.port.ReadTimeout = 1000;
            return true;
        }
        bool ReadAnswer(string str)
        {
            string sourceSuccess = str.Trim();

            string sourceSuccessHex = sourceSuccess.Replace(" ", "").ToLower();
            string sourceSuccessTxt = sourceSuccess.ToLower();

            string bin_answer = "";
            string txt_answer = "";

            Thread.Sleep(300);
            int gotbytes = 0;
            try { gotbytes = Code.port.BytesToRead; }
            catch { };

            try
            {
                for (int i = 0; i < gotbytes; i++)
                {
                    int bit = Code.port.ReadByte();
                    bin_answer += bit.ToString("X2") + " ";
                    txt_answer += ((char)bit).ToString();
                }
            }
            catch (TimeoutException)
            {
                PublicData.write_tv(" НЕТ ОТВЕТА от ТВ");
                //PublicData.errormsg("Перезагрузите телевизор",
                //    "Проблема в Телевизоре / LAN-кабеле");
                return false;
            }            

            bin_answer = bin_answer.Trim();

            string bin_answer_Compare = bin_answer.Replace(" ", "").ToLower();
            string txt_answer_Compare = txt_answer.ToLower();

            PublicData.write_tv("text =  " + txt_answer);
            PublicData.write_tv("code =  " + bin_answer);

            if (bin_answer_Compare.Contains(sourceSuccessHex) || txt_answer_Compare.Contains(sourceSuccessTxt))
            {
                PublicData.write_tv(display + " OK");
                PublicData.stopread = false;
                Thread.Sleep(300);
                return true;
            }

            string mes = display + " FAIL.";
            PublicData.write_tv(mes);
            string ask = display + " FAIL.";
            string hz = "Предупреждение";
            var otvet = MessageBox.Show(ask, hz, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            PublicData.stopread = false;
            return false;
            //byte[] bytesSuccess = Encoding.Default.GetBytes(SuccessTxt);
            //var hex = BitConverter.ToString(bytesSuccess);
            //string SuccessHex = hex.Replace("-", " ");
        }

        private string OpenReading = "ReadyToRead";
        private void CheckAndAddReadyReading()
        {
            foreach (IniFileDuplicate.IniSectionDuplicate.IniKey key in section.Keys)
            {                
                if (key.Value == null) key.Value = "";
                if (key.Name.Trim().ToLower() == "read")
                {
                    ListActionsValues.Add(OpenReading, "");
                }
            }
        }

        private void DefaultSets()
        {
            display = section.Name;
            msg = section.Name;
            image = section.Name;
            label = section.Name;
            FillFillingMethodsList();
            FillListStringToTest();
            FillStaticCopyListStringToTest();
        }
        private void FillListStringToTest()
        {
            ListStringToTest = new Dictionary<string, boolEvent>(StringComparer.InvariantCultureIgnoreCase);
            ListStringToTest.Add("code", Port1_Hex);
            ListStringToTest.Add("port1.code", Port1_Hex);
            ListStringToTest.Add("text", Port1_Txt);
            ListStringToTest.Add("txt", Port1_Txt);
            ListStringToTest.Add("port1.text", Port1_Txt);
            ListStringToTest.Add("port1.txt", Port1_Txt);
            ListStringToTest.Add("genr", GenrSend);                       
            ListStringToTest.Add("port2.code", Port2_Hex);
            ListStringToTest.Add("port2.txt", Port2_Txt);
            ListStringToTest.Add("port2.text", Port2_Txt);
            ListStringToTest.Add("ir", IRSend);
            ListStringToTest.Add("pause", Pause);
            ListStringToTest.Add("stop", Pause);
            ListStringToTest.Add("read", ReadAnswer);
            ListStringToTest.Add(OpenReading, GetReadyToRead);

            ListStringToTest.Add("form", FormUsual);
            ListStringToTest.Add("form1", FormUsual);
            ListStringToTest.Add("form.sound", FormOscillograph);
            ListStringToTest.Add("form2", FormOscillograph);
            #region nothing
            ListStringToTest.Add("display", VisualParameters);
            ListStringToTest.Add("message", VisualParameters);
            ListStringToTest.Add("msg", VisualParameters);            
            ListStringToTest.Add("image", VisualParameters);
            ListStringToTest.Add("label", VisualParameters);
            #endregion            
        }
        private void FillStaticCopyListStringToTest()
        {
            StaticCopyListStringToTest = new Dictionary<string, string>();
            foreach (var tab in ListStringToTest)
            {
                StaticCopyListStringToTest.Add(tab.Key, tab.Value.Method.Name);
            }
        }        
        private void FillFillingMethodsList()
        {
            FillingMethodsList = new Dictionary<string, voidEvent>(StringComparer.InvariantCultureIgnoreCase);
            FillingMethodsList.Add("display", SetDisplay);
            FillingMethodsList.Add("name", SetDisplay);
            FillingMethodsList.Add("image", SetImage);
            FillingMethodsList.Add("message", SetMsg);
            FillingMethodsList.Add("msg", SetMsg);
            FillingMethodsList.Add("label", SetLabel);
        }        
        bool VisualParameters(string value)
        { return true; }
        void SetImage(string value)
        {
            image = value;
        }
        void SetMsg(string value)
        {
            msg = value;
        }
        void SetDisplay(string value)
        {
            display = value;
        }
        void SetLabel(string value)
        {
            label = value;
        }
        public string Display
        {
            get { return display; }
        }
    }
}
