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
            return TweenRunner.RunTween(transform, kPosition, keyFrame, LerpFunctions.Vector3, onComplete , delay);
        }

        public static void TweenLoopMove(this Transform transform, Vector3 destination, float duration, System.Action onComplete = default!,
            EasingFunction easingTypeIn = default!, EasingFunction easingTypeOut = default!, float delay = 0)
        {
            var keyFrameIn = new KeyFrame<Vector3>((duration / 2) , destination, easingTypeIn ?? Easing.Sinusoidal.InOut);
            var keyFrameOut = new KeyFrame<Vector3>(duration , transform.position, easingTypeOut ?? Easing.Sinusoidal.InOut);
            TweenRunner.RunTweenLooped(transform, kPosition, keyFrameIn, keyFrameOut, LerpFunctions.Vector3, onComplete , delay);
        }

        public static void TweenMoveSequence(this Transform transform, Sequence<Vector3> tweenSequence , System.Action onComplete = default!)
        {
            TweenRunner.RunSequence(transform, kPosition, tweenSequence, onComplete);
        }
        
        public static void TweenLoopMoveSequence(this Transform transform, Sequence<Vector3> tweenSequence, System.Action onComplete = default!)
        {
            TweenRunner.RunSequence(transform, kPosition, tweenSequence, onComplete, true);
        }

        public static void TweenToRotation(this Transform transform, Quaternion rotation, float duration, System.Action onComplete = default!,
            EasingFunction easingType = default!, float delay = 0)
        {
            var keyFrame = new KeyFrame<Quaternion>(duration , rotation, easingType ?? Easing.Cubic.InOut);
            TweenRunner.RunTween(transform, kRotation, keyFrame, LerpFunctions.Quaternion, onComplete, delay);
        }
        
        public static void TweenByRotation(this Transform transform, Quaternion rotation, float duration, System.Action onComplete = default!,
            EasingFunction easingType = default!, float delay = 0)
        {
            var keyFrame = new KeyFrame<Quaternion>(duration, transform.rotation * rotation, easingType ?? Easing.Cubic.In);
            TweenRunner.RunTween(transform, kRotation, keyFrame, LerpFunctions.Quaternion, onComplete, delay);
        }

        public static void TweenLoopToRotation(this Transform transform, Quaternion rotation, float duration, System.Action onComplete = default!,
            EasingFunction easingTypeIn = default!, EasingFunction easingTypeOut = default!, float delay = 0)
        {
            var keyFrameIn = new KeyFrame<Quaternion>((duration / 2) , rotation, easingTypeIn ?? Easing.Sinusoidal.InOut);
            var keyFrameOut = new KeyFrame<Quaternion>(duration, transform.rotation, easingTypeOut ?? Easing.Sinusoidal.InOut);
            TweenRunner.RunTweenLooped(transform, kRotation, keyFrameIn, keyFrameOut, LerpFunctions.Quaternion, onComplete, delay);
        }

        public static void TweenLoopByRotation(this Transform transform, Quaternion rotation, float duration, System.Action onComplete = default!,
            EasingFunction easingTypeIn = default!, EasingFunction easingTypeOut = default!, float delay = 0)
        {
            var keyFrameIn = new KeyFrame<Quaternion>((duration / 2) , rotation, easingTypeIn ?? Easing.Sinusoidal.InOut);
            var keyFrameOut = new KeyFrame<Quaternion>(duration , transform.rotation * rotation, easingTypeOut ?? Easing.Sinusoidal.InOut);
            TweenRunner.RunTweenLooped(transform, kRotation, keyFrameIn, keyFrameOut, LerpFunctions.Quaternion, onComplete, delay);
        }
        
        public static void TweenRotateSequence(this Transform transform, Sequence<Quaternion> tweenSequence, System.Action onComplete = default!)
        {
            TweenRunner.RunSequence(transform, kRotation, tweenSequence, onComplete, true);
        }
        
        public static void TweenLoopRotateSequence(this Transform transform, Sequence<Quaternion> tweenSequence, System.Action onComplete = default!)
        {
            TweenRunner.RunSequence(transform, kRotation, tweenSequence, onComplete);
        }

        public static void TweenScale(this Transform transform, Vector3 scale, float duration, System.Action onComplete = default!,
            EasingFunction easingType = default!, float delay = 0)
        {
            var keyFrame = new KeyFrame<Vector3>(duration , scale, easingType ?? Easing.Cubic.InOut);
            TweenRunner.RunTween(transform, kScale, keyFrame, LerpFunctions.Vector3, onComplete, delay);
        }

        public static void TweenLoopScale(this Transform transform, Vector3 scale, float duration, System.Action onComplete = default!,
            EasingFunction easingTypeIn = default!, EasingFunction easingTypeOut = default!, float delay = 0)
        {
            var keyFrameIn = new KeyFrame<Vector3>((duration / 2) , scale, easingTypeIn ?? Easing.Sinusoidal.InOut);
            var keyFrameOut = new KeyFrame<Vector3>(duration , transform.localScale,  easingTypeOut ?? Easing.Sinusoidal.InOut);
            TweenRunner.RunTweenLooped(transform, kScale, keyFrameIn, keyFrameOut, LerpFunctions.Vector3, onComplete, delay);
        }

        public static void TweenScaleSequence(this Transform transform, Sequence<Vector3> tweenSequence, System.Action onComplete = default!)
        {
            TweenRunner.RunSequence(transform, kScale, tweenSequence, onComplete);
        }
        
        public static void TweenLoopScaleSequence(this Transform transform, Sequence<Vector3> tweenSequence, System.Action onComplete = default!)
        {
            TweenRunner.RunSequence(transform, kScale, tweenSequence, onComplete, true);
        }

    }
}