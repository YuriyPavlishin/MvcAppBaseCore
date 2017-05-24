using System;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using BaseApp.Common.Extensions;

namespace BaseApp.Web.Code.Mappers
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

        public static IMappingExpression<TSource, TDest> IgnoreSource<TSource, TDest>(this IMappingExpression<TSource, TDest> expression, Expression<Func<TSource, object>> property)
        {
            expression.ForSourceMember(property, opt => opt.Ignore());
            return expression;
        }

        [Obsolete]
        public static IMappingExpression<TSource, TDest> IgnoreAll<TSource, TDest>(this IMappingExpression<TSource, TDest> expression)
        {
            throw new Exception("Not supported, please use mapper with option MemberList.Source for such cases");
        }

        public static IMappingExpression<TSource, TDest> IgnoreAllUnmappedComplexTypes<TSource, TDest>(this IMappingExpression<TSource, TDest> expression)
        {
            var destType = typeof(TDest);
            expression.ForAllOtherMembers(
                delegate(IMemberConfigurationExpression<TSource, TDest, object> configurationExpression)
                {
                    var propName = configurationExpression.DestinationMember.Name;
                    var propType = destType.GetProperties().First(x => x.Name.EqualsIgnoreCase(propName)).PropertyType;

                    if (!propType.IsValueType
                        && !propType.IsEnum
                        && propType != typeof(string)
                        && propType != typeof(char[]))
                    {
                        configurationExpression.Ignore();
                    }
                });
            
            return expression;
        }

    }
}