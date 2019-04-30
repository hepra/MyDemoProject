using FFmpeg.AutoGen;
using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace œ–”„.VideoPro
{
	public sealed class VideoFrameConverter : IDisposable
	{
		private readonly IntPtr _convertedFrameBufferPtr;

		private readonly Size _destinationSize;

		private readonly byte_ptrArray4 _dstData;

		private readonly int_array4 _dstLinesize;

		private unsafe readonly SwsContext* _pConvertContext;

		public unsafe VideoFrameConverter(Size sourceSize, AVPixelFormat sourcePixelFormat, Size destinationSize, AVPixelFormat destinationPixelFormat)
		{
			this._destinationSize = destinationSize;
			this._pConvertContext = ffmpeg.sws_getContext(sourceSize.Width, sourceSize.Height, sourcePixelFormat, destinationSize.Width, destinationSize.Height, destinationPixelFormat, 1, null, null, null);
			if (this._pConvertContext == null)
			{
				throw new ApplicationException("Could not initialize the conversion context.");
			}
			int convertedFrameBufferSize = ffmpeg.av_image_get_buffer_size(destinationPixelFormat, destinationSize.Width, destinationSize.Height, 1);
			this._convertedFrameBufferPtr = Marshal.AllocHGlobal(convertedFrameBufferSize);
			this._dstData = default(byte_ptrArray4);
			this._dstLinesize = default(int_array4);
			ffmpeg.av_image_fill_arrays(ref this._dstData, ref this._dstLinesize, (byte*)(void*)this._convertedFrameBufferPtr, destinationPixelFormat, destinationSize.Width, destinationSize.Height, 1);
		}

		public unsafe void Dispose()
		{
			Marshal.FreeHGlobal(this._convertedFrameBufferPtr);
			ffmpeg.sws_freeContext(this._pConvertContext);
		}

		public unsafe AVFrame Convert(AVFrame sourceFrame)
		{
			ffmpeg.sws_scale(this._pConvertContext, sourceFrame.data, sourceFrame.linesize, 0, sourceFrame.height, this._dstData, this._dstLinesize);
			byte_ptrArray8 data = default(byte_ptrArray8);
			data.UpdateFrom(this._dstData);
			int_array8 linesize = default(int_array8);
			linesize.UpdateFrom(this._dstLinesize);
			AVFrame result = default(AVFrame);
			result.data = data;
			result.linesize = linesize;
			Size destinationSize = this._destinationSize;
			result.width = destinationSize.Width;
			destinationSize = this._destinationSize;
			result.height = destinationSize.Height;
			return result;
		}
	}
}
