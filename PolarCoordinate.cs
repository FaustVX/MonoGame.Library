using System;

namespace MonoGame.Library
{
    public struct PolarCoordinate
    {
        public PolarCoordinate(double radius, double angle)
        {
            Radius = radius;
            Angle = angle;
        }

        public double Radius { get; set; }
        public double Angle { get; set; }
        public double AngleDegree
        {
            get => FromRadian(Angle);
            set => Angle = ToRadian(value);
        }

        public static double ToRadian(double angle)
            => angle / 360 * (Math.PI * 2);

        public static double FromRadian(double angle)
            => angle / (2 * Math.PI) * 360;


        public static PolarCoordinate FromCartesian(int x, int y)
        {
            var radius = (int)Math.Sqrt(x * x + y * y);
            var angle = Math.Atan2(y, x);
            return new PolarCoordinate(radius, angle);
        }

        public (int x, int y) ToCartesian()
            => ((int)(Radius * Math.Cos(Angle)), (int)(Radius * Math.Sin(Angle)));
    }
}
