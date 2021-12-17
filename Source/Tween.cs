using System;
using System.Collections.Generic;
using System.Linq;
using TweenKey.Interpolation;

namespace TweenKey
{
    public delegate void TweenSetter<in T>(T newValue);

    public enum Loop { Stop, Continue, Reverse, Replay }
    
    public class Tween
    {
        private List<ITweeningValue> _tweeningValues;
        private Queue<ITweeningValue> _expiredValues;

        private float _elapsed;
        private bool _isRunning;
        private Loop _loop = Loop.Stop;

        public bool isExpired { get; private set; }

        public Tween(Loop loop = Loop.Stop)
        {
            _tweeningValues = new List<ITweeningValue>();
            _expiredValues = new Queue<ITweeningValue>();
            _isRunning = true;
            _loop = loop;
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
                    if (_loop == Loop.Stop)
                    {
                        _isRunning = false;
                        isExpired = true;
                        _expiredValues.Clear();
                        return;
                    }

                    while (_expiredValues.Count > 0)
                    {
                        var value = _expiredValues.Dequeue();
                        switch (_loop)
                        {
                            case Loop.Continue:
                                value.AddOffset();
                                break;
                            case Loop.Reverse:
                                value.Reverse();
                                break;
                            case Loop.Replay:
                                break;
                        }
                        value.isExpired = false;
                        _tweeningValues.Add(value);
                    }
                    
                    _elapsed = 0;
                }
            }
        }
        
        public TweeningValue<T> AddValue<T>(object target, string propertyName, LerpFunction<T> lerpFunction, OffsetFunction<T> offsetFunction, Action onComplete)
        {
            var property = target.GetType().GetProperties().FirstOrDefault(x => x.Name == propertyName);
            if (property != null)
            {
                if (property.PropertyType == typeof(T))
                {
                    var t = new TweeningValue<T>(target, property, lerpFunction, offsetFunction, onComplete);
                    _tweeningValues.Add(t);
                    return t;
                }
            }
            
            var field = target.GetType().GetFields().FirstOrDefault(x => x.Name == propertyName);
            if (field != null)
            {
                if (field.FieldType == typeof(T))
                {
                    var t = new TweeningValue<T>(target, field, lerpFunction, offsetFunction, onComplete);
                    _tweeningValues.Add(t);
                    return t;
                }
            }
            
            return null!;
        }
        
        public TweeningValue<T> AddValue<T>(TweenSetter<T> setter, T initialValue, LerpFunction<T> lerpFunction, OffsetFunction<T> offsetFunction, Action onComplete)
        {
            var t = new TweeningValue<T>(setter, initialValue, lerpFunction, offsetFunction, onComplete);
            _tweeningValues.Add(t);
            return t;
        }
    }

}