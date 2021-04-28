using System;
using System.ComponentModel;
using System.Reflection;

namespace PluginSystem.Helpers
{
    public static class ReflectionHelper
    {
        public static object GetInstanceFromCtor(Type type, Type[] ctorTypes, params object[] ctorParameters)
        {
            ConstructorInfo constructorInfo = type.GetConstructor(ctorTypes);
            return constructorInfo.Invoke(ctorParameters);
        }
        
        public static object GetInstanceFromDefaultCtor(Type type)
        {
            ConstructorInfo constructorInfo = type.GetConstructor(new Type[] { });
            return constructorInfo.Invoke(new object[] { });
        }
        
        public static T GetInstanceFromDefaultCtor<T>()
        {
            return (T)GetInstanceFromDefaultCtor(typeof(T));
        }

        public static bool IsMarkedByAttribute(PropertyDescriptor propertyDescriptor, Type typeOfAttribute)
        {
            foreach (Attribute attribute in propertyDescriptor.Attributes)
            {
                if (attribute.GetType().IsSubclassOf(typeOfAttribute))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsMarkedByAttribute(MemberInfo memberInfo, Type typeOfAttribute)
        {
            object[] customAttributes = memberInfo.GetCustomAttributes(typeOfAttribute, true);
            return customAttributes != null && customAttributes.Length > 0;
        }

        public static T GetAttribute<T>(MemberInfo memberInfo) where T : Attribute
        {
            foreach (T attribute in memberInfo.GetCustomAttributes(typeof(T), true))
            {
                return attribute;
            }

            return null;
        }

        public static T GetAttribute<T>(Type type) where T : Attribute
        {
            return GetAttribute<T>(type, true);
        }

        public static T GetAttribute<T>(Type type, bool inherited) where T : Attribute
        {
            foreach (T attribute in type.GetCustomAttributes(typeof(T), inherited))
            {
                return attribute;
            }

            return null;
        }

        public static T GetFieldValue<T>(object o, string fieldName)
        {
            return (T)GetFieldValue(o, fieldName);
        }

        public static object GetFieldValue(object o, string fieldName)
        {
            Type type = o.GetType();
            return type.GetField(fieldName, BindingFlags.Instance | BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.Public).GetValue(o);
        }

        public static object GetPropertyValue(object o, string fieldName)
        {
            Type type = o.GetType();
            return type.GetProperty(fieldName, BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Public).GetValue(o, null);
        }

        public static void SetPropertyValue(object o, string fieldName, object value)
        {
            Type type = o.GetType();
            type.GetProperty(fieldName, BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Public).SetValue(o, value, null);
        }

        public static object InvokeStaticMethod(Type type, string methodName, params object[] parameters)
        {
            MethodInfo method = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            return method.Invoke(null, parameters);
        }

        public static object InvokeStaticMethod(Type type, string methodName, Type[] types, params object[] parameters)
        {
            MethodInfo method = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static, null, types, null);
            return method.Invoke(null, parameters);
        }

        public static object InvokeMethod(object o, string methodName, params object[] parameters)
        {
            Type type = o.GetType();
            MethodInfo method = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            return method.Invoke(o, parameters);
        }

        public static object InvokeMethod(object o, string methodName, Type[] types, params object[] parameters)
        {
            Type type = o.GetType();
            MethodInfo method = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static, null, types, null);
            return method.Invoke(o, parameters);
        }

        public static object InvokeMethodFromInterface(object o, string methodName, Type interfaceType, params object[] parameters)
        {
            MethodInfo method = interfaceType.GetMethod(methodName, BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            return method.Invoke(o, parameters);
        }

        public static void SetFieldValue(object o, string fieldName, object fieldValue)
        {
            Type type = o.GetType();
            type.GetField(fieldName, BindingFlags.Instance | BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.Public).SetValue(o, fieldValue);
        }

        public static void SubscribeForEvent(object o, string eventName, Delegate handler)
        {
            Type type = o.GetType();
            EventInfo eventInfo = type.GetEvent(eventName);
            eventInfo.RemoveEventHandler(o, handler);
            eventInfo.AddEventHandler(o, handler);
        }
    }
}