using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns.Adapter_DesignPattern
{
    public interface IMediaPlayer
    {
        void Play(string fileName);
    }

    public class Mp3Player : IMediaPlayer
    {
        public void Play(string fileName)
        {
            Console.WriteLine($"Playing mp3 file: {fileName}");
        }
    }

    public class Mp4Player
    {
        public void PlayMp4(string fileName)
        {
            Console.WriteLine($"Playing mp4 file: {fileName}");
        }
    }

    public class Mp4Adapter : IMediaPlayer
    {
        private Mp4Player _mp4Player;

        public Mp4Adapter()
        {
            _mp4Player = new Mp4Player();
        }

        public void Play(string fileName)
        {
            _mp4Player.PlayMp4(fileName);
        }
    }
    public class Adapter
    {
        public void Run()
        {
            IMediaPlayer mp3Player = new Mp3Player();
            IMediaPlayer mp4Adapter = new Mp4Adapter();

            mp3Player.Play("song.mp3");       // Works normaly
            mp4Adapter.Play("video.mp4");     // Works via Adapter
        }
    }
}
