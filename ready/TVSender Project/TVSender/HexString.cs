using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TVSender
{
    public class HexString
    {
        #region fields
        private string hex = string.Empty;
        private string txt = string.Empty;
        private byte[] mbyteArray=null;
        private string hexNiceDisplay = string.Empty;
        private string txtNiceDisplay = string.Empty;
        #endregion

        #region props
        public string Hex => hex.Trim();
        public string Txt => txt.Trim('\r', '\n', ' ');
        public string HexNiceDisplay => hexNiceDisplay.Trim();
        public string TxtNiceDisplay => txtNiceDisplay.Trim('\r', '\n', ' ');
        public string Byte => (mbyteArray!=null) ? string.Join(" ", mbyteArray) : null;
        #endregion

        #region ctor
        public HexString() {}
        public HexString(byte[] byteArray)
        {
            mbyteArray = byteArray;
            GetFromByteArray(mbyteArray);           
        }
        #endregion

        #region public methods
        public void AddByte(int incByte)
        {
            string incHex = incByte.ToString("X2");
            string incTxt = ((char)incByte).ToString();
            hexNiceDisplay += $"{incHex} ";
            txtNiceDisplay += incTxt;
            hex += incHex.ToLower();
            txt += incTxt.ToLower();
        }
        public HexString FromLower(string incTxt, string incHex)
        {
            hex = incHex;
            txt = incTxt;
            hexNiceDisplay = incHex;
            txtNiceDisplay = incTxt;
            return this;
        }
        public HexString GetThisWithoutNull()
        {
            return new HexString().FromLower(txt.Replace("\0", ""), hex);
        }        
        public override string ToString()
        {
            return txt;
        }
        #endregion

        #region private methods
        private void GetFromByteArray(byte[] byteArray)
        {
            for (int i = 0; i < byteArray.Length; i++)
            {
                AddByte(byteArray[i]);
            }
        }
        #endregion
    }
}
