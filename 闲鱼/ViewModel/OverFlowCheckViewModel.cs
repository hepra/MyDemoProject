
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using 闲鱼.Model;

namespace 闲鱼.ViewModel
{
    public class OverFlowCheckViewModel:ViewModelBase
    {
        #region 初始化
        Common.TCPHelper.asyncTcpSever tempSever;
        public OverFlowCheckViewModel()
        {
            tempSever = new Common.TCPHelper.asyncTcpSever("192.168.20.102", 10086, 200);
            Thread thSeverListen = new Thread(Listen);
            thSeverListen.IsBackground = true;
            thSeverListen.Start();
        }

        void Listen()
        {
            while (true)
            {
                if (tempSever.client!=null)
                {
                   IP = tempSever.client.RemoteEndPoint.ToString();
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
        private ObservableCollection<DMCode> _CodeList;
        /// <summary>
        /// CodeList
        /// </summary>
        public ObservableCollection<DMCode> CodeList
        {
            get { return _CodeList; }
            set
            {
                _CodeList = value;
                OnPropertyChanged(nameof(CodeList));
            }
        }
        public string _IP;

        public string IP
        {
            get { return _IP; }
            set
            {
                _IP = value;
                OnPropertyChanged(nameof(IP));
            }
        }

        #endregion

        #region 命令
        /// <summary>
        /// 测试按钮
        /// </summary>    
        public ICommand StarCheck => new DelegateCommand(obj =>
        {
            Common.TCPHelper.asyncTcpClient client = new Common.TCPHelper.asyncTcpClient(recieve_message,1024);
            client.连接服务器("192.168.20.102", 10086);
            //MessageBox.Show("StarTriger");
        });
        void recieve_message(byte[] data)
        {
            MessageBox.Show(data.ToString());
        }

        /// <summary>
        /// XXX
        /// </summary>    
        public ICommand StopCheck => new DelegateCommand(obj =>
        {
            MessageBox.Show("StopTriger");
        });

        #endregion
    }
}
