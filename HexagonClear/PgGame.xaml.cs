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
using System.Text;
using System.IO;
using static WpfControls.Dialog.DialogHelper;

namespace HexagonClear
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

        private bool save;
        /// <summary>
        /// 重心横坐标
        /// </summary>
        double CenterX => cvs.ActualWidth / 2;
        /// <summary>
        /// 中心纵坐标
        /// </summary>
        double CenterY => cvs.ActualHeight / 2;
        /// <summary>
        /// 根号三的一半的值
        /// </summary>
        readonly double sqrt32 = Math.Sqrt(3) / 2;
        /// <summary>
        /// “棋盘”的球的集合
        /// </summary>
        private List<UcBall> ballsBoard = new List<UcBall>();
        /// <summary>
        /// 横向的球的集合
        /// </summary>
        private Dictionary<int, List<UcBall>> horizontalLine = new Dictionary<int, List<UcBall>>();
        /// <summary>
        /// 右上到左下的球的集合
        /// </summary>
        private Dictionary<int, List<UcBall>> leftObliqueLine = new Dictionary<int, List<UcBall>>();
        /// <summary>
        /// 左上到右下的球的集合
        /// </summary>
        private Dictionary<int, List<UcBall>> rightObliqueLine = new Dictionary<int, List<UcBall>>();
        private Dictionary<Canvas, HashSet<UcBall>> availablePosition = new Dictionary<Canvas, HashSet<UcBall>>();
        /// <summary>
        /// 分数
        /// </summary>
        private int score;
        /// <summary>
        /// 分数
        /// </summary>
        private int Score
        {
            get => score;
            set
            {
                score = value;
                tbk.ChangeTo(value.ToString());
            }
        }


        /// <summary>
        /// 获取随机的一种图形
        /// </summary>
        /// <returns></returns>
        private UcBall[] GetRandomBallsGroup()
        {
            RNGCryptoServiceProvider csp = new RNGCryptoServiceProvider();
            byte[] bytes = new byte[1];
            csp.GetBytes(bytes);
            // Random r = new Random();
            switch (bytes[0] % 28)
            {
                case 0:
                    /*
                    ···
                    */
                    return new UcBall[] { new UcBall(-2, 0), new UcBall(0, 0), new UcBall(2, 0) };
                case 1:
                    /*
                    ·
                     ·
                      ·
                    */
                    return new UcBall[] { new UcBall(-1, -1), new UcBall(0, 0), new UcBall(1, 1) };
                case 2:
                    /*
                      ·
                     ·
                    ·
                    */
                    return new UcBall[] { new UcBall(-1, 1), new UcBall(0, 0), new UcBall(1, -1) };
                case 3:
                    /*
                    ····
                    */
                    return new UcBall[] { new UcBall(-3, 0), new UcBall(-1, 0), new UcBall(1, 0), new UcBall(3, 0) };
                case 4:
                    /*
                    ·
                     ·
                      ·
                       ·
                    */
                    return new UcBall[] { new UcBall(-1.5, -1.5), new UcBall(-0.5, -0.5), new UcBall(0.5, 0.5), new UcBall(1.5, 1.5) };
                case 5:
                    /*
                       ·
                      ·
                     ·
                    ·
                    */
                    return new UcBall[] { new UcBall(-1.5, 1.5), new UcBall(-0.5, 0.5), new UcBall(0.5, -0.5), new UcBall(1.5, -1.5) };
                case 6:
                    /*
                   ·
                   ···
                   */
                    return new UcBall[] { new UcBall(-2, 0), new UcBall(0, 0), new UcBall(2, 0), new UcBall(-1, 1) };
                case 7:
                    /*
                     ·
                   ···
                   */
                    return new UcBall[] { new UcBall(-2, 0), new UcBall(0, 0), new UcBall(2, 0), new UcBall(1, 1) };
                case 8:
                    /*
                   ···
                   ·
                   */
                    return new UcBall[] { new UcBall(-2, 0), new UcBall(0, 0), new UcBall(2, 0), new UcBall(-1, -1) };
                case 9:
                    /*
                   ···
                     ·
                   */
                    return new UcBall[] { new UcBall(-2, 0), new UcBall(0, 0), new UcBall(2, 0), new UcBall(1, -1) };
                case 10:
                    /*
                   ·
                  · ·
                     ·
                   */
                    return new UcBall[] { new UcBall(-2, 0), new UcBall(-1, 1), new UcBall(0, 0), new UcBall(1, -1) };
                case 11:
                    /*
                    ·
                     ·
                    · ·
                    */
                    return new UcBall[] { new UcBall(-1, 1), new UcBall(-1, -1), new UcBall(0, 0), new UcBall(1, -1) };
                case 12:
                    /*
                   ·  ·
                    ·
                     ·
                   */
                    return new UcBall[] { new UcBall(-1, 1), new UcBall(1, 1), new UcBall(0, 0), new UcBall(1, -1) };
                case 13:
                    /*
                    ·
                     ·  ·
                      ·
                    */
                    return new UcBall[] { new UcBall(-1, 1), new UcBall(0, 0), new UcBall(1, -1), new UcBall(2, 0) };
                case 14:
                    /*
                    · ·
                     ·
                    ·
                   */
                    return new UcBall[] { new UcBall(-1, 1), new UcBall(1, 1), new UcBall(0, 0), new UcBall(-1, -1) };
                case 15:
                    /*
                     ·
                 ·  ·
                  ·
                 */
                    return new UcBall[] { new UcBall(-2, 0), new UcBall(1, 1), new UcBall(0, 0), new UcBall(-1, -1) };
                case 16:
                    /*
                   ·
                  · ·
                 ·
                */
                    return new UcBall[] { new UcBall(-1, -1), new UcBall(0, 0), new UcBall(1, 1), new UcBall(2, 0) };
                case 17:
                    /*
                  ·
                 ·
                · ·
               */
                    return new UcBall[] { new UcBall(-1, -1), new UcBall(0, 0), new UcBall(1, 1), new UcBall(1, -1) };

                case 18:
                    //口朝上
                    return new UcBall[] { new UcBall(-2, 0), new UcBall(-1, -1), new UcBall(1, -1), new UcBall(2, 0) };
                case 19:
                    //口朝左上
                    return new UcBall[] { new UcBall(-1, -1), new UcBall(1, -1), new UcBall(2, 0), new UcBall(1, 1) };
                case 20:
                    //口朝左下
                    return new UcBall[] { new UcBall(-1, 1), new UcBall(1, 1), new UcBall(2, 0), new UcBall(1, -1) };
                case 21:
                    //口朝下
                    return new UcBall[] { new UcBall(-2, 0), new UcBall(-1, 1), new UcBall(1, 1), new UcBall(2, 0) };
                case 22:
                    //口朝右下
                    return new UcBall[] { new UcBall(-2, 0), new UcBall(-1, -1), new UcBall(-1, 1), new UcBall(1, 1) };
                case 23:
                    //口朝右上
                    return new UcBall[] { new UcBall(-2, 0), new UcBall(-1, -1), new UcBall(-1, 1), new UcBall(1, -1) };
                case 24:
                    //点
                    return new UcBall[] { new UcBall(0, 0) };
                case 25:
                    //点
                    return new UcBall[] { new UcBall(0, 0) };
                case 26:
                    //··
                    return new UcBall[] { new UcBall(-1, 0), new UcBall(1, 0) };
                case 27:
                    /*
                    ·
                    ·
                    */
                    return new UcBall[] { new UcBall(0, 1), new UcBall(0, -1) };
                default:
                    return null;


            }


        }

        /// <summary>
        /// 窗体加载完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowLoadedEventHandler(object sender, RoutedEventArgs e)
        {
            grdMain.Width = (2 * Length - 1 + 4) * D;
            grdMain.RowDefinitions[0].Height = new GridLength((2 * Length - 1 + 2) * D);
            grdMain.RowDefinitions[2].Height = new GridLength(3 * D);
            grdMain.Arrange(new Rect(new Size(grdMain.Width, grdMain.Height = grdMain.RowDefinitions.Sum(p => p.Height.Value))));
            PageSizeChangedEventHandler(null, null);

            InitializeBoard();
            PutNewGroup(GetRandomBallsGroup(), 0);
            PutNewGroup(GetRandomBallsGroup(), 1);
            PutNewGroup(GetRandomBallsGroup(), 2);
            if (save)
            {
                try
                {
                    List<Point> points = new List<Point>();
                    string[] datas = File.ReadAllLines("HexagonClearSave.ini");//.Select(p=>p.Replace("\n","").Replace("\r","")).ToArray();
                    Score = int.Parse(datas[0]);
                    for (int i = 1; i < datas.Length; i++)
                    {
                        string[] point = datas[i].Split(',');
                        if (point.Length != 2)
                        {
                            ShowError("保存的进度读取失败！");
                            Judge();
                            return;
                        }
                        points.Add(new Point(int.Parse(point[0]), int.Parse(point[1])));
                    }

                    foreach (var ball in ballsBoard)
                    {
                        if (points.Any(p => p.X == ball.X && p.Y == ball.Y))
                        {
                            ball.Status = BallStatus.Occupied;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowException("读取保存的进度失败！", ex);
                }
            }
            Judge();

        }
        /// <summary>
        /// 初始化“棋盘”
        /// </summary>
        private void InitializeBoard()
        {
            for (int x = -2 * Length; x <= 2 * Length; x++)
            {
                for (int y = -Length; y <= Length; y++)
                {
                    if ((x + y) % 2 == 0 && Math.Abs(x) + Math.Abs(y) <= Length * 2 - 2 && Math.Abs(y) < Length)
                    {
                        Draw(x, y);
                    }
                }
            }


        }
        /// <summary>
        /// 根据纵横坐标进行绘制
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void Draw(int x, int y)
        {
            UcBall ball = new UcBall(x, y);

            if (!horizontalLine.ContainsKey(y))
            {
                horizontalLine.Add(y, new List<UcBall>());
            }
            horizontalLine[y].Add(ball);

            if (!leftObliqueLine.ContainsKey(x - y))
            {
                leftObliqueLine.Add(x - y, new List<UcBall>());
            }
            leftObliqueLine[x - y].Add(ball);

            if (!rightObliqueLine.ContainsKey(x + y))
            {
                rightObliqueLine.Add(x + y, new List<UcBall>());
            }
            rightObliqueLine[x + y].Add(ball);

            Draw(ball, cvs);
        }
        /// <summary>
        /// 提供球实例进行绘制
        /// </summary>
        /// <param name="ball"></param>
        /// <param name="canvas"></param>
        /// <param name="center"></param>
        private void Draw(UcBall ball, Canvas canvas, Point? center = null)
        {
            Point point;
            if (center == null)
            {
                point = GetActualLocation(new Point(ball.X, ball.Y));
                ballsBoard.Add(ball);

            }
            else
            {
                point = GetActualLocation(new Point(ball.X, ball.Y), center.Value);

            }
            Canvas.SetLeft(ball, point.X);
            Canvas.SetTop(ball, point.Y);
            canvas.Children.Add(ball);
        }

        /// <summary>
        /// 获取球的实际位置
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        private Point GetActualLocation(Point location)
        {
            return GetActualLocation(location, new Point(CenterX, CenterY));
        }
        /// <summary>
        /// 获取球的实际位置，指定中心
        /// </summary>
        /// <param name="location"></param>
        /// <param name="center"></param>
        /// <returns></returns>
        private Point GetActualLocation(Point location, Point center)
        {
            double x = location.X * D / 2 + center.X - R;
            double y = center.Y - location.Y * D * sqrt32 - R;
            return new Point(x, y);
        }
        /// <summary>
        /// 放置新的球组合
        /// </summary>
        /// <param name="balls"></param>
        /// <param name="column"></param>
        private void PutNewGroup(IEnumerable<UcBall> balls, int column)
        {
            double width = grdMain.ActualWidth / 3;
            double height = grdGroup.ActualHeight;
            Canvas cvs = new Canvas()
            {
                Opacity = 0,
                Background = new SolidColorBrush(Colors.Transparent),
                RenderTransform = new TranslateTransform(),
            };
            cvs.MouseLeftButtonDown += CanvasMouseLeftButtonDownEventHandler;
            cvs.MouseMove += CanvasMouseMoveEventHandler;
            cvs.MouseLeftButtonUp += CanvasMouseLeftButtonUpEventHandler;

            Grid.SetColumn(cvs, column);

            foreach (var ball in balls)
            {
                ball.Status = BallStatus.Occupied;
                Draw(ball, cvs, new Point(width / 2, height / 2));
            }
            grdGroup.Children.Add(cvs);
            BeginDoubleAnimation(1, cvs, new PropertyPath(OpacityProperty));
        }

        private void BeginDoubleAnimation(double To, FrameworkElement target, PropertyPath property, EventHandler complete = null)
        {
            DoubleAnimation ani = new DoubleAnimation(To, AnimationDuration);
            Storyboard.SetTarget(ani, target);
            Storyboard.SetTargetProperty(ani, property);
            Storyboard story = new Storyboard();
            story.Children.Add(ani);
            if (complete != null)
            {
                story.Completed += complete;
            }
            story.Begin();
        }

        /// <summary>
        /// 鼠标上一次的位置
        /// </summary>
        Point lastPosition = new Point();
        /// <summary>
        /// 原画板尺寸
        /// </summary>
        Size canvasSize;
        /// <summary>
        /// 正在被掌控的画板
        /// </summary>
        private Canvas clickedCanvas = null;
        /// <summary>
        /// 组合画板鼠标左键按下事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CanvasMouseLeftButtonDownEventHandler(object sender, MouseButtonEventArgs e)
        {
            Canvas currentCanvas = sender as Canvas;
            if (ClickToHold && isHold)
            {
                ReleaseGroup(currentCanvas);
                currentCanvas.Cursor = Cursors.Arrow;
                isHold = false;
                return;
            }

            if (ClickToHold)
            {
                isHold = true;
                currentCanvas.Cursor = Cursors.None;
            }

            foreach (var ball in availablePosition[currentCanvas].Intersect(ballsBoard))
            {
                ball.Status = BallStatus.Occupable;
                ball.Occupable = true;
                // ball.normalStatus = BallStatus.Occupable;
            }
            lastPosition = e.GetPosition(null);
            currentCanvas.CaptureMouse();
            canvasSize = new Size(currentCanvas.ActualWidth, currentCanvas.ActualHeight);
            currentCanvas.Opacity = 0.5;
            clickedCanvas = currentCanvas;
        }
        /// <summary>
        /// 移动组合
        /// </summary>
        /// <param name="currentCanvas"></param>
        /// <param name="currentPosition"></param>
        private void MoveGroup(Canvas currentCanvas, Point currentPosition)
        {
            //double dx = currentPosition.X - lastPosition.X + tmp.Margin.Left;
            // double dy = currentPosition.Y - lastPosition.Y + tmp.Margin.Top;
            // tmp.Margin = new Thickness(dx, dy, 0, 0);
            TranslateTransform translate = currentCanvas.RenderTransform as TranslateTransform;
            translate.X += (currentPosition.X - lastPosition.X) / multiple;
            translate.Y += (currentPosition.Y - lastPosition.Y) / multiple;
            lastPosition = currentPosition;

            Point point = GetHexagonBasedPosition(currentCanvas);
            double minPosition = double.MaxValue;
            UcBall minBall = null;
            foreach (var ballBoard in ballsBoard)
            {
                double left = Canvas.GetLeft(ballBoard);
                double top = Canvas.GetTop(ballBoard);

                if (Math.Abs(left - point.X) <= R && Math.Abs(top - point.Y) <= R)
                {
                    double position = Math.Sqrt(Math.Pow(left - point.X, 2) + Math.Pow(top - point.Y, 2));
                    if (minPosition > position)
                    {
                        minPosition = position;
                        minBall = ballBoard;
                    }
                }
            }
            if (minBall == null)
            {
                SetEmptyStatus();
            }
            else
            {
                IsOverBoardBalls(currentCanvas, minBall, BallStatus.Standby);
            }
        }
        /// <summary>
        /// 是否正在被掌控
        /// </summary>
        private bool isHold = false;
        /// <summary>
        /// 组合画板鼠标移动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CanvasMouseMoveEventHandler(object sender, MouseEventArgs e)
        {
            if (!ClickToHold && e.LeftButton != MouseButtonState.Pressed)
            {
                return;
            }
            Canvas currentCanvas = (Canvas)sender;
            if (currentCanvas != clickedCanvas)
            {
                return;
            }
            MoveGroup(currentCanvas, e.GetPosition(null));
        }
        /// <summary>
        /// 置所有未占据的球空状态
        /// </summary>
        private void SetEmptyStatus()
        {
            foreach (var ball in ballsBoard)
            {
                if (ball.Status != BallStatus.Occupied && ball.Status != (ball.Occupable ? BallStatus.Occupable : BallStatus.Empty))
                {
                    ball.Status = ball.Occupable ? BallStatus.Occupable : BallStatus.Empty;
                }
            }
        }
        /// <summary>
        /// 获取组合位于“棋盘”上空时下方的球
        /// </summary>
        /// <param name="currentCanvas"></param>
        /// <param name="ballInBoard"></param>
        private List<UcBall> IsOverBoardBalls(Canvas currentCanvas, UcBall ballInBoard)
        {
            List<Point> points = new List<Point>(); ;
            foreach (var ball in currentCanvas.Children.Cast<UcBall>())
            {
                Point p = new Point(ballInBoard.X + ball.X - (currentCanvas.Children[0] as UcBall).X, ballInBoard.Y + ball.Y - (currentCanvas.Children[0] as UcBall).Y);
                points.Add(p);
            }
            return AreAvailable(points) ?? new List<UcBall>();

        }
        /// <summary>
        /// 处理组合位于“棋盘”上空的情况
        /// </summary>
        /// <param name="currentCanvas"></param>
        /// <param name="ballInBoard"></param>
        private void IsOverBoardBalls(Canvas currentCanvas, UcBall ballInBoard, BallStatus targetStatus)
        {

            List<UcBall> avaliableBalls = IsOverBoardBalls(currentCanvas, ballInBoard);
            if (avaliableBalls != null)
            {
                foreach (var ball in ballsBoard)
                {
                    if (avaliableBalls.Contains(ball))
                    {
                        ball.Status = targetStatus;
                    }
                    else if (ball.Status == BallStatus.Standby)
                    {
                        ball.Status = ball.Occupable ? BallStatus.Occupable : BallStatus.Empty;
                    }

                }

            }
            else
            {
                SetEmptyStatus();
            }
        }
        /// <summary>
        /// 当前可能的位置是否能够容下组合
        /// </summary>
        /// <param name="balls"></param>
        /// <returns></returns>
        private List<UcBall> AreAvailable(IEnumerable<Point> balls)
        {
            List<UcBall> avaliableBalls = new List<UcBall>();
            foreach (var ball in ballsBoard)
            {
                if (balls.Any(p => p.X == ball.X && p.Y == ball.Y && ball.Status != BallStatus.Occupied))
                {
                    avaliableBalls.Add(ball);
                }
            }
            if (avaliableBalls.Count == balls.Count())
            {
                return avaliableBalls;
            }
            return null;
        }
        /// <summary>
        /// 组合画板鼠标左键抬起事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CanvasMouseLeftButtonUpEventHandler(object sender, MouseButtonEventArgs e)
        {
            if (!ClickToHold)
            {
                ReleaseGroup(sender as Canvas);
            }
        }
        /// <summary>
        /// 释放组合
        /// </summary>
        /// <param name="currentCanvas"></param>
        private void ReleaseGroup(Canvas currentCanvas)
        {
            clickedCanvas = null;
            currentCanvas.Opacity = 1;
            currentCanvas.ReleaseMouseCapture();

            bool changed = false;

            foreach (var ball in ballsBoard)
            {
                if (ball.Status == BallStatus.Standby)
                {
                    ball.Status = BallStatus.Occupied;
                    changed = true;
                }
                else if (ball.Status == BallStatus.Occupable)
                {
                    ball.Status = BallStatus.Empty;
                }
                ball.Occupable = false;
            }
            if (changed)
            {
                int column = Grid.GetColumn(currentCanvas);
                int score = 0;

                BeginDoubleAnimation(0, currentCanvas, new PropertyPath(OpacityProperty), (p1, p2) => grdGroup.Children.Remove(currentCanvas));

                score += currentCanvas.Children.Count;
                PutNewGroup(GetRandomBallsGroup(), column);
                score += Clear();
                Score += score;

                if (!Judge(currentCanvas))
                {
                    WpfControls.Dialog.DialogHelper.ShowPrompt("您已没有可以放置组合的空间了。" + Environment.NewLine + "本局得分：" + Score + "分");
                    main.ReturnToStart();
                    //grdControl.Visibility = Visibility.Visible;
                }

            }
            else
            {

                DoubleAnimation aniX = new DoubleAnimation(0, AnimationDuration);
                aniX.EasingFunction = new CircleEase() { EasingMode = EasingMode.EaseInOut };
                Storyboard.SetTarget(aniX, currentCanvas);
                Storyboard.SetTargetProperty(aniX, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));
                DoubleAnimation aniY = new DoubleAnimation(0, AnimationDuration);
                aniY.EasingFunction = new CircleEase() { EasingMode = EasingMode.EaseInOut };
                Storyboard.SetTarget(aniY, currentCanvas);
                Storyboard.SetTargetProperty(aniY, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));
                Storyboard story = new Storyboard() { Children = { aniX, aniY }, FillBehavior = FillBehavior.Stop };
                story.Completed += (p1, p2) => currentCanvas.RenderTransform = new TranslateTransform();
                story.Begin();
            }
        }
        /// <summary>
        /// 清除同一排的球
        /// </summary>
        private int Clear()
        {
            HashSet<UcBall> clearableBalls = new HashSet<UcBall>();
            //同一  —  的，y相同
            //同一  /  的，x-y相同
            //同一  \  的，x+y相同
            foreach (var line in horizontalLine)
            {
                if (!line.Value.Any(p => p.Status != BallStatus.Occupied))
                {
                    foreach (var ball in line.Value)
                    {
                        clearableBalls.Add(ball);
                    }
                }
            }

            foreach (var line in leftObliqueLine)
            {
                if (!line.Value.Any(p => p.Status != BallStatus.Occupied))
                {
                    foreach (var ball in line.Value)
                    {
                        clearableBalls.Add(ball);
                    }
                }
            }

            foreach (var line in rightObliqueLine)
            {
                if (!line.Value.Any(p => p.Status != BallStatus.Occupied))
                {
                    foreach (var ball in line.Value)
                    {
                        clearableBalls.Add(ball);
                    }
                }
            }
            int score = 0;
            foreach (var ball in clearableBalls)
            {
                ball.Status = BallStatus.Empty;
                score++;
            }
            if (score > 0)
            {
                return 2 * (score - 3);
            }
            return 0;
        }
        /// <summary>
        /// 判断是否“无子可下”
        /// </summary>
        private bool Judge(Canvas currentCanvas = null)
        {
            availablePosition.Clear();
            HashSet<UcBall> avaliable;
            foreach (Canvas canvas in grdGroup.Children.Cast<Canvas>())
            {
                if (currentCanvas == canvas)
                {
                    continue;
                }
                avaliable = new HashSet<UcBall>();
                foreach (var ballPosition in ballsBoard)
                {
                    foreach (var ball in IsOverBoardBalls(canvas, ballPosition))
                    {
                        avaliable.Add(ball);
                    }
                }
                availablePosition.Add(canvas, avaliable);
            }
            //    Debug.WriteLine(availablePosition.Sum(p => p.Value.Count));
            if (availablePosition.Any(p => p.Value.Count > 0))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取组合画板基于主“棋盘”的位置
        /// </summary>
        /// <param name="canvas"></param>
        /// <returns></returns>
        private Point GetHexagonBasedPosition(Canvas canvas)
        {
            double x = Grid.GetColumn(canvas) * grdGroup.ColumnDefinitions[0].ActualWidth
                + (canvas.RenderTransform as TranslateTransform).X
                + canvasSize.Width / 2 + (canvas.Children[0] as UcBall).X * R - R;
            double y = (canvas.RenderTransform as TranslateTransform).Y
                + grdMain.RowDefinitions[0].ActualHeight + grdMain.RowDefinitions[1].ActualHeight
                + canvasSize.Height / 2 - (canvas.Children[0] as UcBall).Y * D * sqrt32 - R;

            //double x = Grid.GetColumn(canvas) * grdGroup.ColumnDefinitions[0].ActualWidth + canvas.Margin.Left + canvasSize.Width / 2 + (canvas.Children[0] as UcBall).X * R - R;
            //double y = canvas.Margin.Top + grdMain.RowDefinitions[0].ActualHeight + grdMain.RowDefinitions[1].ActualHeight
            //    + canvasSize.Height / 2 - (canvas.Children[0] as UcBall).Y * D * sqrt32 - R;
            return new Point(x, y);
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
                    StringBuilder str = new StringBuilder();
                    str.AppendLine(Score.ToString());
                    foreach (var ball in ballsBoard.Where(p => p.Status == BallStatus.Occupied))
                    {
                        str.AppendLine(ball.X.ToString() + "," + ball.Y.ToString());
                    }
                    try
                    {
                        File.WriteAllText("HexagonClearSave.ini", str.ToString());
                        main.ReturnToStart();
                    }
                    catch (Exception ex)
                    {
                        ShowException("进度保存失败", ex);
                    }
                    break;
                case 2:
                    break;
            }
        }
    }
}
