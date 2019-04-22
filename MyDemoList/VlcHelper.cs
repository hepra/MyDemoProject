using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Vlc.DotNet.Wpf;

namespace MyDemoList
{
    public static class VlcHelper
    {
        public static void Initialize(this VlcControl control)
        {
            var currentAssembly = Assembly.GetEntryAssembly();
            var currentDirectory = @"F:\Work\003git\MyDemoProject\MyDemoList\libs";
            if (currentDirectory == null)
                return;

            var vlcLibDirectory = new DirectoryInfo(System.IO.Path.Combine(currentDirectory, "libvlc", IntPtr.Size == 4 ? "x86" : "x64"));

            var options = new string[]
            {

            };
            control.SourceProvider.CreatePlayer(vlcLibDirectory, options);
        }

        public static void Stop(this VlcControl control)
        {
            control.SourceProvider.MediaPlayer.Stop();
        }

        public static void Source(this VlcControl control, string uri)
        {
            control.SourceProvider.MediaPlayer.Play(new Uri(uri));
        }

        public static void Source(this VlcControl control, Stream stream)
        {
            control.SourceProvider.MediaPlayer.Play(stream);
        }

        public static bool IsStop(this VlcControl control)
        {
            return !(control.SourceProvider.MediaPlayer.IsPlaying());
        }

        public static float GetPosition(this VlcControl control)
        {
            return control.SourceProvider.MediaPlayer.Position;
        }

        public static bool IsMuted(this VlcControl control, bool value)
        {
            return control.SourceProvider.MediaPlayer.Audio.IsMute = value;
        }

        public static bool CanPause(this VlcControl control)
        {
            return control.SourceProvider.MediaPlayer.IsPlaying();
        }

        public static void Pause(this VlcControl control)
        {
            control.SourceProvider.MediaPlayer.Pause();
        }
    }
}
