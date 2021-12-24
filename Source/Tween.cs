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
        private readonly TweenSetter<T> SetValue;

        private LerpFunction<T> lerpFunction { get; set; }
        private OffsetFunction<T> offsetFunction { get; set; }
        private Action onComplete { get; set; }

        private object _target = null!;
        private PropertyInfo _property = null!;
        private FieldInfo _field = null!;

        private List<KeyFrame<T>> _keyFrames;
        private KeyFrame<T> _lastKey = null;
        private KeyFrame<T> _nextKey = null;
        private int _nextKeyIndex = 1;
        
        private float _elapsed;
        private Loop _loop = Loop.Stop;

        public bool isExpired { get; set; }

        public Tween(object target, PropertyInfo propertyInfo, T initialValue, T finalValue, float duration, EasingFunction easingFunction)
        {
            _property = propertyInfo;
            SetValue = SetPropertyValue;

            _keyFrames = new List<KeyFrame<T>>();
            _target = target;

            var startFrame = new KeyFrame<T>(0f, initialValue, easingFunction);
            var endFrame = new KeyFrame<T>(duration, finalValue, easingFunction);
            
            InsertFrames(startFrame, endFrame);
        }

        public Tween(object target, FieldInfo fieldInfo, T initialValue, T finalValue, float duration, EasingFunction easingFunction)
        {
            _field = fieldInfo;
            SetValue = SetFieldValue;

            _keyFrames = new List<KeyFrame<T>>();
            _target = target;

            var startFrame = new KeyFrame<T>(0f, initialValue, easingFunction);
            var endFrame = new KeyFrame<T>(duration, finalValue, easingFunction);
            
            InsertFrames(startFrame, endFrame);
        }
        
        public Tween(TweenSetter<T> setter, T initialValue, T finalValue, float duration, EasingFunction easingFunction)
        {
            SetValue = setter;
            _keyFrames = new List<KeyFrame<T>>();

            var startFrame = new KeyFrame<T>(0f, initialValue, easingFunction);
            var endFrame = new KeyFrame<T>(duration, finalValue, easingFunction);
            
            InsertFrames(startFrame, endFrame);
        }
        
        public void SetLooping(Loop loop)
        {
            _loop = loop;
        }

        public void SetInterpolation(LerpFunction<T> lerpFunction, OffsetFunction<T> offsetFunction)
        {
            this.lerpFunction = lerpFunction;
            this.offsetFunction = offsetFunction;
        }

        public void SetCallback(Action onComplete)
        {
            this.onComplete = onComplete;
        }

        public void SetDelay(float delay)
        {
            if (delay == 0)
                return;
            
            _keyFrames.ForEach(key => key.frame += delay);
        }

        public void Update(float deltaTime)
        {
            if (_keyFrames.Count < 2 || isExpired)
            {
                return;
            }
            
            _elapsed += deltaTime;

            _lastKey = _keyFrames[_nextKeyIndex - 1];
            _nextKey = _keyFrames[_nextKeyIndex];

            if (_nextKey.frame < _elapsed)
            {
                ++_nextKeyIndex;
                if (_nextKeyIndex >= _keyFrames.Count)
                {
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

                Restart();
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

        public void AppendFrame(KeyFrame<T> keyFrame)
        {
            keyFrame.frame += _keyFrames[^1].frame;
            _keyFrames.Add(keyFrame);
        }
        
        public void InsertFrame(KeyFrame<T> keyFrame)
        {
            _keyFrames.Add(keyFrame);
            _keyFrames.Sort((x, y) => x.frame.CompareTo(y.frame));
        }

        public void InsertFrames(params KeyFrame<T>[] frames)
        {
            _keyFrames.AddRange(frames);
            _keyFrames.Sort((x, y) => x.frame.CompareTo(y.frame));
        }
        
        public void InsertFrames(List<KeyFrame<T>> frames)
        {
            _keyFrames.AddRange(frames);
            _keyFrames.Sort((x, y) => x.frame.CompareTo(y.frame));
        }

        public void RemoveFrame(KeyFrame<T> keyFrame)
        {
            _keyFrames.Remove(keyFrame);
            _keyFrames.Sort((x, y) => x.frame.CompareTo(y.frame));
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
            float finalFrame = _keyFrames[^1].frame;
            foreach (var key in _keyFrames)
            {
                key.frame = finalFrame - key.frame;
            }
            _keyFrames.Sort((x, y) => x.frame.CompareTo(y.frame));
        }

        private void AddOffset()
        {
            if (offsetFunction == default)
                return;
            
            T finalValue = _keyFrames[^1].value;
            T initValue = _keyFrames[0].value;
            
            foreach (var key in _keyFrames)
            {
                key.value = offsetFunction(key.value, initValue, finalValue);
            }
            _keyFrames.Sort((x, y) => x.frame.CompareTo(y.frame));
        }
        
        public void Cancel(bool silent = false)
        {
            isExpired = true;
            if (!silent)
                onComplete?.Invoke();
        }

        public void Restart()
        {
            _elapsed = 0;
            _nextKeyIndex = 1;
            isExpired = false;
        }

        public void Supersede(Tween<T> tween, bool silent = false)
        {
            _keyFrames = tween._keyFrames;
            Cancel(silent);
            Restart();
        }
    }

}