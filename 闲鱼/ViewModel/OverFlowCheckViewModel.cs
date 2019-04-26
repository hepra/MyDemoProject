
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using 闲鱼.Model;

namespace 闲鱼.ViewModel
{
    public class OverFlowCheckViewModel:ViewModelBase
    {
        #region 初始化
        public OverFlowCheckViewModel()
        {
            
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

        #endregion

        #region 命令
        /// <summary>
        /// 测试按钮
        /// </summary>    
        public ICommand StarCheck => new DelegateCommand(obj =>
        {
            MessageBox.Show("StarTriger");
        });


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
