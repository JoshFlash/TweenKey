using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TweenKey.Interpolation;

namespace TweenKey
{
    public delegate void TweenSetter<in T>(T newValue);

    public enum Loop { Stop, Continue, Reverse, Replay }

    public interface ITween
    {
        void Update(float deltaTime);
        bool isExpired { get; set; }
    }
    
    public class Tween<T> : ITween
    {
        internal T initialValue { get; }

        private readonly TweenSetter<T> SetValue;
        private Action onComplete { get; }

        private List<KeyFrame<T>> keyFrames { get; }
        private LerpFunction<T> lerpFunction { get; }
        private OffsetFunction<T> offsetFunction { get; }

        private object _target = null!;
        private PropertyInfo _property = null!;
        private FieldInfo _field = null!;

        private KeyFrame<T> _lastKey = null;
        private KeyFrame<T> _nextKey = null;
        private int _nextKeyIndex = 1;
        
        private float _elapsed;
        private Loop _loop = Loop.Stop;

        public bool isExpired { get; set; }

        public void SetLooping(Loop loop)
        {
            _loop = loop;
        }

        public void Update(float deltaTime)
        {
            if (keyFrames.Count < 2 || isExpired)
            {
                return;
            }
            
            _elapsed += deltaTime;

            _lastKey = keyFrames[_nextKeyIndex - 1];
            _nextKey = keyFrames[_nextKeyIndex];

            if (_nextKey.frame < _elapsed)
            {
                ++_nextKeyIndex;
                if (_nextKeyIndex >= keyFrames.Count)
                {
                    _nextKeyIndex = 1;
                    isExpired = true;
                }
            }

            if (isExpired)
            {
                onComplete?.Invoke();
                switch (_loop)
                {
                    case Loop.Stop:
                        isExpired = true;
                        return;
                    case Loop.Continue:
                        AddOffset();
                        break;
                    case Loop.Reverse:
                        Reverse();
                        break;
                    case Loop.Replay:
                        break;
                }

                isExpired = false;
                _elapsed = 0;
            }
            else
            {
                float lastKeyFrame = _lastKey.frame;
                float progress = (_elapsed - lastKeyFrame) / (_nextKey.frame - lastKeyFrame);
                float easedProgress = _nextKey.easingFunction(progress);

                T lastValue = _lastKey.value;
                T newValue = lerpFunction(lastValue, _nextKey.value, easedProgress);

                SetValue(newValue);
            }
        }

        public Tween(object target, PropertyInfo propertyInfo, LerpFunction<T> lerpFunction, OffsetFunction<T> offsetFunction, Action onComplete)
        {
            _property = propertyInfo;
            SetValue = SetPropertyValue;

            initialValue = (T)_property.GetValue(target)!;
            keyFrames = new List<KeyFrame<T>>();

            this._target = target;
            this.onComplete = onComplete;
            this.lerpFunction = lerpFunction;
            this.offsetFunction = offsetFunction;
        }

        public Tween(object target, FieldInfo fieldInfo, LerpFunction<T> lerpFunction, OffsetFunction<T> offsetFunction, Action onComplete)
        {
            _field = fieldInfo;
            SetValue = SetFieldValue;

            initialValue = (T)_field.GetValue(target)!;
            keyFrames = new List<KeyFrame<T>>();

            this._target = target;
            this.onComplete = onComplete;
            this.lerpFunction = lerpFunction;
            this.offsetFunction = offsetFunction;
        }
        
        public Tween(TweenSetter<T> setter, T initialValue, LerpFunction<T> lerpFunction, OffsetFunction<T> offsetFunction, Action onComplete)
        {
            SetValue = setter;

            keyFrames = new List<KeyFrame<T>>();
            this.initialValue = initialValue;

            this.onComplete = onComplete;
            this.lerpFunction = lerpFunction;
            this.offsetFunction = offsetFunction;
        }

        public void AddFrame(KeyFrame<T> keyFrame)
        {
            keyFrames.Add(keyFrame);
            keyFrames.Sort((x, y) => x.frame.CompareTo(y.frame));
        }

        public void RemoveFrame(KeyFrame<T> keyFrame)
        {
            keyFrames.Remove(keyFrame);
            keyFrames.Sort((x, y) => x.frame.CompareTo(y.frame));
        }

        private void SetPropertyValue(T val)
        {
            _property.SetValue(_target, val);
        }

        private void SetFieldValue(T val)
        {
            _field.SetValue(_target, val);
        }

        private void Reverse()
        {
            float finalFrame = keyFrames[^1].frame;
            foreach (var key in keyFrames)
            {
                key.frame = finalFrame - key.frame;
            }
            keyFrames.Sort((x, y) => x.frame.CompareTo(y.frame));
        }

        private void AddOffset()
        {
            T finalValue = keyFrames[^1].value;
            T initValue = keyFrames[0].value;
            
            foreach (var key in keyFrames)
            {
                key.value = offsetFunction(key.value, initValue, finalValue);
            }
            keyFrames.Sort((x, y) => x.frame.CompareTo(y.frame));
        }
    }

}