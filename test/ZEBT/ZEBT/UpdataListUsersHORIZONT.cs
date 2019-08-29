using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using System.Data;
using System.Text;
using System.IO;
using System;

using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.Protocols;

using Outlook = Microsoft.Office.Interop.Outlook;
using AddressEntries = Microsoft.Office.Interop.Outlook.AddressEntries;

namespace ZEBT
{
    public partial class UpdataListUsersHORIZONT : Form
    {
        public UpdataListUsersHORIZONT()
        {
            InitializeComponent();
        }


        //кнопка обновления учетных записей horizont.by
        private void button1_Click(object sender, EventArgs e)
        {
            Thread threads = new Thread(EnumerateGAL);
            threads.Start();
        }

        //Получает все учетные записи с почты.
        private void EnumerateGAL()
        {
            try
            {
                Outlook.Application app = new Outlook.Application();
                Outlook.NameSpace ns = app.GetNamespace("MAPI");
                ns.Logon("", "", false, true);

                Outlook.AddressList gal = ns.Session.GetGlobalAddressList();
                progressBar1.Maximum = gal.AddressEntries.Count;
                if (gal != null)
                {
                    int n = 1;
                    for (int i = 1; i <= gal.AddressEntries.Count; i++)
                    {
                        Outlook.AddressEntry addrEntry = gal.AddressEntries[i];
                        if (addrEntry.AddressEntryUserType == Outlook.OlAddressEntryUserType.olExchangeUserAddressEntry ||
                            addrEntry.AddressEntryUserType == Outlook.OlAddressEntryUserType.olExchangeRemoteUserAddressEntry)
                        {
                            Outlook.ExchangeUser exchUser = addrEntry.GetExchangeUser();
                            Program.UpdataUsers.Add(exchUser.Name, exchUser.PrimarySmtpAddress);
                            progressBar1.Value = i;
                        }
                        #region один логин выпадает там
                        //if (addrEntry.AddressEntryUserType == Outlook.OlAddressEntryUserType.olExchangeDistributionListAddressEntry)
                        //{
                        //    Outlook.ExchangeDistributionList exchDL = addrEntry.GetExchangeDistributionList();
                        //    //MessageBox.Show(exchDL.Name + " " + exchDL.PrimarySmtpAddress);
                        //        using (StreamWriter sw = new StreamWriter(nameUsers2, true, System.Text.Encoding.Default))
                        //        {
                        //            sw.WriteLine("2 file: " + exchDL.Name + " - " + exchDL.PrimarySmtpAddress);
                        //        }
                        //    progressBar1.Value = i;
                        //}
                        #endregion
                        n++;
                    }
                    if (n > gal.AddressEntries.Count)
                        progressBar1.Value = 0;
                        Close();
                }
            }
            catch (Exception e) { MessageBox.Show(e.Message); }
        }
    }
}
