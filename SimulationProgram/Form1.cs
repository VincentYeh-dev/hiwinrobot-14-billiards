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

            UpdateImage();
        }

        private void UpdateImage()
        {
            // 底色。
            var img = new Image<Bgr, byte>(600, 400, new Bgr(035, 100, 035));

            // 畫球袋。
            var pocketPosition = GetPocketPosition();
            var pocket = new CircleF(pocketPosition, _pocketRadius);
            img.Draw(pocket, new Bgr(0, 0, 0), 3);

            // 畫母球。
            var cueBallPosition = GetCueBallPosition();
            var cueBall = new CircleF(cueBallPosition, _ballRadius);
            img.Draw(cueBall, new Bgr(255, 255, 255), _ballThickness);

            // 畫目標球。
            var objBallPosition = GetObjBallPosition();
            var objBall = new CircleF(objBallPosition, _ballRadius);
            img.Draw(objBall, new Bgr(055, 055, 255), _ballThickness);

            // 計算假母球位置。
            var ghostPosition = CollisionPathPlanningHandler.GetGhostCueBallPosition(
                new Ball(BallType.NumberedBall, objBallPosition), new Pocket(PocketType.Side, pocketPosition));

            // 計算夾角。
            var angle = CollisionPathPlanningHandler.GetAngle(
                new Ball(BallType.CueBall, cueBallPosition), ghostPosition, new Pocket(PocketType.Side, pocketPosition));
            labelAttackAngle.Text = angle.ToString();

            // 判斷路徑是否可行。
            var isPassible = CollisionPathPlanningHandler.IsPossiblePath(
                new Ball(BallType.CueBall, cueBallPosition), ghostPosition, new Pocket(PocketType.Side, pocketPosition));

            if (!isPassible)
            {
                // 若路徑不可行，畫十字。
                var ghostCross = new Cross2DF(ghostPosition, _ballRadius, _ballRadius);
                img.Draw(ghostCross, new Bgr(0, 0, 255), 2);
            }

            // 畫假母球。
            var ghostBall = new CircleF(ghostPosition, _ballRadius);
            img.Draw(ghostBall, new Bgr(055, 055, 055), _ballThickness);

            // 畫球袋到假母球的線。
            var l1 = new LineSegment2DF(pocketPosition, ghostPosition);
            img.Draw(l1, new Bgr(155, 155, 155), 1);

            // 畫母球到假母球的線。
            var l2 = new LineSegment2DF(cueBallPosition, ghostPosition);
            img.Draw(l2, new Bgr(5, 5, 155), 1);

            pictureBoxMain.Image = img.ToBitmap();
        }

        private PointF GetPocketPosition()
        {
            PointF positon;
            switch (numericUpDownPocketType.Value)
            {
                case 0:
                    positon = new PointF(0, 0);
                    break;

                case 1:
                    positon = new PointF(300, 0);
                    break;

                case 2:
                    positon = new PointF(600, 0);
                    break;

                case 3:
                    positon = new PointF(0, 400);
                    break;

                case 4:
                    positon = new PointF(300, 400);
                    break;

                default:
                case 5:
                    positon = new PointF(600, 400);
                    break;
            }
            return positon;
        }

        private PointF GetCueBallPosition()
        {
            return new PointF((float)numericUpDownCueBallX.Value, (float)numericUpDownCueBallY.Value);
        }

        private PointF GetObjBallPosition()
        {
            return new PointF((float)numericUpDownObjBallX.Value, (float)numericUpDownObjBallY.Value);
        }

        private void numericUpDownCueBallX_ValueChanged(object sender, EventArgs e)
        {
            UpdateImage();
        }

        private void numericUpDownCueBallY_ValueChanged(object sender, EventArgs e)
        {
            UpdateImage();
        }

        private void numericUpDownObjBallX_ValueChanged(object sender, EventArgs e)
        {
            UpdateImage();
        }

        private void numericUpDownObjBallY_ValueChanged(object sender, EventArgs e)
        {
            UpdateImage();
        }

        private void pictureBoxMain_MouseDown(object sender, MouseEventArgs e)
        {
            UpdateBallPositionByMouse(e);
        }

        private void pictureBoxMain_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                UpdateBallPositionByMouse(e);
            }
        }

        private void UpdateBallPositionByMouse(MouseEventArgs e)
        {
            // 取得滑鼠座標。
            var mousePosition = e.Location;

            if (radioButtonCueBall.Checked)
            {
                numericUpDownCueBallX.Value = mousePosition.X;
                numericUpDownCueBallY.Value = mousePosition.Y;
            }
            else if (radioButtonObjBall.Checked)
            {
                numericUpDownObjBallX.Value = mousePosition.X;
                numericUpDownObjBallY.Value = mousePosition.Y;
            }
            UpdateImage();
        }
    }
}