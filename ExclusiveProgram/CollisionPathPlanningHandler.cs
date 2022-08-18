using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace ExclusiveProgram
{
    /// <summary>
    /// 碰撞與路徑規劃處理器。
    /// </summary>
    public class CollisionPathPlanningHandler
    {
        /// <summary>
        /// 球的半徑。
        /// </summary>
        private static readonly double _ballRadius = 87.0;

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

        public static Image<Bgr, byte> Drawing(Image<Bgr, byte> image, Ball objectBall, Ball cueBall, PointF ghostCueBallPosition, Pocket pocket)
        {
            var previewImage = image.Clone();

            CvInvoke.Circle(previewImage, Point.Round(ghostCueBallPosition), (int)cueBall.Radius, new MCvScalar(255, 255, 255), 5);
            CvInvoke.Circle(previewImage, Point.Round(pocket.Position), (int)cueBall.Radius * 2, new MCvScalar(0, 0, 0), 10);
            CvInvoke.Circle(previewImage, Point.Round(cueBall.Position), (int)cueBall.Radius, new MCvScalar(0, 110, 255), 8);
            CvInvoke.Circle(previewImage, Point.Round(objectBall.Position), (int)objectBall.Radius, new MCvScalar(10, 10, 255), 8);
            CvInvoke.Line(previewImage, Point.Round(cueBall.Position), Point.Round(ghostCueBallPosition), new MCvScalar(0, 0, 255), 3);
            CvInvoke.Line(previewImage, Point.Round(objectBall.Position), Point.Round(pocket.Position), new MCvScalar(220, 220, 220), 3);

            return previewImage;
        }

        public static double FindMostPossiblePath(List<Ball> allTheBalls,
                                                  List<Pocket> pockets,
                                                  out Pocket goalPocket,
                                                  out Ball objectBall,
                                                  out Ball cueBall,
                                                  out PointF ghostCueBallPosition,
                                                  out double angleDeg,
                                                  Image<Bgr, byte> image = null)
        {
            var maxScore = -1.0;
            goalPocket = null;
            objectBall = null;
            ghostCueBallPosition = new PointF();
            angleDeg = double.NaN;

            DiscreteCueBall(allTheBalls, out var numberedBalls, out cueBall);

            foreach (var pocket in pockets)
            {
                // For each balls.
                for (int i = 0; i < numberedBalls.Count; i++)
                {
                    var ob = numberedBalls[i];
                    var gcb = GetGhostCueBallPosition(ob, pocket);

                    var score = Score(cueBall, numberedBalls, i, pocket);
                    if (score > maxScore)
                    {
                        maxScore = score;

                        objectBall = ob;
                        ghostCueBallPosition = gcb;
                        goalPocket = pocket;
                    }

                    if (image != null)
                    {
                        var img = Drawing(image, ob, cueBall, gcb, pocket);
                        img.Save($"paths/[{Math.Round(score)}]{pocket.Id}-{i}.jpg");
                    }
                }
            }

            if (maxScore > 0)
            {
                angleDeg = -GetGhostCueBallAngle(cueBall, ghostCueBallPosition) * 180.0 / Math.PI;
            }

            return maxScore;
        }

        public static double Score(Ball cueBall, List<Ball> numberedBalls, int objectBallIndex, Pocket goalPocket)
        {
            var objectBall = numberedBalls[objectBallIndex];
            var ghostCueBallPosition = GetGhostCueBallPosition(objectBall, goalPocket);

            if (!IsPossiblePathAngle(cueBall.Position, ghostCueBallPosition, goalPocket.Position))
            {
                return -9; // Impossible path.
            }

            for (int i = 0; i < numberedBalls.Count; i++)
            {
                if (i == objectBallIndex)
                {
                    continue; // skip self.
                }
                var currentBall = numberedBalls[i];

                var path1 = IsPossiblePathBetweenCueBallAndGhostCueBall(cueBall.Position,
                                                                         ghostCueBallPosition,
                                                                         currentBall.Position);
                if (!path1)
                {
                    return -1;
                }

                var path2 = IsPossiblePathBetweenPackageAndGhostCueBall(ghostCueBallPosition,
                                                                       goalPocket.Position,
                                                                       currentBall.Position);
                if (!path2)
                {
                    return -2;
                }
            }

            var angle = GetAngleBetweenTwoLine(cueBall.Position,
                                               ghostCueBallPosition,
                                               objectBall.Position,
                                               goalPocket.Position);

            var distanceObjectBallToPocket = Distance(objectBall.Position, goalPocket.Position);

            double score = 0.0;
            score += Math.Abs(90 - Math.Abs(angle)) * (100.0 / 90.0) * 0.85;
            score += (3721.0 - distanceObjectBallToPocket) * (100.0 / 3271.0) * 0.15;
            return score;
        }

        public static void DiscreteCueBall(List<Ball> allTheBalls, out List<Ball> numberedBalls, out Ball cueBall)
        {
            cueBall = null;
            numberedBalls = new List<Ball>();

            foreach (var b in allTheBalls)
            {
                if (b.IsCueBall)
                {
                    cueBall = b;
                }
                else
                {
                    numberedBalls.Add(b);
                }
            }
        }

        /// <summary>
        /// 判斷是否爲可行的路徑角度。
        /// </summary>
        /// <param name="cueBallPosition">母球位置。</param>
        /// <param name="ghostCueBallPosition">假母球位置。</param>
        /// <param name="pocketPosition">球袋位置。</param>
        /// <returns>是否可行。</returns>
        public static bool IsPossiblePathAngle(PointF cueBallPosition, PointF ghostCueBallPosition, PointF pocketPosition)
        {
            var isPossible = false;

            // Pocket, GhostCue, Cue, GhostCue.
            var angle = GetAngleBetweenTwoLine(pocketPosition, ghostCueBallPosition, cueBallPosition, ghostCueBallPosition);

            if (pocketPosition.Y <= 100)
            {
                if (Math.Abs(angle) <= 95)
                {
                    isPossible = false;
                }
                else
                {
                    isPossible = true;
                }
            }
            else if (pocketPosition.Y > 300)
            {
                if (Math.Abs(angle) >= 85)
                {
                    isPossible = false;
                }
                else
                {
                    isPossible = true;
                }
            }
            else
            {
                isPossible = true;
            }

            return isPossible;
        }

        /// <summary>
        /// 球袋-假母球路徑阻擋判斷。
        /// </summary>
        /// <param name="ghostCueBallPosition"></param>
        /// <param name="pocketPosition"></param>
        /// <param name="objectBallPosition2"></param>
        ///
        /// <returns></returns>
        public static bool IsPossiblePathBetweenPackageAndGhostCueBall(PointF ghostCueBallPosition, PointF pocketPosition, PointF objectBallPosition2)
        {
            var isPossible_pg = false;
            /////////球袋-假母球////////////////
            if (Focus_j(objectBallPosition2, ghostCueBallPosition, pocketPosition) > 0 || Focus_j(objectBallPosition2, ghostCueBallPosition, pocketPosition) == 0)
            {
                if (pocketPosition.X > ghostCueBallPosition.X)
                {
                    if (Focus_x(objectBallPosition2, ghostCueBallPosition, pocketPosition) > ghostCueBallPosition.X)
                    {
                        isPossible_pg = false;
                    }
                    else
                    {
                        isPossible_pg = true;
                    }
                }
                else if (pocketPosition.X < ghostCueBallPosition.X)
                {
                    if (Focus_x(objectBallPosition2, ghostCueBallPosition, pocketPosition) < ghostCueBallPosition.X)
                    {
                        isPossible_pg = false;
                    }
                    else
                    {
                        isPossible_pg = true;
                    }
                }
            }
            else if (Focus_j(objectBallPosition2, ghostCueBallPosition, pocketPosition) < 0)
            {
                isPossible_pg = true;
            }
            ///////////////////////////////////

            return isPossible_pg;
        }

        /// <summary>
        /// 假母球-母球路徑阻擋判斷。
        /// </summary>
        /// <param name="cueBallPosition"></param>
        /// <param name="ghostCueBallPosition"></param>
        /// <param name="objectBallPosition2"></param>
        ///
        /// <returns></returns>
        public static bool IsPossiblePathBetweenCueBallAndGhostCueBall(PointF cueBallPosition, PointF ghostCueBallPosition, PointF objectBallPosition2)
        {
            var isPossible_gc = false;
            ////////////假母球-母球////////////////
            if (Focus_j(objectBallPosition2, ghostCueBallPosition, cueBallPosition) > 0 || Focus_j(objectBallPosition2, ghostCueBallPosition, cueBallPosition) == 0)
            {
                if (cueBallPosition.X > ghostCueBallPosition.X)
                {
                    if (Focus_x(objectBallPosition2, ghostCueBallPosition, cueBallPosition) < cueBallPosition.X && Focus_x(objectBallPosition2, ghostCueBallPosition, cueBallPosition) > ghostCueBallPosition.X)
                    {
                        isPossible_gc = false;
                    }
                    else
                    {
                        isPossible_gc = true;
                    }
                }
                else if (cueBallPosition.X < ghostCueBallPosition.X)
                {
                    if (Focus_x(objectBallPosition2, ghostCueBallPosition, cueBallPosition) > cueBallPosition.X && Focus_x(objectBallPosition2, ghostCueBallPosition, cueBallPosition) < ghostCueBallPosition.X)
                    {
                        isPossible_gc = false;
                    }
                    else
                    {
                        isPossible_gc = true;
                    }
                }
            }
            else if (Focus_j(objectBallPosition2, ghostCueBallPosition, cueBallPosition) < 0)
            {
                isPossible_gc = true;
            }
            else
            {
                isPossible_gc = true;
            }
            ///////////////////////////////////

            return isPossible_gc;
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

        //////////座標距離運算/////////////////
        public static double Distance(PointF a1, PointF a2)
        {
            var D = Math.Pow((Math.Pow((a1.X - a2.X), 2) + Math.Pow((a1.Y - a2.Y), 2)), 0.5);
            return D;
        }

        /////////////路徑評分///////////////////////////////////////
        public static double Score(PointF objectBallPosition, PointF objectBallPosition2,
                                                              PointF objectBallPosition3,
                                                              PointF objectBallPosition4,
                                                              PointF CueBallPosition, PointF pocketPosition, PointF ghostCueBallPosition)
        {
            var Score = 0;
            var D_op = Distance(objectBallPosition, pocketPosition);
            var D_co = Distance(CueBallPosition, objectBallPosition);
            var isPossible = IsPossiblePathAngle(CueBallPosition, ghostCueBallPosition, pocketPosition);
            var isPossible_pg = IsPossiblePathBetweenPackageAndGhostCueBall(ghostCueBallPosition, pocketPosition, objectBallPosition2);
            var isPossible_gc = IsPossiblePathBetweenCueBallAndGhostCueBall(CueBallPosition, ghostCueBallPosition, objectBallPosition2);
            var isPossible3 = IsPossiblePathAngle(CueBallPosition, ghostCueBallPosition, pocketPosition);
            var isPossible_pg3 = IsPossiblePathBetweenPackageAndGhostCueBall(ghostCueBallPosition, pocketPosition, objectBallPosition3);
            var isPossible_gc3 = IsPossiblePathBetweenCueBallAndGhostCueBall(CueBallPosition, ghostCueBallPosition, objectBallPosition3);
            var isPossible4 = IsPossiblePathAngle(CueBallPosition, ghostCueBallPosition, pocketPosition);
            var isPossible_pg4 = IsPossiblePathBetweenPackageAndGhostCueBall(ghostCueBallPosition, pocketPosition, objectBallPosition4);
            var isPossible_gc4 = IsPossiblePathBetweenCueBallAndGhostCueBall(CueBallPosition, ghostCueBallPosition, objectBallPosition4);

            if (isPossible == false || isPossible_gc == false || isPossible_pg == false
                || isPossible3 == false || isPossible_gc3 == false || isPossible_pg3 == false
                || isPossible4 == false || isPossible_gc4 == false || isPossible_pg4 == false)
            {
                Score = 0;
            }
            else
            {
                Score = (int)(600 - D_op);
            }

            return Score;
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

        ////圓方程式//////////
        private static double CircleEquation(PointF objectBallPosition2, PointF unknown)
        {
            var CE = Math.Pow((unknown.X - objectBallPosition2.X), 2) + Math.Pow((unknown.Y - objectBallPosition2.Y), 2) - Math.Pow((37.85 * 3 / 2), 2);
            return CE;
        }

        /////////直線方程式////////
        private static double LineEquation_a(PointF a1, PointF a2)
        {
            var LEa = (a1.Y - a2.Y) / (a1.X - a2.X);
            var LEb = (a1.Y) - (a1.X * LEa);
            return LEa;
        }

        private static double LineEquation_b(PointF a1, PointF a2)
        {
            var LEa = (a1.Y - a2.Y) / (a1.X - a2.X);
            var LEb = (a1.Y) - (a1.X * LEa);
            return LEb;
        }

        private static double LineEquation(PointF ghostCueBallPosition, PointF pocketPosition, PointF unknown)
        {
            var LE_a = LineEquation_a(ghostCueBallPosition, pocketPosition);
            var LE_b = LineEquation_b(ghostCueBallPosition, pocketPosition);
            var LE_Q = (unknown.X * LE_a) - unknown.Y + LE_b;
            return LE_Q;
        }

        //////////判斷焦點/////////////////
        private static double Focus_j(PointF objectBallPosition2, PointF ghostCueBallPosition, PointF pocketPosition)
        {
            var LE_a = LineEquation_a(ghostCueBallPosition, pocketPosition);
            var LE_b = LineEquation_b(ghostCueBallPosition, pocketPosition);
            var F_a = 1 + Math.Pow(LE_a, 2);
            var F_b = (-2 * objectBallPosition2.X) + (2 * LE_a * LE_b) - (2 * LE_a * objectBallPosition2.Y);
            var F_c = Math.Pow(objectBallPosition2.X, 2) + Math.Pow(LE_b, 2) - (2 * LE_b * objectBallPosition2.Y) + Math.Pow(objectBallPosition2.Y, 2) - Math.Pow(((37.85 * 2.5) / 2), 2);
            var F_Q = Math.Pow(F_b, 2) - (4 * F_a * F_c);
            return F_Q;
        }

        private static double Focus_x(PointF objectBallPosition2, PointF ghostCueBallPosition, PointF pocketPosition)
        {
            var LE_a = LineEquation_a(ghostCueBallPosition, pocketPosition);
            var LE_b = LineEquation_b(ghostCueBallPosition, pocketPosition);
            var F_a = 1 + Math.Pow(LE_a, 2);
            var F_b = (-2 * objectBallPosition2.X) + (2 * LE_a * LE_b) - (2 * LE_a * objectBallPosition2.Y);
            var F_c = Math.Pow(objectBallPosition2.X, 2) + Math.Pow(LE_b, 2) - (2 * LE_b * objectBallPosition2.Y) + Math.Pow(objectBallPosition2.Y, 2) - Math.Pow(((37.85 * 3) / 2), 2);
            var F_Q = (-F_b + Math.Pow(Math.Pow(F_b, 2) - (4 * F_a * F_c), 0.5)) / (2 * F_a);
            return F_Q;
        }

        private static double Focus_y(PointF objectBallPosition2, PointF ghostCueBallPosition, PointF pocketPosition)
        {
            var LE_a = LineEquation_a(ghostCueBallPosition, pocketPosition);
            var LE_b = LineEquation_b(ghostCueBallPosition, pocketPosition);
            var F_a = 1 + Math.Pow(LE_a, 2);
            var F_b = (-2 * objectBallPosition2.X) + (2 * LE_a * LE_b) - (2 * LE_a * objectBallPosition2.Y);
            var F_c = Math.Pow(objectBallPosition2.X, 2) + Math.Pow(LE_b, 2) - (2 * LE_b * objectBallPosition2.Y) + Math.Pow(objectBallPosition2.Y, 2) - Math.Pow(((37.85 * 3) / 2), 2);
            var F_Q_x = (-F_b + Math.Pow(Math.Pow(F_b, 2) - (4 * F_a * F_c), 0.5)) / (2 * F_a);
            var F_Q = (LE_a * F_Q_x) + LE_b;
            return F_Q;
        }

        ////////////////////////////////////////////////////////////

        #endregion Basic Math Functions
    }
}