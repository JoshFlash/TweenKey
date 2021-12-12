using System;
using System.Collections.Generic;
using System.Linq;
using TweenKey.Interpolation;

namespace TweenKey
{
    public class Tween
    {
        private List<ITweeningValue> _tweeningValues;
        private Queue<ITweeningValue> _expiredValues;

        private float _elapsed;
        private bool _isRunning;

        public bool loop { get; init; }

        public bool isExpired => _tweeningValues.Count == 0 && _expiredValues.Count > 0;
        
        public Tween()
        {
            _tweeningValues = new List<ITweeningValue>();
            _expiredValues = new Queue<ITweeningValue>();
            _isRunning = true;
        }
        
        public void Update(float deltaTime)
        {
            if (_isRunning)
            {
                _elapsed += deltaTime;

                for (int i = _tweeningValues.Count - 1; i >= 0; i--)
                {
                    var tweenValue = _tweeningValues[i];
                    tweenValue.Update(_elapsed);
                    if (tweenValue.isExpired)
                    {
                        _tweeningValues.Remove(tweenValue);
                        _expiredValues.Enqueue(tweenValue);
                    }
                }

                if (_tweeningValues.Count == 0)
                {
                    _isRunning = loop;
                    if (_isRunning)
                    {
                        while (_expiredValues.Count > 0)
                        {
                            _tweeningValues.Add(_expiredValues.Dequeue());
                        }
                        _elapsed = 0;
                        _tweeningValues.ForEach(val => val.isExpired = false);
                    }
                }
            }
        }
        
        public TweeningValue<T> AddValue<T>(object target, string propertyName, LerpFunction<T> lerpFunction, Action onComplete)
        {
            var property = target.GetType().GetProperties().FirstOrDefault(x => x.Name == propertyName);
            if (property != null)
            {
                if (property.PropertyType == typeof(T))
                {
                    var t = new TweeningValue<T>(target, property, lerpFunction, onComplete);
                    _tweeningValues.Add(t);
                    return t;
                }
            }
            
            var field = target.GetType().GetFields().FirstOrDefault(x => x.Name == propertyName);
            if (field != null)
            {
                if (field.FieldType == typeof(T))
                {
                    var t = new TweeningValue<T>(target, field, lerpFunction, onComplete);
                    _tweeningValues.Add(t);
                    return t;
                }
            }
            
            return null!;
        }
    }

}