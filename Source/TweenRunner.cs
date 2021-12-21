using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweenKey.Interpolation;
using UnityEngine;

namespace TweenKey
{
    public static class TweenRunner
    {
        private static readonly List<ITween> _tweens = new List<ITween>();
        private static readonly Queue<ITween> _tweensQueue = new Queue<ITween>();

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
                ITween tween = _tweens[i];
                tween.Update(deltaTime);
                if (tween.isExpired)
                {
                    _tweens.Remove(tween);
                }
            }

            await Task.Yield();
        }
        
        public static Tween<T> RunTween<T>
            (object target, string valueName, KeyFrame<T> keyFrame, LerpFunction<T> lerpFunction, OffsetFunction<T> offsetFunction, Loop loopOption,
                 System.Action onComplete, float delay)
        {
            var tween = CreateTween(target, valueName, lerpFunction, offsetFunction, onComplete);
            tween.SetLooping(loopOption);
            
            keyFrame.frame += delay;
            tween.AddFrame(new KeyFrame<T>(delay, tween.initialValue, Easing.Linear));
            tween.AddFrame(keyFrame);
            
            _tweensQueue.Enqueue(tween);
            return tween;
        }

        public static Tween<T> RunSequence<T>
            (object target, string valueName, Sequence<T> sequence, Loop loopOption, System.Action onComplete)
        {
            var tween = CreateTween(target, valueName, sequence.lerpFunction, sequence.offsetFunction, onComplete);
            tween.SetLooping(loopOption);
            
            foreach (var keyFrame in sequence.keyFrames)
            {
                tween.AddFrame(keyFrame);
            }
            
            _tweensQueue.Enqueue(tween);
            return tween;
        }

        public static Tween<T> RunTween<T>
        (TweenSetter<T> setter, T initialValue, KeyFrame<T> keyFrame, LerpFunction<T> lerpFunction, OffsetFunction<T> offsetFunction, Loop loopOption,
            System.Action onComplete, float delay)
        {
            var tween = CreateTween(setter, initialValue, lerpFunction, offsetFunction, onComplete);
            tween.SetLooping(loopOption);
            
            keyFrame.frame += delay;
            tween.AddFrame(new KeyFrame<T>(delay, tween.initialValue, Easing.Linear));
            tween.AddFrame(keyFrame);
            
            _tweensQueue.Enqueue(tween);
            return tween;
        }
        
        public static Tween<T> RunSequence<T>
            (TweenSetter<T> setter, T initialValue, Sequence<T> sequence, Loop loopOption, System.Action onComplete)
        {
            var tween = CreateTween(setter, initialValue, sequence.lerpFunction, sequence.offsetFunction, onComplete);
            tween.SetLooping(loopOption);
            
            foreach (var keyFrame in sequence.keyFrames)
            {
                tween.AddFrame(keyFrame);
            }
            
            _tweensQueue.Enqueue(tween);
            return tween;
        }
        
        private static Tween<T> CreateTween<T>(object target, string propertyName, LerpFunction<T> lerpFunction, OffsetFunction<T> offsetFunction, System.Action onComplete)
        {
            var property = target.GetType().GetProperties().FirstOrDefault(x => x.Name == propertyName);
            if (property != null)
            {
                if (property.PropertyType == typeof(T))
                {
                    var t = new Tween<T>(target, property, lerpFunction, offsetFunction, onComplete);
                    return t;
                }
            }
            
            var field = target.GetType().GetFields().FirstOrDefault(x => x.Name == propertyName);
            if (field != null)
            {
                if (field.FieldType == typeof(T))
                {
                    var t = new Tween<T>(target, field, lerpFunction, offsetFunction, onComplete);
                    return t;
                }
            }
            
            return null!;
        }
        
        public static Tween<T> CreateTween<T>(TweenSetter<T> setter, T initialValue, LerpFunction<T> lerpFunction, OffsetFunction<T> offsetFunction, System.Action onComplete)
        {
            var t = new Tween<T>(setter, initialValue, lerpFunction, offsetFunction, onComplete);
            return t;
        }
    }
}