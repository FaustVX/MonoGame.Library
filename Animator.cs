using Microsoft.Xna.Framework;
using System;

namespace MonoGame.Library
{
    public interface IAnimator<TOutput>
    {
        TOutput Animate(double input);
    }

    public abstract class TimedAnimator<TOutput> : IAnimator<TOutput>
    {
        public TimedAnimator(TimeSpan totalDuration, TOutput startValue, AnimatorType animatorType)
        {
            TotalDuration = totalDuration;
            AnimatorType = animatorType;
            CurrentValue = StartValue = startValue;
        }

        public TimedAnimator(TimeSpan totalDuration, TOutput currentValue)
            : this(totalDuration, currentValue, AnimatorType.Continue)
        { }

        public TimeSpan TotalDuration { get; }
        public AnimatorType AnimatorType { get; }
        protected TimeSpan _duration { get; private set; }
        public TOutput CurrentValue { get; private set; }
        public bool Finished { get; private set; }
        public TOutput StartValue { get; }

        public TOutput Animate(GameTime gameTime)
            => Animate(gameTime.ElapsedGameTime);
        public TOutput Animate(TimeSpan time)
        {
            _duration += time;
            switch (AnimatorType)
            {
                case AnimatorType.Continue:
                    if (_duration >= TotalDuration)
                        Finished = true;
                    break;
                case AnimatorType.OneTime:
                    if (_duration >= TotalDuration)
                        Finished = true;
                    if (Finished)
                        _duration = TotalDuration;
                    break;
                case AnimatorType.Restart:
                    Finished = _duration >= TotalDuration;
                    if (Finished)
                        _duration -= TotalDuration;
                    break;
                case AnimatorType.BackAndForth:
                    if (_duration >= TotalDuration)
                        Finished = true;
                    else if (_duration < TimeSpan.Zero)
                        Finished = false;
                    if (Finished)
                        _duration -= time + time;
                    break;
            }
            var ratio = _duration.TotalMilliseconds / TotalDuration.TotalMilliseconds;
            return CurrentValue = Animate(ratio);
        }

        public abstract TOutput Animate(double input);
    }

    public enum AnimatorType
    {
        Continue,
        OneTime,
        Restart,
        BackAndForth
    }

    public static class Animator
    {
        private class DelegateAnimator<T> : IAnimator<T>
        {
            private readonly Func<double, T> _function;

            public DelegateAnimator(Func<double, T> function)
            {
                _function = function;
            }

            public T Animate(double input)
                => _function(input);
        }

        private class DelegateTimedAnimator<T> : TimedAnimator<T>
        {
            private readonly Func<double, T> _function;

            public DelegateTimedAnimator(Func<double, T> function, TimeSpan totalDuration, T startValue, AnimatorType animatorType)
                : base(totalDuration, startValue, animatorType)
            {
                _function = function;
            }

            public override T Animate(double input)
                => _function(input);
        }

        private class ContinuationTimedAnimator<T> : TimedAnimator<T>
        {
            private readonly TimedAnimator<T> _NextAnimator;
            private readonly TimedAnimator<T> _baseAnimator;

            public ContinuationTimedAnimator(TimedAnimator<T> nextAnimator, TimedAnimator<T> baseAnimator)
                : base(baseAnimator.TotalDuration, baseAnimator.StartValue, baseAnimator.AnimatorType)
            {
                _NextAnimator = nextAnimator;
                _baseAnimator = baseAnimator;
            }

            public override T Animate(double input)
                => _baseAnimator.Finished ? _NextAnimator.Animate(input * 2 - 1) : _baseAnimator.Animate(input * 2);
        }

        public static IAnimator<double> CosineAnimator { get; } = FromDelegate(Math.Cos);
        public static IAnimator<double> SineAnimator { get; } = FromDelegate(Math.Sin);
        public static IAnimator<double> EaseInOut { get; } = FromDelegate(t => (t * t) * (3.0f - 2.0f * t));
        public static IAnimator<double> InverseAnimator { get; } = FromDelegate(input => 1 - input);
        public static IAnimator<Vector2> CircleAnimator { get; } = FromDelegate(rad => new Vector2((float)CosineAnimator.Animate(rad), (float)SineAnimator.Animate(rad)));

        public static IAnimator<T> FromDelegate<T>(this Func<double, T> function)
            => new DelegateAnimator<T>(function);

        public static IAnimator<T> Add<T>(this IAnimator<T> first, IAnimator<double> second)
            => new DelegateAnimator<T>(input => first.Animate(second.Animate(input)));

        public static TimedAnimator<T> Add<T>(this TimedAnimator<T> first, IAnimator<double> second)
            => new DelegateTimedAnimator<T>(input => first.Animate(second.Animate(input)), first.TotalDuration, first.CurrentValue, first.AnimatorType);

        public static TimedAnimator<T> ContinueWith<T>(this TimedAnimator<T> first, Func<double, T> function)
            => new ContinuationTimedAnimator<T>(function.FromDelegate().ToTimed(first.TotalDuration, first.StartValue, first.AnimatorType), first);

        public static TimedAnimator<T> ContinueWith<T>(this TimedAnimator<T> first, TimedAnimator<T> next)
            => new ContinuationTimedAnimator<T>(next, first);

        public static TimedAnimator<T> ToTimed<T>(this IAnimator<T> animator, TimeSpan totalDuration, T currentValue, AnimatorType animatorType = AnimatorType.Continue)
            => new DelegateTimedAnimator<T>(animator.Animate, totalDuration, currentValue, animatorType);

        public static TimedAnimator<T> WhenFinished<T>(this TimedAnimator<T> animator, Action<TimedAnimator<T>> action)
        {
            return new DelegateTimedAnimator<T>(t =>
            {
            var res = animator.Animate(TimeSpan.FromMilliseconds(t * animator.TotalDuration.TotalMilliseconds));
                if (animator.Finished)
                    action(animator);
                return res;
            }, animator.TotalDuration, animator.StartValue, animator.AnimatorType);
        }
    }
}
