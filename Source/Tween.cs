using System;
using System.Collections.Generic;
using TweenKey.Interpolation;

namespace TweenKey
{
    public class Tween<T>
    {
        public List<KeyFrame<T>> keyFrames = default;
        public LerpFunction<T> lerpFunction = default;
        public System.Action onComplete = null;

        public Tween(List<KeyFrame<T>> keyFrames, LerpFunction<T> lerpFunction, Action onComplete = null)
        {
            this.keyFrames = keyFrames;
            this.lerpFunction = lerpFunction;
            this.onComplete = onComplete;
        }
    }

}