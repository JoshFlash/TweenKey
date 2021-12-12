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
        }

        public Sequence(AnimationCurve curve, LerpFunction<T> lerpFunction, Func<float, T> evaluationMethod, int framesPerSecond = 60)
        {
            List<KeyFrame<T>> keyFrames = new List<KeyFrame<T>>();
            float frameCount = curve.keys[^1].time * framesPerSecond;
            for (; frameCount > 0; frameCount--)
            {
                var frameValue = frameCount / framesPerSecond;
                keyFrames.Add(new KeyFrame<T>(frameValue, evaluationMethod(curve.Evaluate(frameValue))));
            }
            
            this.keyFrames = keyFrames;
            this.lerpFunction = lerpFunction;
        }
    }
}