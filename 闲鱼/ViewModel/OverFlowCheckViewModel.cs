
using FFmpeg.AutoGen;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Input;
using System.Windows.Interop;
using 闲鱼.Model;
using 闲鱼.VideoPro;

namespace 闲鱼.ViewModel
{
    public class OverFlowCheckViewModel:ViewModelBase
    {
        #region 初始化
       public  Common.TCPHelper.asyncTcpSever tempSever;
        public OverFlowCheckViewModel()
        {
            tempSever = new Common.TCPHelper.asyncTcpSever("192.168.20.102", 10086, 200);
            tempSever.Recieve_Message_BufferSize = 1024;
            Thread thSeverListen = new Thread(Listen);
            thSeverListen.IsBackground = true;
            thSeverListen.Start();
            tempSever.Message_receive = sever_recieve_message;
            FFmpegBinariesHelper.RegisterFFmpegBinaries();
          //  SetupLogging();
        }
      public  void sever_recieve_message(byte[] data)
        {
             string message = Encoding.ASCII.GetString(data);
            Common.TCPHelper.COMMANDER cmd = new Common.TCPHelper.COMMANDER(message);
            messageList.Add(cmd);
            DMCode temp = new DMCode();
            temp.CodeID = cmd.BoxId;
            temp.CodeName = cmd.CommandType;
            temp.Email = cmd.PackagePosition;
            temp.Info = cmd.PackagePositionCount;
            temp.Phone = cmd.DATETIME;
            severmeeage = cmd.GenerateSendSuccessMessage();
            byte[] data1 = Encoding.ASCII.GetBytes(severmeeage);
           // tempSever.clientList[0].BeginSend(data1,0,data1.Length,SocketFlags.None, new AsyncCallback(Message_Send), tempSever.clientList[0]);
            CodeList.Add(temp);
        }
        public string severmeeage { get; set; }
        void Message_Send(IAsyncResult ar)
        {
            var socket = ar.AsyncState as Socket;
        }
        private void ClientAccepted(IAsyncResult ar)
        {
          
        }
        void Listen()
        {
            while (true)
            {
                if (tempSever.clientList.Count >0)
                {
                   IP = tempSever.clientList[0].RemoteEndPoint.ToString();
                    Pendpoint = tempSever.clientList[0].RemoteEndPoint;
                    ConnectDate = System.DateTime.Now.ToString("yy-MM-dd HH:mm:ss");
                    return;
                }
                else
                {
                    Thread.Sleep(1000);
                }

            }

        }
        #endregion

        #region 属性
        private List<DMCode> _CodeList = new List<DMCode>();
        /// <summary>
        /// CodeList
        /// </summary>
        public List<DMCode> CodeList
        {
            get { return _CodeList; }
            set
            {
                _CodeList = value;
                OnPropertyChanged(nameof(CodeList));
            }
        }

        public List<Common.TCPHelper.COMMANDER> messageList = new List<Common.TCPHelper.COMMANDER>();
        private string _IP;
        public string IP
        {
            get { return _IP; }
            set
            {
                _IP = value;
                OnPropertyChanged(nameof(IP));
            }
        }
        private System.Net.EndPoint _IPendpoint;
        public System.Net.EndPoint Pendpoint
        {
            get { return _IPendpoint; }
            set
            {
                _IPendpoint = value;
                OnPropertyChanged(nameof(Pendpoint));
            }
        }

        private string _ConnectDate;
        public string ConnectDate
        {
            get { return _ConnectDate; }
            set
            {
                _ConnectDate = value;
                OnPropertyChanged(nameof(ConnectDate));
            }
        }

        

        private string _SeverIP;
        public string SeverIP
        {
            get { return _SeverIP; }
            set
            {
                _SeverIP = value;
                OnPropertyChanged(nameof(SeverIP));
            }
        }

        private System.Windows.Media.ImageSource _Image;
        public System.Windows.Media.ImageSource Image
        {
            get { return _Image; }
            set
            {
                _Image = value;
                OnPropertyChanged(nameof(Image));
            }
        }
        private string _FileDir;
        /// <summary>
        /// 图片保存路径
        /// </summary>
        public string FileDir
        {
            get { return _FileDir; }
            set
            {
                _FileDir = value;
                OnPropertyChanged(nameof(FileDir));
            }
        }

        private int _Second;
        /// <summary>
        /// 调度时间
        /// </summary>
        public int Second
        {
            get { return _Second; }
            set
            {
                _Second = value;
                OnPropertyChanged(nameof(Second));
            }
        }

        #endregion

        #region 命令
        /// <summary>
        /// 测试按钮
        /// </summary>    
        public ICommand StarCheck => new DelegateCommand(obj =>
        {
            if(tempSever.clientList.Count<1)
            {
                Common.TCPHelper.asyncTcpClient client = new Common.TCPHelper.asyncTcpClient(recieve_message, 1024);
                client.连接服务器("192.168.20.102", 10086);
                ClientWindow clientWindow = new ClientWindow(client);
                clientWindow.Show();
            }
            else
            {
                return;
            }
            
            //Init();
            //if (this.scheduler == null)
            //{
            //    this.Init();
            //}
            //else if (this.scheduler.IsStarted)
            //{
            //    this.scheduler = null;
            //}
            //else
            //{
            //    this.scheduler.Shutdown();
            //}
            //DecodeAllFramesToImages(IP);
        });
        void recieve_message(byte[] data)
        {
            System.Windows.MessageBox.Show(data.ToString());
        }
        /// <summary>
        /// XXX
        /// </summary>    
        public ICommand StopCheck => new DelegateCommand(obj =>
        {
            if (this.scheduler.IsStarted)
            {
                this.scheduler = null;
            }
            else
            {
                this.scheduler.Shutdown();
            }
            System.Windows.MessageBox.Show("StopTriger");
        });

        #endregion

        #region 方法
        private unsafe static void SetupLogging()
        {
            //ffmpeg.av_log_set_level(40);
            //av_log_set_callback_callback logCallback = delegate (void* p0, int level, string format, byte* vl)
            //{
            //    if (level <= ffmpeg.av_log_get_level())
            //    {
            //        int num = 1024;
            //        byte* ptr = stackalloc byte[(int)(uint)num];
            //        int num2 = 1;
            //        ffmpeg.av_log_format_line(p0, level, format, vl, ptr, num, &num2);
            //        string value = System.Runtime.InteropServices.Marshal.PtrToStringAnsi((IntPtr)(void*)ptr);
            //        Console.ForegroundColor = ConsoleColor.Yellow;
            //        Console.Write(value);
            //        Console.ResetColor();
            //    }
            //};
            //ffmpeg.av_log_set_callback(logCallback);
        }
        private unsafe void DecodeAllFramesToImages(string url)
        {
            using (VideoStreamDecoder vsd = new VideoStreamDecoder(url))
            {
                IReadOnlyDictionary<string, string> info = vsd.GetContextInfo();
                System.Drawing.Size sourceSize = vsd.FrameSize;
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
                         this.Image = Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, System.Windows.Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
                    }
                }
            }
        }
        private IScheduler scheduler;
        private void Init()
        {
            this.scheduler = null;
            Quartz.Impl.StdSchedulerFactory schedulerFactory = new StdSchedulerFactory();
            this.scheduler = schedulerFactory.GetScheduler();
            this.scheduler.Start();
            //调度
            int iSeconds = Second;
            //保存路径
            Steamer.FileDir = FileDir;
            //设备流路径
            Steamer.url = IP;
            IJobDetail job = JobBuilder.Create<HelCunJob>().WithIdentity("HelCunJob", "HelCunG").Build();
            ITrigger trigger = TriggerBuilder.Create().WithIdentity("HelCun", "HelCunG").StartNow()
                .WithSimpleSchedule(delegate (SimpleScheduleBuilder x)
                {
                    x.WithIntervalInSeconds(iSeconds).RepeatForever();
                })
                .Build();
            this.scheduler.ScheduleJob(job, trigger);
        }
        #endregion
    }
}
