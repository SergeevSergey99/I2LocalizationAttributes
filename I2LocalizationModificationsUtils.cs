using System;
using System.Reflection;

namespace Localization.I2LocalizationModifications
{
    public class I2LocalizationModificationsUtils
    {
        public static object GetMemberValue(object target, string memberName)
        {
            if (target == null)
                return null;

            Type targetType = target.GetType();
            MemberInfo[] members = targetType.GetMember(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            if (members.Length > 0)
            {
                MemberInfo member = members[0];
                switch (member.MemberType)
                {
                    case MemberTypes.Field:
                        return ((FieldInfo)member).GetValue(target);
                    case MemberTypes.Property:
                        return ((PropertyInfo)member).GetValue(target);
                    case MemberTypes.Method:
                        return ((MethodInfo)member).Invoke(target, null);
                }
            }

            return null;
        }
    }
}