
using System;
using System.Collections.Generic;
using TweenKey.Interpolation;

namespace TweenKey
{
    public class Sequence<T>
    {
        public List<KeyFrame<T>> keyFrames = default;
        public LerpFunction<T> lerpFunction = default;
        public System.Action onComplete = null;

        public Sequence(List<KeyFrame<T>> keyFrames, LerpFunction<T> lerpFunction, Action onComplete = null)
        {
            this.keyFrames = keyFrames;
            this.lerpFunction = lerpFunction;
            this.onComplete = onComplete;
        }
    }
}