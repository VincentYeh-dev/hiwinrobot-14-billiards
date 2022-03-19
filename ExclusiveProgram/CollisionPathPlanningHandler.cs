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
        /// <summary>
        /// 球的半徑（單位：mm）。
        /// </summary>
        private static readonly double _ballRadius = (37.85 / 2);

        public static bool Calculate(List<Ball> allTheBalls, Ball objectBall, Pocket pocket)
        {
            var isLegalPath = false;
            var cueBall = FindCueBall(allTheBalls);

            throw new NotImplementedException();

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
        /// 取得假母球的位置。
        /// </summary>
        /// <param name="objectBall">目標球。</param>
        /// <param name="goalPocket">目標球袋。</param>
        /// <returns>假母球的位置。</returns>
        private static PointF GetGhostCueBallPosition(Ball objectBall, Pocket goalPocket)
        {
            var m = GetSlope(objectBall.Position, goalPocket.Position);
            var angle = ConvertSlopToAngle(m);

            var offsetX = GetProjectionDistanceX(angle, 2 * _ballRadius);
            var offsetY = GetProjectionDistanceY(angle, 2 * _ballRadius);

            return new PointF(objectBall.Position.X - (float)offsetX,
                              objectBall.Position.Y - (float)offsetY);
        }

        /// <summary>
        /// 取得假母球與母球之間的夾角。
        /// </summary>
        /// <param name="cueBall">母球。</param>
        /// <param name="ghostCueBallPosition">假母球位置。</param>
        /// <returns>夾角。</returns>
        private static double GetGhostCueBallAngle(Ball cueBall, PointF ghostCueBallPosition)
        {
            var m = GetSlope(cueBall.Position, ghostCueBallPosition);
            return ConvertSlopToAngle(m);
        }

        // TODO
        private static bool IsPossibleGhostCueBallPosition(Ball cueBall, PointF ghostCueBallPosition)
        {
            throw new NotImplementedException();
        }

        #region Basic Math Functions

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

        /// <summary>
        /// 取得斜率。
        /// </summary>
        /// <param name="a">點 A。</param>
        /// <param name="b">點 B。</param>
        /// <returns>斜率。</returns>
        private static double GetSlope(PointF a, PointF b)
        {
            return (b.Y - a.Y) / (b.X - a.X);
        }

        /// <summary>
        /// 從斜率取得角度。
        /// </summary>
        /// <param name="slope">斜率。</param>
        /// <returns>角度。</returns>
        private static double ConvertSlopToAngle(double slope)
        {
            return Math.Atan(slope);
        }

        /// <summary>
        /// 取得 X 軸上的投影長度，即鄰邊長度。
        /// </summary>
        /// <param name="angle">角度。</param>
        /// <param name="hypotenuse">斜邊長度。</param>
        /// <returns>X 軸上的投影長度。</returns>
        private static double GetProjectionDistanceX(double angle, double hypotenuse)
        {
            return Math.Cos(angle) * hypotenuse;
        }

        /// <summary>
        /// 取得 Y 軸上的投影長度，即對邊長度。
        /// </summary>
        /// <param name="angle">角度。</param>
        /// <param name="hypotenuse">斜邊長度。</param>
        /// <returns>Y 軸上的投影長度。</returns>
        private static double GetProjectionDistanceY(double angle, double hypotenuse)
        {
            return Math.Sin(angle) * hypotenuse;
        }

        #endregion Basic Math Functions
    }
}