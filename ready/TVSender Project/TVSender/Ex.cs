using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks.Schedulers;
using System.Reflection;

namespace TVSender
{
    public static class Ex
    {
        #region const
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static readonly string n = Environment.NewLine;
        private static readonly string defaultFunc = "${callsite:cleanNamesOfAnonymousDelegates=true:cleanNamesOfAsyncContinuations=true}";
        public static readonly TaskFactory LongTask = new TaskFactory(TaskCreationOptions.LongRunning, TaskContinuationOptions.None);
        private static readonly TaskScheduler IOTask = new IOTaskScheduler();
        public static readonly Task TaskEmpty = TaskEx.Run(() => { });
        #endregion

        #region props
        public static bool isAutonomMode { get; set; } = false;
        #endregion

        #region Public Methods
        public static Task LongRun(Action func)
        {            
            return Task.Factory.StartNew(func
                //, new CancellationToken()
                , TaskCreationOptions.LongRunning
                //, IOTask
                );
        }
        public static Task<T> LongRun<T>(Func<T> func)
        {
            return Task.Factory.StartNew(func
                , new CancellationToken()
                , TaskCreationOptions.LongRunning
                , IOTask
                );
        }
        public static async Task RunUIAwait(TaskScheduler context, Action func)
        {
            await Task.Factory.StartNew(func, new CancellationToken(), TaskCreationOptions.None, context);
        }
        public static Task RunUI(TaskScheduler context, Action func)
        {
            return Task.Factory.StartNew(func, new CancellationToken(), TaskCreationOptions.None, context);
        }        
        public static bool Try(Action func)
        {
            return Try(true, func);
        }
        public static bool Try(bool isLog, Action func)
        {
            try
            {
                func();
                return true;
            }
            catch (Exception ex)
            {
                if (isLog) { logger.Trace(ex, ex.Message); }
                return false;
            }
        }
        public static async Task<bool> TryAsync(Action func, string msg = null)
        {
            Task task = TaskEx.Run(() => func);
            return await Try(task);
        }
        public static async Task<bool> Try(Task func, string msg = null)
        {
            try
            {
                await func;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool TryLog(Action func)
        {
            try
            {
                func();
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(func, ex);
                return false;
            }
        }
        public static void Log(this Exception ex, string msg=null)
        {
            string result = ex.Message.prefix(msg);
            logger.ErrorStack(ex, result);
            PublicData.write_info(result);
        }
        public static bool Try(Action tryFunc, Action<Exception> catchFunc)
        {
            try
            {
                tryFunc();
                return true;
            }
            catch(Exception ex)
            {                
                catchFunc(ex);
                return false;
            }
        }
        public static bool Catch(Action func, string msg = null)
        {
            try
            {
                func();
                return true;
            }
            catch (Exception ex)
            {
                ex.Show(func);                
                return false;
            }
        }
        public static bool Catch(string msg, Action func)
        {
            return Catch(func, msg);
        }
        public static async Task<bool> Catch(Task func, string msg = null)
        {
            try
            {
                await func;
                return true;
            }
            catch (Exception ex)
            {
                ex.Show(msg);
                return false;
            }
        }
        public static async Task<T> Catch<T>(Task<T> func, T ifError = default(T))
        {
            try
            {
                return await func;
            }
            catch (Exception ex)
            {
                ex.Show("Critical ERROR !!! CatchTask");
                return ifError;
            }
        }
        public static async Task<bool> CatchAsync(Action func, string msg = null)
        {
            Task task = TaskEx.Run(() => func);
            return await Catch(task);
        }
        public static void Throw(Action func)
        {
            try
            {
                func();
            } catch (Exception ex)
            {
                logger.Error(func, ex, "throw "+ex.Message);                
                throw;
            }
        }
        public static void Throw(this Exception ex)
        {
            logger.ErrorStack(ex, "throw "+ex.Message);
            throw ex;
        }
        public static void Show(this Exception ex, string msg = null)
        {
            string resultMsg = msg;
            Try(()=>
            {
                resultMsg = ex.Message.prefix(msg);
                logger.ErrorStack(ex, resultMsg);
            });
            Show(resultMsg);
        }
        public static void Show(this Exception ex, Action func)
        {
            logger.Error(func, ex);
            Show(ex.Message);
        }
        public static void Show(this Exception ex, Action<Exception> func, string msg=null)
        {          
            string result = ex.Message.prefix(msg);
            func(ex);
            Show(result);
        }
        public static void Show(string msg, MessageBoxIcon icon = MessageBoxIcon.Error, MessageBoxButtons buttons = MessageBoxButtons.OK)
        {
            if (isAutonomMode)
            {
                LongRun(() => MessageBox.Show(msg.space(), "", buttons, icon));
            }
            else //false isAutonomMode
            {
                MessageBox.Show(msg.space(), "", buttons, icon);
            }
        }
        public static string Info(this Exception ex)
        {
            return $"{ex.Message}{n}{n}{ex.GetType().FullName}:{n}{ex.ReversedStackTrace()}";
        }
        public static void timeout(Action func)
        {
            //var task = Task.Run(func);
            var task = Task.Factory.StartNew(func);
            if (task.Wait(TimeSpan.FromSeconds(2)))
            { }
            else
            {
                throw new TimeoutException("Timed out");
            }
        }
        public static T CreateCopy<T>(T aobject)
        {
            ICloneable cl = (aobject as ICloneable);
            if (null != cl)
                return (T)cl.Clone();
            MethodInfo memberwiseClone = aobject?.GetType().GetMethod("MemberwiseClone",
                         BindingFlags.Instance | BindingFlags.NonPublic);
            T Copy = (T)memberwiseClone?.Invoke(aobject, null);
            foreach (FieldInfo f in typeof(T).GetFields(
                          BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
            {
                object original = f.GetValue(aobject);
                f.SetValue(Copy, CreateCopy(original));
            }
            return Copy;
        }
        #endregion

        #region private Methods
        private static void Error(this Logger thisLogger, Action func, Exception ex, string msg = null)
        {
            string methodName = $"{func.Method.DeclaringType.FullName}.{func.Method.Name}";
            methodName = methodName.Replace('<', '(').Replace('>', ')');
            LogManager.Configuration.Variables["func"] = methodName;
            LogManager.Configuration.Variables["myStackTrace"] = StackTraceNoSystem(Environment.StackTrace);
            logger.Error(ex, msg ?? ex.Message);
            LogManager.Configuration.Variables["func"] = defaultFunc;
            LogManager.Configuration.Variables["myStackTrace"] = string.Empty;
        }
        private static void ErrorStack(this Logger thisLogger, Exception ex, string msg = null)
        {
            var cleanStackTrace = StackTraceNoSystem(ex.StackTraceInner());
            cleanStackTrace = StackTraceNoEx(cleanStackTrace);
            LogManager.Configuration.Variables["func"] = GetFirstLine(cleanStackTrace);
            LogManager.Configuration.Variables["myStackTrace"] = cleanStackTrace;
            logger.Error(ex, msg ?? ex.Message);
            LogManager.Configuration.Variables["func"] = defaultFunc;
            LogManager.Configuration.Variables["myStackTrace"] = string.Empty;
        }
        private static void LogDebug(this Exception ex, string msg = null)
        {
            var cleanStackTrace = StackTraceNoSystem(ex.StackTraceInner());
            cleanStackTrace = StackTraceNoEx(cleanStackTrace);
            LogManager.Configuration.Variables["func"] = GetFirstLine(cleanStackTrace);
            LogManager.Configuration.Variables["myStackTrace"] = cleanStackTrace;
            logger.Debug(ex, ex.Message.prefix(msg));
            LogManager.Configuration.Variables["func"] = defaultFunc;
            LogManager.Configuration.Variables["myStackTrace"] = string.Empty;
        }
        private static string ReversedStackTrace(this Exception ex)
        {
            return StackTraceNoSystem(ex.StackTraceInner(), true);
        }
        private static string StackTraceInner(this Exception ex)
        {
            return ex.StackTrace ?? ex.InnerException?.StackTraceInner() ?? string.Empty;
        }
        private static string StackTraceNoSystem(string text, bool isReverse = false)
        {
            if (string.IsNullOrEmpty(text))
            { return string.Empty; }
            var parts = text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder result = new StringBuilder();
            var regex = new Regex(@"\bSystem.");
            var partsToCheck = isReverse ? parts.Reverse() : parts;
            var prevLine = "";
            bool firstmatch = false;
            foreach (var tab in partsToCheck)
            {
                string line = tab.Trim().Trim('\r');                
                if (!regex.IsMatch(line))
                {
                    if(firstmatch==false)
                    { result.AppendLine($" {prevLine}"); }
                    result.AppendLine($" {line}");
                    firstmatch = true;
                }
                prevLine = line;
            }
            return result.ToString();
        }
        private static string StackTraceNoEx(string text)
        {
            if (string.IsNullOrEmpty(text))
            { return string.Empty; }
            var parts = text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder result = new StringBuilder();
            var regex = new Regex(@"Ex.");
            var partsToCheck = parts;
            foreach (var tab in partsToCheck)
            {
                string line = tab.Trim().Trim('\r');
                if (!regex.IsMatch(line))
                {
                    result.AppendLine($" {line}");
                }
            }
            return result.ToString();
        }
        private static string GetFirstLine(string text)
        {
            if (string.IsNullOrEmpty(text))
            { return string.Empty; }
            string result = string.Empty;
            //string[] exclude = { "Server stack trace", "\bSystem.", "\bEx." };
            var exclude = new[] 
            { new Regex(@"Server stack trace")
            , new Regex(@"\bSystem.")
            , new Regex(@"\bEx.")
            };
            try
            {
                var parts = text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                var regex = new Regex(@"[^\s(]{3,}");
                for (int i = 0; i < parts.Length; i++)
                {
                    if (regex.IsMatch(parts[i]))
                    {
                        bool isFoundExclusion = false;
                        for (int j = 0; j < exclude.Length; j++)
                        {
                            if (exclude[j].IsMatch(parts[i]))
                            {
                                isFoundExclusion = true;
                                break;
                            }
                        }
                        if (!isFoundExclusion)
                        {
                            result = regex.Match(parts?[i]).Value;
                            result = result.Replace('<', '(');
                            result = result.Replace('>', ')');
                            result = Regex.Replace(result, @"[:?*]", "");
                        }
                    }
                }
            }
            catch (Exception ex)
            { logger.Error(ex, ex.Message); }
            return result;
        }
        #endregion
    }
}
