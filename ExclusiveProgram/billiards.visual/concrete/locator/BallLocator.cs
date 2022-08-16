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
        private readonly int minRadius;
        private readonly int maxRadius;
        private readonly IPreprocessImpl preProcessImpl;
        private readonly IGrayConversionImpl grayConversionImpl;
        private readonly IThresholdImpl thresholdImpl;
        private readonly IBinaryPreprocessImpl binaryPreprocessImpl;

        public BallLocator(IPreprocessImpl preProcessImpl, IGrayConversionImpl grayConversionImpl, IThresholdImpl thresholdImpl, IBinaryPreprocessImpl binaryPreprocessImpl, int minRadius=190, int maxRadius=220 )
        {
            this.minRadius = minRadius;
            this.maxRadius = maxRadius;
            this.preProcessImpl = preProcessImpl;
            this.grayConversionImpl = grayConversionImpl;
            this.thresholdImpl = thresholdImpl;
            this.binaryPreprocessImpl = binaryPreprocessImpl;
        }


        public List<LocationResult> Locate(Image<Bgr, byte> rawImage)
        {
            var preprocessImage = rawImage.Clone();
            //CvInvoke.MedianBlur(preprocessImage, preprocessImage, 9);

            if (preProcessImpl != null)
                preProcessImpl.Preprocess(preprocessImage, preprocessImage);

            var grayImage = new Image<Gray, byte>(preprocessImage.Size);
            grayConversionImpl.ConvertToGray(preprocessImage, grayImage);

            if (thresholdImpl != null)
                thresholdImpl.Threshold(grayImage, grayImage);

            if (binaryPreprocessImpl!= null)
                binaryPreprocessImpl.BinaryPreprocess(grayImage, grayImage);

            grayImage.Save("results/binary.jpg");
            var circles=CvInvoke.HoughCircles(grayImage,HoughModes.Gradient,4,40,100,120,minRadius,maxRadius);
            List<LocationResult> location_results = new List<LocationResult>();

            for(int i=0;i<circles.Length;i++)
            {
                var circle = circles[i];
                LocationResult result = new LocationResult();
                result.ID = i;
                result.Coordinate = circle.Center;
                result.Radius= circle.Radius;
                result.ROI = GetROI(result.Coordinate,result.Radius,rawImage);
                result.ROI.Save($"results/Ball_{result.ID}.jpg");
                location_results.Add(result);
                CvInvoke.Circle(preprocessImage, Point.Round(circle.Center), (int)circle.Radius, new MCvScalar(0, 0, 255),3);
            }
            preprocessImage.Save("results/circles.jpg");
            return location_results;
        }

        private Image<Bgr, byte> GetROI(PointF Coordinate,double Radius, Image<Bgr, byte> input)
        {
            Rectangle rect = new Rectangle((int)(Coordinate.X - Radius), (int)(Coordinate.Y - Radius),(int)(2*Radius),(int)(2*Radius));
            input.ROI = rect;
            var newImage = input.Copy();
            input.ROI = Rectangle.Empty;
            return newImage;
        }
    }
}
