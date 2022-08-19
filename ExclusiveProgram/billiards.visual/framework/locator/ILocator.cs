using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExclusiveProgram.puzzle.visual.framework
{
    

    public struct LocationResult
    {
        public int ID;
        public PointF Coordinate;
        public Size Size;
        public Image<Bgr,byte> ROI;
        public double Radius;
    }

    public interface ILocator
    {
        //List<LocationResult> Locate(Image<Bgr, byte> rawImage);
        List<LocationResult> Locate(Image<Bgr, byte> rawImage, List<Pocket> pockets);
    }
}
