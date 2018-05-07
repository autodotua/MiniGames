using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static HexagonClear.UcBall;
using static HexagonClear.Datas;
using System.Windows.Media.Animation;
using System.Diagnostics;

namespace HexagonClear
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        PgGame gamePage;
        PgSettings settingPage;

        /// <summary>
        /// 构造函数
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            main = this;
        }

        public void ReturnToStart()
        {
            frm.Content = null;
            grdControl.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 按钮单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonClickEventHandler(object sender, RoutedEventArgs e)
        {
            switch ((sender as Button).Name)
            {
                case "btnStart":

                    gamePage = new PgGame();

                    //  frm.Navigate(new Uri("PgGame.xaml", UriKind.Relative));
                    frm.Content = gamePage;
                    break;

                case "btnSettings":
                    //frm.Navigate(new Uri("PgSettings.xaml",UriKind.Relative));

                    settingPage = new PgSettings();

                    frm.Content = settingPage;
                    break;
            }
            grdControl.Visibility = Visibility.Collapsed;
        }

    }

}
