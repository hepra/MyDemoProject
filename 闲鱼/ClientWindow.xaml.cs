using System.Text;
using System.Windows;
namespace 闲鱼
{
    /// <summary>
    /// ClientWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ClientWindow : Window
    {
        Common.TCPHelper.asyncTcpClient  client;
        public ClientWindow(Common.TCPHelper.asyncTcpClient cl)
        {
            InitializeComponent();
            client = cl;
            client.Message_receive = getData;
        }
        void getData(byte[] data)
        {
            tbRecvive.Text = Encoding.UTF8.GetString(data);
        }
       
        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            client.Send(tbMessage.Text);
        }
    }
}
