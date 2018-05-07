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
using static HexagonClear.Datas;

namespace HexagonClear
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
        private BallStatus status = BallStatus.Empty;
        public BallStatus Status
        {
            get => status;
            set
            {
                switch (value)
                {
                    case BallStatus.Empty:
                        if (status == BallStatus.Occupied)
                        {
                            //ThicknessAnimation aniToSmall = new ThicknessAnimation(new Thickness(R), AnimationDuration);
                            //ThicknessAnimation aniToBig = new ThicknessAnimation(new Thickness(), AnimationDuration);

                            //Storyboard.SetTarget(aniToSmall, ball);
                            //Storyboard.SetTarget(aniToBig, ball);
                            //Storyboard.SetTargetProperty(aniToSmall, new PropertyPath(MarginProperty));
                            //Storyboard.SetTargetProperty(aniToBig, new PropertyPath(MarginProperty));
                            //Storyboard storyToSmall = new Storyboard();
                            //Storyboard storyToBig = new Storyboard();
                            //storyToSmall.Children.Add(aniToSmall);
                            //storyToBig.Children.Add(aniToBig);
                            //storyToSmall.Completed += (p1, p2) =>
                            //{
                            //    BallColor = new SolidColorBrush(Color.FromRgb(0xDD, 0xDD, 0xDD));

                            //    storyToBig.Begin();
                            //};
                            //storyToSmall.Begin();
                            // (ball.RenderTransform as ScaleTransform).ScaleX = 0.1;
                            TimeSpan aniTimeSpan = TimeSpan.FromMilliseconds(AnimationDuration.TotalMilliseconds / 2);
                            DoubleAnimation aniXToSmall = new DoubleAnimation(0, aniTimeSpan);
                            Storyboard.SetTarget(aniXToSmall, circle);
                            Storyboard.SetTargetProperty(aniXToSmall, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleX)"));
                            DoubleAnimation aniYToSmall = new DoubleAnimation(0, aniTimeSpan);
                            Storyboard.SetTarget(aniYToSmall, circle);
                            Storyboard.SetTargetProperty(aniYToSmall, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleY)"));
                            Storyboard storyToSmall = new Storyboard() { Children = { aniXToSmall, aniYToSmall } };

                            DoubleAnimation aniXToBig = new DoubleAnimation(1, aniTimeSpan);
                            Storyboard.SetTarget(aniXToBig, circle);
                            Storyboard.SetTargetProperty(aniXToBig, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleX)"));
                            DoubleAnimation aniYToBig = new DoubleAnimation(1, aniTimeSpan);
                            Storyboard.SetTarget(aniYToBig, circle);
                            Storyboard.SetTargetProperty(aniYToBig, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleY)"));
                            Storyboard storyToBig = new Storyboard() { Children = { aniXToBig, aniYToBig } };

                            storyToSmall.Completed += (p1, p2) =>
                            {
                                BallColor = new SolidColorBrush(Color.FromRgb(0xDD, 0xDD, 0xDD));
                                storyToBig.Begin();
                            };
                            storyToSmall.Begin();
                        }
                        else
                        {
                            BallColor = new SolidColorBrush(Color.FromRgb(0xDD, 0xDD, 0xDD));
                        }
                        break;
                    case BallStatus.Standby:
                        //normalStatus = status;
                        BallColor = new SolidColorBrush(Color.FromRgb(0xFF, 0xAA, 0xAA));
                        break;
                    case BallStatus.Occupied:
                        if (status == BallStatus.Standby)
                        {
                            ColorAnimation aniStandby = new ColorAnimation(Color.FromRgb(0xFF, 0x55, 0x55), AnimationDuration);
                            Storyboard.SetTarget(aniStandby, this);
                            Storyboard.SetTargetProperty(aniStandby, new PropertyPath("BallColor.(SolidColorBrush.Color)"));
                            Storyboard storyStandby = new Storyboard();
                            storyStandby.Children.Add(aniStandby);
                            storyStandby.Begin();
                        }
                        else
                        {
                            BallColor = new SolidColorBrush(Color.FromRgb(0xFF, 0x55, 0x55));
                        }
                        break;
                    case BallStatus.Occupable:
                        ColorAnimation aniOccupable = new ColorAnimation(Color.FromRgb(0xAA, 0xFF, 0xAA), AnimationDuration);
                        Storyboard.SetTarget(aniOccupable, this);
                        Storyboard.SetTargetProperty(aniOccupable, new PropertyPath("BallColor.(SolidColorBrush.Color)"));
                        Storyboard storyOccupable = new Storyboard();
                        storyOccupable.Children.Add(aniOccupable);
                        storyOccupable.Begin();
                        break;
                }
                status = value;


            }
        }

        public bool Occupable { get => occupable; set => occupable = value; }

        public enum BallStatus
        {
            Empty,
            Standby,
            Occupied,
            Occupable,
        }
    }
}
