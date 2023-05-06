using UnityEngine;

public static class MLog
{
    public static void Log(string keyword, string message)
    {
        Debug.Log(keyword + ": " + message);
    }

    public static void LogError(string keyword, string message)
    {
        Debug.LogError(keyword + ": " + message);
    }

    public static string ArrayToString(int[] array)
    {
        string tmp = "";
        foreach (int value in array)
        {
            tmp += value.ToString() + " ";
        }
        return tmp;
    }
}
