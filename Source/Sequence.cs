using System;
using System.Collections.Generic;
using TweenKey.Interpolation;
using UnityEngine;

namespace TweenKey
{
    public class Sequence<T>
    {
        public List<KeyFrame<T>> keyFrames = default;
        public LerpFunction<T> lerpFunction = default;
        public Action onComplete = null;
        
        public Sequence(List<KeyFrame<T>> keyFrames, LerpFunction<T> lerpFunction, Action onComplete = null)
        {
            this.keyFrames = keyFrames;
            this.lerpFunction = lerpFunction;
            this.onComplete = onComplete;
        }

        public Sequence(AnimationCurve curve, LerpFunction<T> lerpFunction, Func<float, T> evaluation, Action onComplete = null, int framesPerSecond = 60)
        {
            List<KeyFrame<T>> keyFrames = new List<KeyFrame<T>>();
            float frameCount = curve.keys[^1].time * framesPerSecond;
            for (; frameCount > 0; frameCount--)
            {
                var frameValue = frameCount / framesPerSecond;
                keyFrames.Add(new KeyFrame<T>(frameValue, evaluation(curve.Evaluate(frameValue))));
            }
            
            this.keyFrames = keyFrames;
            this.lerpFunction = lerpFunction;
            this.onComplete = onComplete;
        }
    }
}