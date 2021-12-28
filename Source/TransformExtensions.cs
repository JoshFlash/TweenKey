using TweenKey.Interpolation;
using UnityEngine;

namespace TweenKey
{
    public static class TransformExtensions
    {
        private static void SetPosition(this Transform transform, Vector3 position) => transform.position = position;
        private static void SetRotation(this Transform transform, Quaternion rotation) => transform.rotation = rotation;
        private static void SetScale(this Transform transform, Vector3 scale) => transform.localScale = scale;


        public static Tween<Vector3> TweenMove(this Transform transform, Vector3 destination, float duration, System.Action onComplete = default!,
            EasingFunction easingType = default!, float delay = 0)
        {
            var tween = TweenCreator.Create(transform.SetPosition, transform.position, destination, duration, 
                LerpFunctions.Vector3, OffsetFunctions.Vector3, easingType ?? Easing.Cubic.InOut);
            
            return TweenRunner.RunTween(tween, Loop.Stop, onComplete, delay);
        }

        public static Tween<Vector3> TweenLoopMove(this Transform transform, Vector3 destination, float duration, 
            Loop loopOption = Loop.Reverse, System.Action onComplete = default!, EasingFunction easingType = default!, float delay = 0)
        {
            var tween = TweenCreator.Create(transform.SetPosition, transform.position, destination, duration, 
                LerpFunctions.Vector3, OffsetFunctions.Vector3, easingType ?? Easing.Sinusoidal.InOut);
            
            return TweenRunner.RunTween(tween, loopOption, onComplete, delay);
        }

        public static Tween<Vector3> TweenMoveSequence(this Transform transform, Sequence<Vector3> tweenSequence, System.Action onComplete = default!)
        {
            var tween = TweenCreator.CreateSequence(transform.SetPosition, tweenSequence);
            
            return TweenRunner.RunTween(tween, Loop.Stop, onComplete, 0f);
        }
        
        public static Tween<Vector3> TweenLoopMoveSequence(this Transform transform, Sequence<Vector3> tweenSequence,
            Loop loopOption = Loop.Replay, System.Action onComplete = default!)
        {
            var tween = TweenCreator.CreateSequence(transform.SetPosition, tweenSequence);
            
            return TweenRunner.RunTween(tween, loopOption, onComplete, 0f);
        }

        public static Tween<Quaternion> TweenToRotation(this Transform transform, Quaternion rotation, float duration, System.Action onComplete = default!,
            EasingFunction easingType = default!, float delay = 0)
        { 
            var tween = TweenCreator.Create(transform.SetRotation, transform.rotation, rotation, duration,
                LerpFunctions.Quaternion, OffsetFunctions.Quaternion, easingType ?? Easing.Cubic.InOut);
            
            return TweenRunner.RunTween(tween, Loop.Stop, onComplete, delay);
        }
        
        public static Tween<Quaternion> TweenByRotation(this Transform transform, Quaternion rotation, float duration, System.Action onComplete = default!,
            EasingFunction easingType = default!, float delay = 0)
        {
            var tween = TweenCreator.Create(transform.SetRotation, transform.rotation, transform.rotation * rotation, duration,
                LerpFunctions.Quaternion, OffsetFunctions.Quaternion, easingType ?? Easing.Cubic.InOut);
            
            return TweenRunner.RunTween(tween, Loop.Stop, onComplete, delay);
        }

        public static Tween<Quaternion> TweenLoopToRotation(this Transform transform, Quaternion rotation, float duration, 
            Loop loopOption = Loop.Reverse, System.Action onComplete = default!, EasingFunction easingType = default!, float delay = 0)
        {
            var tween = TweenCreator.Create(transform.SetRotation, transform.rotation, rotation, duration,
                LerpFunctions.Quaternion, OffsetFunctions.Quaternion, easingType ?? Easing.Cubic.InOut);
            
            return TweenRunner.RunTween(tween, loopOption, onComplete, delay);
        }

        public static Tween<Quaternion> TweenLoopByRotation(this Transform transform, Quaternion rotation, float duration, 
            Loop loopOption = Loop.Continue, System.Action onComplete = default!, EasingFunction easingType = default!, float delay = 0)
        {
            var tween = TweenCreator.Create(transform.SetRotation, transform.rotation, transform.rotation * rotation, duration,
                LerpFunctions.Quaternion, OffsetFunctions.Quaternion, easingType ?? Easing.Cubic.InOut);
            
            return TweenRunner.RunTween(tween, Loop.Stop, onComplete, delay);
        }
        
        public static Tween<Quaternion> TweenRotateSequence(this Transform transform, Sequence<Quaternion> tweenSequence, System.Action onComplete = default!)
        {
            var tween = TweenCreator.CreateSequence(transform.SetRotation, tweenSequence);
            
            return TweenRunner.RunTween(tween, Loop.Stop, onComplete, 0f);
        }
        
        public static Tween<Quaternion> TweenLoopRotateSequence(this Transform transform, Sequence<Quaternion> tweenSequence, 
            Loop loopOption = Loop.Continue, System.Action onComplete = default!)
        {
            var tween = TweenCreator.CreateSequence(transform.SetRotation, tweenSequence);
            
            return TweenRunner.RunTween(tween, loopOption, onComplete, 0f);
        }

        public static Tween<Vector3> TweenScale(this Transform transform, Vector3 scale, float duration, System.Action onComplete = default!,
            EasingFunction easingType = default!, float delay = 0)
        {
            var tween = TweenCreator.Create(transform.SetScale, transform.localScale, scale, duration, 
                LerpFunctions.Vector3, OffsetFunctions.Vector3, easingType ?? Easing.Cubic.InOut);
            
            return TweenRunner.RunTween(tween, Loop.Stop, onComplete, delay);
        }

        public static Tween<Vector3> TweenLoopScale(this Transform transform, Vector3 scale, float duration, 
            Loop loopOption = Loop.Reverse, System.Action onComplete = default!, EasingFunction easingType = default!, float delay = 0)
        {
            var tween = TweenCreator.Create(transform.SetScale, transform.localScale, scale, duration, 
                LerpFunctions.Vector3, OffsetFunctions.Vector3, easingType ?? Easing.Sinusoidal.InOut);
            
            return TweenRunner.RunTween(tween, loopOption, onComplete, delay);
        }

        public static Tween<Vector3> TweenScaleSequence(this Transform transform, Sequence<Vector3> tweenSequence, System.Action onComplete = default!)
        {
            var tween = TweenCreator.CreateSequence(transform.SetScale, tweenSequence);
            
            return TweenRunner.RunTween(tween, Loop.Stop, onComplete, 0f);
        }
        
        public static Tween<Vector3> TweenLoopScaleSequence(this Transform transform, Sequence<Vector3> tweenSequence,
            Loop loopOption = Loop.Replay, System.Action onComplete = default!)
        {
            var tween = TweenCreator.CreateSequence(transform.SetScale, tweenSequence);
            
            return TweenRunner.RunTween(tween, loopOption, onComplete, 0f);
        }

    }
}