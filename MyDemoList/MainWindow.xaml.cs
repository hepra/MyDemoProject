using MyDemoList.Models;
using System;
using System.Collections.Generic;
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
using Wemew.Program.ZipInfo;

namespace MyDemoList
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow 
    {
        public MainWindow()
        {
            InitializeComponent();
            var zipRepertory = new ZipRepertory();
            var filename = @"F:\Work\bak\9月-2无logo.zip";
            VlcStream CurrentStream = zipRepertory.GetVideoStream(filename, ZipHashCode.HashKey);
            VlcHelper.Initialize(VlcPlayer);
            VlcPlayer.SourceProvider.MediaPlayer.Play(CurrentStream.Stream.Entries.FirstOrDefault(CurrentStream.check).OpenEntryStream());
        }
    }
}
