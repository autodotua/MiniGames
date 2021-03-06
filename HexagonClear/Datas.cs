﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static WPfCodes.Program.Config;

namespace HexagonClear
{
    public static class Datas
    {
        /// <summary>
        /// 半径
        /// </summary>
        public static double R => 18;
        /// <summary>
        /// 直径
        /// </summary>
        public static double D => 2 * R;

        public static int Length { get => length; set => length = value; }

        public static TimeSpan AnimationDuration = TimeSpan.FromMilliseconds(200);

        public static MainWindow main;

       public static Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        /// <summary>
        /// 边长
        /// </summary>
        private static int length = 5;
        
        /// <summary>
        /// 是否点击以后保持对组合的控制
        /// </summary>
        public static bool ClickToHold { get; set; }

        public static Cursor HoldCursor { get; set; }

        public static void InitializeConfigs()
        {
            Length = config.GetInt("Length", 5);
            ClickToHold = config.GetBool("ClickToHold",true);
            switch( config.GetInt("HoldCursor", 2))
            {
                case 0:
                    HoldCursor = Cursors.Arrow;
                    break;
                case 1:
                    HoldCursor = Cursors.Hand;
                    break;
                case 2:
                    HoldCursor = Cursors.None;
                    break;
            }
        }
    }
}
