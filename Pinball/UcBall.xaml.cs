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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Pinball.Datas;

namespace Pinball
{
    /// <summary>
    /// UcBall.xaml 的交互逻辑
    /// </summary>
    public partial class UcBall : UserControl
    {

        public double X { get; set; }
        public double Y { get; set; }
        public UcBall()
        {
            InitializeComponent();
        }

        public UcBall(double x, double y) : this()
        {
            Width = D;
            Height = D;
            X = x;
            Y = y;
            txt.Text = x + "," + y;
        }

        public static readonly DependencyProperty BallColorProperty =
            DependencyProperty.Register("BallColor",
                typeof(SolidColorBrush),
                typeof(UcBall),
                new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0xDD, 0xDD, 0xDD))));

        private SolidColorBrush BallColor
        {
            get => GetValue(BallColorProperty) as SolidColorBrush;
            set => SetValue(BallColorProperty, value);
        }
        public static readonly DependencyProperty DebugModeProperty =
    DependencyProperty.Register("DebugMode",
        typeof(Visibility),
        typeof(UcBall),
        new PropertyMetadata(Visibility.Collapsed));

        private bool DebugMode
        {
            get => GetValue(DebugModeProperty) .Equals( Visibility.Visible);
            set => SetValue(DebugModeProperty, value?Visibility.Visible:Visibility.Collapsed);
        }
        private bool occupable=false;


        private void BeginColorAnimation(Color To,FrameworkElement target,PropertyPath property)
        {
            ColorAnimation aniOccupable = new ColorAnimation(To, AnimationDuration);
            Storyboard.SetTarget(aniOccupable, this);
            Storyboard.SetTargetProperty(aniOccupable,property);
            Storyboard storyOccupable = new Storyboard();
            storyOccupable.Children.Add(aniOccupable);
            storyOccupable.Begin();
        }

        public bool Occupable { get => occupable; set => occupable = value; }


    }
}
