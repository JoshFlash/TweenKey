using TweenKey.Interpolation;

namespace TweenKey
{
    public struct KeyFrame<T>
    {
        public float frame { get; set; }
        public T value { get; set; }
        public EasingFunction easingFunction { get; set; }

        public KeyFrame(float frame, T value, EasingFunction easingFunction)
        {
            this.frame = frame;
            this.value = value;
            this.easingFunction = easingFunction;
        }
        
        public KeyFrame(float frame, T value)
        {
            this.frame = frame;
            this.value = value;
            easingFunction = Easing.Linear;
        }
    }
}