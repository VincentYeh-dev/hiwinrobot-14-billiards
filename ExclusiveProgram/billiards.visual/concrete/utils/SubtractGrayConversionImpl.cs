using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using ExclusiveProgram.puzzle.visual.framework;
using ExclusiveProgram.puzzle.visual.framework.utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExclusiveProgram.puzzle.visual.concrete.utils
{
    public class SubtractGrayConversionImpl : IGrayConversionImpl
    {
        public void ConvertToGray(Image<Bgr, byte> input, Image<Gray, byte> output)
        {

            var sub= new Image<Bgr, byte>(input.Size);
            var background = new Image<Bgr, byte>(input.Size.Width,input.Size.Height,GetBackgroundColor(input));
            CvInvoke.Subtract(input,background,sub);

            sub.Save("results\\sub.jpg");
            var channels = new VectorOfMat();
            CvInvoke.Split(input, channels);
            CvInvoke.EqualizeHist(channels[0], channels[0]);
            CvInvoke.EqualizeHist(channels[1], channels[1]);
            CvInvoke.EqualizeHist(channels[2], channels[2]);
            var ss= new Image<Bgr, byte>(input.Size);
            CvInvoke.Merge(channels,ss);
            ss.Save("results\\hist_equalize.jpg");
            
            CvInvoke.CvtColor(ss, output, ColorConversion.Bgr2Gray);
            output.Save("results\\gray.jpg");
        }

        private Bgr GetBackgroundColor(Image<Bgr, byte> input)
        {
            VectorOfMat channels = new VectorOfMat();
            CvInvoke.Split(input, channels);
            var b_index=GetMaxHistElement(channels[0]);
            var g_index=GetMaxHistElement(channels[1]);
            var r_index=GetMaxHistElement(channels[2]);

            return new Bgr(b_index,g_index,r_index);
        }
        private int GetMaxHistElement(Mat channel)
        {
            float[] ranges = new float[] {0, 256};
            var hist = new Mat();
            VectorOfMat c = new VectorOfMat();
            c.Push(channel);
            CvInvoke.CalcHist(c,new int[] {0}, null, hist, new int[] {256}, ranges, false);
            var matrix = new Matrix<float>(hist.Size);
            //hist.ConvertTo(matrix, DepthType.Cv8U);
            hist.CopyTo(matrix);
            var max = -1f;
            var max_index = 0;
            for (int i = 0; i < matrix.Rows; i++)
            {
                var current = matrix.Data[i, 0];
                if (current > max)
                {
                    max = current;
                    max_index = i;
                }
            }

            return max_index;
            
        }
    }
}
