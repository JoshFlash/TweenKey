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
            return TweenRunner.RunTween(transform, kPosition, keyFrame, LerpFunctions.Vector3, OffsetFunctions.Vector3, Loop.Stop, onComplete , delay);
        }

        public static Tween TweenLoopMove(this Transform transform, Vector3 destination, float duration, 
            Loop loopOption = Loop.Reverse, System.Action onComplete = default!, EasingFunction easingTypeIn = default!, float delay = 0)
        {
            var keyFrameIn = new KeyFrame<Vector3>(duration, destination, easingTypeIn ?? Easing.Sinusoidal.InOut);
            return TweenRunner.RunTween(transform, kPosition, keyFrameIn, LerpFunctions.Vector3, OffsetFunctions.Vector3, loopOption, onComplete, delay);
        }

        public static Tween TweenMoveSequence(this Transform transform, Sequence<Vector3> tweenSequence, System.Action onComplete = default!)
        {
            return TweenRunner.RunSequence(transform, kPosition, tweenSequence, Loop.Stop, onComplete);
        }
        
        public static Tween TweenLoopMoveSequence(this Transform transform, Sequence<Vector3> tweenSequence,
            Loop loopOption = Loop.Reverse, System.Action onComplete = default!)
        {
            return TweenRunner.RunSequence(transform, kPosition, tweenSequence, loopOption, onComplete);
        }

        public static Tween TweenToRotation(this Transform transform, Quaternion rotation, float duration, System.Action onComplete = default!,
            EasingFunction easingType = default!, float delay = 0)
        {
            var keyFrame = new KeyFrame<Quaternion>(duration , rotation, easingType ?? Easing.Cubic.InOut);
            return TweenRunner.RunTween(transform, kRotation, keyFrame, LerpFunctions.Quaternion, OffsetFunctions.Quaternion, Loop.Stop, onComplete, delay);
        }
        
        public static Tween TweenByRotation(this Transform transform, Quaternion rotation, float duration, System.Action onComplete = default!,
            EasingFunction easingType = default!, float delay = 0)
        {
            var keyFrame = new KeyFrame<Quaternion>(duration, transform.rotation * rotation, easingType ?? Easing.Cubic.In);
            return TweenRunner.RunTween(transform, kRotation, keyFrame, LerpFunctions.Quaternion, OffsetFunctions.Quaternion, Loop.Stop, onComplete, delay);
        }

        public static Tween TweenLoopToRotation(this Transform transform, Quaternion rotation, float duration, 
            Loop loopOption = Loop.Reverse, System.Action onComplete = default!,
            EasingFunction easingTypeIn = default!, float delay = 0)
        {
            var keyFrameIn = new KeyFrame<Quaternion>(duration, rotation, easingTypeIn ?? Easing.Sinusoidal.InOut);
            return TweenRunner.RunTween(transform, kRotation, keyFrameIn, LerpFunctions.Quaternion, OffsetFunctions.Quaternion, loopOption, onComplete, delay);
        }

        public static Tween TweenLoopByRotation(this Transform transform, Quaternion rotation, float duration, 
            Loop loopOption = Loop.Continue, System.Action onComplete = default!, EasingFunction easingTypeIn = default!, float delay = 0)
        {
            var keyFrameIn = new KeyFrame<Quaternion>(duration / 2, transform.rotation * rotation, easingTypeIn ?? Easing.Sinusoidal.InOut);
            return TweenRunner.RunTween(transform, kRotation, keyFrameIn, LerpFunctions.Quaternion, OffsetFunctions.Quaternion, loopOption, onComplete, delay);
        }
        
        public static Tween TweenRotateSequence(this Transform transform, Sequence<Quaternion> tweenSequence, System.Action onComplete = default!)
        {
            return TweenRunner.RunSequence(transform, kRotation, tweenSequence, Loop.Stop, onComplete);
        }
        
        public static Tween TweenLoopRotateSequence(this Transform transform, Sequence<Quaternion> tweenSequence, 
            Loop loopOption = Loop.Continue, System.Action onComplete = default!)
        {
            return TweenRunner.RunSequence(transform, kRotation, tweenSequence, loopOption, onComplete);
        }

        public static Tween TweenScale(this Transform transform, Vector3 scale, float duration, System.Action onComplete = default!,
            EasingFunction easingType = default!, float delay = 0)
        {
            var keyFrame = new KeyFrame<Vector3>(duration , scale, easingType ?? Easing.Cubic.InOut);
            return TweenRunner.RunTween(transform, kScale, keyFrame, LerpFunctions.Vector3, OffsetFunctions.Vector3, Loop.Stop, onComplete, delay);
        }

        public static Tween TweenLoopScale(this Transform transform, Vector3 scale, float duration, 
            Loop loopOption = Loop.Reverse, System.Action onComplete = default!, EasingFunction easingTypeIn = default!, float delay = 0)
        {
            var keyFrameIn = new KeyFrame<Vector3>(duration / 2, scale, easingTypeIn ?? Easing.Sinusoidal.InOut);
            return TweenRunner.RunTween(transform, kScale, keyFrameIn, LerpFunctions.Vector3, OffsetFunctions.Vector3, loopOption, onComplete, delay);
        }

        public static Tween TweenScaleSequence(this Transform transform, Sequence<Vector3> tweenSequence, System.Action onComplete = default!)
        {
            return TweenRunner.RunSequence(transform, kScale, tweenSequence, Loop.Stop, onComplete);
        }
        
        public static Tween TweenLoopScaleSequence(this Transform transform, Sequence<Vector3> tweenSequence,
            Loop loopOption = Loop.Reverse, System.Action onComplete = default!)
        {
            return TweenRunner.RunSequence(transform, kScale, tweenSequence, loopOption, onComplete);
        }

    }
}