
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweenKey.Interpolation;

namespace TweenKey
{
    public class Tween
    {
        public List<ITweeningValue> tweeningValues;
        public Queue<ITweeningValue> expiredValues;
        
        public float elapsed;
        public bool loop;
        public bool isRunning;
        
        public Tween()
        {
            tweeningValues = new List<ITweeningValue>();
            expiredValues = new Queue<ITweeningValue>();
            isRunning = true;
        }
        
        public async Task Update(float deltaTime)
        {
            if (isRunning)
            {
                elapsed += deltaTime;

                for (int i = tweeningValues.Count - 1; i >= 0; i--)
                {
                    var tweenValue = tweeningValues[i];
                    tweenValue.Update(elapsed);
                    if (tweenValue.isExpired)
                    {
                        tweeningValues.Remove(tweenValue);
                        expiredValues.Enqueue(tweenValue);
                    }
                }

                if (tweeningValues.Count == 0)
                {
                    isRunning = loop;
                    if (isRunning)
                    {
                        while (expiredValues.Count > 0)
                        {
                            tweeningValues.Add(expiredValues.Dequeue());
                        }
                        elapsed = 0;
                        tweeningValues.ForEach(val => val.isExpired = false);
                    }
                }
            }
            await Task.Yield();
        }
        
        public TweeningValue<T> AddValue<T>(object target, string propertyName, LerpFunction<T> lerpFunction, Action onComplete)
        {
            var property = target.GetType().GetProperties().FirstOrDefault(x => x.Name == propertyName);
            if (property != null)
            {
                if (property.PropertyType == typeof(T))
                {
                    var t = new TweeningValue<T>(target, property, lerpFunction, onComplete);
                    tweeningValues.Add(t);
                    return t;
                }
            }
            
            var field = target.GetType().GetFields().FirstOrDefault(x => x.Name == propertyName);
            if (field != null)
            {
                if (field.FieldType == typeof(T))
                {
                    var t = new TweeningValue<T>(target, field, lerpFunction, onComplete);
                    tweeningValues.Add(t);
                    return t;
                }
            }
            
            return null;
        }
    }

}