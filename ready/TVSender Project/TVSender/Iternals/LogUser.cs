using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Security.Permissions;
using System.Diagnostics;
using NLog;

//[assembly: SecurityPermissionAttribute(SecurityAction.RequestMinimum, UnmanagedCode = true)]
//[assembly: PermissionSetAttribute(SecurityAction.RequestMinimum, Name = "FullTrust")]

public class LogUser
{
    private static Logger logger = LogManager.GetCurrentClassLogger();
    string domainName = "HORIZONT";
    string userName;
    string password;
    public bool isLogIn = false;
    IntPtr tokenHandle = new IntPtr(0);
    WindowsImpersonationContext impersonatedUser;
    #region WinAPI
    [DllImport("advapi32.dll", SetLastError = true)]
    public static extern bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword,
    int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

    [DllImport("kernel32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
    private unsafe static extern int FormatMessage(int dwFlags, ref IntPtr lpSource,
    int dwMessageId, int dwLanguageId, ref String lpBuffer, int nSize, IntPtr* Arguments);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    public extern static bool CloseHandle(IntPtr handle);

    [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public extern static bool DuplicateToken(IntPtr ExistingTokenHandle,
    int SECURITY_IMPERSONATION_LEVEL, ref IntPtr DuplicateTokenHandle);

    [PermissionSetAttribute(SecurityAction.Demand, Name = "FullTrust")]
    #endregion

    public LogUser(string _userName, string _password)
    {
        userName = _userName;
        password = _password;
        In();
    }
    public LogUser()
    {
        userName = "savchuk";
        password = "Syi 789";
        In();
    }

    private void In()
    {
        IntPtr dupeTokenHandle = new IntPtr(0);
        try
        {
            // получаем пользовательский токен для определения пользователя, домена, и пароля используется небезопасный LogonUser метод
            //Имя локальной машины может быть использовано для доменного имени, чтобы играть роль пользователя этой машины
            const int LOGON32_PROVIDER_DEFAULT = 0;
            const int LOGON32_LOGON_INTERACTIVE = 2;
            tokenHandle = IntPtr.Zero;

            //Вызов LogonUser, чтобы получить дескриптор для доступа 
            bool returnValue = LogonUser(userName, domainName, password,
            LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT,
            ref tokenHandle);


            if (false == returnValue)
            {
                int ret = Marshal.GetLastWin32Error();
                logger.Info($"throw LogonUser failed with error code : {ret}" );
                throw new System.ComponentModel.Win32Exception(ret);
            }

            // проверить идентификацию 
            logger.Info("Пользователь до подмены учетной записи: " + WindowsIdentity.GetCurrent().Name);

            // Используемый дескриптор токена возвратить LogonUser-у.
            WindowsIdentity newId = new WindowsIdentity(tokenHandle);
            impersonatedUser = newId.Impersonate();

            // текущий пользователь
            logger.Info("После подмены учетной записи: " + WindowsIdentity.GetCurrent().Name);
            isLogIn = true;
        }
        catch (Exception ex)
        {
            logger.Error(ex, ex.Message);          
        }
    }
    public void Out()
    {
        logger.Debug("Перед отменой учетной записи: " + WindowsIdentity.GetCurrent().Name);
        try
        {            
            // выйти из-под учетной записи
            impersonatedUser.Undo();
            // текущий пользователь
            logger.Debug("После отмены подмены учетной записи: " + WindowsIdentity.GetCurrent().Name);
            // освобождаем токены
            if (tokenHandle != IntPtr.Zero)
                CloseHandle(tokenHandle);
            isLogIn = false;
        }
        catch (Exception ex)
        {
            logger.Error(ex, ex.Message);            
        }
    }    
}

