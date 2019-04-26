using FFmpeg.AutoGen;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace VideoPro
{
	public class Form1 : Form
	{
		private Thread _Thread = null;

		private IScheduler scheduler;

		private IContainer components = null;

		private Button button1;

		private TextBox textBox1;

		private PictureBox playwnd1;

		private Label label1;

		private TextBox textBox2;

		private Label label2;

		private GroupBox groupBox1;

		private TextBox textBox3;

		private Label label3;

		private Button button3;

		private Button button2;

		public Form1()
		{
			this.InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(this.textBox1.Text.ToString()))
			{
				MessageBox.Show("先填写海康设备流地址", "提示");
			}
			else
			{
				this.DecodeAllFramesToImages(this.textBox1.Text.ToString());
			}
		}

		private unsafe void DecodeAllFramesToImages(string url)
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
					int frameNumber = 0;
					AVFrame frame = default(AVFrame);
					vsd.TryDecodeNextFrame(out frame);
					AVFrame convertedFrame = vfc.Convert(frame);
					using (Bitmap bitmap = new Bitmap(convertedFrame.width, convertedFrame.height, convertedFrame.linesize[0u], PixelFormat.Format24bppRgb, (IntPtr)(void*)convertedFrame.data[0u]))
					{
						Graphics g = this.playwnd1.CreateGraphics();
						g.SmoothingMode = SmoothingMode.HighSpeed;
						Graphics graphics = g;
						Bitmap image = bitmap;
						Size size = this.playwnd1.Size;
						int width = size.Width;
						size = this.playwnd1.Size;
						graphics.DrawImage(image, 0, 0, width, size.Height);
					}
				}
			}
		}

		private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Console.WriteLine(e.ExceptionObject.ToString());
			Console.ReadLine();
		}

		private void OnPlay(int index)
		{
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			FFmpegBinariesHelper.RegisterFFmpegBinaries();
			Form1.SetupLogging();
		}

		private unsafe static void SetupLogging()
		{
			ffmpeg.av_log_set_level(40);
			av_log_set_callback_callback logCallback = delegate(void* p0, int level, string format, byte* vl)
			{
				if (level <= ffmpeg.av_log_get_level())
				{
					int num = 1024;
					byte* ptr = stackalloc byte[(int)(uint)num];
					int num2 = 1;
					ffmpeg.av_log_format_line(p0, level, format, vl, ptr, num, &num2);
					string value = Marshal.PtrToStringAnsi((IntPtr)(void*)ptr);
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.Write(value);
					Console.ResetColor();
				}
			};
			ffmpeg.av_log_set_callback(logCallback);
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			Environment.Exit(0);
		}

		private void label1_Click(object sender, EventArgs e)
		{
		}

		private void button2_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(this.textBox1.Text.ToString()))
			{
				MessageBox.Show("先填写海康设备流地址", "提示");
			}
			else if (this.scheduler == null)
			{
				this.Init();
			}
			else if (this.scheduler.IsStarted)
			{
				this.button2.Text = "停止";
				this.scheduler = null;
			}
			else
			{
				this.scheduler.Shutdown();
				this.button2.Text = "开始";
			}
		}

		private void Init()
		{
			this.button2.Text = "停止";
			this.scheduler = null;
			StdSchedulerFactory schedulerFactory = new StdSchedulerFactory();
			this.scheduler = schedulerFactory.GetScheduler();
			this.scheduler.Start();
			int iSeconds = 10;
			int.TryParse(this.textBox3.Text.ToString(), out iSeconds);
			if (!string.IsNullOrEmpty(this.textBox2.Text.Trim()))
			{
				Steamer.FileDir = this.textBox2.Text.ToString().Trim();
			}
			Steamer.url = this.textBox1.Text.ToString().Trim();
			IJobDetail job = JobBuilder.Create<HelCunJob>().WithIdentity("HelCunJob", "HelCunG").Build();
			ITrigger trigger = TriggerBuilder.Create().WithIdentity("HelCun", "HelCunG").StartNow()
				.WithSimpleSchedule(delegate(SimpleScheduleBuilder x)
				{
					x.WithIntervalInSeconds(iSeconds).RepeatForever();
				})
				.Build();
			this.scheduler.ScheduleJob(job, trigger);
		}

		private void button3_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog fbd = new FolderBrowserDialog();
			if (fbd.ShowDialog() == DialogResult.OK)
			{
				this.textBox2.Text = fbd.SelectedPath;
				Steamer.FileDir = fbd.SelectedPath;
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.playwnd1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.playwnd1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(462, 15);
            this.button1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(81, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "采集图片";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(99, 18);
            this.textBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(360, 21);
            this.textBox1.TabIndex = 1;
            // 
            // playwnd1
            // 
            this.playwnd1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.playwnd1.BackColor = System.Drawing.SystemColors.Desktop;
            this.playwnd1.Location = new System.Drawing.Point(8, 150);
            this.playwnd1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.playwnd1.Name = "playwnd1";
            this.playwnd1.Size = new System.Drawing.Size(743, 394);
            this.playwnd1.TabIndex = 10;
            this.playwnd1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 11;
            this.label1.Text = "海康流地址";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(76, 18);
            this.textBox2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(360, 21);
            this.textBox2.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 20);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 11;
            this.label2.Text = "存图地址";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Location = new System.Drawing.Point(23, 42);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Size = new System.Drawing.Size(727, 93);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "定时";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(76, 52);
            this.textBox3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(68, 21);
            this.textBox3.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 54);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 11;
            this.label3.Text = "调度(秒)";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(431, 18);
            this.button3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(35, 19);
            this.button3.TabIndex = 0;
            this.button3.Text = "...";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(147, 44);
            this.button2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(45, 32);
            this.button2.TabIndex = 0;
            this.button2.Text = "开始";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(769, 562);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.playwnd1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form1";
            this.Text = "读取海康摄像头";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.playwnd1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
    }
}
