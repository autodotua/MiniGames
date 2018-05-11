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
        public static int index = 0;

        public UcBall()
        {
            InitializeComponent();
            Width = D;
            Height = D;
            BallColor = new SolidColorBrush(Colors.Black);
            DebugMode = true;
            txt.Text = index++.ToString();
        }

        //public UcBall(double x, double y) : this()
        //{
        //    Width = D;
        //    Height = D;
        //    txt.Text = x + "," + y;
        //}

        private double vx;
        private double vy;
        public double X => (RenderTransform as TranslateTransform).X + R;
        public double Y => (RenderTransform as TranslateTransform).Y + R;
        private bool hasCollided = false;

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
            get => GetValue(DebugModeProperty).Equals(Visibility.Visible);
            set => SetValue(DebugModeProperty, value ? Visibility.Visible : Visibility.Collapsed);
        }

        public IBlock LastCollidedBlock;

        private void BeginColorAnimation(Color To, FrameworkElement target, PropertyPath property)
        {
            ColorAnimation aniOccupable = new ColorAnimation(To, AnimationDuration);
            Storyboard.SetTarget(aniOccupable, this);
            Storyboard.SetTargetProperty(aniOccupable, property);
            Storyboard storyOccupable = new Storyboard();
            storyOccupable.Children.Add(aniOccupable);
            storyOccupable.Begin();
        }

        public double Vy { get => vy; set => vy = value; }
        public double Vx { get => vx; set => vx = value; }
        public bool HasCollided { get => hasCollided; set => hasCollided = value; }
    }
}
