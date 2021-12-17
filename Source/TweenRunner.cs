using System.Collections.Generic;
using System.Threading.Tasks;
using TweenKey.Interpolation;
using UnityEngine;

namespace TweenKey
{
    public static class TweenRunner
    {
        private static readonly List<Tween> _tweens = new List<Tween>();
        private static readonly Queue<Tween> _tweensQueue = new Queue<Tween>();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static async void Init()
        {
            while (Application.isPlaying)
            {
                await UpdateTweens(Time.deltaTime);
            }
        }

        private static async Task UpdateTweens(float deltaTime)
        {
            while (_tweensQueue.Count > 0)
            {
                _tweens.Add(_tweensQueue.Dequeue());
            }

            for (int i = _tweens.Count - 1; i >= 0; i--)
            {
                Tween tween = _tweens[i];
                tween.Update(deltaTime);
                if (tween.isExpired)
                {
                    _tweens.Remove(tween);
                }
            }

            await Task.Yield();
        }
        
        public static Tween RunTween<T>
            (object target, string valueName, KeyFrame<T> keyFrame, LerpFunction<T> lerpFunction, OffsetFunction<T> offsetFunction, Loop loopOption,
                 System.Action onComplete, float delay)
        {
            var tween = new Tween(loopOption);
            
            var tweeningValue = tween.AddValue(target, valueName, lerpFunction, offsetFunction, onComplete);
            keyFrame.frame += delay;
            tweeningValue.AddFrame(new KeyFrame<T>(delay, tweeningValue.initialValue, Easing.Linear));
            tweeningValue.AddFrame(keyFrame);
            
            _tweensQueue.Enqueue(tween);
            return tween;
        }

        public static Tween RunSequence<T>
            (object target, string valueName, Sequence<T> sequence, Loop loopOption, System.Action onComplete)
        {
            var tween = new Tween(loopOption);
            
            var tweeningValue = tween.AddValue(target, valueName, sequence.lerpFunction, null, onComplete);
            
            foreach (var keyFrame in sequence.keyFrames)
            {
                tweeningValue.AddFrame(keyFrame);
            }
            
            _tweensQueue.Enqueue(tween);
            return tween;
        }

        public static Tween RunTween<T>
        (TweenSetter<T> setter, T initialValue, KeyFrame<T> keyFrame, LerpFunction<T> lerpFunction, OffsetFunction<T> offsetFunction, Loop loopOption,
            System.Action onComplete, float delay)
        {
            var tween = new Tween(loopOption);
            
            var tweeningValue = tween.AddValue(setter, initialValue, lerpFunction, offsetFunction, onComplete);
            keyFrame.frame += delay;
            tweeningValue.AddFrame(new KeyFrame<T>(delay, tweeningValue.initialValue, Easing.Linear));
            tweeningValue.AddFrame(keyFrame);
            
            _tweensQueue.Enqueue(tween);
            return tween;
        }
        
        public static Tween RunSequence<T>
            (TweenSetter<T> setter, T initialValue, Sequence<T> sequence, Loop loopOption, System.Action onComplete)
        {
            var tween = new Tween(loopOption);
            
            var tweeningValue = tween.AddValue(setter, initialValue, sequence.lerpFunction, null, onComplete);
            
            foreach (var keyFrame in sequence.keyFrames)
            {
                tweeningValue.AddFrame(keyFrame);
            }
            
            _tweensQueue.Enqueue(tween);
            return tween;
        }
    }
}