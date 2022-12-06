// -----------------------------------------------------------------------
// <copyright file="DictionaryExtensions.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace RolXServer.Common.Util;

/// <summary>
/// Extension methods for Dictionaries.
/// </summary>
public static class DictionaryExtensions
{
    /// <summary>
    /// Method for getting value in dictionary or default if key not found.
    /// </summary>
    /// <typeparam name="TKey">Type parameter TKey.</typeparam>
    /// <typeparam name="TValue">Type parameter TValue.</typeparam>
    /// <param name="dictionary">The dictionary.</param>
    /// <param name="key">The key.</param>
    /// <param name="defaultValue">The default value returned if key not found.</param>
    /// <returns>Value or default if key not found.</returns>
    public static TValue? GetValueOrDefault<TKey, TValue>(
    this IDictionary<TKey, TValue> dictionary,
    TKey key,
    TValue? defaultValue = default)
    {
        return dictionary.TryGetValue(key, out var value) ? value : defaultValue;
    }

    /// <summary>
    /// Method for getting value in dictionary or default if key not found.
    /// </summary>
    /// <typeparam name="TKey">Type parameter TKey.</typeparam>
    /// <typeparam name="TValue">Type parameter TValue.</typeparam>
    /// <param name="dictionary">The dictionary.</param>
    /// <param name="key">The key.</param>
    /// <param name="defaultValueProvider">The default value provider function returned if key not found.</param>
    /// <returns>Value or default if key not found.</returns>
    public static TValue GetValueOrDefault<TKey, TValue>(
        this IDictionary<TKey, TValue> dictionary,
        TKey key,
        Func<TValue> defaultValueProvider)
    {
        return dictionary.TryGetValue(key, out var value) ? value : defaultValueProvider();
    }
}
