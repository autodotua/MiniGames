using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static Pinball.UcBall;
using static Pinball.Datas;
using System.Windows.Media.Animation;
using System.Diagnostics;
using System.IO;
using System.Configuration;
using static WPfCodes.Program.Config;

namespace Pinball
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
            WpfControls.Dialog.DialogHelper.DefautDialogOwner = this;
            btnContinue.IsEnabled = File.Exists("PinballrSave.ini");
            InitializeConfigs();
        }

        private void InitializeConfigs()
        {

        }

        public void ReturnToStart()
        {
            frm.Content = null;
            stkControl.Visibility = Visibility.Visible;
            btnContinue.IsEnabled = File.Exists("PinballSave.ini");
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

                    gamePage = new PgGame(false);

                    //  frm.Navigate(new Uri("PgGame.xaml", UriKind.Relative));
                    frm.Content = gamePage;
                    break;

                case "btnContinue":
                    gamePage = new PgGame(true);
                    frm.Content = gamePage;
                    break;

                case "btnSettings":
                    //frm.Navigate(new Uri("PgSettings.xaml",UriKind.Relative));

                    settingPage = new PgSettings();

                    frm.Content = settingPage;
                    break;
            }
            stkControl.Visibility = Visibility.Collapsed;
        }

    }

}
