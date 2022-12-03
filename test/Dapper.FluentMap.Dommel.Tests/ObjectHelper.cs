using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace Dapper.FluentMap.Dommel.Tests
{
    public static class ObjectHelper
    {
        private static readonly Lazy<ConcurrentDictionary<string, dynamic>> GetterCache =
            new Lazy<ConcurrentDictionary<string, dynamic>>(() => new ConcurrentDictionary<string, dynamic>());

        private static readonly Lazy<ConcurrentDictionary<string, dynamic>> SetterCache =
            new Lazy<ConcurrentDictionary<string, dynamic>>(() => new ConcurrentDictionary<string, dynamic>());

        private static readonly Lazy<ConcurrentDictionary<string, string>> NameCache =
            new Lazy<ConcurrentDictionary<string, string>>(() => new ConcurrentDictionary<string, string>());


        public static Action<TClass, TValue> Setter<TClass, TValue>(
            Expression<Func<TClass, TValue>> propertyAccessor)
        {
            var key = $"{typeof(TClass)}_{propertyAccessor}";
            return SetterCache.Value.GetOrAdd(key, _ =>
            {
                var prop = GetMemberInfo(propertyAccessor).Member;
                var typeParam = Expression.Parameter(typeof(TClass));
                var valueParam = Expression.Parameter(typeof(TValue));
                return Expression.Lambda<Action<TClass, TValue>>(
                    Expression.Assign(
                        Expression.MakeMemberAccess(typeParam, prop), valueParam),
                    typeParam,
                    valueParam)
                    .Compile();
            });
        }

        public static Func<TClass, TValue> Getter<TClass, TValue>(
            Expression<Func<TClass, TValue>> propertyAccessor)
        {
            var key = $"{typeof(TClass)}_{propertyAccessor}";
            return GetterCache.Value.GetOrAdd(key, _ =>
            {
                var prop = GetMemberInfo(propertyAccessor).Member;
                var propertyInfo = prop as PropertyInfo;
                if (propertyInfo == null)
                {
                    throw new InvalidOperationException($"Member with Name '{prop.Name}' is not a property.");
                }

                var instance = Expression.Parameter(typeof(TClass), "instance");
                var getterCall = Expression.Call(instance, propertyInfo.GetGetMethod());
                var body = Expression.Convert(getterCall, typeof(TValue));
                var parameters = new[] { instance };
                var expression = Expression.Lambda<Func<TClass, TValue>>(body, parameters);
                return expression.Compile();
            });
        }

        public static string Name<TClass, TValue>(Expression<Func<TClass, TValue>> propertyAccessor)
        {
            var key = propertyAccessor.ToString();
            return NameCache.Value.GetOrAdd(key, _ =>
            {
                var me = GetMemberInfo(propertyAccessor);
                return !(me.Expression is MemberExpression nestedMe) ? me.Member.Name : $"{nestedMe.Member.Name}.{me.Member.Name}";
            });
        }

        public static MemberExpression GetMemberInfo(Expression method)
        {
            if (!(method is LambdaExpression lambda))
            {
                throw new ArgumentNullException(nameof(method));
            }

            var memberExpr = lambda.Body.NodeType switch
            {
                ExpressionType.Convert => ((UnaryExpression)lambda.Body).Operand as MemberExpression,
                ExpressionType.MemberAccess => lambda.Body as MemberExpression,
                _ => null
            };

            if (memberExpr == null)
            {
                throw new ArgumentException("method");
            }

            return memberExpr;
        }
    }
}