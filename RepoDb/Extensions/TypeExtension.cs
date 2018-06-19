﻿using System;

namespace RepoDb.Extensions
{
    /// <summary>
    /// Contains the extension methods type <i>System.Type</i> object.
    /// </summary>
    public static class TypeExtension
    {
        /// <summary>
        /// Gets a custom attribute defined on the type.
        /// </summary>
        /// <typeparam name="T">The custom attribute that is defined into the type.</typeparam>
        /// <param name="type">The type of where the custom attribute is defined.</param>
        /// <returns>The custom attribute.</returns>
        public static T GetCustomAttribute<T>(this Type type) where T : Attribute
        {
            var attributes =type.GetCustomAttributes(typeof(T), false);
            return attributes?.Length > 0 ? (T)attributes[0] : null;
        }
    }
}