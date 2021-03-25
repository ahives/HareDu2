namespace HareDu.Core.Extensions
{
    using System;
    using System.Linq;

    public static class TypeExtensions
    {
        public static bool IsDerivedFrom(this Type type, Type fromType)
        {
            while (!type.IsNull() && type != typeof(object))
            {
                Type currentType = type.IsGenericType ? type.GetGenericTypeDefinition() : type;

                if (fromType == currentType)
                    return true;

                type = type.BaseType;
            }

            return false;
        }

        public static Type Find(this Type[] types, Predicate<Type> predicate) => Array.Find(types, predicate);

        public static bool InheritsFromInterface(this Type type, Type findType)
        {
            return findType.IsGenericType
                ? type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == findType)
                : type.GetInterfaces().Any(x => x == findType);
        }
    }
}