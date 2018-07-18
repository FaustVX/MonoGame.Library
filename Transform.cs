using Microsoft.Xna.Framework;

namespace MonoGame.Library
{
    public abstract class Transform
    {
        private sealed class NoneTransform : Transform
        {
            public NoneTransform()
                : base(null)
            { }

            public override Vector2 Apply(Vector2 vector)
                => vector;
        }

        public sealed class MoveTransform : Transform
        {
            public Vector2 Value { get; }

            public MoveTransform(Vector2 value, Transform previous)
                : base(previous)
            {
                Value = value;
            }
            public override Vector2 Apply(Vector2 vector)
                => Previous.Apply(vector + Value);
        }

        public sealed class OtherTransform : Transform
        {
            public OtherTransform(TransformType type, float value, Transform previous)
                : base(previous)
            {
                Type = type;
                Value = value;
            }

            public enum TransformType
            {
                Scale,
                RotateRadian,
                RotateDegree,
            }

            public TransformType Type { get; }
            public float Value { get; }

            public override Vector2 Apply(Vector2 vector)
            {
                switch (Type)
                {
                    case TransformType.Scale:
                        vector *= Value;
                        break;
                    case TransformType.RotateRadian:
                        break;
                    case TransformType.RotateDegree:
                        break;
                }
                return Previous.Apply(vector);
            }
        }

        private Transform Previous { get; }

        private Transform(Transform previous)
        {
            Previous = previous;
        }

        public Transform Move(Vector2 value)
            => new MoveTransform(value, this);

        public Transform Scale(float factor)
            => new OtherTransform(OtherTransform.TransformType.Scale, factor, this);

        public Transform RotateRadian(float angle)
            => new OtherTransform(OtherTransform.TransformType.RotateRadian, angle, this);

        public Transform RotateDegree(float angle)
            => new OtherTransform(OtherTransform.TransformType.RotateDegree, angle, this);

        public abstract Vector2 Apply(Vector2 vector);

        public Rectangle Apply(Rectangle rectangle)
        {
            var location1 = Apply(rectangle.Location);
            var location2 = Apply(rectangle.Location + rectangle.Size);
            return new Rectangle(location1, location2 - location1);
        }

        public Point Apply(Point point)
            => Apply(point.ToVector2()).ToPoint();

        public static Transform None() => new NoneTransform();
    }
}
