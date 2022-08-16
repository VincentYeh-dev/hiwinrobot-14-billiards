﻿using Emgu.CV;
using Emgu.CV.Structure;
using ExclusiveProgram.puzzle.visual.framework;
using ExclusiveProgram.threading;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ExclusiveProgram.puzzle.visual.concrete
{
    public class DefaultBallFactory : IBallFactory
    {
        private IRecognizer recognizer;
        private ILocator locator;
        private readonly IResultMerger merger;
        private PuzzleFactoryListener listener;
        private readonly TaskFactory factory;
        private readonly CancellationTokenSource cts;

        public DefaultBallFactory(ILocator locator, IRecognizer recognizer, IResultMerger merger, int threadCount)
        {
            this.recognizer = recognizer;
            this.locator = locator;
            this.merger = merger;

            // Create a scheduler that uses two threads.
            LimitedConcurrencyLevelTaskScheduler lcts = new LimitedConcurrencyLevelTaskScheduler(threadCount);

            // Create a TaskFactory and pass it our custom scheduler.
            factory = new TaskFactory(lcts);
            cts = new CancellationTokenSource();
        }

        public List<Ball> Execute(Image<Bgr, byte> input)
        {
            List<LocationResult> dataList;

            dataList = locator.Locate(input);
            if (listener != null)
                listener.onLocated(dataList);

            List<Ball> results = new List<Ball>();

            List<Task> tasks = new List<Task>();
            foreach (LocationResult location in dataList)
            {
                Task task = factory.StartNew(() =>
                {
                    var recognized_result = recognizer.Recognize(location.ID, location.ROI,location.Radius);
                    if (listener != null)
                        listener.onRecognized(recognized_result);
                    results.Add(merger.merge(location, location.ROI, recognized_result));

                }, cts.Token);
                task.Wait();
                tasks.Add(task);
            }

            //Task.WaitAll(tasks.ToArray());
            cts.Dispose();
            return results;
        }

        public void setListener(PuzzleFactoryListener listener)
        {
            this.listener = listener;
        }
    }

}