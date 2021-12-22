using System.Linq;
using TweenKey;
using TweenKey.Interpolation;

public class TweenCreator
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
        var frames = sequence.keyFrames;
        var t = Create(target, propertyName, frames[0].value, frames[^1].value, frames[^1].frame,
            sequence.lerpFunction, sequence.offsetFunction, Easing.Linear);

        frames.RemoveAt(0);
        frames.RemoveAt(frames.Count - 1);

        t.InsertFrames(frames);

        return t;
    }
        
    public static Tween<T> Create<T>(TweenSetter<T> setter, T initialValue, T finalValue, float duration,
        LerpFunction<T> lerpFunction, OffsetFunction<T> offsetFunction, EasingFunction easingFunction)
    {
        var t = new Tween<T>(setter, initialValue, finalValue, duration, easingFunction);
        t.SetInterpolation(lerpFunction, offsetFunction);
        return t;
    }

    public static Tween<T> CreateSequence<T>(TweenSetter<T> setter, T initialValue, Sequence<T> sequence)
    {
        var frames = sequence.keyFrames;
        var t = Create(setter, initialValue, frames[^1].value, frames[^1].frame, sequence.lerpFunction, sequence.offsetFunction, Easing.Linear);

        frames.RemoveAt(frames.Count - 1);
        t.InsertFrames(frames);

        return t;
    }
}