using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Shared.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static string CatchFirstName(this string name)
        {
            name = string.IsNullOrEmpty(name) ? "" : name;
            string pattern = @"^(\w+)";
            Match match = Regex.Match(name, pattern);

            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            else
            {
                return name;
            }
        }

        public static string ToQueryStringUsingNewtonsoftJson(this object obj, string prefix = "")
        {
            bool IsDefaultType(Type type) =>
                type.IsPrimitive || type == typeof(string) || type == typeof(DateTime);

            var properties = new Dictionary<string, object>();
            var queryString = new List<string>();

            if (obj is IEnumerable && !(obj is string))
            {
                if (string.IsNullOrEmpty(prefix))
                {
                    throw new InvalidOperationException(
                        "You should not serialize an array without a prefix"
                    );
                }

                var enumerator = ((IEnumerable)obj).GetEnumerator();
                int i = 0;
                while (enumerator.MoveNext())
                {
                    properties.Add(i.ToString(), enumerator.Current);
                    i++;
                }
            }
            else
            {
                properties = obj.GetType()
                    .GetProperties()
                    .Where(property => property.CanRead && property.GetValue(obj, null) != null)
                    .ToDictionary(
                        property => property.Name,
                        property => property.GetValue(obj, null)
                    );

                if (properties.Count == 0 && IsDefaultType(obj.GetType()))
                {
                    properties.Add(prefix, obj);
                }
            }

            foreach (var prop in properties.Where(kv => kv.Value != null))
            {
                string key = string.IsNullOrEmpty(prefix) ? prop.Key : $"{prefix}.{prop.Key}";
                var value = prop.Value;

                var valueType = value.GetType();
                if (IsDefaultType(valueType))
                {
                    if (valueType == typeof(DateTime))
                    {
                        value = ((DateTime)value).ToString("yyyy-MM-ddTHH:mm:ss");
                    }
                    queryString.Add(key + "=" + value.ToString());
                }
                else
                {
                    queryString.Add(value.ToQueryStringUsingNewtonsoftJson(key));
                }
            }

            return string.Join("&", queryString).Replace("&&", "&");
        }
    }
}
