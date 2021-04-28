using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using MediaFiles.Sound;
using Microsoft.Win32;
using PluginSystem.API;
using PluginSystem.Common;
using System.IO;

namespace MediaFiles
{
    public partial class DlgRecording : BaseForm
    {
        private const string REGISTRY_LAST_DEVICE_KEY = "LastSelectedRecordingDevice";
        private const string REGISTRY_NORMALIZE_FILTER_KEY = "Normalize";
        private const string REGISTRY_CONVERT_TO_MP3_KEY = "ConvertToMp3";

        private readonly Recorder m_Recorder = new Recorder(Path.GetTempFileName());

        public string OutputFileName
        {
            get; private set;
        }

        public string Extension { get; private set; }

        public DlgRecording()
        {
            InitializeComponent();
        }

        private int? GetSelectedDeviceNumber()
        {
            var selectedDeviceName = (string) cbxDevice.SelectedItem;
            
            if (!string.IsNullOrEmpty(selectedDeviceName))
            {
                IList<string> devices = m_Recorder.GetDevices();
                return devices.IndexOf(selectedDeviceName);
            }
            
            return null;
        }

        private string NormalizeOutputFile()
        {
            var srcFile = new WavFile();
            srcFile.Open(m_Recorder.Filename, WavFile.WavFileMode.Read);
            srcFile.SeekToAudioDataStart();
            srcFile.GetNextSample16Bit();

            var destFile = new WavFile();
            string filename = Path.GetTempFileName();
            destFile.Create(filename, srcFile.IsStereo, srcFile.SampleRateHz, srcFile.BitsPerSample, true);

            const int SAMPLE_TRESHOLD = 15;
            const int MAX_SILENCE_LENGTH = 1000;
            int samplesBelowTreshold = 0;
            
            while (srcFile.NumSamplesRemaining > 0)
            {
                long sample = srcFile.GetNextSample16Bit();

                if (Math.Abs(sample) <= SAMPLE_TRESHOLD)
                {
                    samplesBelowTreshold++;
                }
                else
                {
                    samplesBelowTreshold = 0;
                }

                if (samplesBelowTreshold < MAX_SILENCE_LENGTH)
                {
                    destFile.AddSample16Bit((short)sample);
                }
            }

            string sourceFileName = srcFile.Filename;
            srcFile.Close();
            destFile.Close();

            File.Delete(sourceFileName);
            
            return filename;
        }

        private string ConvertToMp3()
        {
            string executingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().FullName);
            string lamePath = Path.Combine(executingDirectory, @"Plugins\lame.exe");
            if (File.Exists(lamePath))
            {
                string mp3OutputFileName = Path.GetTempFileName();

                var processStartInfo = new ProcessStartInfo(lamePath)
                                           {
                                               CreateNoWindow = true,
                                               Arguments = string.Format("-h {0} {1}", OutputFileName, mp3OutputFileName),
                                               WindowStyle = ProcessWindowStyle.Hidden
                                           };

                Process process = Process.Start(processStartInfo);
                if (process == null)
                {
                    MessageBox.Show("Uruchomienie programu lame.exe nie powiodło się");
                    return OutputFileName;
                }

                process.WaitForExit();
                File.Delete(OutputFileName);
                Extension = ".mp3";
                return mp3OutputFileName;
            }

            MessageBox.Show(
                "Aby zapisywać pliki w formacie Mp3 konieczne jest wgranie do katalogu Plugins programu Lame.exe\r\n" +
                "Program Lame.exe jest darmowy i można go pobrać z adresu: http://www.mp3dev.org");
            return OutputFileName;
        }

        private static RegistryKey GetConfigRegistryKey()
        {
            return Registry.CurrentUser.CreateSubKey("Software").CreateSubKey("SuperMemoPlugins");
        }

        private void RestoreLastSettings()
        {
            var lastSelectedDevice = (int)GetConfigRegistryKey().GetValue(REGISTRY_LAST_DEVICE_KEY, 0);
            if (lastSelectedDevice >= 0 && lastSelectedDevice < cbxDevice.Items.Count)
            {
                cbxDevice.SelectedIndex = lastSelectedDevice;
            }

            bool useFilter = (int)GetConfigRegistryKey().GetValue(REGISTRY_NORMALIZE_FILTER_KEY, 1) != 0;
            chbNomalize.Checked = useFilter;

            bool convertToMp3 = (int)GetConfigRegistryKey().GetValue(REGISTRY_CONVERT_TO_MP3_KEY, 1) != 0;
            chbConvertToMp3.Checked = convertToMp3;
        }

        private void SaveLastSettings()
        {
            GetConfigRegistryKey().SetValue(REGISTRY_LAST_DEVICE_KEY, cbxDevice.SelectedIndex);
            GetConfigRegistryKey().SetValue(REGISTRY_NORMALIZE_FILTER_KEY, chbNomalize.Checked ? 1 : 0);
            GetConfigRegistryKey().SetValue(REGISTRY_CONVERT_TO_MP3_KEY, chbNomalize.Checked ? 1 : 0);
        }

        private void DlgRecording_Load(object sender, EventArgs e)
        {
            IList<string> devices = m_Recorder.GetDevices();

            foreach (var device in devices)
            {
                cbxDevice.Items.Add(device);                
            }

            RestoreLastSettings();
        }

        private void CbxDeviceSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int? deviceNumber = GetSelectedDeviceNumber();

                if (deviceNumber.HasValue)
                {
                    if (m_Recorder.IsRecording)
                    {
                        m_Recorder.StopRecording();
                    }

                    m_Recorder.StartRecording(deviceNumber.Value);
                }

                btnOk.Enabled = deviceNumber.HasValue;
            }
            catch(Exception ex)
            {
                btnOk.Enabled = false;
                MessageBox.Show("Nie można nagrywać na wybranym urządzeniu: " + ex);
            }
        }

        private void Timer1Tick(object sender, EventArgs e)
        {
            if (m_Recorder.IsRecording)
            {
                TimeSpan length = m_Recorder.SampleLength;
                label1.Text = string.Format("Nagrywanie rozpoczęte, długość {0}:{1}", length.Seconds, length.Milliseconds);
            }
            else
            {
                label1.Text = "Nie nagrywane";
            }
        }

        private void BtnOkClick(object sender, EventArgs e)
        {
            m_Recorder.StopRecording();
            DialogResult = DialogResult.OK;
            Close();
            Extension = ".wav";

            if (chbNomalize.Checked)
            {
                OutputFileName = NormalizeOutputFile();
            }

            if (chbConvertToMp3.Checked)
            {
                OutputFileName = ConvertToMp3();
            }

            SaveLastSettings();
        }

        private void BtnCancelClik(object sender, EventArgs e)
        {
            m_Recorder.StopRecording();
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}