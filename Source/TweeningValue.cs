using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TweenKey.Interpolation;

namespace TweenKey
{
    public interface ITweeningValue
    {
        void Update(float timeElapsed);
        bool isExpired { get; set; }

        void Reverse();
        void AddOffset();
    }

    public class TweeningValue<T> : ITweeningValue
    {
        public bool isExpired { get; set; }
        internal T initialValue { get; }

        private readonly TweenSetter<T> SetValue;
        private object target { get; }
        private Action onComplete { get; }

        private List<KeyFrame<T>> keyFrames { get; }
        private LerpFunction<T> lerpFunction { get; }
        private OffsetFunction<T> offsetFunction { get; }

        private PropertyInfo property { get; } = null!;
        private FieldInfo field { get; } = null!;

        private KeyFrame<T> _lastKey = null;
        private KeyFrame<T> _nextKey = null;
        private int _nextKeyIndex = 1;

        public TweeningValue(object target, PropertyInfo propertyInfo, LerpFunction<T> lerpFunction, OffsetFunction<T> offsetFunction, Action onComplete)
        {
            property = propertyInfo;
            SetValue = SetPropertyValue;

            initialValue = (T)property.GetValue(target)!;
            keyFrames = new List<KeyFrame<T>>();
            isExpired = false;

            this.target = target;
            this.onComplete = onComplete;
            this.lerpFunction = lerpFunction;
            this.offsetFunction = offsetFunction;
        }

        public TweeningValue(object target, FieldInfo fieldInfo, LerpFunction<T> lerpFunction, OffsetFunction<T> offsetFunction, Action onComplete)
        {
            field = fieldInfo;
            SetValue = SetFieldValue;

            initialValue = (T)field.GetValue(target)!;
            keyFrames = new List<KeyFrame<T>>();

            this.target = target;
            this.onComplete = onComplete;
            this.lerpFunction = lerpFunction;
            this.offsetFunction = offsetFunction;
        }
        
        public TweeningValue(TweenSetter<T> setter, T initialValue, LerpFunction<T> lerpFunction, OffsetFunction<T> offsetFunction, Action onComplete)
        {
            SetValue = setter;

            keyFrames = new List<KeyFrame<T>>();
            this.initialValue = initialValue;

            this.onComplete = onComplete;
            this.lerpFunction = lerpFunction;
            this.offsetFunction = offsetFunction;
        }

        public void Update(float timeElapsed)
        {
            if (keyFrames.Count < 2)
            {
                return;
            }
            
            _lastKey = keyFrames[_nextKeyIndex - 1];
            _nextKey = keyFrames[_nextKeyIndex];

            if (_nextKey.frame < timeElapsed)
            {
                ++_nextKeyIndex;
                if (_nextKeyIndex >= keyFrames.Count)
                {
                    _nextKeyIndex = 0;
                    isExpired = true;
                    onComplete?.Invoke();
                    return;
                }
            }

            float lastKeyFrame = _lastKey.frame;
            float progress = (timeElapsed - lastKeyFrame) / (_nextKey.frame - lastKeyFrame);
            float easedProgress = _nextKey.easingFunction(progress);

            T lastValue = _lastKey.value;
            T newValue = lerpFunction(lastValue, _nextKey.value, easedProgress);

            SetValue(newValue);
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
            property.SetValue(target, val);
        }

        private void SetFieldValue(T val)
        {
            field.SetValue(target, val);
        }

        public void Reverse()
        {
            keyFrames.Sort((x, y) => x.frame.CompareTo(y.frame));
            float finalFrame = keyFrames[^1].frame;
            
            foreach (var key in keyFrames)
            {
                key.frame = finalFrame - key.frame;
            }
        }

        public void AddOffset()
        {
            if (offsetFunction == default)
                return;
            
            keyFrames.Sort((x, y) => x.frame.CompareTo(y.frame));
            T finalValue = keyFrames[^1].value;
            T initialValue = keyFrames[0].value;
            
            foreach (var key in keyFrames)
            {
                key.value = offsetFunction(key.value, initialValue, finalValue);
            }
        }
    }
}