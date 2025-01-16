using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chatbotWPF
{
    static class utils
    {
        private static WaveOutEvent _waveOut;

        public static void PlayAudioFromStream(MemoryStream data)
        {
            _waveOut?.Stop();
            _waveOut?.Dispose();
            _waveOut = new WaveOutEvent();

            using (var waveReader = new WaveFileReader(data))
            {
                _waveOut.Init(waveReader);
                _waveOut.Play();

                while (_waveOut.PlaybackState == PlaybackState.Playing)
                {
                    Task.Delay(500).Wait();
                }
            }
        }
    }
}
