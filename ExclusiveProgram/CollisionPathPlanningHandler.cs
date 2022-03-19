using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExclusiveProgram
{
    /// <summary>
    /// 碰撞與路徑規劃處理器。
    /// </summary>
    public class CollisionPathPlanningHandler
    {
        public static bool Calculate(List<Ball> allTheBalls, Ball objectBall, Pocket pocket)
        {
            var isLegalPath = false;
            var cueBall = FindCueBall(allTheBalls);
            return isLegalPath;
        }

        private static Ball FindCueBall(List<Ball> balls)
        {
            foreach (var b in balls)
            {
                if (b.IsCueBall)
                {
                    return b;
                }
            }
            throw new Exception("Couldn't find any cue ball.");
        }

        /// <summary>
        /// 取得兩點之間的距離。
        /// </summary>
        /// <param name="a">點 A。</param>
        /// <param name="b">點 B。</param>
        /// <returns>距離。</returns>
        private static double GetDistance(PointF a, PointF b)
        {
            return Math.Abs(Math.Sqrt(Math.Pow(a.X - b.X, 2) +
                                      Math.Pow(a.Y - b.Y, 2)));
        }
    }
}