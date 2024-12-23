using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Abp.UI;

namespace Icon.BaseManagement
{
    public static class BaseHelper
    {
        public static string GetServiceName(string backendServiceName)
        {
            return backendServiceName.Replace("AppService", "ServiceProxy");
        }

        public static string GetMethodName(string methodName)
        {
            return methodName.Substring(0, 1).ToLower() + methodName.Substring(1);
        }

        public static string ConvertPathToLowerCamelCase(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return path;
            }

            var segments = path.Split('.').Select(segment =>
            {
                return segment.Length > 1
                    ? char.ToLowerInvariant(segment[0]) + segment.Substring(1)
                    : segment.ToLowerInvariant();
            });

            return string.Join(".", segments);
        }


        public static string GetPropertyPath<T, TProperty>(Expression<Func<T, TProperty>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            var path = new List<string>();

            while (memberExpression != null)
            {
                path.Insert(0, memberExpression.Member.Name);
                memberExpression = memberExpression.Expression as MemberExpression;
            }

            return string.Join(".", path);
        }





    }
}