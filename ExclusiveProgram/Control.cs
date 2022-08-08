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
            var locator = new BallLocator(null,new WeightGrayConversionImpl(green_weight:0.3),
                new NormalThresoldImpl(95),new DilateErodeBinaryPreprocessImpl(new Size(3,3)));
            var recognizer = new BallRecognizer(null);
            var factory = new DefaultBallFactory(locator, recognizer, new BallResultMerger(), 3);
            List<Ball> balls= factory.Execute(new Image<Bgr,byte>("Test2.jpg"));

        }
    }
}
