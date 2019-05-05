using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using 闲鱼.Model;

using 闲鱼.ViewModel;

namespace 闲鱼
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        OverFlowCheckViewModel viewModel = new OverFlowCheckViewModel();
        public MainWindow()
        {
            LoginWindow login = new LoginWindow();
            var rt= login.ShowDialog();
            //login.Close();
            //if (rt==true)
            //{
                InitializeComponent();

                this.DataContext = viewModel;
                AduDataGrids.ItemsSource = (this.DataContext as OverFlowCheckViewModel).CodeList;

            //}

        }

      
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
        }
    }
}
