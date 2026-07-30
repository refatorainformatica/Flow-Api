using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Microsoft.AspNetCore.OData.Query.Wrapper;

namespace Shared.Infrastructure.Persistences
{
    public static class OutputFormatter
    {
        public static object GetValue(object target, string name)
        {
            return target is ISelectExpandWrapper selectExpandWrapper
                ? selectExpandWrapper.ToDictionary()[name]
                : target.GetType().GetProperty(name).GetValue(target);
        }

        public static IEnumerable<KeyValuePair<string, Type>> GetPropertiesFromSelect(
            string queryString,
            Type type
        )
        {
            var select = HttpUtility.ParseQueryString(queryString)["$select"];
            var selectedPropertyNames = select != null ? select.Split(",") : Array.Empty<string>();

            Type elementType = typeof(ISelectExpandWrapper).IsAssignableFrom(type)
                ? type.GenericTypeArguments[0]
                : type;

            return GetProperties(elementType).Where(p => selectedPropertyNames.Contains(p.Key));
        }

        public static IEnumerable<KeyValuePair<string, Type>> GetProperties(Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && IsSimpleType(p.PropertyType))
                .Select(p => new KeyValuePair<string, Type>(p.Name, p.PropertyType));
        }

        public static bool IsSimpleType(Type type)
        {
            var underlyingType =
                type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)
                    ? Nullable.GetUnderlyingType(type)
                    : type;

            if (underlyingType == typeof(Guid) || underlyingType == typeof(DateTimeOffset))
                return true;

            var typeCode = Type.GetTypeCode(underlyingType);

            return typeCode switch
            {
                TypeCode.Boolean
                or TypeCode.Byte
                or TypeCode.Char
                or TypeCode.DateTime
                or TypeCode.Decimal
                or TypeCode.Double
                or TypeCode.Int16
                or TypeCode.Int32
                or TypeCode.Int64
                or TypeCode.SByte
                or TypeCode.Single
                or TypeCode.String
                or TypeCode.UInt16
                or TypeCode.UInt32
                or TypeCode.UInt64 => true,
                _ => false,
            };
        }
    }
}
