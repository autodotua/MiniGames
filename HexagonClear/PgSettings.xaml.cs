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
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           main.ReturnToStart();
        }
    }
}
