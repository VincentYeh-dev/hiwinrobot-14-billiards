using Emgu.CV;
using Emgu.CV.Structure;
using ExclusiveProgram.puzzle.visual.framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExclusiveProgram.puzzle.visual.concrete
{
    public class BallResultMerger : IResultMerger
    {
        public BallResultMerger()
        {
        }

        public Ball2D merge(LocationResult locationResult, Image<Bgr, byte> correctedImage, RecognizeResult recognizeResult)
        {

            return new Ball2D(locationResult.ID,recognizeResult.Type,locationResult.Coordinate,locationResult.Radius); 
        }
    }
}
