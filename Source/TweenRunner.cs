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
        
        public static Tween<T> RunTween<T>(Tween<T> tween, Loop loopOption, System.Action onComplete, float delay)
        {
            tween.SetLooping(loopOption);
            tween.SetCallback(onComplete);
            tween.SetDelay(delay);
            
            _tweensQueue.Enqueue(tween);
            return tween;
        }
        
        public static Tween<T> RunTween<T>(Tween<T> tween)
        {
            _tweensQueue.Enqueue(tween);
            return tween;
        }
    }
}