using FFmpeg.AutoGen;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;

namespace VideoPro
{
	public sealed class VideoStreamDecoder : IDisposable
	{
		private unsafe readonly AVCodecContext* _pCodecContext;

		private unsafe readonly AVFormatContext* _pFormatContext;

		private readonly int _streamIndex;

		private unsafe readonly AVFrame* _pFrame;

		private unsafe readonly AVPacket* _pPacket;

		public string CodecName
		{
			get;
		}

		public Size FrameSize
		{
			get;
		}

		public AVPixelFormat PixelFormat
		{
			get;
		}

		public unsafe VideoStreamDecoder(string url)
		{
			this._pFormatContext = ffmpeg.avformat_alloc_context();
			AVFormatContext* pFormatContext = this._pFormatContext;
			ffmpeg.avformat_open_input(&pFormatContext, url, null, null).ThrowExceptionIfError();
			ffmpeg.avformat_find_stream_info(this._pFormatContext, null).ThrowExceptionIfError();
			AVStream* pStream = null;
			int i = 0;
			while (i < this._pFormatContext->nb_streams)
			{
				if (this._pFormatContext->streams[i]->codec->codec_type != 0)
				{
					i++;
					continue;
				}
				pStream = this._pFormatContext->streams[i];
				break;
			}
			if (pStream == null)
			{
				throw new InvalidOperationException("Could not found video stream.");
			}
			this._streamIndex = pStream->index;
			this._pCodecContext = pStream->codec;
			AVCodecID codecId = this._pCodecContext->codec_id;
			AVCodec* pCodec = ffmpeg.avcodec_find_decoder(codecId);
			if (pCodec == null)
			{
				throw new InvalidOperationException("Unsupported codec.");
			}
			ffmpeg.avcodec_open2(this._pCodecContext, pCodec, null).ThrowExceptionIfError();
			this.CodecName = ffmpeg.avcodec_get_name(codecId);
			this.FrameSize = new Size(this._pCodecContext->width, this._pCodecContext->height);
			this.PixelFormat = this._pCodecContext->pix_fmt;
			this._pPacket = ffmpeg.av_packet_alloc();
			this._pFrame = ffmpeg.av_frame_alloc();
		}

		public unsafe void GetAVFormatContext(string out_filename)
		{
			AVFormatContext* ofmt_ctx = default(AVFormatContext*);
			ffmpeg.avformat_alloc_output_context2(&ofmt_ctx, null, "flv", out_filename);
			ffmpeg.av_interleaved_write_frame(ofmt_ctx, this._pPacket);
			AVStream* pStream2 = null;
			int i = 0;
			while (true)
			{
				if (i < this._pFormatContext->nb_streams)
				{
					if (this._pFormatContext->streams[i]->codec->codec_type != 0)
					{
						i++;
						continue;
					}
					break;
				}
				return;
			}
			pStream2 = this._pFormatContext->streams[i];
			AVStream* out_stream = ffmpeg.avformat_new_stream(ofmt_ctx, pStream2->codec->codec);
		}

		public unsafe void Dispose()
		{
			ffmpeg.av_frame_unref(this._pFrame);
			ffmpeg.av_free(this._pFrame);
			ffmpeg.av_packet_unref(this._pPacket);
			ffmpeg.av_free(this._pPacket);
			ffmpeg.avcodec_close(this._pCodecContext);
			AVFormatContext* pFormatContext = this._pFormatContext;
			ffmpeg.avformat_close_input(&pFormatContext);
		}

		public unsafe bool TryDecodeNextFrame(out AVFrame frame)
		{
			ffmpeg.av_frame_unref(this._pFrame);
			int error2;
			do
			{
				try
				{
					do
					{
						error2 = ffmpeg.av_read_frame(this._pFormatContext, this._pPacket);
						if (error2 == ffmpeg.AVERROR_EOF)
						{
							frame = *this._pFrame;
							return false;
						}
						error2.ThrowExceptionIfError();
					}
					while (this._pPacket->stream_index != this._streamIndex);
					ffmpeg.avcodec_send_packet(this._pCodecContext, this._pPacket).ThrowExceptionIfError();
				}
				finally
				{
					ffmpeg.av_packet_unref(this._pPacket);
				}
				error2 = ffmpeg.avcodec_receive_frame(this._pCodecContext, this._pFrame);
			}
			while (error2 == ffmpeg.AVERROR(11));
			error2.ThrowExceptionIfError();
			frame = *this._pFrame;
			return true;
		}

		public unsafe IReadOnlyDictionary<string, string> GetContextInfo()
		{
			AVDictionaryEntry* tag = null;
			Dictionary<string, string> result = new Dictionary<string, string>();
			while ((tag = ffmpeg.av_dict_get(this._pFormatContext->metadata, "", tag, 2)) != null)
			{
				string key = Marshal.PtrToStringAnsi((IntPtr)(void*)tag->key);
				string value = Marshal.PtrToStringAnsi((IntPtr)(void*)tag->value);
				result.Add(key, value);
			}
			return result;
		}
	}
}
