using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExclusiveProgram
{
    /// <summary>
    /// 球的種類。
    /// </summary>
    public enum BallType
    {
        /// <summary>
        /// 母球。
        /// </summary>
        CueBall = 0,

        /// <summary>
        /// 號球。
        /// </summary>
        NumberedBall
    }

    /// <summary>
    /// 球。
    /// </summary>
    public class Ball
    {

        public readonly int ID;
        public readonly double Radius;
        /// <summary>
        /// 種類。
        /// </summary>
        public readonly BallType Type;

        /// <summary>
        /// 位置。
        /// </summary>
        public readonly PointF Position;

        /// <summary>
        /// 球。
        /// </summary>
        /// <param name="type">種類。</param>
        /// <param name="position">位置。</param>
        public Ball(int id,BallType type, PointF position,double radius)
        {
            this.ID = id;
            Type = type;
            Position = position;
            Radius = radius;
        }

        /// <summary>
        /// 是母球。
        /// </summary>
        public bool IsCueBall { get => Type == BallType.CueBall; }
    }
}