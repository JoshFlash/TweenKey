using System.Linq;
using TweenKey;
using TweenKey.Interpolation;

public static class TweenCreator
{
    public static Tween<T> Create<T>(object target, string propertyName, T initialValue, T finalValue, float duration,
        LerpFunction<T> lerpFunction, OffsetFunction<T> offsetFunction, EasingFunction easingFunction)
    {
        Tween<T> t = null;
        
        var property = target.GetType().GetProperties().FirstOrDefault(x => x.Name == propertyName);
        if (property != null)
        {
            if (property.PropertyType == typeof(T))
            {
                t = new Tween<T>(target, property, initialValue, finalValue, duration, easingFunction);
                t.SetInterpolation(lerpFunction, offsetFunction);
            }
        }
            
        var field = target.GetType().GetFields().FirstOrDefault(x => x.Name == propertyName);
        if (field != null)
        {
            if (field.FieldType == typeof(T))
            {
                t = new Tween<T>(target, field, initialValue, finalValue, duration, easingFunction);
                t.SetInterpolation(lerpFunction, offsetFunction);
            }
        }

        return t!;
    }
    
    public static Tween<T> CreateSequence<T>(object target, string propertyName, Sequence<T> sequence)
    {
        var initialKey = sequence.keyFrames[0];
        var finalKey = sequence.keyFrames[^1];
        var t = Create(target, propertyName, initialKey.value, finalKey.value, finalKey.frame, 
            sequence.lerpFunction, sequence.offsetFunction, Easing.Linear);

        sequence.keyFrames.Remove(finalKey);
        sequence.keyFrames.Remove(initialKey);
        t.InsertFrames(sequence.keyFrames);

        return t;
    }
        
    public static Tween<T> Create<T>(TweenSetter<T> setter, T initialValue, T finalValue, float duration,
        LerpFunction<T> lerpFunction, OffsetFunction<T> offsetFunction, EasingFunction easingFunction)
    {
        var t = new Tween<T>(setter, initialValue, finalValue, duration, easingFunction);
        t.SetInterpolation(lerpFunction, offsetFunction);
        return t;
    }

    public static Tween<T> CreateSequence<T>(TweenSetter<T> setter, Sequence<T> sequence)
    {
        var initialKey = sequence.keyFrames[0];
        var finalKey = sequence.keyFrames[^1];
        var t = Create(setter, initialKey.value, finalKey.value, finalKey.frame, 
            sequence.lerpFunction, sequence.offsetFunction, Easing.Linear);

        sequence.keyFrames.Remove(finalKey);
        sequence.keyFrames.Remove(initialKey);
        t.InsertFrames(sequence.keyFrames);

        return t;
    }
}