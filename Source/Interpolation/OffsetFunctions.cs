using System.Collections.ObjectModel;
using UnityEngine;

namespace TweenKey.Interpolation
{
    public delegate T OffsetFunction<T>(T value, T initial, T final);
    
    public static class OffsetFunctions
    {
        public static readonly OffsetFunction<Vector2>     Vector2     = (a, i, f) => a + (f - i); 
        public static readonly OffsetFunction<Vector3>     Vector3     = (a, i, f) => a + (f - i); 
        public static readonly OffsetFunction<Vector4>     Vector4     = (a, i, f) => a + (f - i); 
        public static readonly OffsetFunction<Color>       Color       = (a, i, f) => a + (f - i); 
        public static readonly OffsetFunction<Quaternion>  Quaternion  = (a, i, f) => a * (UnityEngine.Quaternion.Inverse(i) * f); 
        public static readonly OffsetFunction<float>       Float       = (a, i, f) => a + (f - i);
        public static readonly OffsetFunction<Rect>        Rectangle   = (a, i, f) => 
        {
            var x = a.x + f.x - i.x;
            var y = a.y + f.y - i.y;
            var width = a.width + f.width - i.width;
            var height = a.height + f.height - i.height;
            return new Rect(x, y, width, height);
        };
    }
}