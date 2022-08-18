using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExclusiveProgram
{
    /// <summary>
    /// 球袋種類。
    /// </summary>
    public enum PocketType
    {
        /// <summary>
        /// 邊。
        /// </summary>
        Side,

        /// <summary>
        /// 角落。
        /// </summary>
        Corner
    }

    /// <summary>
    /// 球袋。
    /// </summary>
    public class Pocket
    {
        public readonly int Id;

        /// <summary>
        /// 種類。
        /// </summary>
        public readonly PocketType Type;

        /// <summary>
        /// 位置。
        /// </summary>
        public readonly PointF Position;

        /// <summary>
        /// 球袋。
        /// </summary>
        /// <param name="type">種類。</param>
        /// <param name="position">位置。</param>
        public Pocket(PointF position, PocketType type, int id)
        {
            Type = type;
            Position = position;
            Id = id;
        }
    }
}