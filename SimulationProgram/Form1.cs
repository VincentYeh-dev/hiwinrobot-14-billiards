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

            ////////////////////// 畫目標球。////////////////////////////////

            var objBallPosition = GetObjBallPosition();
            var objBall = new CircleF(objBallPosition, _ballRadius);
            img.Draw(objBall, new Bgr(055, 055, 255), _ballThickness);

            var objBallPosition2 = GetObjBallPosition2();
            var objBall2 = new CircleF(objBallPosition2, _ballRadius);
            img.Draw(objBall2, new Bgr(055, 155, 255), _ballThickness);

            var objBallPosition3 = GetObjBallPosition3();
            var objBall3 = new CircleF(objBallPosition3, _ballRadius);
            img.Draw(objBall3, new Bgr(055, 255, 255), _ballThickness);

            var objBallPosition4 = GetObjBallPosition4();
            var objBall4 = new CircleF(objBallPosition4, _ballRadius);
            img.Draw(objBall4, new Bgr(055, 255, 155), _ballThickness);

            /////////////////////////////////////////////////////////////////
            // 計算假母球位置。
            var ghostPosition = CollisionPathPlanningHandler.GetGhostCueBallPosition(objBallPosition, pocketPosition);

            // 計算夾角。
            var angle = CollisionPathPlanningHandler.GetAngle(cueBallPosition, ghostPosition, pocketPosition);
            labelAttackAngle.Text = angle.ToString();
            //計算距離
            var D_co = CollisionPathPlanningHandler.Distance(cueBallPosition, objBallPosition);
            labelDistance_co.Text = D_co.ToString();
            var D_op = CollisionPathPlanningHandler.Distance(objBallPosition, pocketPosition);
            labelDistance_op.Text = D_op.ToString();
            //路徑評分
            var Score = CollisionPathPlanningHandler.Score(objBallPosition, objBallPosition2, objBallPosition3, objBallPosition4, cueBallPosition, pocketPosition, ghostPosition);
            labelScore.Text = Score.ToString();

            // 判斷路徑是否可行。
            var isPassible = CollisionPathPlanningHandler.IsPossiblePathAngle(cueBallPosition, ghostPosition, pocketPosition);
            if (!isPassible)
            {
                // 若路徑不可行，畫十字。
                var ghostCross = new Cross2DF(ghostPosition, _ballRadius, _ballRadius);
                img.Draw(ghostCross, new Bgr(0, 0, 255), 2);
            }
            //////////號碼球234////////////
            var isPassible_pg = CollisionPathPlanningHandler.IsPossiblePathBetweenPackageAndGhostCueBall(ghostPosition, pocketPosition, objBallPosition2);
            if (!isPassible_pg)
            {
                var ghostCross = new Cross2DF(ghostPosition, _ballRadius, _ballRadius);
                img.Draw(ghostCross, new Bgr(0, 0, 255), 2);
            }

            var isPassible_gc = CollisionPathPlanningHandler.IsPossiblePathBetweenCueBallAndGhostCueBall(cueBallPosition, ghostPosition, objBallPosition2);
            if (!isPassible_gc)
            {
                var ghostCross = new Cross2DF(ghostPosition, _ballRadius, _ballRadius);
                img.Draw(ghostCross, new Bgr(0, 0, 255), 2);
            }

            var isPassible_pg2 = CollisionPathPlanningHandler.IsPossiblePathBetweenPackageAndGhostCueBall(ghostPosition, pocketPosition, objBallPosition3);
            if (!isPassible_pg2)
            {
                var ghostCross = new Cross2DF(ghostPosition, _ballRadius, _ballRadius);
                img.Draw(ghostCross, new Bgr(0, 0, 255), 2);
            }

            var isPassible_gc2 = CollisionPathPlanningHandler.IsPossiblePathBetweenCueBallAndGhostCueBall(cueBallPosition, ghostPosition, objBallPosition3);
            if (!isPassible_gc2)
            {
                var ghostCross = new Cross2DF(ghostPosition, _ballRadius, _ballRadius);
                img.Draw(ghostCross, new Bgr(0, 0, 255), 2);
            }

            var isPassible_pg3 = CollisionPathPlanningHandler.IsPossiblePathBetweenPackageAndGhostCueBall(ghostPosition, pocketPosition, objBallPosition4);
            if (!isPassible_pg3)
            {
                var ghostCross = new Cross2DF(ghostPosition, _ballRadius, _ballRadius);
                img.Draw(ghostCross, new Bgr(0, 0, 255), 2);
            }

            var isPassible_gc3 = CollisionPathPlanningHandler.IsPossiblePathBetweenCueBallAndGhostCueBall(cueBallPosition, ghostPosition, objBallPosition4);
            if (!isPassible_gc3)
            {
                var ghostCross = new Cross2DF(ghostPosition, _ballRadius, _ballRadius);
                img.Draw(ghostCross, new Bgr(0, 0, 255), 2);
            }
            /////////////////////////////////
            // 畫假母球。
            var ghostBall = new CircleF(ghostPosition, _ballRadius);
            img.Draw(ghostBall, new Bgr(055, 055, 055), _ballThickness);

            //////////////////////////////////////////////////////////////////////////////

            // 畫球袋到假母球的線。
            var l1 = new LineSegment2DF(pocketPosition, ghostPosition);
            img.Draw(l1, new Bgr(155, 155, 155), 1);

            // 畫母球到假母球的線。
            var l2 = new LineSegment2DF(cueBallPosition, ghostPosition);
            img.Draw(l2, new Bgr(5, 5, 155), 1);

            pictureBoxMain.Image = img.ToBitmap();
            //////////////////////////////////////////////////////////////////////////////
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

        ///////////目標球位置。////////////////////////////////////////////////////////////////////
        private PointF GetObjBallPosition()
        {
            return new PointF((float)numericUpDownObjBallX.Value, (float)numericUpDownObjBallY.Value);
        }

        private PointF GetObjBallPosition2()
        {
            return new PointF((float)numericUpDownObjBallX2.Value, (float)numericUpDownObjBallY2.Value);
        }

        private PointF GetObjBallPosition3()
        {
            return new PointF((float)numericUpDownObjBallX3.Value, (float)numericUpDownObjBallY3.Value);
        }

        private PointF GetObjBallPosition4()
        {
            return new PointF((float)numericUpDownObjBallX4.Value, (float)numericUpDownObjBallY4.Value);
        }

        /////////////////////////////////////////////////////////////////////////////////////
        private void numericUpDownCueBallX_ValueChanged(object sender, EventArgs e)
        {
            UpdateImage();
        }

        private void numericUpDownCueBallY_ValueChanged(object sender, EventArgs e)
        {
            UpdateImage();
        }

        /////////////目標球XY。///////////////////////////////////////////////////////////////////////
        private void numericUpDownObjBallX_ValueChanged(object sender, EventArgs e)
        {
            UpdateImage();
        }

        private void numericUpDownObjBallY_ValueChanged(object sender, EventArgs e)
        {
            UpdateImage();
        }

        private void numericUpDownObjBallX2_ValueChanged(object sender, EventArgs e)
        {
            UpdateImage();
        }

        private void numericUpDownObjBallY2_ValueChanged(object sender, EventArgs e)
        {
            UpdateImage();
        }

        private void numericUpDownObjBallX3_ValueChanged(object sender, EventArgs e)
        {
            UpdateImage();
        }

        private void numericUpDownObjBallY3_ValueChanged(object sender, EventArgs e)
        {
            UpdateImage();
        }

        private void numericUpDownObjBallX4_ValueChanged(object sender, EventArgs e)
        {
            UpdateImage();
        }

        private void numericUpDownObjBallY4_ValueChanged(object sender, EventArgs e)
        {
            UpdateImage();
        }

        //////////////////////////////////////////////////////////////////////////////////////
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
            //////////目標球XY位置。//////////////////////////////////////////////////////////
            else if (radioButtonObjBall.Checked)
            {
                numericUpDownObjBallX.Value = mousePosition.X;
                numericUpDownObjBallY.Value = mousePosition.Y;
            }
            else if (radioButtonObjBall2.Checked)
            {
                numericUpDownObjBallX2.Value = mousePosition.X;
                numericUpDownObjBallY2.Value = mousePosition.Y;
            }
            else if (radioButtonObjBall3.Checked)
            {
                numericUpDownObjBallX3.Value = mousePosition.X;
                numericUpDownObjBallY3.Value = mousePosition.Y;
            }
            else if (radioButtonObjBall4.Checked)
            {
                numericUpDownObjBallX4.Value = mousePosition.X;
                numericUpDownObjBallY4.Value = mousePosition.Y;
            }
            /////////////////////////////////////////////////////////////////////
            UpdateImage();
        }
    }
}