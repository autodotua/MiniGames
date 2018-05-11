using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinball
{
    public interface IBlock
    {
        double X { get; }
        double Y { get; }

        int Count { get; set; }


        Turn CheckCollision(UcBall ball);
    }
}
