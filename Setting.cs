using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snake_game_0._1
{
    internal class Setting
    {
        public static int height;
        public static int width;
        public static string direction;
        public Setting()
        {
            height = 16;
            width = 16;
            direction = "left";
        }
    }
}
