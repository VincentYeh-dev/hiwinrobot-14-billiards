using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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

namespace ExclusiveProgram
{
    public partial class Control : MainForm.ExclusiveControl
    {
        public Control()
        {
            InitializeComponent();
            Config = new Config();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            //var ss = new SubtractGrayConversionImpl();
            var ss = new WeightGrayConversionImpl(green_weight: 0.2, blue_weight: 0.4, red_weight: 0.4);
            var locator = new BallLocator(null,ss,
                new NormalThresoldImpl(50),new DilateErodeBinaryPreprocessImpl(new Size(4,4)),55,100);
            var recognizer = new BallRecognizer(null);
            var factory = new DefaultBallFactory(locator, recognizer, new BallResultMerger(), 3);
            var testImage = new Image<Bgr, byte>("Test6.jpg");

            //var output= new Image<Gray, byte>(testImage.Size);
            //ss.ConvertToGray(testImage,output);
            //output.Save("results\\sub.jpg");
            var preview_image= testImage.Clone();
            List<Ball2D> balls= factory.Execute(testImage);
            foreach(var ball in balls)
            {
                CvInvoke.Circle(preview_image, Point.Round(ball.Coordinate), (int)ball.Radius, new MCvScalar(0,0 , 255), 3);
                var p = new Point((int)(ball.Coordinate.X - ball.Radius),(int)(ball.Coordinate.Y - ball.Radius)-20);
                CvInvoke.PutText(preview_image,$"{ball.Type}",p,Emgu.CV.CvEnum.FontFace.HersheyPlain,3, new MCvScalar(0,0, 255));
                Console.WriteLine($"ID:{ball.ID} -> {ball.Type}");
            }
            preview_image.Save("results\\result.jpg");
        }
    }
}
