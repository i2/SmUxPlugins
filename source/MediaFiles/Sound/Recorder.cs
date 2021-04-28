using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic.CompilerServices;

namespace MediaFiles.Sound
{
    public class Recorder
    {
        #region Types

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct WAVEINCAPS
        {
            public short wMid;
            public short wPid;
            public int vDriverVersion;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string szPname;

            public int dwFormats;
            public short wChannels;
            public short wReserved;
            public int dwSupport;
        }

        #endregion
        #region Enums

        public enum BitsPerSampleValue
        {
            High = 0x10,
            Low = 8
        }

        public enum ChannelValue
        {
            Mono = 1,
            Stereo = 2
        }

        public enum SamplesPerSecValue
        {
            High = 0xac44,
            Low = 0x2b11,
            Medium = 0x5622
        }

        #endregion
        #region Private fields

        private BitsPerSampleValue m_BitsPerSample;
        private ChannelValue m_Channels;
        private bool m_IsPaused;
        private bool m_IsRecording;
        private SamplesPerSecValue m_SamplesPerSecond;
        private DateTime m_RecordingStartTime;

        #endregion
        #region Properties

        public BitsPerSampleValue BitsPerSample
        {
            get { return m_BitsPerSample; }
            set { m_BitsPerSample = value; }
        }

        public ChannelValue Channels
        {
            get { return m_Channels; }
            set { m_Channels = value; }
        }

        public string Filename { get; private set; }

        public bool IsRecording
        {
            get { return m_IsRecording; }
            set { m_IsRecording = value; }
        }

        public bool Pause
        {
            get { return m_IsPaused; }
            set
            {
                if (!m_IsRecording)
                {
                    m_IsPaused = false;
                }
                else
                {
                    m_IsPaused = value;
                    string parameter = Conversions.ToString(0L);
                    if (m_IsPaused)
                    {
                        SendMCICommand("pause capture", parameter);
                    }
                    else
                    {
                        SendMCICommand("resume capture", parameter);
                    }
                }
            }
        }

        public SamplesPerSecValue SamplesPerSecond
        {
            get { return m_SamplesPerSecond; }
            set { m_SamplesPerSecond = value; }
        }

        public TimeSpan SampleLength
        {
            get
            {
                return DateTime.Now - m_RecordingStartTime;
            }
        }

        #endregion
        #region Ctors

        public Recorder(string wavFileName)
        {
            m_IsPaused = false;
            m_Channels = ChannelValue.Stereo;
            m_SamplesPerSecond = SamplesPerSecValue.High;
            m_BitsPerSample = BitsPerSampleValue.High;
            Filename = "";
            m_IsRecording = false;
            Filename = wavFileName;
        }

        #endregion
        #region Public methods

        public bool SoundCardExist()
        {
            return (waveInGetNumDevs() > 0);
        }

        public void StartRecording(int deviceInputNumber)
        {
            if (IsRecording)
            {
                throw new InvalidOperationException();
            }

            var bitsPerSample = (double)m_BitsPerSample;
            var channels = (double)m_Channels;
            var samplesPerSecond = (double)m_SamplesPerSecond;

            var bytepersec = (int)Math.Round(bitsPerSample * channels * samplesPerSecond / 8.0);
            var alignment = (int)Math.Round(bitsPerSample * channels / 8.0);

            //SendMCICommand("close capture");

            SendMCICommand("open new type waveaudio alias capture");

            SendMCICommand(
                "set capture bitspersample {0} channels {1}  alignment {2} samplespersec {3} bytespersec {4} format tag pcm wait",
                (int) m_BitsPerSample, (int) m_Channels,
                Conversions.ToString(alignment),
                Conversions.ToString((int) m_SamplesPerSecond),
                Conversions.ToString(bytepersec));

            SendMCICommand("set capture input {0}", deviceInputNumber);
            SendMCICommand("record capture");

            m_IsRecording = true;
            m_RecordingStartTime = DateTime.Now;
        }

        public void StopRecording()
        {
            if (Filename == "")
            {
                throw new Exception("No file specified to save to.");
            }

            SendMCICommand("stop capture");
            SendMCICommand("save capture {0}", Filename);
            SendMCICommand("close capture");
            
            m_IsRecording = false;
        }

        public IList<string> GetDevices()
        {
            var result = new List<string>();
            var waveoutcaps = new WAVEINCAPS();
            var sizeOf = (uint)Marshal.SizeOf(waveoutcaps);
            
            for (int i = 0; i < waveInGetNumDevs(); i++)
            {
                waveInGetDevCaps((IntPtr)i, ref waveoutcaps, sizeOf);
                result.Add(waveoutcaps.szPname);
            }

            return result;
        }

        #endregion
        #region Private methods

        private static void SendMCICommand(string command, params object[] p)
        {
            SendMCICommand(false, command, p);
        }

        private static int SendMCICommand(bool ignoreError, string command, params object[] p)
        {
            string parameter = Conversions.ToString(0L);
            string fullCommand = string.Format(command, p);

            int result = mciSendString(ref fullCommand, ref parameter, 0, 0);

            if (!ignoreError && result != 0)
            {
                string message = string.Format("Podczas wykonywania polecenia MCI: {0} wyst¹pi³ b³¹d nr: {1}", command, result);
                throw new InvalidOperationException(message);
            }

            return result;
        }

        [DllImport("winmm.dll", EntryPoint = "mciSendStringA", CharSet = CharSet.Ansi, SetLastError = true,
            ExactSpelling = true)]
        private static extern int mciSendString([MarshalAs(UnmanagedType.VBByRefStr)] ref string lpstrCommand,
                                                [MarshalAs(UnmanagedType.VBByRefStr)] ref string lpstrReturnString,
                                                int uReturnLength, int hwndCallback);

        [DllImport("winmm.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int waveInGetNumDevs();

        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint waveInGetDevCaps(IntPtr hwo, ref WAVEINCAPS pwoc, uint cbwoc);

        #endregion
    }
}