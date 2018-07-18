using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MonoGame.Library
{
    public class Using<T> : IDisposable
    {
        public Action<T> Property { get; }
        public T CurrentValue { get; }
        public Using(Expression<Func<T>> property)
        {
            var rhs = property.Body as MemberExpression;
            if (rhs.Member is System.Reflection.PropertyInfo p && !p.CanWrite)
                throw new FieldAccessException(rhs.ToString());

            var param = Expression.Parameter(typeof(T), "val");
            var expr = Expression.Lambda<Action<T>>(Expression.Assign(rhs, param), param);
            Property = expr.Compile();
            CurrentValue = property.Compile()();
        }
        public Using(Expression<Func<T>> property, T setValue)
            : this(property)
        {
            Property(setValue);
        }

        public void Dispose()
        {
            Property(CurrentValue);
        }
    }

    public static class Helpers
    {
        public static IDisposable Using<T>(Expression<Func<T>> property, T setValue)
            => new Using<T>(property, setValue);

        public static IDisposable Using<T>(Expression<Func<T>> property)
            => new Using<T>(property);

        public static bool Intersects(this Rectangle rectangle, Point point)
            => point.X >= rectangle.Left && point.X <= rectangle.Right && point.Y >= rectangle.Top && point.Y <= rectangle.Bottom;
        private static readonly System.Random _rng = new System.Random();

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> list)
        {
            return list.OrderBy(i => _rng.Next());
        }

        public static OrderedParallelQuery<T> Shuffle<T>(this ParallelQuery<T> list)
        {
            return list.OrderBy(i => _rng.Next());
        }

        public static IEnumerable<T> Enumerate<T>(this IEnumerable<T> enumerable)
            => Enumerate(enumerable.GetEnumerator());

        public static IEnumerable<T> Enumerate<T>(this IEnumerator<T> enumerator)
        {
            while (enumerator.MoveNext())
                yield return enumerator.Current;
        }

        public static IEnumerable<T> Append<T>(this IEnumerable<T> list, T lastValue)
        {
            foreach (var item in list)
                yield return item;
            yield return lastValue;
        }

        public static IEnumerable<T> Prepend<T>(this IEnumerable<T> list, T firstValue)
        {
            yield return firstValue;
            foreach (var item in list)
                yield return item;
        }

        public static IEnumerable<(T current, (T element, bool hasElement) next)> CurrentNext<T>(this IEnumerable<T> enumerable)
            => enumerable.GetEnumerator().CurrentNext();

        public static IEnumerable<(T current, (T element, bool hasElement) next)> CurrentNext<T>(this IEnumerator<T> enumerator)
        {
            if (!enumerator.TryGetNext(out var next))
                yield break;
            var moved = false;
            while (true)
            {
                if (!enumerator.MoveNext())
                    if (!moved)
                    {
                        yield return (next, (default, false));
                        yield break;
                    }
                    else
                        yield break;
                yield return (next, (next = enumerator.Current, true));
                moved = true;
            }
        }

        public static IEnumerable<(T element, bool isLast)> IsLast<T>(this IEnumerable<T> list)
        {
            var next = default(T);
            foreach (var element in list.CurrentNext())
            {
                if (!element.next.hasElement)
                {
                    yield return (element.current, true);
                    yield break;
                }
                yield return (element.current, false);
                next = element.next.element;
            }
            yield return (next, true);
        }

        public static IEnumerable<(T element, int index)> GetIndices<T>(this IEnumerable<T> list)
        {
            var index = 0;
            foreach (var element in list)
                yield return (element, index++);
        }

        public static TOut If<TIn, TOut>(this TIn element, bool boolean, System.Func<TIn, TOut> @true, System.Func<TIn, TOut> @false)
            => (boolean ? @true : @false)(element);

        public static T IfTrue<T>(this T element, bool boolean, System.Func<T, T> @true)
            => boolean ? @true(element) : element;

        public static T IfFalse<T>(this T element, bool boolean, System.Func<T, T> @false)
            => boolean ? element : @false(element);

        public static T Do<T>(this T @this, System.Action<T> action)
        {
            action(@this);
            return @this;
        }

        public static IEnumerable<T> Exclude<T>(this IEnumerable<T> list, System.Func<T, bool> excludeFunc)
            => list.Where(item => !excludeFunc(item));

        public static ParallelQuery<T> Exclude<T>(this ParallelQuery<T> list, System.Func<T, bool> excludeFunc)
            => list.Where(item => !excludeFunc(item));

        public static bool TryGetNext<T>(this IEnumerator<T> enumerator, out T current)
        {
            current = default;
            var moved = enumerator.MoveNext();
            if (moved)
                current = enumerator.Current;
            return moved;
        }

        public static IEnumerable<T> Cycle<T>(this IEnumerator<T> enumerator)
            => enumerator.Enumerate().Cycle();

        public static IEnumerable<T> Cycle<T>(this IEnumerable<T> list)
        {
            var enumerator = GetEnumerator();
            while (true)
            {
                if (!enumerator.TryGetNext(out var current))
                {
                    enumerator = GetEnumerator();
                    if (!enumerator.TryGetNext(out current))
                        yield break;
                }
                yield return current;
            }

            IEnumerator<T> GetEnumerator()
                => list.GetEnumerator();
        }

        public static int From2DTo1D(int x, int y, int width)
            => x + y * width;

        public static bool Try(this System.Action action, int times)
        {
            for (int i = 0; i < times; i++)
                try
                {
                    action();
                    return true; ;
                }
                catch
                { }
            return false;
        }

        public static bool Try(this System.Action action, System.Func<System.Exception, bool> breakOnException, int times)
        {
            for (int i = 0; i < times; i++)
                try
                {
                    action();
                    return true;
                }
                catch (System.Exception ex)
                {
                    if (breakOnException(ex))
                        break;
                }
            return false;
        }

        public static bool Try<T>(this System.Action action, System.Func<T, bool> breakOnException, int times)
            where T : System.Exception
        {
            for (int i = 0; i < times; i++)
                try
                {
                    action();
                    return true;
                }
                catch (T ex)
                {
                    if (breakOnException(ex))
                        break;
                }
            return false;
        }

        public static bool Try<T1, T2>(this System.Action action, System.Func<T1, bool> breakOnException1, System.Func<T2, bool> breakOnException2, int times)
            where T1 : System.Exception
            where T2 : System.Exception
        {
            for (int i = 0; i < times; i++)
                try
                {
                    action();
                    return true;
                }
                catch (T1 ex)
                {
                    if (breakOnException1(ex))
                        break;
                }
                catch (T2 ex)
                {
                    if (breakOnException2(ex))
                        break;
                }
            return false;
        }

        public static bool Try<T1, T2, T3>(this System.Action action, System.Func<T1, bool> breakOnException1, System.Func<T2, bool> breakOnException2, System.Func<T3, bool> breakOnException3, int times)
            where T1 : System.Exception
            where T2 : System.Exception
            where T3 : System.Exception
        {
            for (int i = 0; i < times; i++)
                try
                {
                    action();
                    break;
                }
                catch (T1 ex)
                {
                    if (breakOnException1(ex))
                        break;
                }
                catch (T2 ex)
                {
                    if (breakOnException2(ex))
                        break;
                }
                catch (T3 ex)
                {
                    if (breakOnException3(ex))
                        break;
                }
            return false;
        }

        public static decimal ToFrequency(this GameTime gameTime)
            => ToFrequency(gameTime.ElapsedGameTime);

        public static decimal ToFrequency(this System.TimeSpan time)
            => time.Ticks == 0 ? 0 : 1m / ((decimal)time.Ticks / System.TimeSpan.TicksPerSecond);
        
        public static System.TimeSpan FromFrequency(this decimal frequency)
            => System.TimeSpan.FromTicks(frequency == 0 ? 0 : (long)((1m / frequency) * System.TimeSpan.TicksPerSecond));//.FromSeconds(frequency == 0 ? 0 : 1 / (double)frequency);
    }
}
