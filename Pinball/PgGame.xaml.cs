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
using System.Text;
using System.IO;
using static WpfControls.Dialog.DialogHelper;

namespace Pinball
{
    /// <summary>
    /// PgGame.xaml 的交互逻辑
    /// </summary>
    public partial class PgGame : Page
    {
        public PgGame(bool save)
        {
            InitializeComponent();
            this.save = save;
        }
        private int score;
        private bool save;

        public int Score { get => score; set => score = value; }

        /// <summary>
        /// 窗体加载完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowLoadedEventHandler(object sender, RoutedEventArgs e)
        {

        }
     
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            switch (ShowMessage("是否要关闭？" +
                Environment.NewLine + "当前得分：" + Score,
                WpfControls.Dialog.DialogType.Information,
                new string[] { "关闭", "暂停", "取消" }))
            {
                case 0:
                    Visibility = Visibility.Collapsed;
                    main.ReturnToStart();
                    break;
                case 1:
                 
                    break;
                case 2:
                    break;
            }
        }

        double multiple = 1;
        private void PageSizeChangedEventHandler(object sender, SizeChangedEventArgs e)
        {

            if (double.IsNaN(grdMain.Height))
            {
                return;
            }
            var scale = grdMain.RenderTransform as ScaleTransform;
            multiple = Math.Min(ActualHeight / grdMain.Height, ActualWidth / grdMain.Width);
            scale.ScaleX = multiple;
            scale.ScaleY = multiple;

        }
    }
}
