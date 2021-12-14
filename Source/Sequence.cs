using System;
using System.Collections.Generic;
using TweenKey.Interpolation;
using UnityEngine;

namespace TweenKey
{
    public class Sequence<T>
    {
        public List<KeyFrame<T>> keyFrames;
        public LerpFunction<T> lerpFunction;
        
        public Sequence(List<KeyFrame<T>> keyFrames, LerpFunction<T> lerpFunction)
        {
            this.keyFrames = keyFrames;
            this.lerpFunction = lerpFunction;
            keyFrames.Sort((x, y) => x.frame.CompareTo(y.frame));
        }

        public Sequence(AnimationCurve curve, LerpFunction<T> lerpFunction, Func<float, T> evaluationMethod, int framesPerSecond = -1)
        {
            List<KeyFrame<T>> keyFrames = new List<KeyFrame<T>>();
            int targetFrameRate = framesPerSecond > 0 ? framesPerSecond : Application.targetFrameRate > 0 ? Application.targetFrameRate : 60;
            float frameCount = curve.keys[^1].time * targetFrameRate;
            for (; frameCount > 0; frameCount--)
            {
                var frameValue = frameCount / targetFrameRate;
                keyFrames.Add(new KeyFrame<T>(frameValue, evaluationMethod(curve.Evaluate(frameValue))));
            }
            
            this.keyFrames = keyFrames;
            this.lerpFunction = lerpFunction;
            keyFrames.Sort((x, y) => x.frame.CompareTo(y.frame));
        }

        public Sequence<T> Reverse()
        {
            keyFrames.Sort((x, y) => x.frame.CompareTo(y.frame));
            float finalFrame = keyFrames[^1].frame;
            
            List<KeyFrame<T>> reversedFrames = new List<KeyFrame<T>>();
            foreach (var key in keyFrames)
            {
                reversedFrames.Add(new KeyFrame<T>(finalFrame - key.frame, key.value));
            }
            
            return new Sequence<T>(reversedFrames, lerpFunction);
        }

        public void Append(params Sequence<T>[] sequences)
        {
            keyFrames.Sort((x, y) => x.frame.CompareTo(y.frame));
            float offsetTime = keyFrames[^1].frame;
            
            foreach (var sequence in sequences)
            {
                foreach (var key in sequence.keyFrames)
                {
                    keyFrames.Add(new KeyFrame<T>(key.frame + offsetTime, key.value));
                }

                offsetTime += sequence.keyFrames[^1].frame;
            }
        }
    }
}