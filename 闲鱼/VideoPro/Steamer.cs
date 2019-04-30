using FFmpeg.AutoGen;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace œ–”„.VideoPro
{
	public class Steamer
	{
		public static string FileDir
		{
			get;
			set;
		}

		public static string url
		{
			get;
			set;
		}

		public static void SaveImage()
		{
			if (!string.IsNullOrEmpty(Steamer.url))
			{
				DateTime date = DateTime.Now;
				string filePath2 = "";
				filePath2 = ((!string.IsNullOrEmpty(Steamer.FileDir)) ? (Steamer.FileDir + "//" + date.ToString("yyyyMMddHHmmss") + ".jpg") : (date.ToString("yyyyMMddHHmmss") + ".jpg"));
				Steamer.DecodeAllFramesToImages(Steamer.url, filePath2);
			}
		}

		private unsafe static void DecodeAllFramesToImages(string url, string filePath)
		{
			using (VideoStreamDecoder vsd = new VideoStreamDecoder(url))
			{
				IReadOnlyDictionary<string, string> info = vsd.GetContextInfo();
				Size sourceSize = vsd.FrameSize;
				AVPixelFormat sourcePixelFormat = vsd.PixelFormat;
				Size destinationSize = sourceSize;
				AVPixelFormat destinationPixelFormat = AVPixelFormat.AV_PIX_FMT_BGR24;
				using (VideoFrameConverter vfc = new VideoFrameConverter(sourceSize, sourcePixelFormat, destinationSize, destinationPixelFormat))
				{
					AVFrame frame = default(AVFrame);
					vsd.TryDecodeNextFrame(out frame);
					AVFrame convertedFrame = vfc.Convert(frame);
					using (Bitmap bitmap = new Bitmap(convertedFrame.width, convertedFrame.height, convertedFrame.linesize[0u], PixelFormat.Format24bppRgb, (IntPtr)(void*)convertedFrame.data[0u]))
					{
						bitmap.Save(filePath, ImageFormat.Jpeg);
					}
				}
			}
		}

		public unsafe static Bitmap DecodeAllFramesToImages(string url)
		{
			Bitmap _Bitmap = null;
			using (VideoStreamDecoder vsd = new VideoStreamDecoder(url))
			{
				IReadOnlyDictionary<string, string> info = vsd.GetContextInfo();
				Size sourceSize = vsd.FrameSize;
				AVPixelFormat sourcePixelFormat = vsd.PixelFormat;
				Size destinationSize = sourceSize;
				AVPixelFormat destinationPixelFormat = AVPixelFormat.AV_PIX_FMT_BGR24;
				using (VideoFrameConverter vfc = new VideoFrameConverter(sourceSize, sourcePixelFormat, destinationSize, destinationPixelFormat))
				{
					AVFrame frame = default(AVFrame);
					vsd.TryDecodeNextFrame(out frame);
					AVFrame convertedFrame = vfc.Convert(frame);
					using (Bitmap bitmap = new Bitmap(convertedFrame.width, convertedFrame.height, convertedFrame.linesize[0u], PixelFormat.Format24bppRgb, (IntPtr)(void*)convertedFrame.data[0u]))
					{
						_Bitmap = (Bitmap)bitmap.Clone();
					}
				}
			}
			return _Bitmap;
		}

		private static ImageCodecInfo GetEncoderInfo(string mimeType)
		{
			ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();
			for (int i = 0; i < encoders.Length; i++)
			{
				if (encoders[i].MimeType == mimeType)
				{
					return encoders[i];
				}
			}
			return null;
		}
	}
}
