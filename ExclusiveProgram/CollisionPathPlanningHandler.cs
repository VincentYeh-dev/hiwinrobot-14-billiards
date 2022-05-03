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

        // TODO
        public static bool Calculate(List<Ball> allTheBalls, Ball objectBall, Pocket pocket)
        {
            var isLegalPath = false;
            var cueBall = FindCueBall(allTheBalls);

            throw new NotImplementedException();

            return isLegalPath;
        }

        /// <summary>
        /// 取得假母球的位置。
        /// </summary>
        /// <param name="objectBall">目標球。</param>
        /// <param name="goalPocket">球袋。</param>
        /// <returns>假母球的位置。</returns>
        public static PointF GetGhostCueBallPosition(Ball objectBall, Pocket goalPocket)
        {
            return GetGhostCueBallPosition(objectBall.Position, goalPocket.Position);
        }

        /// <summary>
        /// 取得假母球的位置。
        /// </summary>
        /// <param name="objectBallPosition">目標球位置。</param>
        /// <param name="pocketPosition">球袋位置。</param>
        /// <returns>假母球的位置。</returns>
        public static PointF GetGhostCueBallPosition(PointF objectBallPosition, PointF pocketPosition)
        {
            // 計算斜率、夾角。
            var m = GetSlope(objectBallPosition, pocketPosition);
            var angle = ConvertSlopToAngle(m);

            // 以夾角和斜邊長（球的直徑）計算鄰邊長（X）和對邊長（Y）。
            var offsetX = GetProjectionDistanceX(angle, 2 * _ballRadius);
            var offsetY = GetProjectionDistanceY(angle, 2 * _ballRadius);

            // 將目標球位置向後延伸一個球的直徑即爲假母球位置。
            var position = new PointF();
            if (pocketPosition.X < objectBallPosition.X)
            {
                position.X = objectBallPosition.X + (float)offsetX;
                position.Y = objectBallPosition.Y + (float)offsetY;
            }
            else
            {
                position.X = objectBallPosition.X - (float)offsetX;
                position.Y = objectBallPosition.Y - (float)offsetY;
            }

            return position;
        }

        /// <summary>
        /// 取得母球-假母球與假母球-球袋兩線之間的夾角。
        /// </summary>
        /// <param name="cueBall">母球。</param>
        /// <param name="ghostCueBallPosition">假母球位置。</param>
        /// <param name="pocket">球袋。</param>
        /// <returns>夾角。</returns>
        public static double GetAngle(Ball cueBall, PointF ghostCueBallPosition, Pocket pocket)
        {
            return GetAngle(cueBall.Position, ghostCueBallPosition, pocket.Position);
        }

        /// <summary>
        /// 取得母球-假母球與假母球-球袋兩線之間的夾角。
        /// </summary>
        /// <param name="cueBallPosition">母球位置。</param>
        /// <param name="ghostCueBallPosition">假母球位置。</param>
        /// <param name="pocketPosition">球袋位置。</param>
        /// <returns>夾角。</returns>
        public static double GetAngle(PointF cueBallPosition, PointF ghostCueBallPosition, PointF pocketPosition)
        {
            // Pocket, GhostCue, Cue, GhostCue.
            var angle = GetAngleBetweenTwoLine(pocketPosition, ghostCueBallPosition, cueBallPosition, ghostCueBallPosition);
            return angle;
        }

        /// <summary>
        /// 判斷是否爲可行的路徑。
        /// </summary>
        /// <param name="cueBall">母球。</param>
        /// <param name="ghostCueBallPosition">假母球位置。</param>
        /// <param name="pocket">球袋。</param>
        /// <returns>是否可行。</returns>
        public static bool IsPossiblePath(Ball cueBall, PointF ghostCueBallPosition, Pocket pocket)
        {
            return IsPossiblePath(cueBall.Position, ghostCueBallPosition, pocket.Position);
        }

        // TODO
        /// <summary>
        /// 判斷是否爲可行的路徑。
        /// </summary>
        /// <param name="cueBallPosition">母球位置。</param>
        /// <param name="ghostCueBallPosition">假母球位置。</param>
        /// <param name="pocketPosition">球袋位置。</param>
        /// <returns>是否可行。</returns>
        public static bool IsPossiblePath(PointF cueBallPosition, PointF ghostCueBallPosition, PointF pocketPosition)
        {
            var isPossible = false;

            // Pocket, GhostCue, Cue, GhostCue.
            var angle = GetAngleBetweenTwoLine(pocketPosition, ghostCueBallPosition, cueBallPosition, ghostCueBallPosition);

            if (Math.Abs(angle) >= 90)
            {
                isPossible = false;
            }
            else
            {
                isPossible = true;
            }

            return isPossible;
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

        private static double GetAngleBetweenTwoLine(PointF a1, PointF a2, PointF b1, PointF b2)
        {
            // 算斜率。
            var mA = (a2.Y - a1.Y) / (a2.X - a1.X);
            var mB = (b2.Y - b1.Y) / (b2.X - b1.X);

            var angleA = ConvertSlopToAngle(mA);
            var angleB = ConvertSlopToAngle(mB);

            angleA = ConvertRadToDegree(angleA);
            angleB = ConvertRadToDegree(angleB);

            double angle;
            if (mA > 0)
            {
                angle = angleB - angleA;

                if (b1.X > b2.X)
                {
                    angle = 180 + angle;
                }

                if (b1.X > b2.X && b1.Y > b2.Y)
                {
                    if (mB > mA)
                    {
                        angle -= 360;
                    }
                }
            }
            else
            {
                angle = angleA - angleB;

                if (b1.X < b2.X)
                {
                    angle = 180 + angle;
                }

                if (b1.X < b2.X && b1.Y > b2.Y)
                {
                    if (mB < mA)
                    {
                        angle -= 360;
                    }
                }
            }

            return angle;
        }

        private static double ConvertDegreeToRad(double degraa)
        {
            return degraa * (Math.PI / 180.0);
        }

        private static double ConvertRadToDegree(double rad)
        {
            return rad * (180.0 / Math.PI);
        }

        #endregion Basic Math Functions
    }
}