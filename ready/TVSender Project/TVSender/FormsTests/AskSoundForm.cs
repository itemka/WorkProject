using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using SoundCatcher;

namespace TVSender //менюшка осциллографа
{
    public partial class AskSoundForm : Form
    {              
        private WaveInRecorder _recorder;
        private byte[] _recorderBuffer;
        private WaveOutPlayer _player;
        private byte[] _playerBuffer;
        private WaveFormat _waveFormat;
        private AudioFrame _audioFrame;
        private FifoStream _streamOut;
        private MemoryStream _streamMemory;
        private Stream _streamWave;
        private FileStream _streamFile;
        private bool _isPlayer = false;  // audio output for testing
        private bool _isTest = false;  // signal generation for testing
        private bool _isSaving = false;
        private bool _isShown = true;
        private string _sampleFilename;
        private DateTime _timeLastDetection;
        private string consoletext;

        public AskSoundForm(string _msg, string name)
        {
            InitializeComponent();

            //FormMain f = new FormMain();
            //f.Show();
            //f.MdiParent = this;
            this.Text = name;
            label1.Text = _msg;
            //String[] word = name.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            //pictureBox1.Image = (Image)Properties.Resources.ResourceManager.GetObject(image);         
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {

            if (keyData == Keys.Enter)
            {
                if (!button2.Focused)
                {
                    button1.PerformClick();                    
                }
                return true;
            }

            if (keyData == Keys.Escape)
            {
                button2.PerformClick();
                return true;
            }

            if (keyData == Keys.Back)
            {
                button3.PerformClick();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Retry;
            Close();
        }

        private void AskForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Stop();
            if (_isSaving == true)
            {
                byte[] waveBuffer = new byte[16];
                _streamWave = WaveStream.CreateStream(_streamMemory, _waveFormat);
                waveBuffer = new byte[_streamWave.Length - _streamWave.Position];
                _streamWave.Read(waveBuffer, 0, waveBuffer.Length);
                //if ("" != "")
                //    _streamFile = new FileStream("" + "\\" + _sampleFilename, FileMode.Create);
                //else
                _streamFile = new FileStream(_sampleFilename, FileMode.Create);
                _streamFile.Write(waveBuffer, 0, waveBuffer.Length);
                _isSaving = false;
            }
            if (_streamOut != null)
                try
                {
                    _streamOut.Close();
                }
                catch { }
                finally
                {
                    _streamOut = null;
                }
            if (_streamWave != null)
                try
                {
                    _streamWave.Close();
                }
                catch { }
                finally
                {
                    _streamWave = null;
                }
            if (_streamFile != null)
                try
                {
                    _streamFile.Close();
                }
                catch { }
                finally
                {
                    _streamFile = null;
                }
            if (_streamMemory != null)
                try
                {
                    _streamMemory.Close();
                }
                catch { }
                finally
                {
                    _streamMemory = null;
                }
        }

        private void AskSoundForm_Load(object sender, EventArgs e)
        {
            if (WaveNative.waveInGetNumDevs() == 0)
            {
                consoletext += DateTime.Now.ToString() + " : No audio input devices detected\r\n";
            }
            else
            {
                consoletext += DateTime.Now.ToString() + " : Audio input device detected\r\n";
                if (_isPlayer == true)
                    _streamOut = new FifoStream();
                _audioFrame = new AudioFrame(_isTest);
                //_audioFrame.IsDetectingEvents = TVSender.Properties.Settings.Default.SettingIsDetectingEvents;
                //_audioFrame.AmplitudeThreshold = TVSender.Properties.Settings.Default.SettingAmplitudeThreshold;
                _streamMemory = new MemoryStream();
                Start();
            }
        }

        private void AskSoundForm_Resize(object sender, EventArgs e)
        {
            if (_audioFrame != null)
            {
                _audioFrame.RenderTimeDomainLeft(ref pictureBoxTimeDomainLeft);
                _audioFrame.RenderTimeDomainRight(ref pictureBoxTimeDomainRight);
            }
        }

        private void AskSoundForm_SizeChanged(object sender, EventArgs e)
        {
            if (_isShown & this.WindowState == FormWindowState.Minimized)
            {
                foreach (Form f in this.MdiChildren)
                {
                    f.WindowState = FormWindowState.Normal;
                }
                this.ShowInTaskbar = false;
                this.Visible = false;                
                _isShown = false;
            }
        }

        private void Start()
        {
            Stop();
            try
            {
                _waveFormat = new WaveFormat(44100, 16, 2);
                _recorder = new WaveInRecorder( (-1), _waveFormat, 16384, 3, new BufferDoneEventHandler(DataArrived) );
                if (_isPlayer == true)
                    _player = new WaveOutPlayer( (-1), _waveFormat, 16384 , 3, new BufferFillEventHandler(Filler) );
                consoletext += DateTime.Now.ToString() + " : Audio input device polling started\r\n";
                consoletext += DateTime.Now + " : Device = " + (-1).ToString() + "\r\n";
                consoletext += DateTime.Now + " : Channels = " + 2.ToString() + "\r\n";
                consoletext += DateTime.Now + " : Bits per sample = " + 16.ToString() + "\r\n";
                consoletext += DateTime.Now + " : Samples per second = " + 44100.ToString() + "\r\n";
                consoletext += DateTime.Now + " : Frame size = " + 8192.ToString() + "\r\n";
            }
            catch
            {
                //if (ex.InnerException != null) consoletext += DateTime.Now + " : " + ex.InnerException.ToString() + "\r\n";
            }
        }

        private void Stop()
        {
            if (_recorder != null)
                try
                {
                    _recorder.Dispose();
                }
                catch { }
                finally
                {
                    _recorder = null;
                }
            if (_isPlayer == true)
            {
                if (_player != null)
                    try
                    {
                        _player.Dispose();
                    }
                    catch { }
	                finally
                    {
                        _player = null;
                    }
                _streamOut.Flush(); // clear all pending data
            }
            consoletext += DateTime.Now.ToString() + " : Audio input device polling stopped\r\n";
        }

        private void Filler(IntPtr data, int size)
        {
            if (_isPlayer == true)
            {
                if (_playerBuffer == null || _playerBuffer.Length < size)
                    _playerBuffer = new byte[size];
                if (_streamOut.Length >= size)
                    _streamOut.Read(_playerBuffer, 0, size);
                else
                    for (int i = 0; i < _playerBuffer.Length; i++)
                        _playerBuffer[i] = 0;
                System.Runtime.InteropServices.Marshal.Copy(_playerBuffer, 0, data, size);
            }
        }
        private void DataArrived(IntPtr data, int size)
        {
            if (_isSaving == true)
            {
                byte[] recBuffer = new byte[size];
                System.Runtime.InteropServices.Marshal.Copy(data, recBuffer, 0, size);
                _streamMemory.Write(recBuffer, 0, recBuffer.Length);
            }
            if (_recorderBuffer == null || _recorderBuffer.Length != size)
                _recorderBuffer = new byte[size];
            if (_recorderBuffer != null)
            {
                System.Runtime.InteropServices.Marshal.Copy(data, _recorderBuffer, 0, size);
                if (_isPlayer == true)
                    _streamOut.Write(_recorderBuffer, 0, _recorderBuffer.Length);
                _audioFrame.Process(ref _recorderBuffer);
                if (_audioFrame.IsEventActive == true)
                {
                    if (_isSaving == false && false == true)
                    {
                        _sampleFilename = DateTime.Now.ToString("yyyyMMddHHmmss") + ".wav";
                        _timeLastDetection = DateTime.Now;
                        _isSaving = true;
                    }
                    else
                    {
                        _timeLastDetection = DateTime.Now;
                    }
                    Invoke(new MethodInvoker(AmplitudeEvent) );
                }
                if (_isSaving == true && DateTime.Now.Subtract(_timeLastDetection).Seconds > 3)
                {
                    byte[] waveBuffer = new byte[16];
                    _streamWave = WaveStream.CreateStream(_streamMemory, _waveFormat);
                    waveBuffer = new byte[_streamWave.Length - _streamWave.Position];
                    _streamWave.Read(waveBuffer, 0, waveBuffer.Length);
                    //if ("" != "")
                    //    _streamFile = new FileStream("" + "\\" + _sampleFilename, FileMode.Create);
                    //else
                    _streamFile = new FileStream(_sampleFilename, FileMode.Create);
                    _streamFile.Write(waveBuffer, 0, waveBuffer.Length);
                    if (_streamWave != null) { _streamWave.Close(); }
                    if (_streamFile != null) { _streamFile.Close(); }
                    _streamMemory = new MemoryStream();
                    _isSaving = false;
                    Invoke(new MethodInvoker(FileSavedEvent) );
                }
                _audioFrame.RenderTimeDomainLeft(ref pictureBoxTimeDomainLeft);
                _audioFrame.RenderTimeDomainRight(ref pictureBoxTimeDomainRight);
            }
        }
        private void AmplitudeEvent()
        {
            
        }
        private void FileSavedEvent()
        {
            consoletext += _timeLastDetection.ToString() + " : File " + _sampleFilename + " saved\r\n";
        }
    }
}
