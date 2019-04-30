using FFmpeg.AutoGen;
using System;
using System.Runtime.InteropServices;

namespace  œ–”„.VideoPro
{
	internal static class FFmpegHelper
	{
		public unsafe static string av_strerror(int error)
		{
			int bufferSize = 1024;
			byte* buffer = stackalloc byte[(int)(uint)bufferSize];
			ffmpeg.av_strerror(error, buffer, (ulong)bufferSize);
			return Marshal.PtrToStringAnsi((IntPtr)(void*)buffer);
		}

		public static int ThrowExceptionIfError(this int error)
		{
			if (error < 0)
			{
				throw new ApplicationException(FFmpegHelper.av_strerror(error));
			}
			return error;
		}
	}
}
