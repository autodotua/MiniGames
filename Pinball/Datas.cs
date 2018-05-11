using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WPfCodes.Program.Config;

namespace Pinball
{
    public static class Datas
    {
        /// <summary>
        /// 半径
        /// </summary>
        public static double R => 8;
        /// <summary>
        /// 直径
        /// </summary>
        public static double D => 2 * R;

        public static double initialVelocity = 360;

        public static double blockSize = 32;

        public static double attenuation = 0.9;


        public static TimeSpan AnimationDuration = TimeSpan.FromMilliseconds(200);

        public static MainWindow main;

       public static Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        
        
        public static void InitializeConfigs()
        {
        }
    }
    
    public class Turn
    {
        private double vx;
        private double vy;

        public Turn(double vx, double vy)
        {
            Vy = vy;
            Vx = vx;
        }

        public void Add(Turn turn)
        {
            Vx *= turn.Vx;
            Vy *= turn.Vy;
        }

        public bool IsCollided()
        {
            return Vx != 1 || Vy != 1;
        }

        public void TurnBall(UcBall ball)
        {
            ball.Vx *= Vx;
            ball.Vy *= Vy;
        }

        public double Vy { get => vy; set => vy = value; }
        public double Vx { get => vx; set => vx = value; }
    }
}
