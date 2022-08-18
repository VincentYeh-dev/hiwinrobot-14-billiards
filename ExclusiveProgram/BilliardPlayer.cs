using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Hiwin = RASDK.Arm.Hiwin;
using RASDK.Arm;
using RASDK.Arm.Type;
using RASDK.Basic;
using RASDK.Basic.Message;
using RASDK.Vision;
using RASDK.Vision.IDS;
using RASDK.Vision.Positioning;
using ExclusiveProgram.puzzle.visual.concrete;
using ExclusiveProgram.puzzle.visual.concrete.utils;
using System.Drawing;
using MotionParam = RASDK.Arm.AdditionalMotionParameters;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;

namespace ExclusiveProgram
{
    public class BilliardPlayer
    {
        #region Position

        /// <summary>
        /// 手臂擊球位置-高。
        /// </summary>
        private readonly double _zUpper = 200;

        /// <summary>
        /// 手臂擊球位置-低。
        /// </summary>
        private readonly double _zLower = 50;

        /// <summary>
        /// 預備位置1.
        /// </summary>
        private double[] _standByPositionJoint1 => new double[] { -45, 0, 0, 0, -90, 0 };

        /// <summary>
        /// 預備位置2.
        /// </summary>
        private double[] _standByPositionJoint2 => new double[] { -90, 0, 0, 0, -90, 0 };

        /// <summary>
        /// 拍照的位置。笛卡爾座標。
        /// </summary>
        private double[] _takePiecurePosition => new double[] { 0, 368, 294, 180, 0, 90 };

        #endregion Position

        private readonly RoboticArm _arm;
        private readonly IDSCamera _camera;
        private readonly MessageHandler _messageHandler;
        private readonly CCIA _positioner;
        private readonly List<Pocket> _pockets;
        private readonly PictureBox _pictureBox;
        private readonly Action _hitTheBallFunc;
        private DefaultBallFactory _ballFactory;

        /// <summary>
        /// 路徑計算結果-母球。
        /// </summary>
        private Ball _cueBall;

        /// <summary>
        /// 路徑計算結果-母球角度。
        /// </summary>
        private double _cueBallAngle;

        public BilliardPlayer(RoboticArm arm, IDSCamera camera, MessageHandler messageHandler, PictureBox pictureBox, Action hitTheBallFunc, List<Pocket> pockets)
        {
            _arm = arm;
            _camera = camera;
            _messageHandler = messageHandler;
            _pictureBox = pictureBox;
            _hitTheBallFunc = hitTheBallFunc;
            _pockets = pockets;

            _positioner = CCIA.LoadFromCsv("ccia_param.csv");
        }

        /// <summary>
        /// 辨識並計算最佳路徑。
        /// </summary>
        /// <param name="saveAllPathImage"></param>
        /// <exception cref="Exception"></exception>
        public void FindThePath(bool saveAllPathImage = false)
        {
            // 到拍照的位置。
            _arm.Speed = 25;
            _arm.MoveAbsolute(_standByPositionJoint1, new MotionParam { CoordinateType = CoordinateType.Joint });
            _arm.Speed = 30;
            _arm.MoveAbsolute(_takePiecurePosition);

            // 拍照。
            var image = TakeAPicture();
            var previewImage = image.Clone();

            // 影像辨識找球。
            _ballFactory = MakeBallFactory();
            var balls = _ballFactory.Execute(image);
            foreach (var ball in balls)
            {
                var color = new MCvScalar(0, 0, 0);
                CvInvoke.Circle(previewImage, Point.Round(ball.Position), (int)ball.Radius, color, 3);
                var point = new Point((int)(ball.Position.X - ball.Radius), (int)(ball.Position.Y - ball.Radius) - 20);
                CvInvoke.PutText(previewImage, $"{ball.Type} ID: {ball.ID}", point, Emgu.CV.CvEnum.FontFace.HersheyPlain, 3, color, 2);
            }

            // 計算可能的路徑。
            if (!saveAllPathImage)
            {
                image = null;
            }
            var score = CollisionPathPlanningHandler.FindMostPossiblePath(balls,
                                                                          _pockets,
                                                                          out var pocket,
                                                                          out var objectBall,
                                                                          out var cueBall,
                                                                          out var ghostCueBallPosition,
                                                                          out var angleDeg,
                                                                          image);
            if (score < 0)
            {
                throw new Exception("無任何可能的路徑。"); // 跳出。
            }

            _cueBall = cueBall;
            _cueBallAngle = angleDeg;

            // 把路徑結果畫在圖上。
            previewImage = CollisionPathPlanningHandler.Drawing(previewImage.Clone(), objectBall, cueBall, ghostCueBallPosition, pocket);
            CvInvoke.PutText(previewImage, $"Angle {Math.Round(angleDeg, 2)}", Point.Round(cueBall.Position), Emgu.CV.CvEnum.FontFace.HersheyPlain, 3, new MCvScalar(0, 0, 150), 2);
            previewImage.Save("results/preview_image.jpg");
            if (_pictureBox != null)
            {
                _pictureBox.Image = previewImage.ToBitmap();
            }
        }

        /// <summary>
        /// 手臂打球。
        /// </summary>
        /// <param name="showMessage"></param>
        public void MoveAndHit(bool showMessage = true)
        {
            if (_cueBall == null || _cueBallAngle == double.NaN)
            {
                throw new Exception("_cueBall == null || _cueBallAngle == double.NaN");
            }

            // 影像座標轉手臂座標（影像定位）。
            var cueBallPosition = _positioner.ImageToWorld(_cueBall.Position);
            var position = Hiwin.Default.DescartesHomePosition;
            position[0] = cueBallPosition.X;
            position[1] = cueBallPosition.Y;
            position[5] = _cueBallAngle;

            if (showMessage)
            {
                MessageBox.Show("即將移動");
            }

            // 到母球上方。
            _arm.Speed = 30;
            position[2] = _zUpper;
            _arm.MoveAbsolute(position);

            // 下降。。
            _arm.Speed = 10;
            position[2] = _zLower;
            _arm.MoveAbsolute(position, new MotionParam { MotionType = RASDK.Arm.Type.MotionType.Linear });

            // 擊球。
            _hitTheBallFunc();

            // 回到母球上方。
            _arm.Speed = 30;
            position[2] = _zUpper;
            _arm.MoveAbsolute(position, new MotionParam { MotionType = RASDK.Arm.Type.MotionType.Linear });

            Homing(showMessage);
        }

        /// <summary>
        /// 手臂回預備位置。
        /// </summary>
        /// <param name="showMessage"></param>
        public void Homing(bool showMessage = true)
        {
            if (showMessage)
            {
                MessageBox.Show("復歸");
            }

            _arm.Speed = 30;
            _arm.MoveAbsolute(_standByPositionJoint1, new MotionParam { CoordinateType = CoordinateType.Joint });

            _arm.Speed = 25;
            _arm.MoveAbsolute(_standByPositionJoint2, new MotionParam { CoordinateType = CoordinateType.Joint });
        }

        private Image<Bgr, byte> TakeAPicture()
        {
            return new Image<Bgr, byte>("test_2.jpg"); // 讀取檔案。
            //return _camera.GetImage().ToImage<Bgr, byte>(); // 拍照。
        }

        private DefaultBallFactory MakeBallFactory()
        {
            var ss = new WeightGrayConversionImpl(green_weight: 0.2, blue_weight: 0.4, red_weight: 0.4);
            var locator = new BallLocator(null,
                                          ss,
                                          new NormalThresoldImpl(50),
                                          new DilateErodeBinaryPreprocessImpl(new Size(4, 4)),
                                          55,
                                          100);
            var recognizer = new BallRecognizer(null);
            return new DefaultBallFactory(locator, recognizer, new BallResultMerger(), 3);
        }
    }
}