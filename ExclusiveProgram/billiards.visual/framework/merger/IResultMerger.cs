﻿using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExclusiveProgram.puzzle.visual.framework
{
    public interface IResultMerger
    {
        Ball merge(LocationResult locationResult,Image<Bgr,byte> correctedImage,RecognizeResult recognizeResult);
    }
}
