using System;
using System.Linq.Expressions;
using AutoMapper;
using System.Linq;

namespace BaseApp.Web.Code.Extensions
{
    public static class MapperExtensions
    {
        public static IMappingExpression<TSource, TDest> Map<TSource, TDest, TMember>(this IMappingExpression<TSource, TDest> expression, Expression<Func<TDest, object>> propertyDest, Expression<Func<TSource, TMember>> propertySrc)
        {
            expression.ForMember(propertyDest, opt => opt.MapFrom(propertySrc));
            return expression;
        }

        public static IMappingExpression<TSource, TDest> Ignore<TSource, TDest>(this IMappingExpression<TSource, TDest> expression, Expression<Func<TDest, object>> property)
        {
            expression.ForMember(property, opt => opt.Ignore());
            return expression;
        }

        public static IMappingExpression<TSource, TDest> IgnoreAll<TSource, TDest>(this IMappingExpression<TSource, TDest> expression)
        {
            expression.ForAllMembers(opt => opt.Ignore());
            return expression;
        }

        public static IMappingExpression<TSource, TDest> IgnoreAllUnmappedComplexTypes<TSource, TDest>(this IMappingExpression<TSource, TDest> expression)
        {
            var destinationType = typeof(TDest);
            var existingMaps = expression.TypeMap;

            foreach (var property in existingMaps.GetUnmappedPropertyNames())
            {
                var propType = destinationType.GetProperty(property).PropertyType;
                if (!propType.IsValueType 
                    && !propType.IsEnum
                    && propType != typeof(string) 
                    && propType != typeof(char[]))
                {
                    expression.ForMember(property, opt => opt.Ignore());
                }
            }
            return expression;
        }

    }
}