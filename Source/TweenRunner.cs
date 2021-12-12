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
                await UpdateTweens();
            }
        }

        private static async Task UpdateTweens()
        {
            while (_tweensQueue.Count > 0)
            {
                _tweens.Add(_tweensQueue.Dequeue());
            }

            _tweens.ForEach(tween => tween.Update(Time.deltaTime));
            _tweens.RemoveAll(tween => tween.isExpired);
            
            await Task.Yield();
        }
        
        public static Tween RunTween<T>
            (object target, string valueName, KeyFrame<T> keyFrame, LerpFunction<T> lerpFunction, System.Action onComplete, float delay)
        {
            var tween = new Tween();
            
            var tweeningValue = tween.AddValue(target, valueName, lerpFunction, onComplete);
            keyFrame.frame += delay;
            tweeningValue.AddFrame(new KeyFrame<T>(delay, tweeningValue.initialValue, Easing.Linear));
            tweeningValue.AddFrame(keyFrame);
            
            _tweensQueue.Enqueue(tween);
            return tween;
        }
        
        public static Tween RunTweenLooped<T>
            (object target, string valueName, KeyFrame<T> keyFrameIn, KeyFrame<T> keyFrameOut, LerpFunction<T> lerpFunction, System.Action onComplete, float delay)
        {
            var tween = new Tween(loop: true);

            var tweeningValue = tween.AddValue(target, valueName, lerpFunction, onComplete);
            keyFrameIn.frame += delay;
            keyFrameOut.frame += delay;
            tweeningValue.AddFrame(new KeyFrame<T>(delay, tweeningValue.initialValue, Easing.Linear));
            tweeningValue.AddFrame(keyFrameIn);
            tweeningValue.AddFrame(keyFrameOut);
            
            _tweensQueue.Enqueue(tween);
            return tween;
        }

        public static Tween RunSequence<T>
            (object target, string valueName, Sequence<T> sequence, System.Action onComplete, bool looped)
        {
            var tween = new Tween(looped);
            
            var tweeningValue = tween.AddValue(target, valueName, sequence.lerpFunction, onComplete);
            tweeningValue.AddFrame(new KeyFrame<T>(0, tweeningValue.initialValue, Easing.Linear));
            
            foreach (var keyFrame in sequence.keyFrames)
            {
                tweeningValue.AddFrame(keyFrame);
            }
            
            _tweensQueue.Enqueue(tween);
            return tween;
        }
    }
}