using TweenKey.Interpolation;
using UnityEngine;

namespace TweenKey
{
    public static class TransformExtensions
    {
        private static void SetPosition(this Transform transform, Vector3 position) => transform.position = position;
        private static void SetRotation(this Transform transform, Quaternion rotation) => transform.rotation = rotation;
        private static void SetScale(this Transform transform, Vector3 scale) => transform.localScale = scale;


        public static Tween TweenMove(this Transform transform, Vector3 destination, float duration, System.Action onComplete = default!,
            EasingFunction easingType = default!, float delay = 0)
        {
            var keyFrame = new KeyFrame<Vector3>(duration, destination, easingType ?? Easing.Cubic.InOut);
            return TweenRunner.RunTween(transform.SetPosition, transform.position, keyFrame, LerpFunctions.Vector3, OffsetFunctions.Vector3, Loop.Stop, onComplete , delay);
        }

        public static Tween TweenLoopMove(this Transform transform, Vector3 destination, float duration, 
            Loop loopOption = Loop.Reverse, System.Action onComplete = default!, EasingFunction easingTypeIn = default!, float delay = 0)
        {
            var keyFrame = new KeyFrame<Vector3>(duration, destination, easingTypeIn ?? Easing.Sinusoidal.InOut);
            return TweenRunner.RunTween(transform.SetPosition, transform.position, keyFrame, LerpFunctions.Vector3, OffsetFunctions.Vector3, loopOption, onComplete, delay);
        }

        public static Tween TweenMoveSequence(this Transform transform, Sequence<Vector3> tweenSequence, System.Action onComplete = default!)
        {
            return TweenRunner.RunSequence(transform.SetPosition, transform.position, tweenSequence, Loop.Stop, onComplete);
        }
        
        public static Tween TweenLoopMoveSequence(this Transform transform, Sequence<Vector3> tweenSequence,
            Loop loopOption = Loop.Replay, System.Action onComplete = default!)
        {
            return TweenRunner.RunSequence(transform.SetPosition, transform.position, tweenSequence, loopOption, onComplete);
        }

        public static Tween TweenToRotation(this Transform transform, Quaternion rotation, float duration, System.Action onComplete = default!,
            EasingFunction easingType = default!, float delay = 0)
        {
            var keyFrame = new KeyFrame<Quaternion>(duration, rotation, easingType ?? Easing.Cubic.InOut);
            return TweenRunner.RunTween(transform.SetRotation, transform.rotation, keyFrame, LerpFunctions.Quaternion, OffsetFunctions.Quaternion, Loop.Stop, onComplete, delay);
        }
        
        public static Tween TweenByRotation(this Transform transform, Quaternion rotation, float duration, System.Action onComplete = default!,
            EasingFunction easingType = default!, float delay = 0)
        {
            var keyFrame = new KeyFrame<Quaternion>(duration, transform.rotation * rotation, easingType ?? Easing.Cubic.In);
            return TweenRunner.RunTween(transform.SetRotation, transform.rotation, keyFrame, LerpFunctions.Quaternion, OffsetFunctions.Quaternion, Loop.Stop, onComplete, delay);
        }

        public static Tween TweenLoopToRotation(this Transform transform, Quaternion rotation, float duration, 
            Loop loopOption = Loop.Reverse, System.Action onComplete = default!,
            EasingFunction easingTypeIn = default!, float delay = 0)
        {
            var keyFrame = new KeyFrame<Quaternion>(duration, rotation, easingTypeIn ?? Easing.Sinusoidal.InOut);
            return TweenRunner.RunTween(transform.SetRotation, transform.rotation, keyFrame, LerpFunctions.Quaternion, OffsetFunctions.Quaternion, loopOption, onComplete, delay);
        }

        public static Tween TweenLoopByRotation(this Transform transform, Quaternion rotation, float duration, 
            Loop loopOption = Loop.Continue, System.Action onComplete = default!, EasingFunction easingTypeIn = default!, float delay = 0)
        {
            var keyFrame = new KeyFrame<Quaternion>(duration / 2, transform.rotation * rotation, easingTypeIn ?? Easing.Sinusoidal.InOut);
            return TweenRunner.RunTween(transform.SetRotation, transform.rotation, keyFrame, LerpFunctions.Quaternion, OffsetFunctions.Quaternion, loopOption, onComplete, delay);
        }
        
        public static Tween TweenRotateSequence(this Transform transform, Sequence<Quaternion> tweenSequence, System.Action onComplete = default!)
        {
            return TweenRunner.RunSequence(transform.SetRotation, transform.rotation, tweenSequence, Loop.Stop, onComplete);
        }
        
        public static Tween TweenLoopRotateSequence(this Transform transform, Sequence<Quaternion> tweenSequence, 
            Loop loopOption = Loop.Continue, System.Action onComplete = default!)
        {
            return TweenRunner.RunSequence(transform.SetRotation, transform.rotation, tweenSequence, loopOption, onComplete);
        }

        public static Tween TweenScale(this Transform transform, Vector3 scale, float duration, System.Action onComplete = default!,
            EasingFunction easingType = default!, float delay = 0)
        {
            var keyFrame = new KeyFrame<Vector3>(duration , scale, easingType ?? Easing.Cubic.InOut);
            return TweenRunner.RunTween(transform.SetScale, transform.localScale, keyFrame, LerpFunctions.Vector3, OffsetFunctions.Vector3, Loop.Stop, onComplete, delay);
        }

        public static Tween TweenLoopScale(this Transform transform, Vector3 scale, float duration, 
            Loop loopOption = Loop.Reverse, System.Action onComplete = default!, EasingFunction easingTypeIn = default!, float delay = 0)
        {
            var keyFrame = new KeyFrame<Vector3>(duration / 2, scale, easingTypeIn ?? Easing.Sinusoidal.InOut);
            return TweenRunner.RunTween(transform.SetScale, transform.localScale, keyFrame, LerpFunctions.Vector3, OffsetFunctions.Vector3, loopOption, onComplete, delay);
        }

        public static Tween TweenScaleSequence(this Transform transform, Sequence<Vector3> tweenSequence, System.Action onComplete = default!)
        {
            return TweenRunner.RunSequence(transform.SetScale, transform.localScale, tweenSequence, Loop.Stop, onComplete);
        }
        
        public static Tween TweenLoopScaleSequence(this Transform transform, Sequence<Vector3> tweenSequence,
            Loop loopOption = Loop.Replay, System.Action onComplete = default!)
        {
            return TweenRunner.RunSequence(transform.SetScale, transform.localScale, tweenSequence, loopOption, onComplete);
        }

    }
}