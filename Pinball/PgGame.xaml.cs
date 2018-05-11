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
using System.Windows.Threading;
using System.Windows.Shapes;
using System.Timers;
using System.Threading.Tasks;

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
            CompositionTarget.Rendering += CompositionTargetRenderingEventHandler;
            timer.Interval = TimeSpan.FromSeconds(0.2);
            timer.Tick += ShootTimerTickEventHandler;
        }

        int ballCount = 10;

        int restOfBalls;

        private void ShootTimerTickEventHandler(object sender, EventArgs e)
        {
            isRunning = true;
   
            restOfBalls--;
            UcBall ball = new UcBall();
            //Canvas.SetTop(ball, 0);
            // Canvas.SetLeft(ball, 0);
            TranslateTransform transform = ball.RenderTransform as TranslateTransform;
            transform.X = canvasWidth / 2;
            transform.Y = 10;
            ball.Vx = initialVelocity * Math.Cos(currentAngle);
            ball.Vy = initialVelocity * Math.Sin(currentAngle);
            if(transform.Y!=0)
            {

            }
            cvs.Children.Add(ball);
            balls.Add(ball);

            if (restOfBalls <= 0)
            {
                timer.Stop();
                // return;
            }
        }

        const double g = 0.5;

        Random random = new Random();

        private void CompositionTargetRenderingEventHandler(object sender, EventArgs e)
        {
            if (!isRunning)
            {
                if(lastTime != DateTime.MinValue)
                {
                    lastTime = DateTime.MinValue;
                }
                return;
            }
            DateTime now = DateTime.Now;

            if (lastTime != DateTime.MinValue)
            {
                TimeSpan span = now - lastTime;
                if(balls.Count==0)
                {
                    PutNewBlocks();
                    isRunning = false;
                    return;
                }
                foreach (var ball in balls.ToArray())
                {

                    TranslateTransform transform = ball.RenderTransform as TranslateTransform;
                    double lastX = transform.X;
                    double lastY = transform.Y;

                    transform.X += ball.Vx * span.TotalSeconds;
                    transform.Y += ball.Vy * span.TotalSeconds;
                    var collisionResult = CheckCollision(ball);
                    if(CheckDie(ball))
                    {
                        continue;
                    }
                    if (collisionResult.IsCollided())
                    {
                        ball.HasCollided = true;
                        collisionResult.TurnBall(ball);
                        ball.Vx *= attenuation;
                        ball.Vy *= attenuation;
                        transform.X = lastX;
                        transform.Y = lastY;
                    }


                    if (ball.HasCollided && Mode == GameMode.UpToDownWithGravity)
                    {
                        ball.Vy += span.TotalMilliseconds * g;
                    }
                }
            }
            lastTime = now;

        }

        private bool CheckDie(UcBall ball)
        {
            if(Mode==GameMode.UpToDownWithGravity)
            {
                if (ball.Y + R > canvasHeight)
                {
                    ball.Visibility = Visibility.Collapsed;
                    cvs.Children.Remove(ball);
                    balls.Remove(ball);
                }
            }
            return false;
        }

        private Turn CheckCollision(UcBall ball)
        {
            Turn result = new Turn(1, 1);
            var tempResult = CheckCollisionWithLine(ball, 1, 0, -canvasWidth);
            if (tempResult != null)
            {
                result.Add(tempResult);
            }
            tempResult = CheckCollisionWithLine(ball, 1, 0, 0);
            if (tempResult != null)
            {
                result.Add(tempResult);
            }

            foreach (var block in blocks.ToArray())
            {
                if (block.Count == 0)
                {
                    blocks.Remove(block);
                    cvs.Children.Remove(block as UIElement);
                }
                tempResult = block.CheckCollision(ball);
                if (tempResult != null)
                {
                    result.Add(tempResult);
                }
            }

            return result;

        }

        private Turn CheckCollisionWithLine(UcBall ball, double a, double b, double c)
        {
            if (!(a == 0 || b == 0))
            {
                throw new Exception("a和b必须有一个为0");
            }
            double distance = Math.Abs(a * ball.X + b * ball.Y + c) / Math.Sqrt(a * a + b * b);
            if (distance > R)
            {
                return null;
            }
            if (b == 0)
            {
                return new Turn(-1, 1);
            }
            else
            {
                return new Turn(1, -1);
            }

        }
        private List<IBlock> blocks = new List<IBlock>();
        private List<UcBall> balls = new List<UcBall>();
        private int score;
        private bool save;
        private double canvasWidth = 450;
        private double canvasHeight = 800;
        DispatcherTimer timer = new DispatcherTimer();
        public int Score { get => score; set => score = value; }

        /// <summary>
        /// 窗体加载完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowLoadedEventHandler(object sender, RoutedEventArgs e)
        {
            bd.Width = canvasWidth;
            bd.Height = canvasHeight;

            line.X1 = canvasWidth / 2;
            line.X2 = canvasWidth / 2 + 20;
            line.RenderTransform = new RotateTransform()
            {
                CenterX = canvasWidth / 2,
                CenterY = line.Y1 + line.StrokeThickness / 2
            };
            
            PageSizeChangedEventHandler(null, null);

            PutNewBlocks();
        }

        private void ButtonClickEventHandler(object sender, RoutedEventArgs e)
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

            if (double.IsNaN(bd.Height))
            {
                return;
            }
            var scale = bd.RenderTransform as ScaleTransform;
            multiple = Math.Min(ActualHeight / canvasHeight, ActualWidth / canvasWidth);
            scale.ScaleX = multiple;
            scale.ScaleY = multiple;

        }

        private double currentAngle;

        private void CanvasPreviewMouseMoveEventHandler(object sender, MouseEventArgs e)
        {
            if (isRunning)
            {
                return;
            }
            Point position = e.GetPosition(cvs);
            if (position.Y < 10)
            {
                return;
            }
            double relativeHeight = position.Y - 10;
            double relativeWidth = position.X - canvasWidth / 2;
            currentAngle = Math.Atan(relativeHeight / relativeWidth);
            if (currentAngle < 0)
            {
                currentAngle = Math.PI + currentAngle;
            }
            double angle = (180 / Math.PI) * currentAngle;

            // Debug.WriteLine( currentAngle);
            // tbkDebug.Text = Math.Floor(relativeHeight) + "," + Math.Floor(relativeWidth);
            tbkDebug.Text = position.X.ToString();
            (line.RenderTransform as RotateTransform).Angle = angle;
        }

        private bool isRunning;

        private void CanvasPreviewMouseDownEventHandler(object sender, MouseButtonEventArgs e)
        {
            if (isRunning)
            {
                return;
            }
            
            restOfBalls = ballCount;
            timer.Start();
        }

        DateTime lastTime = DateTime.MinValue;

        GameMode Mode = GameMode.UpToDownWithGravity;

        private int GetRandomBlockTimes()
        {
            if(random.Next(4)==0)
            {
                return ballCount*2 + random.Next(-5, 5);
            }
            return ballCount  + random.Next(-5, 5);

        }

        private void PutNewBlocks()
        {
            foreach (var block in blocks)
            {
               ( (block as UIElement).RenderTransform as TranslateTransform).Y-=100;
            }
            int count = random.Next(2, 5);
            List<double> blockPoints = new List<double>();
            do
            {
                double x = random.Next(0, (int)canvasWidth);
                if(!blockPoints.Any(p=>Math.Abs(p-x)<=blockSize))
                {
                    blockPoints.Add(x);
                }
            } while (blockPoints.Count < count);

            foreach (var blockX in blockPoints)
            {
                PutBlock(blockX);
            }
        }

        private void PutBlock(double x)
        {
            var block = new UcBlockSquare(GetRandomBlockTimes());
            (block.RenderTransform as TranslateTransform).X = x;
            (block.RenderTransform as TranslateTransform).Y = canvasHeight - 50;

            blocks.Add(block);
            cvs.Children.Add(block);
        }
    }

    public enum GameMode
    {
        UpToDownWithGravity,
        UpToDownWithoutGravity,
        DownToUp,
    }
}
