using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using ExclusiveProgram.puzzle.visual.framework;
using ExclusiveProgram.puzzle.visual.framework.utils;
using System;
using System.Drawing;

namespace ExclusiveProgram.puzzle.visual.concrete
{

    public class BallRecognizer : IRecognizer
    {

        private Image<Bgr, byte> preprocessModelImage = null;
        private readonly IPreprocessImpl puzzlePreProcessImpl;
        //private readonly IGrayConversionImpl grayConversionImpl;
        //private readonly IThresholdImpl thresholdImpl;
        //private readonly IBinaryPreprocessImpl binaryPreprocessImpl;


        public BallRecognizer(IPreprocessImpl puzzlePreProcessImpl)
        {
            this.puzzlePreProcessImpl = puzzlePreProcessImpl;
            //this.grayConversionImpl = grayConversionImpl;
            //this.thresholdImpl = thresholdImpl;
            //this.binaryPreprocessImpl = binaryPreprocessImpl;
        }

        public RecognizeResult Recognize(int id,Image<Bgr, byte> image,double radius)
        {
            var ballArea = Math.PI * Math.Pow(radius, 2);
            var observedImage = image.Clone();

            if (puzzlePreProcessImpl != null)
                puzzlePreProcessImpl.Preprocess(observedImage,observedImage);
            
            var ballImage= GetMaskBall(radius,observedImage);
            //ballImage.Save($"results/MaskBall_{id}.jpg");

            var whiteArea=GetWhiteArea(ballImage,id);

            var result = new RecognizeResult();
            result.ID = id;
            if (whiteArea > ballArea || whiteArea > ballArea * 0.8)
                result.Type = BallType.CueBall;
            else
                result.Type = BallType.NumberedBall;

            ballImage.Dispose();
            observedImage.Dispose();
            return result;
        }

        private int GetWhiteArea(Image<Bgr, byte> ballImage,int id)
        {
            var hsv= new Image<Hsv, byte>(ballImage.Size);
            CvInvoke.CvtColor(ballImage, hsv, ColorConversion.Bgr2Hsv);

            var lowerWhite = new MCvScalar(0, 0, 50);
            var upperWhite = new MCvScalar(255, 128, 255);
            var whiteImage= new Image<Gray, byte>(ballImage.Size);
            CvInvoke.InRange(hsv,new ScalarArray(lowerWhite), new ScalarArray(upperWhite), whiteImage);
            var whiteArea = whiteImage.CountNonzero()[0];

            //var preview_image = whiteImage.Clone();
            //preview_image.Save($"results\\white_{id}.jpg");
            hsv.Dispose();
            whiteImage.Dispose();
            return whiteArea;
        }

        private Image<Bgr,byte> GetMaskBall(double Radius,Image<Bgr,byte> image)
        {
            var ballImage= new Image<Bgr, byte>(image.Size);
            var mask = new Image<Gray,byte>(ballImage.Size);
            CvInvoke.Circle(mask, new Point(image.Width/2,image.Height/2), (int)Radius, new MCvScalar(255), -1);

            image.Mat.CopyTo(ballImage, mask);
            mask.Dispose();
            return ballImage;
        }


        public void setListener(PuzzleRecognizerListener listener)
        {
            throw new NotImplementedException();
        }
    }
}
