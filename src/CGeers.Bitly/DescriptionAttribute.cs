using System;
using System.Linq;
using System.Reflection;

namespace CGeers.Bitly
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class DescriptionAttribute : Attribute
    {
        public DescriptionAttribute(Type resourceType, string resourceName)
        {
            Description = GetResource(resourceType, resourceName);
        }

        public string Description { get; set; }

        public static string GetDescription<TEnum>(TEnum value)
            where TEnum : struct
        {
            var memberInfo = typeof(TEnum).GetMember(value.ToString()).FirstOrDefault();
            if (memberInfo == null)
                return null;

            var attribute = (from a in memberInfo.GetCustomAttributes(typeof(DescriptionAttribute), false)
                             select a).SingleOrDefault() as DescriptionAttribute;

            if (attribute == null)
                return null;

            return attribute.Description;
        }

        private static string GetResource(Type resourceType, string resourceName)
        {
            if ((resourceType != null) && (resourceName != null))
            {
                PropertyInfo property = resourceType.GetProperty(resourceName, BindingFlags.NonPublic | BindingFlags.Static);
                if (property == null)
                {
                    throw new InvalidOperationException(string.Format("Resource Type Does Not Have Property"));
                }
                if (property.PropertyType != typeof(string))
                {
                    throw new InvalidOperationException(string.Format("Resource Property is Not String Type"));
                }
                return (string)property.GetValue(null, null);
            }
            return null;
        }
    }
}
