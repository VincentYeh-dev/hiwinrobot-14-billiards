using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Emgu.Util;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.Structure;
using ExclusiveProgram;

namespace SimulationProgram
{
    public partial class Form1 : Form
    {
        private const int _ballThickness = 5;
        private const float _ballRadius = (float)((37.85 / 2) - (_ballThickness / 2));
        private const float _pocketRadius = 20;

        public Form1()
        {
            InitializeComponent();
        }

        private void UpdateImage()
        {
            // 底色。
            var img = new Image<Bgr, byte>(600, 400, new Bgr(035, 100, 035));

            var cueBallPosition = new PointF(50, 200);
            var cueBall = new CircleF(cueBallPosition, _ballRadius);
            img.Draw(cueBall, new Bgr(255, 255, 255), _ballThickness);

            var objBallPosition = new PointF(230, 140);
            var objBall = new CircleF(objBallPosition, _ballRadius);
            img.Draw(objBall, new Bgr(055, 055, 255), _ballThickness);

            var pocketPosition = new PointF(300, 0);
            var pocket = new CircleF(pocketPosition, _pocketRadius);
            img.Draw(pocket, new Bgr(0, 0, 0), 3);

            var ghostPosition = CollisionPathPlanningHandler.GetGhostCueBallPosition(
                new Ball(BallType.NumberedBall, objBallPosition), new Pocket(PocketType.Side, pocketPosition));

            var ghostBall = new CircleF(ghostPosition, _ballRadius);
            img.Draw(ghostBall, new Bgr(055, 055, 055), _ballThickness);

            var l1 = new LineSegment2DF(pocketPosition, ghostPosition);
            img.Draw(l1, new Bgr(155, 155, 155), 1);

            var l2 = new LineSegment2DF(cueBallPosition, ghostPosition);
            img.Draw(l2, new Bgr(5, 5, 155), 1);

            pictureBoxMain.Image = img.ToBitmap();
        }
    }
}