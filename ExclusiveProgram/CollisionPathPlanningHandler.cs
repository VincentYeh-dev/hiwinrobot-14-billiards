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
    }
}