using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using ExclusiveProgram.puzzle.visual.concrete;
using ExclusiveProgram.puzzle.visual.concrete.utils;
using RASDK.Arm;
using RASDK.Basic;
using RASDK.Basic.Message;
using RASDK.Gripper;
using RASDK.Vision.IDS;
using System.Diagnostics;

namespace ExclusiveProgram
{
    public partial class Control : MainForm.ExclusiveControl
    {
        private readonly int _serialPortBaudrate = 9600;
        private readonly List<Pocket> _pockets = new List<Pocket>();

        private readonly List<MCvScalar> _pocketColors = new List<MCvScalar>
        {
            new MCvScalar(0,0,255),
            new MCvScalar(0,120,235),
            new MCvScalar(0,255,255),
            new MCvScalar(0,255,0),
            new MCvScalar(255,0,0),
            new MCvScalar(255,0,255),
        };

        private IDSCamera _camera;
        private ISerialPortDevice _serialPortDevice;
        private BilliardPlayer _billiardPlayer;
        private int _pocketRadius = 110;

        private Image<Bgr, byte> _sourceImage;

        public Control()
        {
            InitializeComponent();
            Config = new Config();

            _pockets = new List<Pocket>
            {
                new Pocket(new PointF(60,250),PocketType.Corner, 0),
                new Pocket(new PointF(1570, 230),PocketType.Side,1),
                new Pocket(new PointF(3090, 320),PocketType.Corner,2),

                new Pocket(new PointF(50, 1700),PocketType.Corner,3),
                new Pocket(new PointF(1540, 1760),PocketType.Side,4),
                new Pocket(new PointF(3010, 1790),PocketType.Corner,5),
            };

            if (!Directory.Exists("results"))
            {
                Directory.CreateDirectory("results");
            }

            if (!Directory.Exists("paths"))
            {
                Directory.CreateDirectory("paths");
            }
        }

        /// <summary>
        /// 擊球。
        /// </summary>
        private void HitTheBall()
        {
            // 電磁鐵 ON.
            _serialPortDevice.SerialPort.Write(new byte[] { 0x01 }, 0, 1);

            // Delay in ms.
            Thread.Sleep(800);

            // 電磁鐵 OFF.
            _serialPortDevice.SerialPort.Write(new byte[] { 0x00 }, 0, 1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DoOnce();
        }

        /// <summary>
        /// 初始化。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonInit_Click(object sender, EventArgs e)
        {
            try
            {
                _camera = new IDSCamera(MessageHandler);
                _camera.Connect();
                _camera.LoadParameterFromEEPROM();
                _billiardPlayer = new BilliardPlayer(Arm, _camera, MessageHandler, pictureBoxMain, () => HitTheBall(), _pockets);

                // Serial port.
                var comPorts = SerialPort.GetPortNames();
                var sp = new SerialPort(comPorts[0], _serialPortBaudrate, Parity.None, 8, StopBits.One);
                sp.DataReceived += SerialPortDataReceivedHandler;
                sp.Open();
                _serialPortDevice = new SerialPortDevice(sp, MessageHandler);

                buttonInit.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageHandler.Show(ex, LoggingLevel.Error);
                buttonInit.Enabled = true;
            }
        }

        /// <summary>
        /// 進行一次完整的擊球流程。
        /// </summary>
        private void DoOnce()
        {
            MessageHandler.Log($"開始進行一次完整的擊球。", LoggingLevel.Info);
            var sw = Stopwatch.StartNew();

            try
            {
                _billiardPlayer.FindThePath(checkBoxShowMessage.Checked);
                _billiardPlayer.MoveAndHit(checkBoxShowMessage.Checked);
            }
            catch (Exception ex)
            {
                MessageHandler.Log(ex, LoggingLevel.Warn);
                _billiardPlayer.Homing(checkBoxShowMessage.Checked);
                return;
            }

            sw.Stop();
            Console.WriteLine($"擊球總執行時間：{sw.Elapsed}");
            MessageHandler.Log($"擊球總執行時間：{sw.Elapsed}", LoggingLevel.Info);

            Thread.Sleep(1000);
        }

        /// <summary>
        /// Serial port 接收指令事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SerialPortDataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            var sp = sender as SerialPort;
            sp.DataReceived -= SerialPortDataReceivedHandler;

            // Parse.
            var indata = sp.ReadByte();
            switch (indata)
            {
                case 0xF0: // A button pressed.
                    DoOnce();
                    break;

                default:
                    break;
            }

            sp.DiscardInBuffer(); // Cleay buffer.
            sp.DataReceived += SerialPortDataReceivedHandler;
        }

        private void buttonReady_Click(object sender, EventArgs e)
        {
            _billiardPlayer.Homing(false);
        }

        private void buttonGetImage_Click(object sender, EventArgs e)
        {
            var img = _camera.GetImage();
            //var img = new Image<Bgr, byte>("test_1.jpg");
            pictureBoxMain.Image = img;
            _sourceImage = img.ToImage<Bgr, byte>();
            //_sourceImage = img;

            UpdatePocketImage();
        }

        #region Pocket

        private int GetSelectPocketIndex()
        {
            int index = -1;
            if (radioButtonPocket0.Checked)
            { index = 0; }
            else if (radioButtonPocket1.Checked)
            { index = 1; }
            else if (radioButtonPocket2.Checked)
            { index = 2; }
            else if (radioButtonPocket3.Checked)
            { index = 3; }
            else if (radioButtonPocket4.Checked)
            { index = 4; }
            else if (radioButtonPocket5.Checked)
            { index = 5; }
            return index;
        }

        private void UpdatePocketUI()
        {
            var index = GetSelectPocketIndex();
            numericUpDownPocketX.Value = (decimal)_pockets[index].Position.X;
            numericUpDownPocketY.Value = (decimal)_pockets[index].Position.Y;

            UpdatePocketImage();
        }

        private void UpdatePocketImage()
        {
            if (_sourceImage == null)
            {
                return;
            }

            var srcImg = _sourceImage;
            var disImg = srcImg.Clone();

            foreach (var pocket in _pockets)
            {
                var color = _pocketColors[pocket.Id];
                var p = Point.Round(pocket.Position);
                CvInvoke.Circle(disImg, p, _pocketRadius, color, 5);

                var cross = new Cross2DF(p, (float)(_pocketRadius * 0.7), (float)(_pocketRadius * 0.7));
                disImg.Draw(cross, new Bgr(color.V0, color.V1, color.V2), 2);
            }

            pictureBoxMain.Image = disImg.ToBitmap();
        }

        #region Radio button

        private void radioButtonPocket0_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePocketUI();
        }

        private void radioButtonPocket1_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePocketUI();
        }

        private void radioButtonPocket2_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePocketUI();
        }

        private void radioButtonPocket3_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePocketUI();
        }

        private void radioButtonPocket4_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePocketUI();
        }

        private void radioButtonPocket5_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePocketUI();
        }

        #endregion Radio button

        private void numericUpDownPocketX_ValueChanged(object sender, EventArgs e)
        {
            var i = GetSelectPocketIndex();
            _pockets[i].Position.X = (float)numericUpDownPocketX.Value;
            UpdatePocketImage();
        }

        private void numericUpDownPocketY_ValueChanged(object sender, EventArgs e)
        {
            var i = GetSelectPocketIndex();
            _pockets[i].Position.Y = (float)numericUpDownPocketY.Value;
            UpdatePocketImage();
        }

        #endregion Pocket

        private void button2_Click(object sender, EventArgs e)
        {
            var factory=MakeBallFactory();
            var image = _camera.GetImage().ToImage<Bgr, byte>();
            var balls=factory.Execute(image);
            var preview_image = image.Clone();
            foreach(var ball in balls)
            {
                CvInvoke.Circle(preview_image, Point.Round(ball.Position), (int)ball.Radius, new MCvScalar(0,0 , 255), 3);
                var p = new Point((int)(ball.Position.X - ball.Radius),(int)(ball.Position.Y - ball.Radius)-20);
                CvInvoke.PutText(preview_image,$"{ball.Type}",p,Emgu.CV.CvEnum.FontFace.HersheyPlain,3, new MCvScalar(0,0, 255));
                Console.WriteLine($"ID:{ball.ID} -> {ball.Type}");
            }
            preview_image.Save("results\\result.jpg");
        }

        private DefaultBallFactory MakeBallFactory()
        {
            var ss = new WeightGrayConversionImpl(green_weight: 0.1, blue_weight: 0.5, red_weight: 0.9);
            var locator = new BallLocator(null,
                                          ss,
                                          new NormalThresoldImpl(120),
                                          new DilateErodeBinaryPreprocessImpl(new Size(4, 4)),
                                          35,
                                          80);
            var recognizer = new BallRecognizer(null);
            return new DefaultBallFactory(locator, recognizer, new BallResultMerger(), 3);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _billiardPlayer.MoveToCapturePosition();
        }
    }
}