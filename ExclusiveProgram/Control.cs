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
        private ISerialPortDevice _serialPortDevice;
        private BilliardPlayer _billiardPlayer;

        public Control()
        {
            InitializeComponent();
            Config = new Config();

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
            Thread.Sleep(1000);

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
                var camera = new IDSCamera(MessageHandler);
                camera.Connect();
                _billiardPlayer = new BilliardPlayer(Arm, camera, MessageHandler, pictureBoxMain, () => HitTheBall());

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
            Console.WriteLine($"執行時間：{sw.Elapsed}");
            MessageHandler.Log($"執行時間：{sw.Elapsed}", LoggingLevel.Info);

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
    }
}