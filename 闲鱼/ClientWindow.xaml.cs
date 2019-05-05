using System.Text;
using System.Windows;
namespace 闲鱼
{
    /// <summary>
    /// ClientWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ClientWindow : Window
    {
        SimpleTCP.SimpleTcpClient client;
        SimpleTCP.SimpleTcpServer s;
        public ClientWindow(SimpleTCP.SimpleTcpClient cl, SimpleTCP.SimpleTcpServer sever)
        {
            InitializeComponent();
            client = cl;
            s = sever;
            client.DataReceived += (sender,msg)=> {
                this.Dispatcher.Invoke(new System.Action(() =>
                {
                    tbRecvive.Text = Encoding.UTF8.GetString(msg.Data);
                }));
                
            };
        }
       
        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            client.Write(tbMessage.Text);
        }

        private void BtnSeverSend_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
