using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class FuncDisplay
{
    #region private fields
    #endregion

    #region ctor
    public FuncDisplay(Func<bool> func, string displayName)
    {
        Func = func;
        DisplayName = displayName;
    }
    #endregion

    #region properties
    public Func<bool> Func { get; private set; }
    public string DisplayName { get; private set; }
    #endregion

    #region private methods
    #endregion

    #region Public Methods
    #endregion
}

