using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text.RegularExpressions;
using NLog;
using DisableDevice;
using System.IO.Ports;
using System.Threading.Tasks;

//актуальное ядро программы
namespace TVSender
{
    public class TestFactory
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        //private static Logger logger = LogManager.GetLogger("port");
        private readonly string n = Environment.NewLine;
        private HexString LastPortIncome;
        private bool UsePrevRead = false; //dont read port
        #region fields
        private enum ifstate
        {
            normal,
            skip,
            go
        }
        /// <summary>
        /// if true: no any messages/messageboxes to user. but throws exceptions.
        /// if false: no throw exceptions. but shows messages/messageboxes to user.
        /// </summary>
        public bool isTestAutoStart { get; set; } = false;
        private IniFileList.IniSectionList section;
        private ListofPairs<string, string> RunList = new ListofPairs<string, string>();
        private static Dictionary<string, string> StaticCopyListStringToTest;
        private Dictionary<string, Func<string, bool>> StringToTestList;
        private Dictionary<string, Action<string>> FillingMethodsList;
        List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
        private string display;
        private string msg;
        private string image;
        private string label;
        private string failMsg;
        #endregion
        #region ctor
        public TestFactory(IniFileList.IniSectionList _section, bool _silent = false)
        {
            section = _section;
            isTestAutoStart = _silent;

            DefaultSets();

            foreach (IniFileList.IniSectionList.IniKey key in section.Keys)
            {
                if (key.Value == null) key.Value = "";
                RunList.Add(key.Name, key.Value);                
                if (FillingMethodsList.ContainsKey(key.Name) )
                    FillingMethodsList[key.Name](key.Value);
            }
        }
        #endregion
        #region Properties
        public static System.Collections.ICollection KeysToTests { get { return StaticCopyListStringToTest; } }
        public string FilePath { get { return section.IniFile.Path; } }
        public string Id { get { return section.Name; } }
        #endregion                

        public bool Run()
        {
            LastPortIncome = null;
            UsePrevRead = false;
            ifstate state = ifstate.normal;
            bool FirstSelected = false;
            failMsg = null;
            for (int i = 0; i < RunList.Count; i++)
                if (StringToTestList.ContainsKey(RunList[i].Key))
                {
                    switch (state)
                    {
                        case ifstate.normal:
                            #region no
                            if (RunList[i].Key == "{")
                            {
                                state = ifstate.skip;
                                continue;
                            }
                            bool pass = StringToTestList[RunList[i].Key](RunList[i].Value);
                            if (!pass)
                            {
                                if (nextIf(i))
                                {
                                    state = ifstate.go;
                                }
                                else
                                {
                                    return FailRun();
                                }
                            }
                            break;
                        #endregion
                        case ifstate.skip:
                            #region skip
                            if (RunList[i].Key == "}")
                            {
                                state = ifstate.normal;
                            }
                            //else
                            //{
                            //    continue;
                            //}
                            break;
                        #endregion
                        case ifstate.go:
                            #region go
                            if (RunList[i].Key == "}")
                            {
                                return SuccessRun();
                            }
                            bool pass2 = StringToTestList[RunList[i].Key](RunList[i].Value);
                            if (!pass2)
                                return FailRun();
                            break;
                            #endregion
                    }
                }
                else
                {
                    PublicData.write_info($"{display}: \"{RunList[i].Key}\" - программа не знает такого пункта. (F4 редактировать - \"{section.Name}\")");
                    if (!FirstSelected)
                        PublicData.SetFokusListView(this);
                    FirstSelected = true;
                    failMsg = $"Неизвестная команда \"{RunList[i].Key}\" в тесте [{section.Name}]";
                    return FailRun();
                }
            return SuccessRun();
        }
        #region Tests
        bool Port1_Hex(string str)
        {
            string funcName = $"<{nameof(Port1_Hex)}>";
            try
            {
                byte[] send = Code.HexStrToByteHex(str);
                Code.write(send);                
                UsePrevRead = false;
                Thread.Sleep(50);
                return true;
            }
            catch (TimeoutException ex)
            {
                return FailCmd(ex, funcName);
            }
            catch (Exception ex)
            {
                return FailCmd(ex, funcName);
            }
        }
        bool Port1_Txt(string str)
        {
            string funcName = $"<{nameof(Port1_Txt)}>";
            try
            {
                byte[] send = Code.StrTxtToByteHex(str);
                Code.write(send);
                UsePrevRead = false;
                Thread.Sleep(50);
                return true;
            } 
            catch (TimeoutException ex)
            {
                return FailCmd(ex, funcName);
            } catch (Exception ex)
            {
                return FailCmd(ex, funcName);
            }
        }
        bool GenrSend(string income)
        {
            string funcName = $"<{nameof(GenrSend)}>";
            string[] parts = income.Split('_');
            string str = "";
            if (parts.Length >= 1)
            {
                str = $"load tim {parts[0]};load pat ;run;";
            }
            if (parts.Length >= 2)
            {
                str = $"load tim {parts[0]};load pat {parts[1]};run;";
            }

            byte[] send = Encoding.Default.GetBytes(str);
            Code.write_genr(send);
            return OkCmd(funcName);
        }
        bool Port2_Hex(string income)
        {
            string funcName = $"<{nameof(Port2_Hex)}>";
            try
            {
                byte[] send = Code.HexStrToByteHex(income);
                Code.write_genr(send);
                return OkCmd(funcName);
            }
            catch (Exception ex)
            { return FailCmd(ex, funcName); }
        }
        bool Port2_Txt(string income)
        {
            string funcName = $"<{nameof(Port2_Txt)}>";
            try
            {
                byte[] send = Encoding.Default.GetBytes(income);
                Code.write_genr(send);
                return OkCmd(funcName);
            }
            catch(Exception ex)
            { return FailCmd(ex, funcName); }
        }
        bool IRAudioPlay(string str)
        {
            string funcName = $"<{nameof(IRAudioPlay)}>";
            return Tests.sound(str);
        }
        bool Pause(string str)
        {
            int number;
            if (Int32.TryParse(str, out number) )
            { Thread.Sleep(number); }
            return true;
        }
        bool FormUsual(string str)
        {
            return Tests.loadmenu(Run, msg, label, image);
        }
        bool Port1EntryText(string str)
        {
            string funcName = $"<{nameof(Port1EntryText)}>";
            bool formPass = true;
            string msgForm = this.msg;
            while (formPass)
            {
                string input;
                formPass = Tests.loadEntryMenu(null, msgForm, label, image, out input);
                if (!formPass) { return false; }
                if (CheckInput(input, str, ref msgForm) )
                {
                    return Port1_Txt(input);                    
                }                
            }
            return FailCmd(funcName);
        }        
        bool Port1EntryHex(string str)
        {
            string funcName = $"<{nameof(Port1EntryHex)}>";
            bool formPass = true;
            string msgForm = this.msg;
            while (formPass)
            {
                string input;
                formPass = Tests.loadEntryMenu(null, msgForm, label, image, out input);
                if (!formPass) { return false; }
                if (CheckInput(input, str, ref msgForm) )
                {
                    return Port1_Hex(input);
                }
            }
            return FailCmd(funcName);
        }
        bool Port2EntryText(string str)
        {
            string funcName = $"<{nameof(Port2EntryText)}>";
            string input;
            bool _return = Tests.loadEntryMenu(Run, msg, label, image, out input);
            if (_return)
            {
                Port2_Txt(input);
            }
            return _return ? OkCmd(funcName) : FailCmd(funcName);
        } 
        bool Port2EntryHex(string str)
        {
            string funcName = $"<{nameof(Port2EntryHex)}>";
            string input;
            bool _return = Tests.loadEntryMenu(Run, msg, label, image, out input);
            if (_return)
            {
                Port2_Hex(input);
            }
            return _return ? OkCmd(funcName) : FailCmd(funcName);
        }        
        bool FormOscillograph(string str)
        {
            string funcName = $"<{nameof(FormOscillograph)}>";
            return Tests.Oscillograph(Run, msg, label);
        }
        bool ReadFindSuccess(string str)
        {
            string funcName = $"<{nameof(ReadFindSuccess)}>";
            string sourceSuccess = str.Trim().Trim('"');

            string sourceSuccessHex = sourceSuccess.Replace(" ", "").ToLower();
            string sourceSuccessTxt = sourceSuccess.ToLower();

            var read = (UsePrevRead) ? LastPortIncome : ReadPortIncomes();
            if (read.Hex == "") return FailCmd($"{funcName} Не пришло ответа по {Code.port.PortName}-порту.");

            if (read.Hex.Contains(sourceSuccessHex) || read.Txt.Contains(sourceSuccessTxt) )
            {
                return OkCmd(funcName);
            }
            StringBuilder errmsg = new StringBuilder();
            errmsg.AppendLine($"Запрашиваемый ответ не найден в ответе от устройства.{n}");            
            errmsg.AppendLine($"Найти: {"",-8}= {str}");
            errmsg.AppendLine($"Пришло: {"",-8}= {read.Txt.trySubstring(0, 50).space()}");
            errmsg.AppendLine($"(Код): {"",-8}= {read.Hex.trySubstring(0, 50).space()}");
            errmsg.Append($"{n}Команда = <ReadFind = {str}>");
            failMsg = errmsg.ToString();
            //return FailCmd(errmsg.ToString() );
            return FailCmd($"[{funcName} = {str}]{n}Ответ не найден.");
        }
        bool ReadFail(string str)
        {
            string funcName = $"<{nameof(ReadFail)}>";
            string sourceSuccess = str.Trim().Trim('"');

            string sourceSuccessHex = sourceSuccess.Replace(" ", "").ToLower();
            string sourceSuccessTxt = sourceSuccess.ToLower();

            var read = (UsePrevRead) ? LastPortIncome : ReadPortIncomes();
            if (read.Hex == "") return FailCmd($"{funcName} Не пришло ответа по {Code.port.PortName}-порту.");

            if (read.Hex.Contains(sourceSuccessHex) || read.Txt.Contains(sourceSuccessTxt) )
            {
                StringBuilder errmsg = new StringBuilder();
                errmsg.AppendLine("Обнаружен код ошибки в ответе от устройства.");                
                errmsg.AppendLine($"Найден = {str}{n}");
                errmsg.Append($"Команда = <ReadFail = {str}>");
                failMsg = errmsg.ToString();
                return FailCmd($"{funcName} = {str}; Запрашиваемый ответ найден.");
            }
            return OkCmd(funcName);
        }
        bool SaveReadBetween(string income)
        {
            string funcName = $"<{nameof(SaveReadBetween)}>";
            string[] findHex = income.Split(new char[] { ',' }, 2, StringSplitOptions.RemoveEmptyEntries);
            string[] findText = (string[])findHex.Clone();

            if (findHex.Length == 2)
            {
                findText[0] = findText[0].Trim().Trim('"').ToLower(); //.Replace("\"", "")
                findText[1] = findText[1].Trim().Trim('"').ToLower();
                findHex[0] = findHex[0].ToLower().Replace("\"", "").Replace(" ", "");
                findHex[1] = findHex[1].ToLower().Replace("\"", "").Replace(" ", "");
            }
            var getRead = (UsePrevRead) ? LastPortIncome : ReadPortIncomes();
            if (getRead.Hex == "") return FailCmd($"{funcName} Не пришло ответа по {Code.port.PortName}-порту.");

            string find = funcReadBetween(getRead.Hex, findHex[0], findHex[1]);
            if (find != null)
            {
                PublicData.IDDataTV = new HexString().FromLower(Code.HexStrToStrTxt(find), find);
                return OkCmd($"{funcName} = \"{PublicData.IDDataTV}\"");
            }
            find = funcReadBetween(getRead.Txt, findText[0], findText[1]);
            if (find != null)
            {
                PublicData.IDDataTV = new HexString().FromLower(find, "(Converted) "+Code.StrTxtToStrHex(find) );
                return OkCmd($"{funcName} = \"{PublicData.IDDataTV}\"");
            }
            return FailCmd($"{funcName} = {income}; Запрашиваемый ответ не найден.");
        }
        bool SaveReadAfter(string income)
        {
            string funcName = $"<{nameof(SaveReadAfter)}>";
            string[] parts = income.Split(new char[] { ','}, 2, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < parts.Length;i++ )
            {
                parts[i] = parts[i].ToLower().Trim().Trim('"');
            }
            string findHex = parts[0].Replace(" ", ""); ;
            string findText = parts[0];
            int length = 0;
            if (parts.Length == 2)
            {
                try
                {
                    length = Int32.Parse(parts[1].Replace(" ", "") );
                }
                catch
                {
                    return FailCmd($"{funcName} = {income}; \"{parts[1]}\" - не является числом.");
                }
            }

            var read = (UsePrevRead) ? LastPortIncome : ReadPortIncomes();
            if (read.Hex == "") return FailCmd($"{funcName} Не пришло ответа по {Code.port.PortName}-порту.");

            string find = funcReadAfter(read.Hex, findHex, length * 2);
            if (find != null)
            {
                PublicData.IDDataTV = new HexString().FromLower(Code.HexStrToStrTxt(find), find);
                return OkCmd($"{funcName} = \"{PublicData.IDDataTV}\"");
            }
            find = funcReadAfter(read.Txt, findText, length);
            if (find != null)
            {
                PublicData.IDDataTV = new HexString().FromLower(find, "(Converted) " + Code.StrTxtToStrHex(find) );
                return OkCmd($"{funcName} = \"{PublicData.IDDataTV}\"");
            }
            return FailCmd($"{funcName} = {income}; Запрашиваемый ответ не найден.");
        }
        bool CheckTestsPass(string str)
        {
            string funcName = $"<{nameof(CheckTestsPass)}>";
            if (PublicData.IDDataTV == null)
            {
                return FailCmd("Нет записанных данных в память программы");
            }
            if(!Directory.Exists(PublicData.LogsCheckPassPath))
            {
                return FailCmd("Не доступна папка логов тестов.");
            }
            //PublicData.IDDataTV = "13test";
            string file = "ps" + PublicData.IDDataTV + ".txt";
            string path = Path.Combine(PublicData.LogsCheckPassPath, file);

            try
            {                
                string name = funcReadBetween(str, "[", "]");
                var keysWhatToFind = section.IniFile.GetSection(name).Keys;

                if (!File.Exists(path) )
                {
                    string msg = $"Файл не найден \"{path}\"";
                    CheckTestsPass_CopyWithError(file, path, msg);
                    return FailCmd(funcName+msg);
                }
                var iniWhereToFind = new IniFile(path);
                foreach (IniFileList.IniSectionList.IniKey keyToFind in keysWhatToFind)
                {
                    string pass = iniWhereToFind.GetKeyValue(PublicData.IDDataTV.Txt, keyToFind.Name); //.Trim().ToLower();
                    pass = pass?.ToLower();//.Trim();
                    if (pass != "ok")
                    {
                        Ex.Try( (Action)(()=>
                        {
                            logger.Trace($"1) pass != 'ok' = {pass?.ToString() ?? "null"}");
                            logger.Trace($"start search at inBuiltTests");
                            var funcToFind = Form1.InBuiltTests[keyToFind.Name];
                            var keysWhereToFind = iniWhereToFind.GetSection(PublicData.IDDataTV.Txt).Keys;

                            foreach (IniFile.IniSection.IniKey logkey in keysWhereToFind)
                            {
                                bool found = false;
                                Ex.Try( ()=>
                                {
                                    if (funcToFind == Form1.InBuiltTests[logkey.Name])
                                    {
                                        pass = logkey.Value.ToLower();
                                        found = true;
                                    }
                                });
                                if (found) break;
                            }
                        }));
                        if (pass != "ok")
                        {
                            logger.Trace($"2) pass != 'ok' = {pass?.ToString() ?? "null"}");
                            logger.Trace($"after inBuiltTests search");
                            string msg1 = "";
                            string display = "";
                            if (Form1.InBuiltTests.ContainsKey(keyToFind.Name) )
                            { display = Form1.InBuiltTests[keyToFind.Name].DisplayName; }
                            if (Form1.CreatedDisplays.ContainsKey(keyToFind.Name) )
                            { display = Form1.CreatedDisplays[keyToFind.Name]; }
                            msg1 = (pass == null)
                                ? $"\"{display}\" not found!{Environment.NewLine}<{keyToFind.Name}>"
                                : $"\"{display}\" = Fail.{Environment.NewLine}<{keyToFind.Name}>";
                            logger.Trace(msg1);
                            CheckTestsPass_CopyWithError(file, path, msg1);                            
                            return FailCmd(funcName + msg1);
                        }
                    }
                }
                return OkCmd(funcName);
            }
            catch(Exception ex)
            {              
                return FailCmd(ex, funcName);
            }
            finally
            {
                Ex.Try( ()=>File.Delete(path) );
                PublicData.IDDataTV = null;
            }
        }
        bool RegexTxt(string str)
        {
            string funcName = $"<{nameof(RegexTxt)}>";
            try
            {
                if (string.IsNullOrEmpty(str) )
                { new ArgumentNullException($"Пустой параметр: {nameof(str)}={str}").Throw(); }

                var reg = new Regex(str);

                var read = (UsePrevRead) ? LastPortIncome : ReadPortIncomes();
                if (read.Hex == "") return FailCmd($"Не пришло ответа по {Code.port.PortName}-порту.");

                if (reg.IsMatch(read.Txt) )
                {
                    return OkCmd(funcName);
                }
                throw new KeyNotFoundException($"Совпадений не найдено в ответе={read.Txt}; {nameof(str)}={str}");
            }
            catch(Exception ex)
            {
                return FailCmd(ex, funcName);
            }
        }
        bool RegexHex(string str)
        {
            string funcName = $"<{nameof(RegexHex)}>";
            try
            {
                if (string.IsNullOrEmpty(str) )
                { throw new ArgumentNullException($"Пустой параметр: {nameof(str)}={str}"); }

                var reg = new Regex(str);

                var read = (UsePrevRead) ? LastPortIncome : ReadPortIncomes();
                if (read.Hex == "") return FailCmd($"Не пришло ответа по {Code.port.PortName}-порту.\r\n[RegexHex]");

                if (reg.IsMatch(read.Hex) )
                {
                    return OkCmd(funcName);
                }
                throw new KeyNotFoundException($"Совпадений не найдено в ответе={read.Hex}; {nameof(str)}={str}");
            }
            catch(Exception ex)
            {
                return FailCmd(ex, funcName);
            }
        }
        bool RedRatNECSend(string str)
        {
            string funcName = $"<{nameof(RedRatNECSend)}>";
            logger.Trace("start");
            var custSignal = new CustomSignal().Nec(str);            
            SignalOutput SSO = new SignalOutput(custSignal);
            SSO.Output();
            logger.Trace("end");
            return OkCmd(funcName);
        }
        bool RedRatIRHexSend(string str)
        {
            string funcName = $"<{nameof(RedRatIRHexSend)}>";
            logger.Trace("start");
            var hex = Code.HexStrToByteHex(str);
            var custSignal = RedratHelper.GetSignalFromSigData(hex);            
            SignalOutput SSO = new SignalOutput(custSignal);
            SSO.Output();
            logger.Trace("end");
            return OkCmd(funcName);
        }
        bool RedRatMainSend(string str)
        {
            string funcName = $"<{nameof(RedRatMainSend)}>";
            logger.Trace("start");
            var hex = Code.HexStrToByteHex(str);
            var custSignal = RedratHelper.GetSignalFromMain(hex);
            SignalOutput SSO = new SignalOutput(custSignal);
            SSO.Output();
            logger.Trace("end");
            return OkCmd(funcName);
        }
        bool RedRatFullIRSend(string str)
        {
            string funcName = $"<{nameof(RedRatFullIRSend)}>";
            logger.Trace("start");
            try
            {
                var custSignal = RedratHelper.GetSignalFromLengthStr(str);
                SignalOutput SSO = new SignalOutput(custSignal);
                SSO.Output();
            }
            catch(Exception ex)
            {
                return FailCmd(ex, funcName);
            }
            logger.Trace("end");
            return OkCmd(funcName);
        }
        bool FailReturn(string str) => false;
        #endregion

        #region returns
        private bool FailCmd(string incmsg = null)
        {
            if (string.IsNullOrEmpty(failMsg) ) failMsg = incmsg;
            string chat = (incmsg == null) ? "" : (incmsg + Environment.NewLine);
            if (!isTestAutoStart) { PublicData.write_info(chat); }
            return false;
        }
        private bool FailCmd(Exception ex, string msg=null)
        {
            logger.Trace($"{nameof(isTestAutoStart)}={isTestAutoStart}");
            ex.Log();
            return FailCmd(ex.Message.prefix(msg, 0));
        }
        private bool OkCmd(string msg = null)
        {
            string result = Extensions.prefix(msg, "OK", 0);
            if (msg != null) { PublicData.write_info(result); }
            return true;
        }
        private bool SuccessRun()
        {
            PublicData.write_info($"OK PASS [{display}]");
            logger.Trace($"Reader Started! (isStopReader=false)");
            Thread.Sleep(200);
            return true;
        }
        private bool FailRun()
        {
            string outputMsg = $"[{display}] - Fail".prefix(failMsg);
            logger.Trace($"{nameof(isTestAutoStart)}={isTestAutoStart}");
            if (!isTestAutoStart)
            {
                PublicData.write_info(outputMsg);
                Ex.Show(outputMsg);
            }
            Thread.Sleep(200);
            return false;
        }
        #endregion

        #region SecondaryFuncs
        private bool nextIf(int i)
        {
            bool _return = false;
            if (i < RunList.Count - 1)
            {
                if (RunList[i + 1].Key == "{")
                {
                    _return = true;
                }
            }
            return _return;
        }
        private static void CheckTestsPass_CopyWithError(string file, string oldPath, string msg)
        {
            LogUser log = new LogUser();
            try
            {
                string newPath = PublicData.PathDeacreaseByFolder(PublicData.LogsCheckPassPath);
                newPath = Path.Combine(newPath, "#Fails_checkpass");
                newPath = Path.Combine(newPath, PublicData.placeNumber);
                if (!Directory.Exists(newPath) ) { Directory.CreateDirectory(newPath); }
                newPath = Path.Combine(newPath, file);
                string contentFile = string.Empty;
                Ex.Try( () =>
                {
                    File.Copy(oldPath, newPath, true);
                    contentFile = File.ReadAllText(newPath);
                });
                File.WriteAllText(newPath, string.Format("{1}{0}{0}{2}", Environment.NewLine, msg, contentFile) );
            }
            finally
            {
                log.Out();
            }
        }
        private static void CreateLog(string incPath, string msg)
        {
            LogUser log = new LogUser();
            string path = PublicData.PathDeacreaseByFolder(PublicData.LogsCheckPassPath);
            path = CreatePath(path, incPath, PublicData.placeNumber);
            string fullPath = Path.Combine(path, DateTime.Now.ToString("yyyy.MM.dd; HH.mm.ss") );
            string contentFile = string.Empty;
            File.WriteAllText(fullPath, string.Format("{1}{0}{0}{2}", Environment.NewLine, msg, contentFile) );
            log.Out();
        }
        private static string CreatePath(string oldPath, string newPath)
        {
            string path = oldPath;
            string[] folders = newPath.Split(new char[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string folder in folders)
            {
                path = Path.Combine(path, folder);
                if (!Directory.Exists(path) )
                {
                    Directory.CreateDirectory(path);
                }
            }
            return path;
        }
        private static string CreatePath(string oldPath, params string[] incFolders)
        {
            string path = oldPath;
            foreach (string folder in incFolders)
            {
                path = Path.Combine(path, folder);
                if (!Directory.Exists(path) )
                {
                    Directory.CreateDirectory(path);
                }
            }
            return path;
        }
        private string funcReadAfter(string input, string find, int length)
        {
            string _return = null;
            int index = input.IndexOf(find);
            if (index == -1)
            {
                //PublicData.write_pc($"[{display}]ReadAfter: Не найдено \"{find}\" в ответе ТВ";
                return _return;
            }
            index += find.Length;
            try
            {
                _return = input.Substring(index, length);
            }
            catch
            {
                PublicData.write_info($"[{display}]ReadAfter: Число запрашиваемых символов ({length}) " +
                "превысило максимальное количество символов в ответе ТВ");
            }
            return _return;
        }
        private string funcReadBetween(string input, string find1, string find2)
        {
            string _return = null;
            string[] separators = { find1, find2 };
            string[] matches = input.Split(separators, StringSplitOptions.None);
            if (matches.Length == 3)
            {
                _return = matches[1];
            }
            if (matches.Length > 3)
            {
                _return = matches[1];
                PublicData.write_info("! " + display + " [ReadBetween]: заданные параметры теста не уникальны."
                    + "Найдено больше значений, чем нужно. Возможно получение неверных данных.");
            }
            return _return;
        }
        private bool CheckInput(string input, string arg, ref string errMsg)
        {
            if (string.IsNullOrEmpty(arg) ) { return true; }
            try
            {
                string name = funcReadBetween(arg, "[", "]");
                var Keys = section.IniFile.GetSection(name).Keys;
                foreach (IniFileList.IniSectionList.IniKey key in Keys)
                {
                    if (key.Name.ToLower() == "regex")
                    {
                        var reg = new Regex(key.Value);
                        if (!reg.IsMatch(input) )
                        {
                            errMsg = $"Неверный ввод: {input}\n{this.msg}";
                            return false;
                        };
                    }
                    else { throw new ArgumentException(); }
                }
            }
            catch
            {
                Ex.Show("Неправильно составлен тест. Неверные параметры.");
                return false;
            }
            return true;
        }
        #endregion

        private HexString ReadPortIncomes()
        {            
            Thread.Sleep(200);
            int gotbytes = 0;
            Ex.Try( () =>
            { gotbytes = Code.port.BytesToRead; });
            logger.Trace($"gotbytes={gotbytes}");
            var gotPortIncome = new HexString();
            try
            {
                for (int i = 0; i < gotbytes; i++)
                {
                    int bit = Code.port.ReadByte();
                    gotPortIncome.AddByte(bit);
                }
            }
            catch (TimeoutException ex)
            { logger.Error(ex, ex.Message); }
            catch(Exception ex)
            { logger.Error(ex, ex.Message); }

            if (!isTestAutoStart)
            {
                PublicData.write_tv("text =  " + gotPortIncome.TxtNiceDisplay);
                PublicData.write_tv("code =  " + gotPortIncome.HexNiceDisplay);
            }
            logger.Trace($"COM Read Txt: {gotPortIncome.TxtNiceDisplay}");
            logger.Trace($"COM Read Bin: {gotPortIncome.HexNiceDisplay}");
            UsePrevRead = true;               
            LastPortIncome = gotPortIncome;
            return gotPortIncome;
        }
        private void ReadPortIncomesAsync(string find)
        {
            SerialDataReceivedEventHandler eventName;
            var incomePortMethod = new HexString();
            eventName = (o, e) =>
            {
                int gotbytes = 0;
                Ex.Try(() =>
                { gotbytes = Code.port.BytesToRead; });
                logger.Trace($"gotbytes={gotbytes}");
                var incomePortEvent = new HexString();
                try
                {
                    for (int i = 0; i < gotbytes; i++)
                    {
                        int bit = Code.port.ReadByte();
                        incomePortEvent.AddByte(bit);
                    }
                }
                catch (Exception ex)
                { ex.Log(); }
                if (!isTestAutoStart)
                {
                    PublicData.write_tv("text =  " + incomePortEvent.TxtNiceDisplay);
                    PublicData.write_tv("code =  " + incomePortEvent.HexNiceDisplay);
                }
                logger.Trace($"COM Read Txt: {incomePortEvent.TxtNiceDisplay}");
                logger.Trace($"COM Read Bin: {incomePortEvent.HexNiceDisplay}");
                UsePrevRead = true;
                incomePortMethod = incomePortEvent;
            };
            try
            {
                Code.port.DataReceived += eventName;
            }
            finally
            {
                Code.port.DataReceived -= eventName;
            }

        }

        #region List of Commands
        private void SetStringToTestList()
        {
            StringToTestList = new Dictionary<string, Func<string, bool>>(StringComparer.InvariantCultureIgnoreCase)
            {
                ["NEC"] = RedRatNECSend,
                ["RedratNEC"] = RedRatNECSend,
                ["NECRedrat"] = RedRatNECSend,
                ["Redrat"] = RedRatFullIRSend,
                ["IR"] = RedRatFullIRSend,
                ["RedratIR"] = RedRatFullIRSend,

                ["code"] = Port1_Hex,
                ["port1.code"] = Port1_Hex,
                ["text"] = Port1_Txt,
                ["txt"] = Port1_Txt,
                ["port1.text"] = Port1_Txt,
                ["port1.txt"] = Port1_Txt,
                ["genr"] = GenrSend,
                ["port2.code"] = Port2_Hex,
                ["port2.txt"] = Port2_Txt,
                ["port2.text"] = Port2_Txt,
                ["pause"] = Pause,
                ["stop"] = Pause,
                ["checkpass"] = CheckTestsPass,
                ["Read"] = ReadFindSuccess,
                ["Find"] = ReadFindSuccess,
                ["ReadFind"] = ReadFindSuccess,
                ["ReadFail"] = ReadFail,
                ["FindFail"] = ReadFail,
                ["ReadNG"] = ReadFail,
                ["ReadBetween"] = SaveReadBetween,
                ["SaveBetween"] = SaveReadBetween,
                ["SaveReadBetween"] = SaveReadBetween,
                ["ReadAfter"] = SaveReadAfter,
                ["SaveAfter"] = SaveReadAfter,
                ["SaveReadAfter"] = SaveReadAfter,
                ["Regex"] = RegexTxt,
                ["RegexTxt"] = RegexTxt,
                ["ReadEx"] = RegexTxt,
                ["RegexHex"] = RegexHex,
                ["RegexCode"] = RegexHex,
                ["False"] = FailReturn,                

                ["form"] = FormUsual,
                ["form1"] = FormUsual,
                ["Enter"] = Port1EntryText,
                ["EnterText"] = Port1EntryText,
                ["Input"] = Port1EntryText,
                ["InputText"] = Port1EntryText,
                ["EntryText"] = Port1EntryText,

                ["EnterCode"] = Port1EntryHex,
                ["EnterHex"] = Port1EntryHex,
                ["InputCode"] = Port1EntryHex,
                ["InputHex"] = Port1EntryHex,
                ["form.sound"] = FormOscillograph,
                ["form2"] = FormOscillograph,
                ["irsound"] = IRAudioPlay,
                ["soundir"] = IRAudioPlay,
                #region nothing
                ["{"] = VisualParameters,
                ["}"] = VisualParameters,
                ["display"] = VisualParameters,
                ["message"] = VisualParameters,
                ["msg"] = VisualParameters,
                ["image"] = VisualParameters,
                ["label"] = VisualParameters,
            };
            #endregion            
        }
        #endregion
        #region default cmds
        private void DefaultSets()
        {
            display = section.Name;
            msg = section.Name;
            image = section.Name;
            label = section.Name;
            FillFillingMethodsList();
            SetStringToTestList();
            FillStaticCopyListStringToTest();
        }
        private void FillStaticCopyListStringToTest()
        {
            StaticCopyListStringToTest = new Dictionary<string, string>();
            foreach (var tab in StringToTestList)
            {
                StaticCopyListStringToTest.Add(tab.Key, tab.Value.Method.Name);
            }
        }        
        private void FillFillingMethodsList()
        {
            FillingMethodsList = new Dictionary<string, Action<string>>(StringComparer.InvariantCultureIgnoreCase)
            {
                ["display"] = SetDisplay,
                ["name"] = SetDisplay,
                ["image"] = SetImage,
                ["message"] = SetMsg,
                ["msg"] = SetMsg,
                ["label"] = SetLabel
            };
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
        #endregion
    }
}
