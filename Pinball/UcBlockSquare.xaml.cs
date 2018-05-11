using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Pinball.Datas;

namespace Pinball
{
    /// <summary>
    /// UcBall.xaml 的交互逻辑
    /// </summary>
    public partial class UcBlockSquare : UserControl, IBlock
    {

        public UcBlockSquare(int count)
        {
            InitializeComponent();
            Width = 32;
            Height = 32;
            Count = count;
            tbkCount.Text = count.ToString();
            BlockColor = new SolidColorBrush(Colors.Red);
        }

        //public UcBall(double x, double y) : this()
        //{
        //    Width = D;
        //    Height = D;
        //    txt.Text = x + "," + y;
        //}

        public double X => (RenderTransform as TranslateTransform).X;
        public double Y => (RenderTransform as TranslateTransform).Y;
        public double CenterX => X + Width / 2;
        public double CenterY => Y + Height / 2;

        public static readonly DependencyProperty BlockColorProperty =
            DependencyProperty.Register("BlockColor",
                typeof(SolidColorBrush),
                typeof(UcBlockSquare),
                new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0xFF, 0x00, 0x00))));

        private SolidColorBrush BlockColor
        {
            get => GetValue(BlockColorProperty) as SolidColorBrush;
            set => SetValue(BlockColorProperty, value);
        }

        private void BeginColorAnimation(Color To, FrameworkElement target, PropertyPath property)
        {
            ColorAnimation aniOccupable = new ColorAnimation(To, AnimationDuration);
            Storyboard.SetTarget(aniOccupable, this);
            Storyboard.SetTargetProperty(aniOccupable, property);
            Storyboard storyOccupable = new Storyboard();
            storyOccupable.Children.Add(aniOccupable);
            storyOccupable.Begin();
        }


        public Turn CheckCollision(UcBall ball)
        {
            //bool cornerX = true;
            //bool cornerY = true;
            if(ball.LastCollidedBlock==this)
            {
                ball.LastCollidedBlock = null;
                return null;
            }
            //bool turnVx = true;
            //bool turnVy = true;
            //bool left = false;
            //bool top = false;
            Point closetPoint = new Point();
            if (ball.X < X)
            {
                closetPoint.X = X;
                // left = true;
            }
            else if (ball.X > X + Width)
            {
                closetPoint.X = X + Width;
            }
            else
            {
                //cornerX = false;

                closetPoint.X = ball.X;
                //turnVx = false;
            }

            if (ball.Y < Y)
            {
                closetPoint.Y = Y;
                //top = true;
            }
            else if (ball.Y > Y + Height)
            {
                closetPoint.Y = Y + Height;

            }
            else
            {
                closetPoint.Y = ball.Y;
                //cornerY = false;
                //turnVy = false;
            }

            double distance = Math.Sqrt(Math.Pow(closetPoint.X - ball.X, 2) + Math.Pow(closetPoint.Y - ball.Y, 2));
            if (distance < R/* && ((!cornerX) || (!cornerY))*/)
            {
         
                double angle = Math.Abs(180 / Math.PI * Math.Atan((CenterY- ball.Y) / (ball.X - CenterX)));


                Count--;
          
                //if(ball.Vx>0 )
                //{
                    if (ball.Vy > 0)//往右下角
                    {
                        if(angle>=45)
                        {
                            return new Turn(1, -1);
                        }
                        else
                        {
                            return new Turn(-1,1);
                        }
                    }
                    else
                    {
                        if (angle < 45)
                        {
                            return new Turn(1, -1);
                        }
                        else
                        {
                            return new Turn(-1, 1);
                        }
                    }
                //}
                //else
                //{
                //    if (ball.Vy > 1)
                //    {
                //        if (angle < 45)
                //        {
                //            return new Turn(1, -1);
                //        }
                //        else
                //        {
                //            return new Turn(-1, 1);
                //        }
                //    }
                //    else
                //    {
                //        if (angle >= 45)
                //        {
                //            return new Turn(1, -1);
                //        }
                //        else
                //        {
                //            return new Turn(-1, 1);
                //        }
                //    }
                //}

               
            }
            return null;
        }

        private int count;
        public int Count
        {
            get
            {
                return count;
            }
            set
            {
                if(value==0)
                {
                    Visibility = Visibility.Collapsed;
                }
                count = value;
                tbkCount.Text = Count.ToString();
            }
        }
    }
}
