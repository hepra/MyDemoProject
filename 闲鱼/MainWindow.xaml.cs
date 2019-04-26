using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new OverFlowCheckViewModel();

            string date = System.DateTime.Now.ToLocalTime().ToString("yy-MM-dd hh:mm-ss");
            ObservableCollection<DMCode> CodeList = new ObservableCollection<DMCode> {
                    new DMCode() {  CodeID=1,CodeName=date,Phone="1870921****",Email="1840921****",Info="追求极致，"}
                    ,new DMCode() {  CodeID=1,CodeName=date,Phone="1840921****",Email="1840921***",Info="追求极致"}
                    ,new DMCode() {  CodeID=1,CodeName=date,Phone="1870921****",Email="1840921****",Info="追求极致，"}
                    ,new DMCode() {  CodeID=1,CodeName=date,Phone="1840921****",Email="1840921****",Info="追求极致"}
                     ,new DMCode() {  CodeID=1,CodeName=date,Phone="1870921****",Email="1840921***",Info="追求极致，"}
                    ,new DMCode() {  CodeID=1,CodeName=date,Phone="1840921****",Email="1840921****",Info="追求极致"}
                    ,new DMCode() {  CodeID=1,CodeName=date,Phone="1870921****",Email="1840921****",Info="追求极致，"}
                    ,new DMCode() {  CodeID=1,CodeName=date,Phone="1870921****",Email="1840921****",Info="追求极致，"}
                };
            AduDataGrids.ItemsSource = CodeList;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
