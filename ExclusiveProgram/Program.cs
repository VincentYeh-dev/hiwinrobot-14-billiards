using Emgu.CV;
using Emgu.CV.Structure;
using ExclusiveProgram.puzzle.visual.concrete;
using ExclusiveProgram.puzzle.visual.concrete.utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExclusiveProgram
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var locator=new BallLocator(new Size(),new Size(),null,new GreenBackgroundGrayConversionImpl(0.4),null,null);
            locator.Locate(CvInvoke.Imread("Test.jpg").ToImage<Bgr, byte>());
            return;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm.MainForm(new Control()));
        }
    }
}
