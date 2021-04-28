using System;
using System.IO;

namespace MediaFiles.Sound
{
    /// <summary>
    /// WAV file class: Allows manipulation of WAV audio files
    /// </summary>
    class WavFile
    {
        /// <summary>
        /// This enumeration specifies file modes supported by the
        /// class.
        /// </summary>
        public enum WavFileMode
        {
            Read,
            Write,
            ReadWrite
        }

        /// <summary>
        /// WavFile class: Static constructor
        /// </summary>
        static WavFile()
        {
            m_DataStartPos = 44;
        }

        /// <summary>
        /// WavFile class: Default constructor
        /// </summary>
        public WavFile()
        {
            InitMembers();
        }

        /// <summary>
        /// Destructor - Makes sure the file is closed.
        /// </summary>
        ~WavFile()
        {
            // Make sure the file is closed
            Close();
        }

        /// <summary>
        /// Opens a WAV file and attemps to read the header & audio information.
        /// </summary>
        /// <param name="pFilename">The name of the file to open</param>
        /// <param name="pMode">The file opening mode.  Only Read and ReadWrite are valid.  If you want to write only, then use Create().</param>
        /// <returns>A blank string on success, or a message on failure.</returns>
        public String Open(String pFilename, WavFileMode pMode)
        {
            m_Filename = pFilename;
            return (Open(pMode));
        }

        /// <summary>
        /// Opens the file specified by m_Filename and reads the file header and audio information.  Does not read any of the audio data.
        /// </summary>
        /// /// <param name="pMode">The file opening mode.  Only Read and ReadWrite are valid.  If you want to write only, then use Create().</param>
        /// <returns>A blank string on success, or a message on failure.</returns>
        public String Open(WavFileMode pMode)
        {
            if (m_FileStream != null)
            {
                m_FileStream.Close();
                m_FileStream.Dispose();
                m_FileStream = null;
            }
            String filenameBackup = m_Filename;
            InitMembers();

            // pMode should be Read or ReadWrite.  Otherwise, throw an exception.  For
            // write-only mode, the user can call Create().
            if ((pMode != WavFileMode.Read) && (pMode != WavFileMode.ReadWrite))
                throw new Exception("File mode not supported: " + WavFileModeStr(pMode));

            if (!File.Exists(filenameBackup))
                return ("File does not exist: " + filenameBackup);

            if (!IsWaveFile(filenameBackup))
                return ("File is not a WAV file: " + filenameBackup);

            m_Filename = filenameBackup;

            String retval = "";

            try
            {
                m_FileStream = File.Open(m_Filename, FileMode.Open);
                m_FileMode = pMode;

                // RIFF chunk (12 bytes total)
                // Read the header (first 4 bytes)
                var buffer = new byte[4];
                m_FileStream.Read(buffer, 0, 4);
                buffer.CopyTo(m_WavHeader, 0);
                // Read the file size (4 bytes)
                m_FileStream.Read(buffer, 0, 4);
                //mFileSizeBytes = BitConverter.ToInt32(buffer, 0);
                // Read the RIFF type
                m_FileStream.Read(buffer, 0, 4);
                buffer.CopyTo(m_RiffType, 0);

                // Format chunk (24 bytes total)
                // "fmt " (ASCII characters)
                m_FileStream.Read(buffer, 0, 4);
                // Length of format chunk (always 16)
                m_FileStream.Read(buffer, 0, 4);
                // 2 bytes (value always 1)
                m_FileStream.Read(buffer, 0, 2);
                // # of channels (2 bytes)
                m_FileStream.Read(buffer, 2, 2);
                m_NumChannels = (BitConverter.IsLittleEndian ? buffer[2] : buffer[3]);
                // Sample rate (4 bytes)
                m_FileStream.Read(buffer, 0, 4);
                if (!BitConverter.IsLittleEndian)
                    Array.Reverse(buffer);
                m_SampleRateHz = BitConverter.ToInt32(buffer, 0);
                // Bytes per second (4 bytes)
                m_FileStream.Read(buffer, 0, 4);
                if (!BitConverter.IsLittleEndian)
                    Array.Reverse(buffer);
                m_BytesPerSec = BitConverter.ToInt32(buffer, 0);
                // Bytes per sample (2 bytes)
                m_FileStream.Read(buffer, 2, 2);
                if (!BitConverter.IsLittleEndian)
                    Array.Reverse(buffer, 2, 2);
                m_BytesPerSample = BitConverter.ToInt16(buffer, 2);
                // Bits per sample (2 bytes)
                m_FileStream.Read(buffer, 2, 2);
                if (!BitConverter.IsLittleEndian)
                    Array.Reverse(buffer, 2, 2);
                m_BitsPerSample = BitConverter.ToInt16(buffer, 2);

                // Data chunk
                // "data" (ASCII characters)
                m_FileStream.Read(buffer, 0, 4);
                // Length of data to follow (4 bytes)
                m_FileStream.Read(buffer, 0, 4);
                if (!BitConverter.IsLittleEndian)
                    Array.Reverse(buffer);
                m_DataSizeBytes = BitConverter.ToInt32(buffer, 0);

                // Total of 44 bytes read up to this point.

                // The data size should be file size - 36 bytes.  If not, then set
                // it to that.
                if (m_DataSizeBytes != FileSizeBytes - 36)
                    m_DataSizeBytes = (int)(FileSizeBytes - 36);

                // The rest of the file is the audio data, which
                // can be read by successive calls to NextSample().

                m_NumSamplesRemaining = NumSamples;
            }
            catch (Exception exc)
            {
                retval = exc.Message;
            }

            return (retval);
        }

        /// <summary>
        /// Closes the file
        /// </summary>
        public void Close()
        {
            if (m_FileStream != null)
            {
                // If in write or read/write mode, write the file size information to
                // the header.
                if ((m_FileMode == WavFileMode.Write) || (m_FileMode == WavFileMode.ReadWrite))
                {
                    // File size: Offset 4, 4 bytes
                    m_FileStream.Seek(4, 0);
                    // Note: Per the WAV file spec, we need to write file size - 8 bytes.
                    // The header is 44 bytes, and 44 - 8 = 36, so we write
                    // mDataBytesWritten + 36.
                    // 2009-03-17: Now using FileSizeBytes - 8 (to avoid mDataBytesWritten).
                    //m_FileStream.Write(BitConverter.GetBytes(mDataBytesWritten+36), 0, 4);
                    int size = (int)FileSizeBytes - 8;
                    byte[] buffer = BitConverter.GetBytes(size);
                    if (!BitConverter.IsLittleEndian)
                        Array.Reverse(buffer);
                    m_FileStream.Write(buffer, 0, 4);
                    // Data size: Offset 40, 4 bytes
                    m_FileStream.Seek(40, 0);
                    //m_FileStream.Write(BitConverter.GetBytes(mDataBytesWritten), 0, 4);
                    size = (int)(FileSizeBytes - m_DataStartPos);
                    buffer = BitConverter.GetBytes(size);
                    if (!BitConverter.IsLittleEndian)
                        Array.Reverse(buffer);
                    m_FileStream.Write(buffer, 0, 4);
                }

                m_FileStream.Close();
                m_FileStream.Dispose();
                m_FileStream = null;
            }

            // Reset the members back to defaults
            InitMembers();
        }

        /// <summary>
        /// When in read mode, returns the next audio sample from the
        /// file.  The return value is a byte array and will contain one
        /// byte if the file contains 8-bit audio or 2 bytes if the file
        /// contains  16-bit audio.  The return value will be null if no
        /// next sample can be read.  For 16-bit samples, the byte array
        /// can be converted to a 16-bit integer using BitConverter.ToInt16().
        /// If there is an error, this method will throw a WAVFileReadException.
        /// </summary>
        /// <returns>A byte array containing the audio sample</returns>
        public byte[] GetNextSample_ByteArray()
        {
            byte[] audioSample;

            // Throw an exception if m_FileStream is null
            if (m_FileStream == null)
                throw new Exception("Read attempted with null internal file stream.");

            // We should be in read or read/write mode.
            if ((m_FileMode != WavFileMode.Read) && (m_FileMode != WavFileMode.ReadWrite))
                throw new Exception("Read attempted in incorrect mode: " + WavFileModeStr(m_FileMode));

            try
            {
                int numBytes = m_BitsPerSample / 8; // 8 bits per byte
                audioSample = new byte[numBytes];
                m_FileStream.Read(audioSample, 0, numBytes);
                --m_NumSamplesRemaining;
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }

            return audioSample;
        }

        /// <summary>
        /// When in read mode, returns the next audio sample from the loaded audio
        /// file.  This is a convenience method that can be used when it is known
        /// that the audio file contains 8-bit audio.  If the audio file is not
        /// 8-bit, this method will throw a WAVFileReadException. 
        /// </summary>
        /// <returns>The next audio sample from the loaded audio file.</returns>
        public byte GetNextSample8Bit()
        {
            // If the audio data is not 8-bit, throw an exception.
            if (m_BitsPerSample != 8)
                throw new Exception("Attempted to retrieve an 8-bit sample when audio is not 8-bit.");

            byte sample8 = 0;

            // Get the next sample using GetNextSample_ByteArray()
            byte[] sample = GetNextSample_ByteArray();
            if (sample != null)
                sample8 = sample[0];

            return sample8;
        }

        /// <summary>
        /// When in read mode, returns the next audio sample from the loaded audio
        /// file.  This is a convenience method that can be used when it is known
        /// that the audio file contains 16-bit audio.  If the audio file is not
        /// 16-bit, this method will throw a WAVFileReadException. 
        /// </summary>
        /// <returns>The next audio sample from the loaded audio file.</returns>
        public short GetNextSample16Bit()
        {
            // If the audio data is not 16-bit, throw an exception.
            if (m_BitsPerSample != 16)
                throw new Exception("Attempted to retrieve a 16-bit sample when audio is not 16-bit.");

            short sample16 = 0;

            // Get the next sample using GetNextSample_ByteArray()
            byte[] sample = GetNextSample_ByteArray();
            if (sample != null)
                sample16 = BitConverter.ToInt16(sample, 0);

            return sample16;
        }

        /// <summary>
        /// When in read mode, returns the next audio sample from the loaded audio
        /// file.  This returns the value as a 16-bit value regardless of whether the
        /// audio file is 8-bit or 16-bit.  If the audio is 8-bit, then the 8-bit sample
        /// value will be scaled to a 16-bit value.
        /// </summary>
        /// <returns>The next audio sample from the loaded audio file, as a 16-bit value, scaled if necessary.</returns>
        public short GetNextSampleAs16Bit()
        {
            short sample16Bit = 0;

            if (m_BitsPerSample == 8)
                sample16Bit = ScaleByteToShort(GetNextSample8Bit());
            else if (m_BitsPerSample == 16)
                sample16Bit = GetNextSample16Bit();

            return sample16Bit;
        }

        /// <summary>
        /// When in read mode, returns the next audio sample from the loaded audio
        /// file.  This returns the value as an 8-bit value regardless of whether the
        /// audio file is 8-bit or 16-bit.  If the audio is 16-bit, then the 16-bit sample
        /// value will be scaled to an 8-bit value.
        /// </summary>
        /// <returns>The next audio sample from the loaded audio file, as an 8-bit value, scaled if necessary.</returns>
        public byte GetNextSampleAs8Bit()
        {
            byte sample8Bit = 0;

            if (m_BitsPerSample == 8)
            {
                sample8Bit = GetNextSample8Bit();
            }
            else if (m_BitsPerSample == 16)
            {
                sample8Bit = ScaleShortToByte(GetNextSample16Bit());
            }

            return sample8Bit;
        }

        /// <summary>
        /// When in write mode, adds a new sample to the audio file.  Takes
        /// an array of bytes representing the sample.  The array should
        /// contain the correct number of bytes to match the sample size.
        /// If there are any errors, this method will throw a WAVFileWriteException.
        /// </summary>
        /// <param name="pSample">An array of bytes representing the audio sample</param>
        public void AddSample_ByteArray(byte[] pSample)
        {
            if (pSample != null)
            {
                // We should be in write or read/write mode.
                if ((m_FileMode != WavFileMode.Write) && (m_FileMode != WavFileMode.ReadWrite))
                    throw new Exception("Write attempted in incorrect mode: " + WavFileModeStr(m_FileMode));

                // Throw an exception if m_FileStream is null
                if (m_FileStream == null)
                    throw new Exception("Write attempted with null internal file stream.");

                // If pSample contains an incorrect number of bytes for the
                // sample size, then throw an exception.
                if (pSample.GetLength(0) != (m_BitsPerSample / 8)) // 8 bits per byte
                    throw new Exception("Attempt to add an audio sample of incorrect size.");

                try
                {
                    int numBytes = pSample.GetLength(0);
                    m_FileStream.Write(pSample, 0, numBytes);
                    //mDataBytesWritten += numBytes;
                }
                catch (Exception exc)
                {
                    throw new Exception(exc.Message);
                }
            }
        }

        /// <summary>
        /// When in write mode, adds an 8-bit sample to the audio file.
        /// Takes a byte containing the sample.  If the audio file is
        /// not 8-bit, this method will throw a WAVFileWriteException.
        /// </summary>
        /// <param name="pSample">The audio sample to add</param>
        public void AddSample8Bit(byte pSample)
        {
            // If the audio data is not 8-bit, throw an exception.
            if (m_BitsPerSample != 8)
                throw new Exception("Attempted to add an 8-bit sample when audio file is not 8-bit.");

            var sample = new byte[1];
            sample[0] = pSample;
            AddSample_ByteArray(sample);
        }

        /// <summary>
        /// When in write mode, adds a 16-bit sample to the audio file.
        /// Takes an Int16 containing the sample.  If the audio file is
        /// not 16-bit, this method will throw a WAVFileWriteException.
        /// </summary>
        /// <param name="pSample">The audio sample to add</param>
        public void AddSample16Bit(short pSample)
        {
            // If the audio data is not 16-bit, throw an exception.
            if (m_BitsPerSample != 16)
                throw new Exception("Attempted to add a 16-bit sample when audio file is not 16-bit.");

            byte[] buffer = BitConverter.GetBytes(pSample);
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(buffer);
            AddSample_ByteArray(buffer);
        }

        /// <summary>
        /// Creates a new WAV audio file.
        /// </summary>
        /// <param name="pFilename">The name of the audio file to create</param>
        /// <param name="pStereo">Whether or not the audio file should be stereo (if this is false, the audio file will be mono).</param>
        /// <param name="pSampleRate">The sample rate of the audio file (in Hz)</param>
        /// <param name="pBitsPerSample">The number of bits per sample (8 or 16)</param>
        /// <param name="pOverwrite">Whether or not to overwrite the file if it exists.  If this is false, then a System.IO.IOException will be thrown if the file exists.</param>
        public void Create(String pFilename, bool pStereo, int pSampleRate, short pBitsPerSample, bool pOverwrite)
        {
            // In case a file is currently open, make sure it
            // is closed.  Note: Close() calls InitMembers().
            Close();

            // If the sample rate is not supported, then throw an exception.
            if (!SupportedSampleRate(pSampleRate))
                throw new Exception("Unsupported sample rate: " + pSampleRate + "WavFile.Create() " + pSampleRate);
            // If the bits per sample is not supported, then throw an exception.
            if (!SupportedBitsPerSample(pBitsPerSample))
                throw new Exception("Unsupported number of bits per sample: " + pBitsPerSample + " WavFile.Create() " + pBitsPerSample);
            // Create the file.  If pOverwrite is true, then use FileMode.Create to overwrite the
            // file if it exists.  Otherwise, use FileMode.CreateNew, which will throw a
            // System.IO.IOException if the file exists.
            m_FileStream = pOverwrite ? File.Open(pFilename, FileMode.Create) : File.Open(pFilename, FileMode.CreateNew);

            m_FileMode = WavFileMode.Write;

            // Set the member data from the parameters.
            m_NumChannels = pStereo ? (byte) 2 : (byte) 1;
            m_SampleRateHz = pSampleRate;
            m_BitsPerSample = pBitsPerSample;

            // Write the parameters to the file header.

            // RIFF chunk (12 bytes total)
            // Write the chunk IDD ("RIFF", 4 bytes)
            byte[] buffer = StrToByteArray("RIFF");
            m_FileStream.Write(buffer, 0, 4);
            if (m_WavHeader == null)
                m_WavHeader = new char[4];
            "RIFF".CopyTo(0, m_WavHeader, 0, 4);
            // File size size (4 bytes) - This will be 0 for now
            Array.Clear(buffer, 0, buffer.GetLength(0));
            m_FileStream.Write(buffer, 0, 4);
            // RIFF type ("WAVE")
            buffer = StrToByteArray("WAVE");
            m_FileStream.Write(buffer, 0, 4);
            if (m_RiffType == null)
                m_RiffType = new char[4];
            "WAVE".CopyTo(0, m_RiffType, 0, 4);

            // Format chunk (24 bytes total)
            // "fmt " (ASCII characters)
            buffer = StrToByteArray("fmt ");
            m_FileStream.Write(buffer, 0, 4);
            // Length of format chunk (always 16, 4 bytes)
            Array.Clear(buffer, 0, buffer.GetLength(0));
            buffer[0] = 16;
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(buffer);
            m_FileStream.Write(buffer, 0, 4);
            // 2 bytes (always 1)
            Array.Clear(buffer, 0, buffer.GetLength(0));
            buffer[0] = 1;
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(buffer, 0, 2);
            m_FileStream.Write(buffer, 0, 2);
            // # of channels (2 bytes)
            Array.Clear(buffer, 0, buffer.GetLength(0));
            buffer[0] = m_NumChannels;
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(buffer, 0, 2);
            m_FileStream.Write(buffer, 0, 2);
            // Sample rate (4 bytes)
            buffer = BitConverter.GetBytes(m_SampleRateHz);
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(buffer);
            m_FileStream.Write(buffer, 0, 4);
            // Calculate the # of bytes per sample: 1=8 bit Mono, 2=8 bit Stereo or
            // 16 bit Mono, 4=16 bit Stereo
            short bytesPerSample;
            if (pStereo)
                bytesPerSample = (short) ((m_BitsPerSample/8)*2);
            else
                bytesPerSample = (short) (m_BitsPerSample/8);
            // Write the # of bytes per second (4 bytes)
            m_BytesPerSec = m_SampleRateHz*bytesPerSample;
            buffer = BitConverter.GetBytes(m_BytesPerSec);
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(buffer);
            m_FileStream.Write(buffer, 0, 4);
            // Write the # of bytes per sample (2 bytes)
            byte[] buffer2Bytes = BitConverter.GetBytes(bytesPerSample);
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(buffer2Bytes);
            m_FileStream.Write(buffer2Bytes, 0, 2);
            // Bits per sample (2 bytes)
            buffer2Bytes = BitConverter.GetBytes(m_BitsPerSample);
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(buffer2Bytes);
            m_FileStream.Write(buffer2Bytes, 0, 2);

            // Data chunk
            // "data" (ASCII characters)
            buffer = StrToByteArray("data");
            m_FileStream.Write(buffer, 0, 4);
            // Length of data to follow (4 bytes) - This will be 0 for now
            Array.Clear(buffer, 0, buffer.GetLength(0));
            m_FileStream.Write(buffer, 0, 4);
            m_DataSizeBytes = 0;

            // Total of 44 bytes written up to this point.

            // The rest of the file is the audio data
        }

        /// <summary>
        /// Creates a new WAV audio file.  This is an overload that always overwrites the file if it exists.
        /// </summary>
        /// <param name="pFilename">The name of the audio file to create</param>
        /// <param name="pStereo">Whether or not the audio file should be stereo (if this is false, the audio file will be mono).</param>
        /// <param name="pSampleRate">The sample rate of the audio file (in Hz)</param>
        /// <param name="pBitsPerSample">The number of bits per sample (8 or 16)</param>
        public void Create(String pFilename, bool pStereo, int pSampleRate, short pBitsPerSample)
        {
            Create(pFilename, pStereo, pSampleRate, pBitsPerSample, true);
        }

        /// <summary>
        /// Returns whether or not the WAV file format (mono/stereo,
        /// sample rate, and bits per sampe) match another WAV file's
        /// format.
        /// </summary>
        /// <param name="pWavFile">Another WavFile object to compare with</param>
        /// <returns></returns>
        public bool FormatMatches(WavFile pWavFile)
        {
            bool retval = false;

            if (pWavFile != null)
                retval = ((m_NumChannels == pWavFile.m_NumChannels) &&
                          (m_SampleRateHz == pWavFile.m_SampleRateHz) &&
                          (m_BitsPerSample == pWavFile.m_BitsPerSample));

            return retval;
        }

        /// <summary>
        /// Returns whether or not a file is a WAV audio file.
        /// </summary>
        /// <param name="pFilename">The name of the file to check</param>
        /// <returns>true if the file is a wave audio file, or false if not</returns>
        static public bool IsWaveFile(String pFilename)
        {
            bool result = false;

            if (File.Exists(pFilename))
            {
                try
                {
                    FileStream fileStream = File.Open(pFilename, FileMode.Open);
                    // For a WAV file, the first 4 bytes should be "RIFF", and
                    // the RIFF type (3rd set of 4 bytes) should be "WAVE".
                    var fileId = new char[4];
                    var riffType = new char[4];

                    var buffer = new byte[4];

                    // Read the file ID (first 4 bytes)
                    fileStream.Read(buffer, 0, 4);
                    buffer.CopyTo(fileId, 0);

                    // Read the next 4 bytes (but we don't care about this)
                    fileStream.Read(buffer, 0, 4);

                    // Read the RIFF ID (4 bytes)
                    fileStream.Read(buffer, 0, 4);
                    buffer.CopyTo(riffType, 0);

                    fileStream.Close();

                    var fileIdStr = new string(fileId);
                    var riffTypeStr1 = new string(riffType);

                    result = ((fileIdStr == "RIFF") && (riffTypeStr1 == "WAVE"));
                }
                catch (Exception exc)
                {
                    throw new Exception(exc.Message);
                }
            }

            return result;
        }

        /// <summary>
        /// Returns whether or not a sample rate is supported by this class.
        /// </summary>
        /// <param name="pSampleRateHz">A sample rate (in Hz)</param>
        /// <returns>true if the sample rate is supported, or false if not.</returns>
        public static bool SupportedSampleRate(int pSampleRateHz)
        {
            return ((pSampleRateHz == 8000) || (pSampleRateHz == 11025) ||
                    (pSampleRateHz == 16000) || (pSampleRateHz == 18900) ||
                    (pSampleRateHz == 22050) || (pSampleRateHz == 32000) ||
                    (pSampleRateHz == 37800) || (pSampleRateHz == 44056) ||
                    (pSampleRateHz == 44100) || (pSampleRateHz == 48000));
        }

        /// <summary>
        /// Returns whether or not a number of bits per sample is supported by this class.
        /// </summary>
        /// <param name="pBitsPerSample">A number of bits per sample</param>
        /// <returns>true if the bits/sample is supported by this class, or false if not.</returns>
        public static bool SupportedBitsPerSample(short pBitsPerSample)
        {
            return ((pBitsPerSample == 8) || (pBitsPerSample == 16));
        }
        
        /// <summary>
        /// Moves the file pointer back to the start of the audio data.
        /// </summary>
        public void SeekToAudioDataStart()
        {
            SeekToAudioSample(0);
            // Update mSamplesRemaining - but this is only necessary for read or read/write mode.
            if ((m_FileMode == WavFileMode.Read) || (m_FileMode == WavFileMode.ReadWrite))
                m_NumSamplesRemaining = NumSamples;
        }

        /// <summary>
        /// Moves the file pointer to a given audio sample number.
        /// </summary>
        /// <param name="pSampleNum">The sample number to which to move the file pointer</param>
        public void SeekToAudioSample(long pSampleNum)
        {
            if (m_FileStream != null)
            {
                // Figure out the byte position.  This will be m_DataStartPos + however many
                // bytes per sample * pSampleNum.
                long bytesPerSample = m_BitsPerSample / 8;
                try
                {
                    m_FileStream.Seek(m_DataStartPos + (bytesPerSample * pSampleNum), 0);
                }
                catch (IOException exc)
                {
                    throw new Exception("Unable to to seek to sample " + pSampleNum + ": " + exc.Message);
                }
                catch (NotSupportedException exc)
                {
                    throw new Exception("Unable to to seek to sample " + pSampleNum + ": " + exc.Message);
                }
                catch (Exception exc)
                {
                    throw new Exception(exc.Message);
                }
            }
        }

        /// <summary>
        /// Returns a string representation of a WavFileMode enumeration value.
        /// </summary>
        /// <param name="pMode">A value of the WavFileMode enumeration</param>
        /// <returns>A string representation of pMode</returns>
        public static String WavFileModeStr(WavFileMode pMode)
        {
            String result;

            switch (pMode)
            {
                case WavFileMode.Read:
                    result = "Read";
                    break;
                case WavFileMode.Write:
                    result = "Write";
                    break;
                case WavFileMode.ReadWrite:
                    result = "ReadWrite";
                    break;
                default:
                    result = "Unknown";
                    break;
            }

            return result;
        }

        ////////////////
        // Properties //
        ////////////////

        /// <summary>
        /// Gets the name of the file that was opened.
        /// </summary>
        public String Filename
        {
            get { return m_Filename; }
        }

        /// <summary>
        /// Gets the WAV file header as read from the file (array of 4 chars)
        /// </summary>
        public char[] WAVHeader
        {
            get { return m_WavHeader; }
        }

        /// <summary>
        /// Gets the WAV file header as read from the file, as a String object
        /// </summary>
        public String WavHeaderString
        {
            get
            {
                return new String(m_WavHeader);
            }
        }

        /// <summary>
        /// Gets the RIFF type as read from the file (array of chars)
        /// </summary>
        public char[] RIFFType
        {
            get { return m_RiffType; }
        }

        /// <summary>
        /// Gets the RIFF type as read from the file, as a String object
        /// </summary>
        public String RiffTypeString
        {
            get
            {
                return new String(m_RiffType);
            }
        }

        /// <summary>
        /// Gets the audio file's number of channels
        /// </summary>
        public byte NumChannels
        {
            get { return m_NumChannels; }
        }

        /// <summary>
        /// Gets whether or not the file is stereo.
        /// </summary>
        public bool IsStereo
        {
            get { return (m_NumChannels == 2); }
        }

        /// <summary>
        /// Gets the audio file's sample rate (in Hz)
        /// </summary>
        public int SampleRateHz
        {
            get { return m_SampleRateHz; }
        }

        /// <summary>
        /// Gets the number of bytes per second for the audio file
        /// </summary>
        public int BytesPerSec
        {
            get { return m_BytesPerSec; }
        }

        /// <summary>
        /// Gets the number of bytes per sample for the audio file
        /// </summary>
        public short BytesPerSample
        {
            get { return m_BytesPerSample; }
        }

        /// <summary>
        /// Gets the number of bits per sample for the audio file
        /// </summary>
        public short BitsPerSample
        {
            get { return m_BitsPerSample; }
        }

        /// <summary>
        /// Gets the data size (in bytes) for the audio file.  This is read from a field
        /// in the WAV file header.
        /// </summary>
        public int DataSizeBytes
        {
            get { return m_DataSizeBytes; }
        }

        /// <summary>
        /// Gets the file size (in bytes).  This is read from a field in the WAV file header.
        /// </summary>
        public long FileSizeBytes
        {
            //get { return mFileSizeBytes; }
            get { return m_FileStream.Length; }
        }

        /// <summary>
        /// Gets the number of audio samples in the WAV file.  This is calculated based on
        /// the data size read from the file and the number of bits per sample.
        /// </summary>
        public int NumSamples
        {
            get { return (m_DataSizeBytes / (m_BitsPerSample / 8)); }
        }

        /// <summary>
        /// Gets the number of samples remaining (when in read mode).
        /// </summary>
        public int NumSamplesRemaining
        {
            get { return m_NumSamplesRemaining; }
        }

        /// <summary>
        /// Gets the mode of the open file (read, write, or read/write)
        /// </summary>
        public WavFileMode FileOpenMode
        {
            get { return m_FileMode; }
        }

        /// <summary>
        /// Gets the current file byte position.
        /// </summary>
        public long FilePosition
        {
            get { return (m_FileStream != null ? m_FileStream.Position : 0); }
        }

        /////////////////////
        // Private members //
        /////////////////////

        /// <summary>
        /// Initializes the data members (for the constructors).
        /// </summary>
        private void InitMembers()
        {
            m_Filename = null;
            m_FileStream = null;
            m_WavHeader = new char[4];
            m_DataSizeBytes = 0;
            m_BytesPerSample = 0;
            //mFileSizeBytes = 0;
            m_RiffType = new char[4];

            // These audio format defaults correspond to the standard for
            // CD audio.
            m_NumChannels = 2;
            m_SampleRateHz = 44100;
            m_BytesPerSec = 176400;
            m_BitsPerSample = 16;

            m_FileMode = WavFileMode.Read;

            m_NumSamplesRemaining = 0;
        }

        /// <summary>
        /// Converts a string to a byte array.  The source for this came
        /// from http://www.chilkatsoft.com/faq/DotNetStrToBytes.html .
        /// </summary>
        /// <param name="pStr">A String object</param>
        /// <returns>A byte array containing the data from the String object</returns>
        private static byte[] StrToByteArray(String pStr)
        {
            var encoding = new System.Text.ASCIIEncoding();
            return encoding.GetBytes(pStr);
        }

        /// <summary>
        /// Scales a byte value to a 16-bit (short) value by calculating the value's percentage of
        /// maximum for 8-bit values, then calculating the 16-bit value with that
        /// percentage.
        /// </summary>
        /// <param name="pByteVal">A byte value to convert</param>
        /// <returns>The 16-bit scaled value</returns>
        private static short ScaleByteToShort(byte pByteVal)
        {
            short val16Bit = 0;
            double scaleMultiplier;
            if (pByteVal > 0)
            {
                scaleMultiplier = (double)pByteVal / byte.MaxValue;
                val16Bit = (short)(short.MaxValue * scaleMultiplier);
            }
            else if (pByteVal < 0)
            {
                scaleMultiplier = pByteVal / (double)byte.MinValue;
                val16Bit = (short)(short.MinValue * scaleMultiplier);
            }

            return val16Bit;
        }

        /// <summary>
        /// Scales a 16-bit (short) value to an 8-bit (byte) value by calculating the
        /// value's percentage of maximum for 16-bit values, then calculating the 8-bit
        /// value with that percentage.
        /// </summary>
        /// <param name="pShortVal">A 16-bit value to convert</param>
        /// <returns>The 8-bit scaled value</returns>
        private static byte ScaleShortToByte(short pShortVal)
        {
            byte val8Bit = 0;
            double scaleMultiplier;

            if (pShortVal > 0)
            {
                scaleMultiplier = pShortVal / (double)short.MaxValue;
                val8Bit = (byte)(byte.MaxValue * scaleMultiplier);
            }
            else if (pShortVal < 0)
            {
                scaleMultiplier = pShortVal / (double)short.MinValue;
                val8Bit = (byte)(byte.MinValue * scaleMultiplier);
            }

            return val8Bit;
        }

        private String m_Filename;       // The name of the file open
        private FileStream m_FileStream; // For reading the audio file
        // File header information
        private char[] m_WavHeader;      // The WAV header (4 bytes, "RIFF")
        //private int mFileSizeBytes;     // The file size, in bytes
        private char[] m_RiffType;       // The RIFF type (4 bytes, "WAVE")
        // Audio format information
        private byte m_NumChannels;      // The # of channels (1 or 2)
        private int m_SampleRateHz;      // The audio sample rate (Hz)
        private int m_BytesPerSec;       // Bytes per second
        private short m_BytesPerSample;  // # of bytes per sample (1=8 bit Mono, 2=8 bit Stereo or 16 bit Mono, 4=16 bit Stereo)
        private short m_BitsPerSample;   // # of bits per sample
        private int m_DataSizeBytes;     // The data size (bytes)
        private WavFileMode m_FileMode;  // Specifies the file mode (read, write)

        //private int mDataBytesWritten;  // Used in write mode for keeping track of
        // the number of bytes written
        private int m_NumSamplesRemaining; // When in read mode, this is used for keeping track of how many audio
        // samples remain.  This is updated in GetNextSample_ByteArray().

        // Static members
        private static readonly short m_DataStartPos; // The byte position of the start of the audio data
    }
}