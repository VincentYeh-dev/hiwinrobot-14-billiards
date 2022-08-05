using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System.Collections.Generic;
using System.Drawing;

namespace ExclusiveProgram.puzzle.visual.framework
{
    public struct RecognizeResult
    {
        public int ID;
        public BallType Type;
    };

    public interface PuzzleRecognizerListener
    {
        void OnMatched(int id,Image<Bgr, byte> preprocessModelImage, VectorOfKeyPoint modelKeyPoints, Image<Bgr, byte> observedImage, VectorOfKeyPoint observedKeyPoints, VectorOfVectorOfDMatch matches, Mat mask, long matchTime);
        void OnPerspective(int id,Image<Bgr, byte> warpedPerspectiveImage, string position);
    }

    public interface IRecognizer
    {
        RecognizeResult Recognize(int id,Image<Bgr,byte> image);
        void setListener(PuzzleRecognizerListener listener);
    }

}
