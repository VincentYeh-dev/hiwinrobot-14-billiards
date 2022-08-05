using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using ExclusiveProgram.puzzle.visual.framework;
using ExclusiveProgram.puzzle.visual.framework.utils;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ExclusiveProgram.puzzle.visual.concrete
{
    public class BallLocator : ILocator
    {
        private readonly int minSize;
        private readonly int maxSize;
        private readonly IPreprocessImpl preProcessImpl;
        private readonly IGrayConversionImpl grayConversionImpl;
        private readonly IThresholdImpl thresholdImpl;
        private readonly IBinaryPreprocessImpl binaryPreprocessImpl;

        public BallLocator(int minRadius, int maxRadius, IPreprocessImpl preProcessImpl, IGrayConversionImpl grayConversionImpl, IThresholdImpl thresholdImpl, IBinaryPreprocessImpl binaryPreprocessImpl, double approx_paramater = 0.005f)
        {
            this.minSize = minRadius;
            this.maxSize = maxRadius;
            this.preProcessImpl = preProcessImpl;
            this.grayConversionImpl = grayConversionImpl;
            this.thresholdImpl = thresholdImpl;
            this.binaryPreprocessImpl = binaryPreprocessImpl;
        }


        public List<LocationResult> Locate(Image<Bgr, byte> rawImage)
        {
            var preprocessImage = rawImage.Clone();
            CvInvoke.MedianBlur(preprocessImage, preprocessImage, 9);

            if (preProcessImpl != null)
                preProcessImpl.Preprocess(preprocessImage, preprocessImage);

            var grayImage = new Image<Gray, byte>(preprocessImage.Size);
            grayConversionImpl.ConvertToGray(preprocessImage, grayImage);

            if (thresholdImpl != null)
                thresholdImpl.Threshold(grayImage, grayImage);
            grayImage.Save("results/binary.jpg");
            var circles=CvInvoke.HoughCircles(grayImage,HoughModes.Gradient,4,200,100,150,190,220);
            List<LocationResult> location_results = new List<LocationResult>();

            for(int i=0;i<circles.Length;i++)
            {
                var circle = circles[i];
                LocationResult result = new LocationResult();
                result.ID = i;
                result.Coordinate = circle.Center;
                result.Radius= circle.Radius;

                circle.

                CvInvoke.Circle(preprocessImage, Point.Round(circle.Center), (int)circle.Radius, new MCvScalar(0, 0, 255),3);
            }
            preprocessImage.Save("results/circles.jpg");
            return location_results;
        }

        private Image<Bgr, byte> getROI(Point Coordinate, Size Size, Image<Bgr, byte> input)
        {
            Rectangle rect = new Rectangle((int)(Coordinate.X - Size.Width / 2.0f), (int)(Coordinate.Y - Size.Height / 2.0f), Size.Width, Size.Height);

            //將ROI選取區域使用Mat型式讀取
            return new Mat(input.Mat, rect).ToImage<Bgr, byte>();
        }
        private Image<Gray, byte> getBinaryROI(Point Coordinate, Size Size, Image<Gray, byte> input)
        {
            Rectangle rect = new Rectangle((int)(Coordinate.X - Size.Width / 2.0f), (int)(Coordinate.Y - Size.Height / 2.0f), Size.Width, Size.Height);

            //將ROI選取區域使用Mat型式讀取
            return new Mat(input.Mat, rect).ToImage<Gray, byte>();
        }


        private Point GetCentralCoordinate(VectorOfPoint contour)
        {
            //畫出最小外切圓，獲得圓心用
            CircleF Puzzle_circle = CvInvoke.MinEnclosingCircle(contour);
            return new Point((int)Puzzle_circle.Center.X, (int)Puzzle_circle.Center.Y);
        }

        private VectorOfVectorOfPoint FindContours(Image<Gray, byte> image)
        {
            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            CvInvoke.FindContours(image, contours, null, RetrType.External, ChainApproxMethod.ChainApproxSimple);
            return contours;
        }
    }
}
