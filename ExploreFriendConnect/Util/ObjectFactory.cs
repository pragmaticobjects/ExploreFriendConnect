using System;
using System.Collections;

namespace ExploreFriendConnect.Util
{
    public class ObjectFactory
    {
        /// <summary>
        /// Stores the data types registered against object factory.
        /// </summary>
        static Hashtable typeMap = new Hashtable(new CaseInsensitiveHashCodeProvider(),
            new CaseInsensitiveComparer());

        /// <summary>
        /// Gets an instance of the requested data type, or null if the type is 
        /// not registered with object factory.
        /// </summary>
        /// <param name="typeName">The typename whose instance should be created.</param>
        /// <returns>An object of requested type.</returns>
        public static object getInstance(string typeName)
        {
            if (typeMap.ContainsKey(typeName))
            {
                Type value = (Type)typeMap[typeName];
                if (value != null)
                {
                    return Activator.CreateInstance(value);
                }
            }
            return null;
        }

        /// <summary>
        /// Register a .NET type with the object factory. You can later obtain objects of this type
        /// using the getInstance method, and the friendly type name.
        /// </summary>
        /// <param name="typeName">A friendly name to represent a type.</param>
        /// <param name="type">The actual underlying type.</param>
        public static void RegisterType(string typeName, Type type)
        {
            if (typeMap.ContainsKey(typeName) == false)
            {
                typeMap.Add(typeName, type);
            }
            else
            {
                typeMap[typeName] = type;
            }
        }
    }
}