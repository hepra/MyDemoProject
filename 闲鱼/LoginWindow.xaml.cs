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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace 闲鱼
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            Loaded += IndexWindow_Loaded;
        }
        #region  字段,属性

        /// <summary>
        /// IndexViewModel
        /// </summary>

        private volatile bool _lsLogining;

        private Brush _defaultPasswordBackgroundBrush;
        private Brush _defaultUserNameBackgroundBrush;


        #endregion

        #region  回调函数

        /// <summary>
        /// 窗口头部MouseLeftButtonDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }



        /// <summary>
        /// Loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IndexWindow_Loaded(object sender, RoutedEventArgs e)
        {

            UidInput.Text = Common.ConfigureHelper.Read("uid");
            PwdInput.Password = Common.ConfigureHelper.Read("pwd");
            CboAutoLogin.IsChecked = true;
            CboRememberPwd.IsChecked =true;


            _defaultPasswordBackgroundBrush = UidInput.Background;
            _defaultUserNameBackgroundBrush = PwdInput.Background;

            SetPasswordBackground();
            SetUserNameBackground();

            UidInput.Focus();


            //设置光标位置
            UidInput.SelectionStart = UidInput.Text.Length;
            UidInput.SelectionLength = UidInput.Text.Length;
            SetControlTip();
        }

        void SetControlTip()
        {
            if (UidInput.Text == "")
            {
                LoginBtn.IsEnabled = false;
                this.Dispatcher.Invoke(new Action(() =>
                {
                    tbUserTip.Text = "账号不能为空";
                    borderUser.BorderBrush = (System.Windows.Media.Brush)new BrushConverter().ConvertFromString("#686868");
                    borderUser.BorderThickness = new Thickness(1);
                }));
            }
            else
            {
                tbUserTip.Text = "";
                LoginBtn.IsEnabled = true;
                borderUser.BorderBrush = (System.Windows.Media.Brush)new BrushConverter().ConvertFromString("#686868");
                borderUser.BorderThickness = new Thickness(0);
            }
            if (PwdInput.Password != "")
            {
                if (UidInput.Text == "")
                {
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        tbUserTip.Text = "账号不能为空";
                        borderUser.BorderBrush = (System.Windows.Media.Brush)new BrushConverter().ConvertFromString("#686868");
                        borderUser.BorderThickness = new Thickness(1);
                    }));

                }
            }
        }


        /// <summary>
        /// PasswordChanged
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            SetPasswordBackground();
            tbPasswordTip.Text = "";
            tbUserTip.Text = "";
            borderUser.BorderBrush = (System.Windows.Media.Brush)new BrushConverter().ConvertFromString("#007ACC");
            borderPassword.BorderThickness = new Thickness(0);
            SetControlTip();
        }



        /// <summary>
        /// UserName TextChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtUserName_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            SetUserNameBackground();
            tbPasswordTip.Text = "";
            tbUserTip.Text = "";
            SetControlTip();

        }

        string _loginingText = "请等待";
        /// <summary>
        /// 登录按钮点击回调
        /// </summary>
        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_lsLogining) return;

            var autoLogin = CboAutoLogin.IsChecked == true ? 1 : 0;

            var uid = UidInput.Text;
            var pwd = PwdInput.Password;

            LoginBtn.Content = _loginingText;
            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.3) };
            timer.Tick += delegate
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Send, new Action(() =>
                {
                    var newContent = LoginBtn.Content.ToString();
                    newContent = newContent == _loginingText + "..." ? _loginingText : newContent + ".";
                    LoginBtn.Content = newContent;
                }));
            };
            timer.Start();
            _lsLogining = true;

            new Action(() =>
            {
                try
                {
                    var uidcahe = Common.ConfigureHelper.Read("uid");
                    var pwdcahe = Common.ConfigureHelper.Read("pwd");
                    Dispatcher.BeginInvoke(DispatcherPriority.Send, new Action(() =>
                    {
                        tbPasswordTip.Text = "账户名或密码错误，请重新输入";
                    }));
                }
                catch (Exception ex)
                {
                    tbPasswordTip.Text = ex.Message;
                }
                finally
                {
                    _lsLogining = false;
                    timer.Stop();
                    Dispatcher.BeginInvoke(DispatcherPriority.Send, new Action(() => { LoginBtn.Content = "登录"; }));
                }
            }).BeginInvoke(null, null);
        }



        /// <summary>
        /// IndexWindow_MouseLeftButtonDown  执行窗口拖动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IndexWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }


        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }



        /// <summary>
        /// 最小化窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Min_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        #endregion

        #region  私有函数

        /// <summary>
        /// 设置PasswordBox的背景
        /// </summary>
        private void SetPasswordBackground()
        {
            var passwordBox = PwdInput;
            if (passwordBox.Password.Length == 0)
            {
             //   passwordBox.Background = FindResource("PassWordNullBrush") as Brush;
            }
            else
            {
                passwordBox.Background = _defaultPasswordBackgroundBrush;
            }
        }

        private void SetUserNameBackground()
        {
            var userNameBox = UidInput;
            if (userNameBox.Text.Length == 0)
            {
               // userNameBox.Background = FindResource("UserNameNullBrush") as Brush;
            }
            else
            {
                userNameBox.Background = _defaultUserNameBackgroundBrush;
            }
        }

        #endregion

        private void CboAutoLogin_Checked(object sender, RoutedEventArgs e)
        {
            CboRememberPwd.IsChecked = true;
        }
    }
}
