using System;
using System.Reflection;

namespace ConsoleApp_Turner.Common
{
    public static class PropertyHelper
    {
        public static T2 GetPropertyField<T, T2>(T objectContext, string propertyName) where T : class where T2: class
        {
            Type type = objectContext.GetType();

            var bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

            var propertyValue = type.GetProperty(propertyName, bindingFlags).GetValue(objectContext, null);

            return (propertyValue != null) ? (T2)propertyValue : null;
        }
    }
}
