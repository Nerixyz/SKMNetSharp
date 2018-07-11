using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioAnalyze
{
    class AudioAnalyzer
    {
        private SampleAggregator aggregator;
        private readonly int FFT_LENGTH;
        private readonly WaveFormat waveFormat;
        private readonly int BLOCK_ALIGN;
        private readonly WasapiLoopbackCapture capture;

        public List<AudioBand> Bands { get; set; } 

        public AudioAnalyzer(int fftLength = 512, MMDevice device = null)
        {
            this.FFT_LENGTH = fftLength;
            if (device == null)
                this.capture = new WasapiLoopbackCapture();
            else
                this.capture = new WasapiLoopbackCapture(device);
            this.Bands = new List<AudioBand>();

            this.capture.DataAvailable += Capture_DataAvailable;
            this.BLOCK_ALIGN = capture.WaveFormat.BlockAlign;
            this.waveFormat = capture.WaveFormat;

            this.aggregator = new SampleAggregator(FFT_LENGTH)
            {
                PerformFFT = true
            };
            this.aggregator.FftCalculated += Aggregator_FftCalculated;
        }

        public void StartRecording()
        {
            capture.StartRecording();
        }

        ~AudioAnalyzer()
        {
            capture.StopRecording();
            aggregator.PerformFFT = false;
        }

        private void Capture_DataAvailable(object sender, WaveInEventArgs e)
        {
            byte[] buffer = e.Buffer;
            int bytesRecorded = e.BytesRecorded;
            int bufferIncrement = BLOCK_ALIGN;

            for (int index = 0; index < bytesRecorded; index += bufferIncrement)
            {
                float sample = BitConverter.ToSingle(buffer, index);
                aggregator.Add(sample);
            }
        }

        private void Aggregator_FftCalculated(object sender, FftEventArgs e)
        {
            foreach(AudioBand band in Bands)
            {
                band.OnFFT(e.Result, 1, waveFormat.SampleRate);
            }
        }
    }
}
