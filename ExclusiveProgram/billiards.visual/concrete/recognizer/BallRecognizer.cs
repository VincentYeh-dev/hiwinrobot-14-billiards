using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using ExclusiveProgram.puzzle.visual.framework;
using ExclusiveProgram.puzzle.visual.framework.utils;
namespace ExclusiveProgram.puzzle.visual.concrete
{

    public class BallRecognizer : IRecognizer
    {

        private Image<Bgr, byte> preprocessModelImage = null;
        private readonly IPreprocessImpl puzzlePreProcessImpl;
        private readonly IGrayConversionImpl grayConversionImpl;
        private readonly IThresholdImpl thresholdImpl;
        private readonly IBinaryPreprocessImpl binaryPreprocessImpl;
        private readonly Image<Bgr, byte> modelImage;
        private readonly double uniquenessThreshold;
        private PuzzleRecognizerListener listener;


        public BallRecognizer(Image<Bgr, byte> modelImage, double uniquenessThreshold, IPreprocessImpl puzzlePreProcessImpl,IGrayConversionImpl grayConversionImpl,IThresholdImpl thresholdImpl,IBinaryPreprocessImpl binaryPreprocessImpl)
        {
            this.modelImage = modelImage;
            this.uniquenessThreshold = uniquenessThreshold;
            this.puzzlePreProcessImpl = puzzlePreProcessImpl;
            this.grayConversionImpl = grayConversionImpl;
            this.thresholdImpl = thresholdImpl;
            this.binaryPreprocessImpl = binaryPreprocessImpl;
        }

        public void PreprocessModelImage()
        {
            preprocessModelImage = new Image<Bgr, byte>(modelImage.Size);
            if(puzzlePreProcessImpl!=null)
                puzzlePreProcessImpl.Preprocess(modelImage,preprocessModelImage);
            else
                preprocessModelImage = modelImage;
        }

        public bool ModelImagePreprocessIsDone()
        {
            return preprocessModelImage != null;
        }

        public RecognizeResult Recognize(int id,Image<Bgr, byte> image)
        {
            Image<Bgr, byte> observedImage = image.Clone();

            if (puzzlePreProcessImpl != null)
                puzzlePreProcessImpl.Preprocess(observedImage,observedImage);


            RecognizeResult result = new RecognizeResult();

            return result;
        }

        public void setListener(PuzzleRecognizerListener listener)
        {
            this.listener = listener;
        }
    }
}
