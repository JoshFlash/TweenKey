using static System.MathF;

namespace TweenKey.Interpolation
{
    public delegate float EasingFunction(float val);
    
    public static class Easing
    {
        private const float OHM = 2.622057f;
        
        public static EasingFunction Linear =>      x => x;

        public static class Quadratic
        {
            public static EasingFunction In =>      x => x * x;
            public static EasingFunction Out =>     x => 1f - (1f - x) * (1f - x);

            public static EasingFunction InOut =>   x =>
                x < 0.5f 
                    ? 2f * x * x 
                    : 1f - Pow(-2f * x + 2f, 2f) / 2f;
        }
        
        public static class Cubic
        {
            public static EasingFunction In =>      x => x * x * x;
            public static EasingFunction Out =>     x => 1f - Pow(1f - x, 3f);

            public static EasingFunction InOut =>   x =>
                x < 0.5f 
                    ? 4f * x * x * x 
                    : 1f - Pow(-2f * x + 2f, 3f) / 2f;
        }

        public static class Quartic
        {
            public static EasingFunction In =>      x => x * x * x * x;
            public static EasingFunction Out =>     x => 1f - Pow(1f - x, 4f);

            public static EasingFunction InOut =>   x =>
                x < 0.5f 
                    ? 8f * x * x * x * x 
                    : 1f - Pow(-2f * x + 2f, 4f) / 2f;
        }
        
        public static class Quintic
        {
            public static EasingFunction In =>      x => x * x * x * x * x;
            public static EasingFunction Out =>     x => 1f - Pow(1f - x, 5f);

            public static EasingFunction InOut =>   x =>
                x < 0.5f 
                    ? 16f * x * x * x * x * x 
                    : 1f - Pow(-2f * x + 2f, 5f) / 2f;
        }
        
        public static class Sinusoidal
        {
            public static EasingFunction In =>      x => 1f - Cos(x * PI / 2f);
            public static EasingFunction Out =>     x => Sin(x * PI / 2f);

            public static EasingFunction InOut =>   x => -(Cos(PI * x) - 1f) / 2f;
        }

        public static class Exponential
        {
            public static EasingFunction In =>      x => x == 0f ? 0f : Pow(E, 10f * x - 10f);
            public static EasingFunction Out =>     x => Abs(x - 1f) < System.Single.Epsilon ? 1f : 1f - Pow(E, -10f * x);

            public static EasingFunction InOut =>   x => 
                x == 0f ? 0f : Abs(x - 1f) < System.Single.Epsilon ? 1f : 
                x < 0.5f 
                    ? Pow(2f, 20f * x - 10f) / 2f 
                    : (2f - Pow(2f, -20f * x + 10f)) / 2f;
        }
        
        public static class Circular
        {
            public static EasingFunction In =>      x => 1f - Sqrt(1f - Pow(x, 2f));
            public static EasingFunction Out =>     x => Sqrt(1f - Pow(x - 1f, 2f));

            public static EasingFunction InOut =>   x => 
                x < 0.5f 
                    ? (1f - Sqrt(1f - Pow(2f * x, 2f))) / 2f 
                    : (Sqrt(1 - Pow(-2f * x + 2f, 2f)) + 1f) / 2f;
        }
        
        public static class Back
        {
            public static EasingFunction In =>      x => (E - 1f) * x * x * x - E * x * x;
            public static EasingFunction Out =>     x => 1f + (E - 1f) * Pow(x - 1f, 3f) + E * Pow(x - 1f, 2f);

            public static EasingFunction InOut =>   x =>
                x < 0.5f
                    ? Pow(2f * x, 2f) * ((OHM + 1f) * 2f * x - OHM) / 2f
                    : (Pow(2f * x - 2f, 2f) * ((OHM + 1f) * (x * 2f - 2f) + OHM) + 2f) / 2f;
        }
        
        public static class Elastic
        {
            public static EasingFunction In =>      x =>
                x == 0f ? 0f : Abs(x - 1f) < System.Single.Epsilon ? 1f :
                -Pow(2, 10 * x - 10) * Sin((x * 10f - 10.75f) * 2f * PI / 3f);
            
            public static EasingFunction Out =>     x =>
                x == 0f ? 0f : Abs(x - 1f) < System.Single.Epsilon ? 1f :
                Pow(2, -10 * x) * Sin((x * 10f - 0.75f) * 2f * PI / 3f) + 1f;

            public static EasingFunction InOut =>   x => 
                x == 0f ? 0f : Abs(x - 1f) < System.Single.Epsilon ? 1f : 
                x < 0.5f
                    ? -(Pow(2f, 20f * x - 10f) * Sin((20f * x - 11.125f) * 4f * PI / 9f)) / 2f
                    : Pow(2f, -20f * x + 10f) * Sin(20f * x - 11.125f) * 4f * PI / 9f / 2f + 1f;
        }
        
        public static class Bounce
        {
            public static EasingFunction In =>      x => 1f - Out(1f - x);
            
            public static EasingFunction Out =>     x =>
                    x < 1f / 2.75f      ?   (121f / 16f) * (x * x)
                :   x < 2f / 2.75f      ?   (121f / 16f) * (x -= 1.5f / 2.75f) * x + 0.75f
                :   x < 2.5f / 2.75f    ?   (121f / 16f) * (x -= 2.25f / 2.75f) * x + 0.9375f
                :                           (121f / 16f) * (x -= 2.625f / 2.75f) * x + 0.984375f;

            public static EasingFunction InOut =>   x => 
                x < 0.5f
                    ? (1f - Out(1f - 2f * x)) / 2f
                    : (1f + Out(2f * x - 1f)) / 2f;
        }
    }
}