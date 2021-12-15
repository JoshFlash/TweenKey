using UnityEngine;

namespace TweenKey.Interpolation
{
    public delegate T LerpFunction<T>(T start, T end, float progress);
    
    public static class LerpFunctions
    {
        public static readonly LerpFunction<Vector2>    Vector2     = UnityEngine.Vector2.Lerp;
        public static readonly LerpFunction<Vector3>    Vector3     = UnityEngine.Vector3.Lerp;
        public static readonly LerpFunction<Vector4>    Vector4     = UnityEngine.Vector4.Lerp;
        public static readonly LerpFunction<Color>      Color       = UnityEngine.Color.Lerp;
        public static readonly LerpFunction<Quaternion> Quaternion  = UnityEngine.Quaternion.Slerp;
        public static readonly LerpFunction<float>      Float       = (s, e, p) => s + (e - s) * p;
        public static readonly LerpFunction<Rect>       Rectangle   = (s, e, p) =>
        {
            var pX = s.x + (e.x - s.x) * p;
            var pY = s.y + (e.y - s.y) * p;
            var width = s.width + (e.width - s.width) * p;
            var height = s.height + (e.height - s.height) * p;
            return new Rect(pX, pY, width, height);
        };  
    }

}
