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
using static HexagonClear.Datas;
using static WPfCodes.Program.Config;

namespace HexagonClear
{
    /// <summary>
    /// PgSettings.xaml 的交互逻辑
    /// </summary>
    public partial class PgSettings : Page
    {
        public PgSettings()
        {
            InitializeComponent();
            cbbLength.SelectedIndex = Length - 4;
            cbbClickToHold.SelectedIndex = ClickToHold ? 1 : 0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if ((sender as Button).Name == "btnOk")
            {
                config.SetInt("Length",cbbLength.SelectedIndex + 4);
                config.SetBool("ClickToHold", cbbClickToHold.SelectedIndex == 1);
                config.Save();
                InitializeConfigs();
            }

            main.ReturnToStart();
        }
    }
}
