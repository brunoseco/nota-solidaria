using System;
using System.Collections.Generic;
public static class DictionaryExtensions
{
    public static TAnonymous ToAnonymousType<T1, TAnonymous>(this IDictionary<String, Object> dictionary, Func<T1, TAnonymous> getAnonymousType)
    {
        var parameters = getAnonymousType.Method.GetParameters();
        return getAnonymousType(
            (T1)dictionary[parameters[0].Name]);
    }

    public static TAnonymous ToAnonymousType<T1, T2, TAnonymous>(this IDictionary<String, Object> dictionary, Func<T1, T2, TAnonymous> getAnonymousType)
    {
        var parameters = getAnonymousType.Method.GetParameters();
        return getAnonymousType(
            (T1)dictionary[parameters[0].Name],
            (T2)dictionary[parameters[1].Name]);
    }

    public static TAnonymous ToAnonymousType<T1, T2, T3, TAnonymous>(this IDictionary<String, Object> dictionary, Func<T1, T2, T3, TAnonymous> getAnonymousType)
    {
        var parameters = getAnonymousType.Method.GetParameters();
        return getAnonymousType(
            (T1)dictionary[parameters[0].Name],
            (T2)dictionary[parameters[1].Name],
            (T3)dictionary[parameters[2].Name]);
    }

    public static TAnonymous ToAnonymousType<T1, T2, T3, T4, TAnonymous>(this IDictionary<String, Object> dictionary, Func<T1, T2, T3, T4, TAnonymous> getAnonymousType)
    {
        var parameters = getAnonymousType.Method.GetParameters();
        return getAnonymousType(
            (T1)dictionary[parameters[0].Name],
            (T2)dictionary[parameters[1].Name],
            (T3)dictionary[parameters[2].Name],
            (T4)dictionary[parameters[3].Name]);
    }
}