using TweenKey.Interpolation;
using UnityEngine;

namespace TweenKey
{
    public static class TransformExtensions
    {
        private const string kPosition = "position";
        private const string kRotation = "rotation";
        private const string kScale = "localScale";
        
        public static Tween TweenMove(this Transform transform, Vector3 destination, float duration, System.Action onComplete = default!, 
            EasingFunction easingType = default!, float delay = 0)
        {
            var keyFrame = new KeyFrame<Vector3>(duration, destination, easingType ?? Easing.Cubic.InOut);
            return TweenRunner.RunTween(transform, kPosition, keyFrame, LerpFunctions.Vector3, onComplete ?? delegate {}, delay);
        }
    }
}