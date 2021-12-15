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
            return TweenRunner.RunTween(transform, kPosition, keyFrame, LerpFunctions.Vector3, OffsetFunctions.Vector3, onComplete , delay);
        }

        public static Tween TweenLoopMove(this Transform transform, Vector3 destination, float duration, System.Action onComplete = default!,
            EasingFunction easingTypeIn = default!, float delay = 0)
        {
            var keyFrameIn = new KeyFrame<Vector3>(duration, destination, easingTypeIn ?? Easing.Sinusoidal.InOut);
            return TweenRunner.RunTweenLooped(transform, kPosition, keyFrameIn, LerpFunctions.Vector3, OffsetFunctions.Vector3, onComplete, delay);
        }

        public static Tween TweenMoveSequence(this Transform transform, Sequence<Vector3> tweenSequence , System.Action onComplete = default!)
        {
            return TweenRunner.RunSequence(transform, kPosition, tweenSequence, onComplete, false);
        }
        
        public static Tween TweenLoopMoveSequence(this Transform transform, Sequence<Vector3> tweenSequence, System.Action onComplete = default!)
        {
            return TweenRunner.RunSequence(transform, kPosition, tweenSequence, onComplete, true);
        }

        public static Tween TweenToRotation(this Transform transform, Quaternion rotation, float duration, System.Action onComplete = default!,
            EasingFunction easingType = default!, float delay = 0)
        {
            var keyFrame = new KeyFrame<Quaternion>(duration , rotation, easingType ?? Easing.Cubic.InOut);
            return TweenRunner.RunTween(transform, kRotation, keyFrame, LerpFunctions.Quaternion, OffsetFunctions.Quaternion, onComplete, delay);
        }
        
        public static Tween TweenByRotation(this Transform transform, Quaternion rotation, float duration, System.Action onComplete = default!,
            EasingFunction easingType = default!, float delay = 0)
        {
            var keyFrame = new KeyFrame<Quaternion>(duration, transform.rotation * rotation, easingType ?? Easing.Cubic.In);
            return TweenRunner.RunTween(transform, kRotation, keyFrame, LerpFunctions.Quaternion, OffsetFunctions.Quaternion, onComplete, delay);
        }

        public static Tween TweenLoopToRotation(this Transform transform, Quaternion rotation, float duration, System.Action onComplete = default!,
            EasingFunction easingTypeIn = default!, float delay = 0)
        {
            var keyFrameIn = new KeyFrame<Quaternion>(duration, rotation, easingTypeIn ?? Easing.Sinusoidal.InOut);
            return TweenRunner.RunTweenLooped(transform, kRotation, keyFrameIn, LerpFunctions.Quaternion, OffsetFunctions.Quaternion, onComplete, delay);
        }

        public static Tween TweenLoopByRotation(this Transform transform, Quaternion rotation, float duration, System.Action onComplete = default!,
            EasingFunction easingTypeIn = default!, float delay = 0)
        {
            var keyFrameIn = new KeyFrame<Quaternion>(duration / 2, transform.rotation * rotation, easingTypeIn ?? Easing.Sinusoidal.InOut);
            return TweenRunner.RunTweenLooped(transform, kRotation, keyFrameIn, LerpFunctions.Quaternion, OffsetFunctions.Quaternion, onComplete, delay);
        }
        
        public static Tween TweenRotateSequence(this Transform transform, Sequence<Quaternion> tweenSequence, System.Action onComplete = default!)
        {
            return TweenRunner.RunSequence(transform, kRotation, tweenSequence, onComplete, true);
        }
        
        public static Tween TweenLoopRotateSequence(this Transform transform, Sequence<Quaternion> tweenSequence, System.Action onComplete = default!)
        {
            return TweenRunner.RunSequence(transform, kRotation, tweenSequence, onComplete, false);
        }

        public static Tween TweenScale(this Transform transform, Vector3 scale, float duration, System.Action onComplete = default!,
            EasingFunction easingType = default!, float delay = 0)
        {
            var keyFrame = new KeyFrame<Vector3>(duration , scale, easingType ?? Easing.Cubic.InOut);
            return TweenRunner.RunTween(transform, kScale, keyFrame, LerpFunctions.Vector3, OffsetFunctions.Vector3, onComplete, delay);
        }

        public static Tween TweenLoopScale(this Transform transform, Vector3 scale, float duration, System.Action onComplete = default!,
            EasingFunction easingTypeIn = default!, float delay = 0)
        {
            var keyFrameIn = new KeyFrame<Vector3>(duration / 2, scale, easingTypeIn ?? Easing.Sinusoidal.InOut);
            return TweenRunner.RunTweenLooped(transform, kScale, keyFrameIn, LerpFunctions.Vector3, OffsetFunctions.Vector3, onComplete, delay);
        }

        public static Tween TweenScaleSequence(this Transform transform, Sequence<Vector3> tweenSequence, System.Action onComplete = default!)
        {
            return TweenRunner.RunSequence(transform, kScale, tweenSequence, onComplete, false);
        }
        
        public static Tween TweenLoopScaleSequence(this Transform transform, Sequence<Vector3> tweenSequence, System.Action onComplete = default!)
        {
            return TweenRunner.RunSequence(transform, kScale, tweenSequence, onComplete, true);
        }

    }
}