using System.Collections.Generic;
using UnityEngine;

public static class GlobalVariables
{
    private static readonly Dictionary<string, object> Dictionary = new Dictionary<string, object>();

    public static void SetInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
        Dictionary[key] = value;
    }

    public static int GetInt(string key)
    {
        if (!Dictionary.ContainsKey(key))
        {
            Dictionary[key] = PlayerPrefs.GetInt(key);
        }
        return (int) Dictionary[key];
    }
}
